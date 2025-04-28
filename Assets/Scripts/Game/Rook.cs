using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
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
        return "R";
    }

    public override List<Vector2Int> GetPossibleSpaces()
    {
        List<Vector2Int> possibleSpaces = new List<Vector2Int>();
        for (int i = 1; i <= 9; i++)
        {
            if (boardX - i >= 0)
            {
                if (board.board[boardX - i, boardY] == null)
                {
                    possibleSpaces.Add(new Vector2Int(boardX - i, boardY));
                }
                else
                {
                    possibleSpaces.Add(new Vector2Int(boardX - i, boardY));
                    break;
                }
            }
            else { break; }
        }
        for (int i = 1; i <= 9; i++)
        {
            if (boardX + i <= 9)
            {
                if (board.board[boardX + i, boardY] == null)
                {
                    possibleSpaces.Add(new Vector2Int(boardX + i, boardY));
                }
                else
                {
                    possibleSpaces.Add(new Vector2Int(boardX + i, boardY));
                    break;
                }
            }
            else { break; }
        }
        for (int i = 1; i <= 8; i++)
        {
            if (boardY - i >= 0)
            {
                if (board.board[boardX, boardY - i] == null)
                {
                    possibleSpaces.Add(new Vector2Int(boardX, boardY - i));
                }
                else
                {
                    possibleSpaces.Add(new Vector2Int(boardX, boardY - i));
                    break;
                }
            }
            else { break; }
        }
        for (int i = 1; i <= 8; i++)
        {
            if (boardY + i <= 8)
            {
                if (board.board[boardX, boardY + i] == null)
                {
                    possibleSpaces.Add(new Vector2Int(boardX, boardY + i));
                }
                else
                {
                    possibleSpaces.Add(new Vector2Int(boardX, boardY + i));
                    break;
                }
            }
            else { break; }
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
        return validMoves;
    }
}
