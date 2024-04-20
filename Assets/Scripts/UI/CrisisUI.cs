using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CrisisUI : MonoBehaviour {

    Field Field;
    Pawn Pawn;
    private bool IsConfigured = false;
    private bool AmIDisplayed = false;

    public void Configure(Field field, Pawn pawn)
    {
        Field = field;
        Pawn = pawn;
        IsConfigured = true;
        AmIDisplayed = true;
    }

	void Update ()
    {
        if (IsConfigured)
        {
            ChangeButtonColor();

            ChangeEndingOfTurnAvailability(AmIDisplayed);
        }
    }

    private void ChangeEndingOfTurnAvailability(bool F)
    {
        FindObjectOfType<DisableEndTurnButton>().DisableButton(F);
    }

    private void ChangeButtonColor()
    {
        var buttons = GetComponentsInChildren<Button>();
        var Images = GetComponentsInChildren<Image>();

        if (Pawn.MyMoney < Field.HowMuchSomsiadBuli())
        {
            Images.Last().color = Color.gray;
            buttons.Last().enabled = false;
        }
        else
        {
            Images.Last().color = Color.white;
            buttons.Last().enabled = true;
        }
    }

    public void PayWhatIOwe()
    {
        Field.GetOwnerOfThisField().MyMoney += Field.HowMuchSomsiadBuli();
        Pawn.MyMoney -= Field.HowMuchSomsiadBuli();
        ChangeEndingOfTurnAvailability(false);
        Destroy(gameObject);
    }

    public void Surrender()
    {
        Pawn.ICannotPayAtAll();
        ChangeEndingOfTurnAvailability(false);
        Destroy(gameObject);
    }
}
