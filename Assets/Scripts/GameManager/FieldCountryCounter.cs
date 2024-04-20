using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialMatchToCountry
{
    public Material Material;
    public int NumberOfFields = 0;
}

public class FieldCountryCounter : MonoBehaviour {

    [SerializeField]
    MaterialMatchToCountry[] Matcher;

    public void SetNumberOFCountryFields(Material material)
    {
        foreach(MaterialMatchToCountry X in Matcher)
        {
            if(material == X.Material)
            {
                X.NumberOfFields++;
                return;
            }
        }
    }

    public int HowManyFieldsOfThisCountry(Material material)
    {
        foreach (MaterialMatchToCountry X in Matcher)
        {
            if(X.Material == material)
                return X.NumberOfFields;
        }

        return 0;
    }

    public bool DidIOwnedThemAll(FieldType fieldType, string AskingPlayer)
    {
        int TargetNumber = HowManyFieldsOfThisCountry(fieldType.CountryColor);
        int CurrentNumber = 0;
        var array = FindObjectsOfType<Field>();

        foreach(Field X in array)
        {
            if (X.FieldType.CountryColor == fieldType.CountryColor)
            {
                if (X.WhatIsTheOwnerName().Equals(AskingPlayer))
                {
                    CurrentNumber++;
                }
            }
        }

        //Debug.Log("Posiadam aktualnie: " + CurrentNumber + " na " + TargetNumber);

        if (CurrentNumber == TargetNumber)
            return true;

        else return false;
    }

    public void DestroyHousesIfPlayerSellField(FieldType fieldType, string AskingPlayer, Pawn PlayerWhoHasEntered)
    {
        var array = FindObjectsOfType<Field>();

        foreach (Field X in array)
        {
            if (X.FieldType.CountryColor == fieldType.CountryColor)
            {
                if (X.WhatIsTheOwnerName().Equals(AskingPlayer))
                {
                    //Debug.Log("Indexy pól: " + X.GetMyIndex());
                    X.DestroyAllHousesThatFieldHasV2(this);
                    PlayerWhoHasEntered.MyMoney += X.WorthOfHouses();
                }
            }
        }
    }

}
