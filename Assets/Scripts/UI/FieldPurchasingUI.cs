using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldPurchasingUI : MonoBehaviour {

    private int FinalPrice;
    AuctionStockBroker AuctionStockBroker;

    private void Update()
    {
        FinalPrice = AuctionStockBroker.Price;

        if (AuctionStockBroker.Field.IsUnderMortgage)
            FinalPrice -= AuctionStockBroker.Field.GetMortgageValue();

        var array = GetComponentsInChildren<Text>();
        array.Last().text = string.Format("Repurchase {0}$", FinalPrice);
    }

    public void Configure(AuctionStockBroker auctionStockBroker)
    {
        AuctionStockBroker = auctionStockBroker;

        var array = GetComponentsInChildren<Text>();

        array[0].text = auctionStockBroker.Field.FieldType.Description;
        array[1].text = auctionStockBroker.Pawn.MyName;

        FinalPrice = auctionStockBroker.Price;
    }

    public void RepurchaseThisField()
    {
        var Player = FindObjectOfType<GameManager>().WhoseTurn();

        if (Player.Equals(AuctionStockBroker.Pawn))
            return;

        if (Player.MyMoney < FinalPrice)
            return;

        Player.MyMoney -= FinalPrice;
        AuctionStockBroker.Pawn.MyMoney += FinalPrice;

        AuctionStockBroker.Field.FieldType.BelongingColor = Player.PawnMaterial;
        AuctionStockBroker.Field.GetComponentInChildren<PlayerBelongingColor>().Configure(AuctionStockBroker.Field.FieldType.BelongingColor);
        AuctionStockBroker.Field.ChangeOwner(Player.MyName);
        AuctionStockBroker.Field.ChangeOwnerObject(Player);

        Player.AddPurchasedField(AuctionStockBroker.Field);

        if (AuctionStockBroker.Field.IsUnderMortgage)
            AuctionStockBroker.Field.ReedemMortgage();

        AuctionStockBroker.Field.IsExhibited = false;

        AuctionStockBroker.Field.OnRepurchasingField(Player.MyName, AuctionStockBroker.Pawn.MyName, FinalPrice);

        AuctionStockBroker.Pawn.RemoveSelledField(AuctionStockBroker.Field);
        FindObjectOfType<GameManager>().RemoveFromAuction(AuctionStockBroker.Field, AuctionStockBroker.Pawn);
    }
}
