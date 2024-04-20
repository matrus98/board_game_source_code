using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AuctionConfiguringUI : MonoBehaviour {

    Field Field;
    Pawn Pawn;

    private void Update()
    {
        if (FindObjectsOfType<ListOfPurchasedFieldsUI>().Count() <= 0)
            DestroyMe();
    }

    public void Configure(Field field, Pawn pawn)
    {
        Field = field;
        Pawn = pawn;
    }

    public void AddFieldToAuction()
    {
        int price;
        var Price = GetComponentsInChildren<Text>().Last().text;
        bool succeed = int.TryParse(Price, out price);

        if (!succeed)
        {
            return;
        }
        else
        {
            FindObjectOfType<GameManager>().AddNewFieldToAuction(Field, Pawn, price);
            Destroy(gameObject);
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
