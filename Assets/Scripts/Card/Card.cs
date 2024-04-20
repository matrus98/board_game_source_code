using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    QuestionMarkColor QuestionMarkColor;
    Pawn PlayerWhoTookThisCard;
    CardNotes CardNotes;

    bool ToCamera = true;
    private float DurationOfStay = 5f;

    Camera Camera;
    Vector3 TargetPosition = new Vector3(0, 0, 3f);
    Vector3 TargetRotation = new Vector3(90f, 0, 0);

	void Start () {
        Camera = FindObjectOfType<Camera>();
        transform.parent = Camera.transform;
    }

	void Update ()
    {
        if (ToCamera)
        {
            MoveToCamera();
        }
        else
        {
            MoveAwayFromCamera();
        }
    }

    private void MoveToCamera()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, Time.deltaTime);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(TargetRotation), Time.deltaTime);
    }

    private void MoveAwayFromCamera()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(TargetRotation), Time.deltaTime);
    }

    IEnumerator ChangePlaceCoroutine()
    {
        SetDecription();

        yield return new WaitForSeconds(5f + DurationOfStay);

        TargetPosition = new Vector3();

        if(QuestionMarkColor == QuestionMarkColor.Blue)
        {
            TargetPosition = new Vector3(-2, 0, 2);
        }
        else
        {
            TargetPosition = new Vector3(2, 0, -2);
        }

        TargetRotation = new Vector3(0, -45, 0);
        transform.parent = FindObjectOfType<Board>().transform;
        ToCamera = false;

        CardNotes.PerformAction(PlayerWhoTookThisCard, QuestionMarkColor);
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    public void Configure(QuestionMarkColor questionMarkColor, Pawn player)
    {
        QuestionMarkColor = questionMarkColor;
        PlayerWhoTookThisCard = player;
        CardNotes = GetComponent<CardNotes>();

        StartCoroutine(ChangePlaceCoroutine());
    }

    private void SetDecription()
    {
        GetComponentInChildren<Text>().text = CardNotes.GetRandomNote(QuestionMarkColor);
    }
}
