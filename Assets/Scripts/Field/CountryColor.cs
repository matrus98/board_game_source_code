using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryColor : MonoBehaviour {

    /// <summary>
    /// Ustawia Materiał dla przynależności do kraju
    /// </summary>
    /// <param name="material"></param>
    public void Configure(Material material)
    {
        GetComponent<Renderer>().material = material;
    }
}
