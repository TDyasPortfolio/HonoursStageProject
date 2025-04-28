using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSelectionArea : MonoBehaviour
{
    public int boardX;
    public int boardY;
    public int sourceX;
    public int sourceY;
    private Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
    }
    
    void OnMouseDown()
    {
        if (board.GameIsOffline())
        {
            board.MovePiece(sourceX, sourceY, boardX, boardY);
        }
        else
        {
            board.AttemptMoveServerRPC(sourceX, sourceY, boardX, boardY);
        }
    }
}
