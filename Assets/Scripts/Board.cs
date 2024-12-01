using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject blackSoldier;
    public GameObject redSoldier;

    void Start()
    {
        Instantiate(blackSoldier, WorldSpaceCoords(0, 3), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(blackSoldier, WorldSpaceCoords(2, 3), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(blackSoldier, WorldSpaceCoords(4, 3), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(blackSoldier, WorldSpaceCoords(6, 3), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(blackSoldier, WorldSpaceCoords(8, 3), Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(redSoldier, WorldSpaceCoords(0, 6), Quaternion.Euler(new Vector3(-90, 180, 0)));
        Instantiate(redSoldier, WorldSpaceCoords(2, 6), Quaternion.Euler(new Vector3(-90, 180, 0)));
        Instantiate(redSoldier, WorldSpaceCoords(4, 6), Quaternion.Euler(new Vector3(-90, 180, 0)));
        Instantiate(redSoldier, WorldSpaceCoords(6, 6), Quaternion.Euler(new Vector3(-90, 180, 0)));
        Instantiate(redSoldier, WorldSpaceCoords(8, 6), Quaternion.Euler(new Vector3(-90, 180, 0)));
    }
    
    void Update()
    {
        
    }

    Vector3 WorldSpaceCoords(int x, int y)
    {
        return new Vector3(0.85f - (x * 0.215f), 0.27f, 1.075f - (y * 0.215f));
    }
}
