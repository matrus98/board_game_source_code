using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField]
    GameObject FieldMain;

    [SerializeField]
    GameObject FieldJailOrVisit;

    [SerializeField]
    GameObject FieldFreeParking;

    [SerializeField]
    GameObject FieldGoToJail;

    [SerializeField]
    GameObject FieldStart;

    [SerializeField]
    GameObject RedQuestionMarkField;

    [SerializeField]
    GameObject BlueQuestionMarkField;

    [SerializeField]
    FieldType[] FieldTypes;

    private const int NumberOfFieldsInRow = 8;
    private int MultiplySupport = 1;
    private int NumberOfRotations = 0;
    Quaternion Direction = Quaternion.Euler(Vector3.zero);

    void Start () {
        SetBoard();
	}

	void Update () {
		
	}

    void SetBoard()
    {
        GenerateConstantFields();
        GenerateBuyableField();
    }

    private void GenerateBuyableField()
    {
        for(int i = 0; i < 32; i++)
        {
            var obj = Instantiate(FieldMain);
            obj.transform.parent = transform;
            obj.GetComponent<Field>().Configure((FieldType)FieldTypes.GetValue(i), i, NumberOfRotations);
            obj.transform.rotation *= Direction;

            if(i % ((NumberOfFieldsInRow * MultiplySupport) - 1) == 0 && i != 0)
            {
                ChangeRotation();
                NumberOfRotations++;
            }
        }
    }

    private void ChangeRotation()
    {
        Direction *= Quaternion.Euler(Vector3.up * 90f);
        MultiplySupport++;
    }

    private void GenerateConstantFields()
    {
        var ObjFieldJailOrVisit = Instantiate(FieldJailOrVisit);
        ObjFieldJailOrVisit.transform.position = new Vector3(4.5f, 0.01f, -4.5f);
        ObjFieldJailOrVisit.transform.parent = transform;

        var ObjFieldFreeParking = Instantiate(FieldFreeParking);
        ObjFieldFreeParking.transform.position = new Vector3(-4.5f, 0.01f, -4.5f);
        ObjFieldFreeParking.transform.parent = transform;

        var ObjFieldGoToJail = Instantiate(FieldGoToJail);
        ObjFieldGoToJail.transform.position = new Vector3(-4.5f, 0.01f, 4.5f);
        ObjFieldGoToJail.transform.parent = transform;

        var ObjFieldStart = Instantiate(FieldStart);
        ObjFieldStart.transform.position = new Vector3(4.5f, 0.01f, 4.5f);
        ObjFieldStart.transform.parent = transform;
    }

    public void GenerateQuestionMark(Vector3 Position, int index)
    {
        if(index % 2 == 0)
        {
            var BlueQ = Instantiate(BlueQuestionMarkField);
            BlueQ.GetComponent<QuestionMark>().Configure(QuestionMarkColor.Blue, index);
            BlueQ.transform.parent = transform;
            BlueQ.transform.position = Position;
            BlueQ.transform.rotation *= Direction;
        }
        else
        {
            var BlueQ = Instantiate(RedQuestionMarkField);
            BlueQ.GetComponent<QuestionMark>().Configure(QuestionMarkColor.Red, index);
            BlueQ.transform.parent = transform;
            BlueQ.transform.position = Position;
            BlueQ.transform.rotation *= Direction;
        }
    }
}
