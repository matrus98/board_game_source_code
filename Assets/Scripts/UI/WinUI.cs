using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour {

    [SerializeField]
    string MainMenuScene;

    [SerializeField]
    string OfflineBoardScene;

    [SerializeField]
    Text WinnerNote;

	public void Configure(string winner)
    {
        WinnerNote.text = string.Format("Congratulations {0}\nYou have reedemed the World!", winner);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(OfflineBoardScene);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
