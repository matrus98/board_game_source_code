using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousesAndHotelPosition : MonoBehaviour {

    public Vector3[] HousesAndHotelPositionArray = new Vector3[]
        {
            new Vector3(0.35f, 5.5f, -0.35f),
            new Vector3(0.35f, 5.5f, -0.12f),
            new Vector3(0.35f, 5.5f, 0.12f),
            new Vector3(0.35f, 5.5f, 0.35f),
            new Vector3(0.35f, 8f, 0f)
        };

    public Vector3[] GetHousesAndHotelPositionArray()
    {
        return HousesAndHotelPositionArray;
    }
}
