using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionOfTheField : MonoBehaviour {

    /// <summary>
    /// Ustawia opis pola
    /// </summary>
    /// <param name="textMesh"></param>
    public void Configure(string textMesh, int Price)
    {
        GetComponent<TextMesh>().text = string.Format("{0}\n{1}$", textMesh, Price);
    }
}
