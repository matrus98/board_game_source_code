using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionMark : MonoBehaviour {

    [SerializeField]
    GameObject BlueCard;

    [SerializeField]
    GameObject RedCard;

    public bool TakeAnAction = false;

    Vector3 RedQuestionMarkCardSpawnPoint = new Vector3(2, 0.2f, -2);
    Vector3 BlueQuestionMarkCardSpawnPoint = new Vector3(-2, 0.2f, 2);

    QuestionMarkColor QuestionMarkColor;
    public int MyIndex;

    private Vector3 SpawnPoint;

    public void Configure(QuestionMarkColor questionMarkColor, int index)
    {
        QuestionMarkColor = questionMarkColor;
        MyIndex = index;

        if(QuestionMarkColor == QuestionMarkColor.Blue)
        {
            SpawnPoint = BlueQuestionMarkCardSpawnPoint;
        }
        else
        {
            SpawnPoint = RedQuestionMarkCardSpawnPoint;
        }
    }

    IEnumerator WaitForAWhileToTakeCardCoroutine(Pawn player)
    {
        yield return new WaitForSeconds(1f);

        if (TakeAnAction && player.IndexOfFieldIAmStayingOn == MyIndex)
        {
            StartCoroutine(TakeCardCoroutine(player));
            TakeAnAction = false;
        }
    }

    IEnumerator TakeCardCoroutine(Pawn player)
    {
        FindObjectOfType<DisableEndTurnButton>().DisableButton(true);

        yield return new WaitForSeconds(1f);

        if (QuestionMarkColor == QuestionMarkColor.Blue)
        {
            var obj = Instantiate(BlueCard);
            obj.GetComponent<Card>().Configure(QuestionMarkColor.Blue, player);
            obj.transform.position = SpawnPoint;
        }
        else
        {
            var obj = Instantiate(RedCard);
            obj.GetComponent<Card>().Configure(QuestionMarkColor.Red, player);
            obj.transform.position = SpawnPoint;
        }

        GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<Pawn>();

        if (obj == null)
            return;

        if (obj.IndexOfFieldIAmStayingOn == MyIndex)
        {
            TakeAnAction = true;
        }
            
        StartCoroutine(WaitForAWhileToTakeCardCoroutine(obj));
    }

    private void OnTriggerExit(Collider other)
    {
        if (TakeAnAction)
        {
            TakeAnAction = false;
        }

        var obj = other.gameObject.GetComponent<Pawn>();

        if (obj == null)
            return;
    }

    private void OnTriggerStay(Collider other)
    {
        var obj = other.gameObject.GetComponent<Pawn>();

        if (obj == null)
            return;
    }
}
