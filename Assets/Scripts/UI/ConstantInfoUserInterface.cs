using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConstantInfoUserInterface : MonoBehaviour {

    [SerializeField]
    GameObject NewPlayerFramePrefab;

    [SerializeField]
    Text WhoseTurnText;

    private Field Field;
    private bool CanShow = false;

    private void Awake()
    {
        //Debug.Log("OnPlayerAdded Subcribed");
        FindObjectOfType<GameManager>().OnPlayerAdded += Pawn =>
        {
            var pawninfo = Instantiate(NewPlayerFramePrefab);
            pawninfo.transform.parent = transform;
            pawninfo.GetComponent<RectTransform>().localPosition = new Vector2(335, 180 + (-40 * (FindObjectsOfType<PawnInfo>().Count() - 1)));
            pawninfo.GetComponent<PawnInfo>().Configure(Pawn);
        };

        //Debug.Log("OnPlayerTurnChanged Subcribed");
        FindObjectOfType<GameManager>().OnPlayerTurnChanged += Name =>
        {
            WhoseTurnText.text = "Player's turn: " + Name;
        };
    }

    public void AttachUserInterfaceDescriptionOfTheField(Field field)
    {
        Field = field;
    }

    public void ShowInfo()
    {
        if (!CanShow)
            return;

        else if (!Field.CheckWhetherICanShowInfo())
            Field.DestroyTemporaryCanvaFunction();

        else Field.CreateTemporaryCanvaFunction();
    }

    public void DeactivateShowingInfo()
    {
        CanShow = false;
    }

    public void ActivateShowingInfo()
    {
        CanShow = true;
    }
}
