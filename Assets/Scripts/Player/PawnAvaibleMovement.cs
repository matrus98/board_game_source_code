using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAvaibleMovement : MonoBehaviour {

    public Vector3[] PawnAvaibleMovementArray = new Vector3[]
    {
        new Vector3(4.5f, 0.01f, 4.5f),
        new Vector3(4.5f, 0.01f, 3.5f),
        new Vector3(4.5f, 0.01f, 2.5f),
        new Vector3(4.5f, 0.01f, 1.5f),
        new Vector3(4.5f, 0.01f, 0.5f),
        new Vector3(4.5f, 0.01f, -0.5f),
        new Vector3(4.5f, 0.01f, -1.5f),
        new Vector3(4.5f, 0.01f, -2.5f),
        new Vector3(4.5f, 0.01f, -3.5f),
        new Vector3(4.5f, 0.01f, -4.5f),
        new Vector3(3.5f, 0.01f, -4.5f),
        new Vector3(2.5f, 0.01f, -4.5f),
        new Vector3(1.5f, 0.01f, -4.5f),
        new Vector3(0.5f, 0.01f, -4.5f),
        new Vector3(-0.5f, 0.01f, -4.5f),
        new Vector3(-1.5f, 0.01f, -4.5f),
        new Vector3(-2.5f, 0.01f, -4.5f),
        new Vector3(-3.5f, 0.01f, -4.5f),
        new Vector3(-4.5f, 0.01f, -4.5f),
        new Vector3(-4.5f, 0.01f, -3.5f),
        new Vector3(-4.5f, 0.01f, -2.5f),
        new Vector3(-4.5f, 0.01f, -1.5f),
        new Vector3(-4.5f, 0.01f, -0.5f),
        new Vector3(-4.5f, 0.01f, 0.5f),
        new Vector3(-4.5f, 0.01f, 1.5f),
        new Vector3(-4.5f, 0.01f, 2.5f),
        new Vector3(-4.5f, 0.01f, 3.5f),
        new Vector3(-4.5f, 0.01f, 4.5f),
        new Vector3(-3.5f, 0.01f, 4.5f),
        new Vector3(-2.5f, 0.01f, 4.5f),
        new Vector3(-1.5f, 0.01f, 4.5f),
        new Vector3(-0.5f, 0.01f, 4.5f),
        new Vector3(0.5f, 0.01f, 4.5f),
        new Vector3(1.5f, 0.01f, 4.5f),
        new Vector3(2.5f, 0.01f, 4.5f),
        new Vector3(3.5f, 0.01f, 4.5f),
    };

    public Vector3[] GetPawnAvaibleMovementArray()
    {
        return PawnAvaibleMovementArray;
    }
}
