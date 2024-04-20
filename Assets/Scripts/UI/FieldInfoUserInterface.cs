using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldInfoUserInterface : MonoBehaviour {

    private Field Field;

    [SerializeField]
    Text DesriptionText;

    [SerializeField]
    Text PriceText;

    [SerializeField]
    Text OwnerText;

    public void SetUserInterfaceDescriptionOfTheField(string Description, int Price, string owner, Field field)
    {
        DesriptionText.text = Description;
        PriceText.text = Price + " $";
        OwnerText.text = owner;

        Field = field;
    }

    public void PurchaseFieldIfEnoughtMoney()
    {
        Field.PurchaseByButtonClick();
    }

    public void DestroyCanvaOnButtonClick()
    {
        Field.DestroyTemporaryCanvaFunction();
    }

    public void CreateHouseOnButtonClick()
    {
        Field.CreateHouse();
    }

    public void SellFieldOnButtonClick()
    {
        Field.SellFieldByButtonClick();
    }
}
