using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject AuctionUIPrefab;
    GameObject TemporaryAuctionUIPrefab;
    private bool IsAuctionDisplayed = false;
    List<AuctionStockBroker> AuctionFields = new List<AuctionStockBroker>();
    
    [SerializeField]
    GameObject CrisisUIPrefab;

    [SerializeField]
    GameObject CrisisCardUIPrefab;

    [SerializeField]
    GameObject WinUIPrefab;

    [SerializeField]
    List<Pawn> Players = new List<Pawn>();

    public event Action<Pawn> OnPlayerAdded;
    public event Action<string> OnPlayerTurnChanged;

    public void GiveFirstTurn()
    {
        Players[0].IsMyTurn = true;
    }

    public void AddPlayerPawn(Pawn pawn)
    {
        pawn.IsMyTurn = false;

        if (Players.Count() <= 0)
        {
            pawn.IsMyTurn = false;

            if (OnPlayerTurnChanged != null)
                OnPlayerTurnChanged.Invoke(pawn.MyName);
        }
            
        Players.Add(pawn);
        //Debug.Log("New Player Added: " + pawn.MyName);
        if (OnPlayerAdded != null)
            OnPlayerAdded.Invoke(pawn);
    }

    public void EndTurnOfPlayer()
    {
        foreach(Field F in FindObjectsOfType<Field>())
        {
            F.OnTurnEndDisableShowingInfo();
        }

        foreach(Pawn P in FindObjectsOfType<Pawn>())
        {
            if (P.IsListShowed)
                P.DestroyTemporaryListOfPurchasedFieldUI();
        }

        bool foundNext = false;
        foreach(Pawn P in Players)
        {
            if (foundNext)
            {
                P.SetTurnToPlayer();
                FindObjectOfType<DiceButton>().DisableButton(false);

                if (OnPlayerTurnChanged != null)
                    OnPlayerTurnChanged.Invoke(P.MyName);

                return;
            }
            if (P.IsMyTurn)
            {
                P.IsMyTurn = false;
                if(P == Players.Last())
                {
                    Players.First().SetTurnToPlayer();
                    FindObjectOfType<DiceButton>().DisableButton(false);

                    if (OnPlayerTurnChanged != null)
                        OnPlayerTurnChanged.Invoke(Players.First().MyName);

                    return;
                }
                else foundNext = true;
            }
        }
    }

    public Pawn WhoseTurn()
    {
        var array = FindObjectsOfType<Pawn>();

        foreach(Pawn P in array)
        {
            if (P.IsMyTurn)
                return P;
        }

        return null;
    }

    public void DisplayListOnButtonClick()
    {
        foreach(Pawn P in Players)
        {
            if (P.IsMyTurn)
            {
                P.DisplayListForGameManager();
            }
        }
    }

    public void RollDicesOnButtonClick()
    {
        FindObjectOfType<CreateDices>().RollDices();
        FindObjectOfType<DiceButton>().DisableButton(true);
    }

    public void DisplayAuctionStock()
    {
        if (!IsAuctionDisplayed)
        {
            IsAuctionDisplayed = true;
            TemporaryAuctionUIPrefab = Instantiate(AuctionUIPrefab);
            TemporaryAuctionUIPrefab.GetComponentInChildren<Auction>().Configure(AuctionFields);
        }
        else
        {
            Destroy(TemporaryAuctionUIPrefab);
            IsAuctionDisplayed = false;
        }
    }

    public void AddNewFieldToAuction(Field field, Pawn pawn, int price)
    {
        AuctionFields.Add(
            new AuctionStockBroker()
            {
                Field = field,
                Pawn = pawn,
                Price = price
            }
        );

        if (IsAuctionDisplayed)
        {
            RefreshAuctionInterface();
        }
    }

    public void RemoveFromAuction(Field field, Pawn pawn)
    {
        foreach (AuctionStockBroker ASB in AuctionFields)
        {
            if (ASB.Field == field && ASB.Pawn == pawn)
            {
                AuctionFields.Remove(ASB);
                break;
            }
        }
        if (IsAuctionDisplayed)
            RefreshAuctionInterface();
    }

    public void RemoveFieldFromAutionIfWasExhibited(Field field)
    {
        foreach(AuctionStockBroker ASB in AuctionFields)
        {
            if(ASB.Field == field)
            {
                AuctionFields.Remove(ASB);
                return;
            }
        }
    }
    
    private void RefreshAuctionInterface()
    {
        Destroy(TemporaryAuctionUIPrefab);
        TemporaryAuctionUIPrefab = Instantiate(AuctionUIPrefab);
        TemporaryAuctionUIPrefab.GetComponentInChildren<Auction>().Configure(AuctionFields);
    }

    public GameObject GetCrisisUI()
    {
        return CrisisUIPrefab;
    }

    public GameObject GetCardCrisisUI()
    {
        return CrisisCardUIPrefab;
    }

    public void DestroyPawnIfPlayerIsBroke()
    {
        var pawn = WhoseTurn();
        EndTurnOfPlayer();
        Players.Remove(pawn);
        Destroy(pawn.gameObject);
        //Debug.Log("Player " + pawn.MyName + " Destroyed");

        var PlayersWhoLeft = Players.Count();
        //Debug.Log("Players Who Left: " + PlayersWhoLeft);

        if(PlayersWhoLeft <= 1)
        {
            //Debug.Log("Win of " + Players[0].MyName);
            var canva = Instantiate(WinUIPrefab);
            canva.GetComponent<WinUI>().Configure(Players[0].MyName);
        }
    }

    /*public GameObject MatchMyMaterialToMyPawnColor(Material material)
    {
        for (int i = 0; i < PawnPrefabs.Length; i++)
        {
            if (material == PawnPrefabs[i].Material)
            {
                return PawnPrefabs[i].PawnPrefab;
            }
        }

        return null;
    }

    /*[Command]
    public void CmdSetMaterialForEntirePawn(GameObject gameObjectPawn)
    {
        MeshRenderer[] Components = gameObjectPawn.GetComponentsInChildren<MeshRenderer>();
        Material PawnMaterial = FindObjectOfType<Player>().PlayerMaterial;

        for (int i = 0; i < Components.Length; i++)
        {
            Components[i].material = PawnMaterial;
        }

        //RpcSetMaterialForEntirePawn(gameObjectPawn, PawnMaterial);
    }*/

    /*[ClientRpc]
    private void RpcSetMaterialForEntirePawn(GameObject gameObjectPawn, Material PawnMaterial)
    {
        MeshRenderer[] Components = gameObjectPawn.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < Components.Length; i++)
        {
            Components[i].material = PawnMaterial;
        }
    }*/
}
