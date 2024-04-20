using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    float AwaitingTime = 0.25f;
    Vector3 SavedMovement;
    bool HasStopped = false;

	void Start () {
        StartCoroutine(CheckWheterDiceHasStoppedCoroutine());
	}

	void Update ()
    {
        
    }

    private void GetEyesNumber()
    {
        if (!HasStopped)
            return;

        var array = GetComponentsInChildren<Transform>();

        float TopFace = 0;
        int IndexOfTopFace = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (TopFace < array[i].position.y)
            {
                TopFace = array[i].position.y;
                IndexOfTopFace = i;
            }
        }

        string name = array[IndexOfTopFace].name;

        //Debug.Log("Eyes:" + name[0]);
        switch (name[0])
        {
            case '1':
                {
                    if (GetPlayerWithTurn() == null)
                        return;

                    GetPlayerWithTurn().IncreseIndex(1);
                    break;
                }
            case '2':
                {
                    if (GetPlayerWithTurn() == null)
                        return;

                    GetPlayerWithTurn().IncreseIndex(2);
                    break;
                }
            case '3':
                {
                    if (GetPlayerWithTurn() == null)
                        return;

                    GetPlayerWithTurn().IncreseIndex(3);
                    break;
                }
            case '4':
                {
                    if (GetPlayerWithTurn() == null)
                        return;

                    GetPlayerWithTurn().IncreseIndex(4);
                    break;
                }
            case '5':
                {
                    if (GetPlayerWithTurn() == null)
                        return;

                    GetPlayerWithTurn().IncreseIndex(5);
                    break;
                }
            case '6':
                {
                    if (GetPlayerWithTurn() == null)
                        return;

                    GetPlayerWithTurn().IncreseIndex(6);
                    break;
                }
        }
    }

    IEnumerator CheckWheterDiceHasStoppedCoroutine()
    {
        while (!HasStopped)
        {
            SavedMovement = transform.position;

            yield return new WaitForSeconds(AwaitingTime);

            if (SavedMovement.Equals(transform.position))
            {
                //Debug.Log("Zatrzymano");
                HasStopped = true;
            }
        }

        GetEyesNumber();
    }

    private Pawn GetPlayerWithTurn()
    {
        var array = FindObjectsOfType<Pawn>();

        foreach(Pawn X in array)
        {
            if (X.IsMyTurn)
                return X;
        }

        return null;
    }
}
