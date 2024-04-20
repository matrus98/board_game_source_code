using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNullToConstantFieldInfo : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<ConstantInfoUserInterface>().DeactivateShowingInfo();

        var PlayerWhoHasEntered = other.gameObject.GetComponent<Pawn>();

        if (PlayerWhoHasEntered == null)
            return;

        if(GetComponent<QuestionMark>() == null)
            StartCoroutine(EnableEndTurButtonCoroutine(other, PlayerWhoHasEntered));
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<ConstantInfoUserInterface>().ActivateShowingInfo();
    }

    IEnumerator EnableEndTurButtonCoroutine(Collider other, Pawn PlayerWhoHasEntered)
    {
        var MyIndex = PlayerWhoHasEntered.CurrentIndex;

        yield return new WaitForSeconds(1);

        if (PlayerWhoHasEntered != null && PlayerWhoHasEntered.IndexOfFieldIAmStayingOn == MyIndex)
        {
            FindObjectOfType<DisableEndTurnButton>().DisableButton(false);
        }
    }
}
