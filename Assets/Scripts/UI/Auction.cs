using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Auction : MonoBehaviour {

    [SerializeField]
    GameObject PurchasingFieldUIPrefab;

    public void Configure(List<AuctionStockBroker> AuctionFields)
    {
        foreach (AuctionStockBroker ASB in AuctionFields)
        {
            var obj = Instantiate(PurchasingFieldUIPrefab);
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            obj.GetComponent<FieldPurchasingUI>().Configure(ASB);
            Refresh();
        }
    }

    public void Refresh()
    {
        var array = FindObjectsOfType<FieldPurchasingUI>();

        for (int i = 0; i < array.Count(); i++)
        {
            array[i].transform.localPosition = new Vector3(0, (-30 * i), 0);
        }
    }
}
