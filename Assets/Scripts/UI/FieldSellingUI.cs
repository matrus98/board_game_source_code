using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldSellingUI : MonoBehaviour {

    [SerializeField]
    GameObject AutionConfiguringUI;
    GameObject TemporaryAutionConfiguringUI;
    Field Field;
    Pawn Pawn;

	public void Configure(Field field, Pawn pawn)
    {
        var array = GetComponentsInChildren<Text>();

        Field = field;
        Pawn = pawn;

        array[0].text = field.FieldType.Description;
        array[1].text = "Mortgage " + GetMortgageValue(field) + "$";

        var ImageArray = GetComponentsInChildren<Image>();

        if (field.IsUnderMortgage)
        {
            ImageArray[0].color = Color.red;
        }

        if (field.IsExhibited)
        {
            ImageArray[1].color = Color.green;
        }
    }

    private int GetMortgageValue(Field field)
    {
        return field.FieldType.Price / 5;
    }

    public void SellFieldByListButton()
    {
        Field.SellFieldWhereEverAmI(Field, Pawn, GetMortgageValue(Field));
        Destroy(TemporaryAutionConfiguringUI);
        Destroy(gameObject);
        GetComponentInParent<ListOfPurchasedFieldsUI>().Refresh();
    }

    public void TakeOrReedemMortgageByListButton()
    {
        if (!Field.IsUnderMortgage)
        {
            Pawn.MyMoney += GetMortgageValue(Field);
            Field.SetUnderMortage();

            var array = GetComponentsInChildren<Image>();
            array[0].color = Color.red;
            //Debug.Log("Mortgage took on Field: " + Field.FieldType.Description);
        }
        else
        {
            Pawn.MyMoney -= GetMortgageValue(Field);
            Field.ReedemMortgage();

            var array = GetComponentsInChildren<Image>();
            array[0].color = Color.white;
            //Debug.Log("Mortgage reedemed on Field: " + Field.FieldType.Description);
        }
    }

    public void BuyHotelForField()
    {
        Field.CreateHouseWhereEverIAm(Pawn);
    }

    public void ExhibitFieldOnAuction()
    {
        if (!Field.IsExhibited)
        {
            TemporaryAutionConfiguringUI = Instantiate(AutionConfiguringUI);
            TemporaryAutionConfiguringUI.GetComponent<AuctionConfiguringUI>().Configure(Field, Pawn);
            Field.IsExhibited = true;

            var array = GetComponentsInChildren<Image>();
            array[1].color = Color.green;
        }
        else
        {
            var ACUI = FindObjectsOfType<AuctionConfiguringUI>();

            if (ACUI.Count() > 0)
            {
                ACUI[0].DestroyMe();
                var array = GetComponentsInChildren<Image>();
                array[1].color = Color.white;
                Field.IsExhibited = false;
            }
            else
            {
                Field.IsExhibited = false;

                var array = GetComponentsInChildren<Image>();
                array[1].color = Color.white;

                FindObjectOfType<GameManager>().RemoveFromAuction(Field, Pawn);
            }
        }
    }
}
