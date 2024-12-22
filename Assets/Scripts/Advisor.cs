using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor : Piece
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
        return "A";
    }

    public override List<Vector2Int> GetPossibleSpaces()
    {
        List<Vector2Int> possibleSpaces = new List<Vector2Int>();
        possibleSpaces.Add(new Vector2Int(boardX - 1, boardY - 1));
        possibleSpaces.Add(new Vector2Int(boardX - 1, boardY + 1));
        possibleSpaces.Add(new Vector2Int(boardX + 1, boardY - 1));
        possibleSpaces.Add(new Vector2Int(boardX + 1, boardY + 1));
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
                if (move.x >= 7 && move.x <= 9 && move.y >= 3 && move.y <= 5)
                {
                    if (board.board[move.x, move.y] != null)
                    {
                        Piece pieceScript = board.board[move.x, move.y].GetComponent<Piece>();
                        if (pieceScript.colour != colour)
                        {
                            validMoves.Add(move);
                        }
                    }
                    else
                    {
                        validMoves.Add(move);
                    }
                }
            }
            else
            {
                if (move.x >= 0 && move.x <= 2 && move.y >= 3 && move.y <= 5)
                {
                    if (board.board[move.x, move.y] != null)
                    {
                        Piece pieceScript = board.board[move.x, move.y].GetComponent<Piece>();
                        if (pieceScript.colour != colour)
                        {
                            validMoves.Add(move);
                        }
                    }
                    else
                    {
                        validMoves.Add(move);
                    }
                }
            }
        }
        return validMoves;
    }
}
