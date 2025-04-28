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
    public bool active;
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
        if (active && board.PlayerCanMove())
        {
            board.ClearAllPrefabs();
            List<Vector2Int> possibleSpaces = FilterSelfChecks(GetPossibleSpaces());
            foreach (Vector2Int coOrds in possibleSpaces)
            {
                GameObject selectionArea = Instantiate(selectionPrefab, board.WorldSpaceCoords(coOrds.x, coOrds.y), Quaternion.Euler(new Vector3(-90, 0, 0)));
                SpaceSelectionArea selectionScript = selectionArea.GetComponent<SpaceSelectionArea>();
                selectionScript.boardX = coOrds.x; selectionScript.boardY = coOrds.y;
                selectionScript.sourceX = boardX; selectionScript.sourceY = boardY;
                currentPrefabs.Add(selectionArea);
            }
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

    public List<Vector2Int> FilterSelfChecks(List<Vector2Int> movesList)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        foreach (var move in movesList)
        {
            GameObject targetPiece = board.board[move.x, move.y];
            GameObject movingPiece = board.board[boardX, boardY];
            int originalX = boardX; int originalY = boardY;
            board.board[boardX, boardY] = null;
            board.board[move.x, move.y] = null;
            board.board[move.x, move.y] = movingPiece;
            boardX = move.x; boardY = move.y;

            bool moveCausesCheck = board.getCheck(colour == "R");

            board.board[move.x, move.y] = targetPiece;
            board.board[originalX, originalY] = movingPiece;
            boardX = originalX; boardY = originalY;
            if (!moveCausesCheck)
            {
                validMoves.Add(move);
            }
        }
        return validMoves;
    }
}
