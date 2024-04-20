using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerMoney : MonoBehaviour {

    private int IndexOfThisField = 0;
    private bool IfGiveMoney = false;
    private Pawn PlayerWhoHasEntered;
    private int HowMuchGive = 500;

    private void OnTriggerEnter(Collider other)
    {
        PlayerWhoHasEntered = other.gameObject.GetComponent<Pawn>();
        IfGiveMoney = true;

        if (PlayerWhoHasEntered == null)
            return;

        GivePlayerMoneyAsHePasses();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (PlayerWhoHasEntered == null)
            return;

        if (PlayerWhoHasEntered.IndexOfFieldIAmStayingOn != IndexOfThisField)
            return;

        GivePlayerMoneyAsHePasses();
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerWhoHasEntered == null)
            return;

        GivePlayerMoneyAsHePasses();
    }

    private void GivePlayerMoneyAsHePasses()
    {
        if (IfGiveMoney)
        {
            IfGiveMoney = false;

            var finalPrice = HowMuchGive;

            if (PlayerWhoHasEntered.HalfMoney)
            {
                PlayerWhoHasEntered.HalfMoney = false;
                finalPrice /= 2;
            }

            PlayerWhoHasEntered.MyMoney += finalPrice;
        }
    }
}
