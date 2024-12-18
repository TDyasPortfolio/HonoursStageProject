using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : Piece
{
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    void Update()
    {

    }

    public override string ToString()
    {
        return "E-" + colour;
    }

    public override List<Vector2Int> GetPossibleSpaces()
    {
        List<Vector2Int> possibleSpaces = new List<Vector2Int>();
        if (boardX - 1 >= 0 && boardY - 1 >= 0)
        {
            Debug.Log("Checking (" + (boardX - 1) + ", " + (boardY - 1) + ")");
            if (board.board[boardX - 1, boardY - 1] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX - 2, boardY - 2));
            }
        }
        if (boardX - 1 >= 0 && boardY + 1 <= 8)
        {
            Debug.Log("Checking (" + (boardX - 1) + ", " + (boardY + 1) + ")");
            if (board.board[boardX - 1, boardY + 1] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX - 2, boardY + 2));
            }
        }
        if (boardX + 1 <= 9 && boardY - 1 >= 0)
        {
            Debug.Log("Checking (" + (boardX + 1) + ", " + (boardY - 1) + ")");
            if (board.board[boardX + 1, boardY - 1] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX + 2, boardY - 2));
            }
        }
        if (boardX + 1 <= 9 && boardY + 1 <= 8)
        {
            Debug.Log("Checking (" + (boardX + 1) + ", " + (boardY + 1) + ")");
            if (board.board[boardX + 1, boardY + 1] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX + 2, boardY + 2));
            }
        }
        possibleSpaces = FilterImpossibleMoves(possibleSpaces);
        return possibleSpaces;
    }

    public override List<Vector2Int> FilterImpossibleMoves(List<Vector2Int> movesList)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        foreach (var move in movesList)
        {
            if (colour == "R")
            {
                if (move.x >= 5 && move.x <= 9 && move.y >= 0 && move.y <= 8)
                {
                    validMoves.Add(move);
                }
            }
            else
            {
                if (move.x >= 0 && move.x <= 4 && move.y >= 0 && move.y <= 8)
                {
                    validMoves.Add(move);
                }
            }
        }
        return validMoves;
    }
}
