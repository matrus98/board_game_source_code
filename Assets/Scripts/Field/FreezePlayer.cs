using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FreezePlayer : MonoBehaviour {

    [SerializeField]
    int FreezeDurationInTurns = 2;
    private int IndexOfThisField = 27;
    private bool JailThisTime = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Pawn>() != null)
            JailThisTime = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (JailThisTime)
        {
            StartCoroutine(IGoJailedCoroutine(other));
            JailThisTime = false;
        } 
    }

    private IEnumerator IGoJailedCoroutine(Collider other)
    {
        yield return new WaitForSeconds(0.5f);

        var obj = other.gameObject.GetComponent<Pawn>();

        if(obj != null && obj.IndexOfFieldIAmStayingOn == IndexOfThisField)
        {
            obj.SendMeToPrison(FreezeDurationInTurns);
            //Debug.Log("Kolizja z polem GO TO JAIL");
        }
    }
}
