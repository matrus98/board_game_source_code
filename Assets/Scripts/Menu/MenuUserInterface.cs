using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUserInterface : MonoBehaviour {

    [SerializeField]
    string NameOfSinglePlayerScene;

    public void ExitFromGame()
    {
        Application.Quit();
    }

    public void PlaySinglePlayer()
    {
        SceneManager.LoadScene(NameOfSinglePlayerScene);
        Destroy(FindObjectOfType<FindMe>().gameObject);
    }
}
