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
        board.MovePiece(sourceX, sourceY, boardX, boardY);
    }
}
