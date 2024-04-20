using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour {

    [SerializeField]
    string MainMenu;

    [SerializeField]
    Dropdown Dropdown;

    [SerializeField]
    InputField InputField;

    [SerializeField]
    Text CountOfPlayers;

    [SerializeField]
    Button PlayButton;

    private void Update()
    {
        if (FindObjectsOfType<Pawn>().Count() < 2)
        {
            PlayButton.enabled = false;
            PlayButton.GetComponent<Image>().color = Color.gray;
        }

        else
        {
            PlayButton.enabled = true;
            PlayButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void PlayGame()
    {
        FindObjectOfType<GameManager>().GiveFirstTurn();
        Destroy(gameObject);
    }

    public void AddNewPlayer()
    {
        if (InputField.GetComponentsInChildren<Text>().Last().text.Length < 3)
            return;

        switch (Dropdown.value)
        {
            case 0:
                {
                    var obj = FindObjectOfType<GlobalSettingsPlayerColor>().GetParticularPawn(Dropdown.value);
                    if (obj == null)
                        return;

                    UpdateName(obj);
                    Instantiate(obj);
                    ChangeCurrentCountOfPlayer();
                    break;
                }
            case 1:
                {
                    var obj = FindObjectOfType<GlobalSettingsPlayerColor>().GetParticularPawn(Dropdown.value);
                    if (obj == null)
                        return;

                    UpdateName(obj);
                    Instantiate(obj);
                    ChangeCurrentCountOfPlayer();
                    break;
                }
            case 2:
                {
                    var obj = FindObjectOfType<GlobalSettingsPlayerColor>().GetParticularPawn(Dropdown.value);
                    if (obj == null)
                        return;

                    UpdateName(obj);
                    Instantiate(obj);
                    ChangeCurrentCountOfPlayer();
                    break;
                }
            case 3:
                {
                    var obj = FindObjectOfType<GlobalSettingsPlayerColor>().GetParticularPawn(Dropdown.value);
                    if (obj == null)
                        return;

                    UpdateName(obj);
                    Instantiate(obj);
                    ChangeCurrentCountOfPlayer();
                    break;
                }
            case 4:
                {
                    var obj = FindObjectOfType<GlobalSettingsPlayerColor>().GetParticularPawn(Dropdown.value);
                    if (obj == null)
                        return;

                    UpdateName(obj);
                    Instantiate(obj);
                    ChangeCurrentCountOfPlayer();
                    break;
                }
            case 5:
                {
                    var obj = FindObjectOfType<GlobalSettingsPlayerColor>().GetParticularPawn(Dropdown.value);
                    if (obj == null)
                        return;

                    UpdateName(obj);
                    Instantiate(obj);
                    ChangeCurrentCountOfPlayer();
                    break;
                }
        }
    }

    private void ChangeCurrentCountOfPlayer()
    {
        CountOfPlayers.text = string.Format("Current Count of Players: {0}", FindObjectsOfType<Pawn>().Count());
    }

    private void UpdateName(GameObject obj)
    {
        obj.GetComponent<Pawn>().MyName = InputField.GetComponentsInChildren<Text>().Last().text;
        InputField.GetComponentsInChildren<Text>().Last().text = "";
        InputField.GetComponentsInChildren<Text>().First().text = "Enter Another Name...";
    }
}
