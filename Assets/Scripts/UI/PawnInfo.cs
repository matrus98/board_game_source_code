using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnInfo : MonoBehaviour {

    [SerializeField]
    Image PlayerImage;

    [SerializeField]
    Text PlayerName;

    [SerializeField]
    Text PlayerMoney;

    Pawn Player;

    public void Configure(Pawn Player)
    {
        //Debug.Log("Object PawnInfo Created");

        if(Player.PawnMaterial != null)
            PlayerImage.color = Player.PawnMaterial.color;

        PlayerName.text = Player.MyName;
        PlayerMoney.text = Player.MyMoney.ToString();

        Player.OnMoneyChanged += Money => PlayerMoney.text = Player.MyMoney.ToString();

        this.Player = Player;

        this.Player.OnPlayerBroke += () => GetComponent<Image>().color = Color.gray;
    }

    public void TrackThisPawn()
    {
        if (Player.Equals(null))
            return;

        FindObjectOfType<Camera>().SetPawnAsObjectToTrack(Player.transform);
    }
}
