using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
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
        return "K-" + colour;
    }

    public override List<Vector2Int> GetPossibleSpaces()
    {
        List<Vector2Int> possibleSpaces = new List<Vector2Int>();
        if (boardX - 1 >= 0)
        {
            if (board.board[boardX - 1, boardY] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX - 2, boardY - 1));
                possibleSpaces.Add(new Vector2Int(boardX - 2, boardY + 1));
            }
        }
        if (boardX + 1 <= 9)
        {
            if (board.board[boardX + 1, boardY] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX + 2, boardY - 1));
                possibleSpaces.Add(new Vector2Int(boardX + 2, boardY + 1));
            }
        }
        if (boardY - 1 >= 0)
        {
            if (board.board[boardX, boardY - 1] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX - 1, boardY - 2));
                possibleSpaces.Add(new Vector2Int(boardX + 1, boardY - 2));
            }
        }
        if (boardY + 1 <= 8)
        {
            if (board.board[boardX, boardY + 1] == null)
            {
                possibleSpaces.Add(new Vector2Int(boardX - 1, boardY + 2));
                possibleSpaces.Add(new Vector2Int(boardX + 1, boardY + 2));
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
            if (move.x >= 0 && move.x < 10 && move.y >= 0 && move.y < 9)
            {
                validMoves.Add(move);
            }
        }
        return validMoves;
    }
}
