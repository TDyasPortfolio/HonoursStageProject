using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject[,] board = new GameObject[10, 9];
    // GameObject prefabs
    public GameObject blackGeneral;
    public GameObject blackAdvisor;
    public GameObject blackElephant;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackCannon;
    public GameObject blackSoldier;
    public GameObject redGeneral;
    public GameObject redAdvisor;
    public GameObject redElephant;
    public GameObject redKnight;
    public GameObject redRook;
    public GameObject redCannon;
    public GameObject redSoldier;

    void Start()
    {
        InitializeBoard();
    }
    
    void InitializeBoard()
    {
        PlacePiece(blackGeneral, 0, 4, "black");
        PlacePiece(blackAdvisor, 0, 3, "black");
        PlacePiece(blackAdvisor, 0, 5, "black");
        PlacePiece(blackElephant, 0, 2, "black");
        PlacePiece(blackElephant, 0, 6, "black");
        PlacePiece(blackKnight, 0, 1, "black");
        PlacePiece(blackKnight, 0, 7, "black");
        PlacePiece(blackRook, 0, 0, "black");
        PlacePiece(blackRook, 0, 8, "black");
        PlacePiece(blackCannon, 2, 1, "black");
        PlacePiece(blackCannon, 2, 7, "black");
        PlacePiece(blackSoldier, 3, 0, "black");
        PlacePiece(blackSoldier, 3, 2, "black");
        PlacePiece(blackSoldier, 3, 4, "black");
        PlacePiece(blackSoldier, 3, 6, "black");
        PlacePiece(blackSoldier, 3, 8, "black");
        PlacePiece(redGeneral, 9, 4, "red");
        PlacePiece(redAdvisor, 9, 3, "red");
        PlacePiece(redAdvisor, 9, 5, "red");
        PlacePiece(redElephant, 9, 2, "red");
        PlacePiece(redElephant, 9, 6, "red");
        PlacePiece(redKnight, 9, 1, "red");
        PlacePiece(redKnight, 9, 7, "red");
        PlacePiece(redRook, 9, 0, "red");
        PlacePiece(redRook, 9, 8, "red");
        PlacePiece(redCannon, 7, 1, "red");
        PlacePiece(redCannon, 7, 7, "red");
        PlacePiece(redSoldier, 6, 0, "red");
        PlacePiece(redSoldier, 6, 2, "red");
        PlacePiece(redSoldier, 6, 4, "red");
        PlacePiece(redSoldier, 6, 6, "red");
        PlacePiece(redSoldier, 6, 8, "red");
        Debug.Log(GetBoardAsString());
    }

    void PlacePiece(GameObject prefab, int x, int y, string colour)
    {
        Piece pieceScript = prefab.GetComponent<Piece>();
        pieceScript.boardX = x; pieceScript.boardY = y;
        if (colour == "black")
        {
            GameObject piece = Instantiate(prefab, WorldSpaceCoords(x, y), Quaternion.Euler(new Vector3(-90, 0, 0)));
            board[x, y] = piece;
        }
        else
        {
            GameObject piece = Instantiate(prefab, WorldSpaceCoords(x, y), Quaternion.Euler(new Vector3(-90, 180, 0)));
            board[x, y] = piece;
        }
    }

    public Vector3 WorldSpaceCoords(int y, int x)
    {
        return new Vector3(-0.85f + (x * 0.2125f), 0.27f, 1.075f - (y * 0.215f));
    }

    public string GetBoardAsString()
    {
        string BoardState = "";
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                GameObject currentPiece = board[row, col];
                if (currentPiece != null)
                {
                    BoardState += currentPiece.GetComponent<Piece>().ToString();
                }
                else
                {
                    BoardState += "...";
                }
                if (col < 8) BoardState += " ";
            }
            BoardState += "\n";
        }

        return BoardState;
    }

    public void MovePiece(int xFrom, int yFrom, int xTo, int yTo)
    {
        if (board[xFrom, yFrom] != null)
        {
            if (board[xTo, yTo] != null)
            {
                GameObject pieceDelete = board[xTo, yTo];
                Destroy(pieceDelete);
            }
            GameObject pieceMove = board[xFrom, yFrom];
            Piece pieceScript = pieceMove.GetComponent<Piece>();
            board[xTo, yTo] = pieceMove;
            pieceMove.transform.position = WorldSpaceCoords(xTo, yTo);
            pieceScript.boardX = xTo; pieceScript.boardY = yTo;
            board[xFrom, yFrom] = null;
        }
        ClearAllPrefabs();
    }

    public void ClearAllPrefabs()
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    pieceScript.ClearPrefabs();
                }
            }
        }
    }

    void Update()
    {

    }
}
