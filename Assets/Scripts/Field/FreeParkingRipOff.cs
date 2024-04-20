using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeParkingRipOff : MonoBehaviour {

    private Pawn RipOffOnwer;
    private int RipOffValue;
    private int IndexOfThisField = 18;
    private bool IsRipOffSetFlague = false;

    public event Action<string, int, string> OnFreeParkingRipOff;

    private void Awake()
    {
        OnFreeParkingRipOff += FindObjectOfType<AnnounmentCanva>().OnFreeParkingRipOff;
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<Pawn>();

        if (obj == null)
            return;

        if(IsRipOffSetFlague)
            StartCoroutine(FeeParking(obj));
    }

    private IEnumerator FeeParking(Pawn Player)
    {
        yield return new WaitForSeconds(0.5f);

        if (!Player.Equals(RipOffOnwer) && Player.IndexOfFieldIAmStayingOn == IndexOfThisField)
        {
            if (Player.HaveIEnoughtMoney(RipOffValue))
            {
                RipOffOnwer.MyMoney += RipOffValue;
                Player.MyMoney -= RipOffValue;

                if (OnFreeParkingRipOff != null)
                    OnFreeParkingRipOff.Invoke(RipOffOnwer.MyName, RipOffValue, Player.MyName);
            }
        }
    }

    public void SetRipOff(Pawn ripOffOwner, int ripOffValue)
    {
        RipOffOnwer = ripOffOwner;
        RipOffValue = ripOffValue;
        IsRipOffSetFlague = true;
    }

    public void UnsetRipOff()
    {
        RipOffOnwer = null;
        RipOffValue = 0;
        IsRipOffSetFlague = false;
    }

    public bool IsRipOffSet()
    {
        if (IsRipOffSetFlague)
            return true;

        else return false;
    }
}
