using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBelongingColor : MonoBehaviour {

    /// <summary>
    /// Ustawia kolor pola na kolor wybrany przez gracza, który kupił pole
    /// </summary>
    /// <param name="material"></param>
    public void Configure(Material material)
    {
        GetComponent<Renderer>().material = material;
    }
}
