using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnounmentCanva : MonoBehaviour {

    [SerializeField]
    Text AnnouncementText;

    [SerializeField]
    float AnnouncementDuration = 3f;

    private void Start()
    {
        AnnouncementText.color = new Color(0, 0, 0, 0);
    }

    public void OnRepurchasedFieldFunction(string PlayerWhoRepurchased, string FromWho, string Description, int price)
    {
        AnnouncementTextEffectIn();
        AnnouncementText.text = string.Format("Player {0} has repurchased {1} from {2} for {3}$!", PlayerWhoRepurchased, Description, FromWho, price);
        StartCoroutine(AnnouncementTextEffectOutCoroutine());
    }

    public void OnFieldPurchaseFunction(string WhoBuy, string WhatBuy)
    {
        AnnouncementTextEffectIn();
        AnnouncementText.text = string.Format("Player {0} has bought {1}!", WhoBuy, WhatBuy);
        StartCoroutine(AnnouncementTextEffectOutCoroutine());
    }

    public void OnSomsiadBuliFunction(string PlayerWhoPay, int HowMuch, string Owner)
    {
        AnnouncementTextEffectIn();
        AnnouncementText.text = string.Format("Player {0} pay {1}$ to {2} for entering on his field.", PlayerWhoPay, HowMuch, Owner);
        StartCoroutine(AnnouncementTextEffectOutCoroutine());
    }

    public void OnFreeParkingRipOff(string Victim, int ValueOfRipOff, string RipOffOwner)
    {
        AnnouncementTextEffectIn();
        AnnouncementText.text = string.Format("Player {0} has been ripped-off on {1} by {2}!", Victim, ValueOfRipOff, RipOffOwner);
        StartCoroutine(AnnouncementTextEffectOutCoroutine());
    }

    private void AnnouncementTextEffectIn()
    {
        AnnouncementText.color = new Color(0, 0, 0, 255);
    }

    IEnumerator AnnouncementTextEffectOutCoroutine()
    {
        yield return new WaitForSeconds(AnnouncementDuration);
        AnnouncementText.color = new Color(0, 0, 0, 0);
    }
}
