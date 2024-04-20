using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CrisisUIForCards : MonoBehaviour {

    Pawn Pawn;
    int Fine;
    private bool IsConfigured = false;
    private bool AmIDisplayed = false;

    public void Configure(Pawn pawn, int fine)
    {
        Pawn = pawn;
        Fine = fine;
        IsConfigured = true;
        AmIDisplayed = true;
    }

    void Update()
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

        if (Pawn.MyMoney < Fine)
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
        Pawn.MyMoney -= Fine;

        if (Pawn.FreezedTurns > 0)
            Pawn.SendMeToPrison(0);

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
