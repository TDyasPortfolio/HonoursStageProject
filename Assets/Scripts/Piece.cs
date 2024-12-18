using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece: MonoBehaviour
{
    [SerializeField]
    public string colour;

    public int boardX;
    public int boardY;
    public Board board;
    public GameObject selectionPrefab;
    public List<GameObject> currentPrefabs;

    void Start()
    {
        currentPrefabs = new List<GameObject>();
    }

    void Update()
    {
        
    }

    string Colour() { return colour; }

    public abstract override string ToString();

    public void OnMouseDown()
    {
        board.ClearAllPrefabs();
        List<Vector2Int> possibleSpaces = GetPossibleSpaces();
        foreach (Vector2Int coOrds in possibleSpaces)
        {
            GameObject selectionArea = Instantiate(selectionPrefab, board.WorldSpaceCoords(coOrds.x, coOrds.y), Quaternion.Euler(new Vector3(-90, 0, 0)));
            SpaceSelectionArea selectionScript = selectionArea.GetComponent<SpaceSelectionArea>();
            selectionScript.boardX = coOrds.x; selectionScript.boardY = coOrds.y;
            selectionScript.sourceX = boardX; selectionScript.sourceY = boardY;
            currentPrefabs.Add(selectionArea);
        }
    }

    public void ClearPrefabs()
    {
        foreach (GameObject selection in currentPrefabs)
        {
            Destroy(selection);
        }
    }

    public abstract List<Vector2Int> GetPossibleSpaces();

    public abstract List<Vector2Int> FilterImpossibleMoves(List<Vector2Int> movesList);
}
