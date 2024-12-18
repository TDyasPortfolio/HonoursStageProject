using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Piece
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
        return "S-" + colour;
    }

    public override List<Vector2Int> GetPossibleSpaces()
    {
        List<Vector2Int> possibleSpaces = new List<Vector2Int>();
        bool crossedRiver = false;
        if (colour == "R")
        {
            possibleSpaces.Add(new Vector2Int(boardX - 1, boardY));
            if (boardX < 5) { crossedRiver = true; }
        }
        else
        {
            possibleSpaces.Add(new Vector2Int(boardX + 1, boardY));
            if (boardX > 4) { crossedRiver = true; }
        }
        if (crossedRiver)
        {
            possibleSpaces.Add(new Vector2Int(boardX, boardY - 1));
            possibleSpaces.Add(new Vector2Int(boardX, boardY + 1));
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
