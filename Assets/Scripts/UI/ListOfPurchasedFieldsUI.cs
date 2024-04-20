using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListOfPurchasedFieldsUI : MonoBehaviour {

    [SerializeField]
    GameObject SellingFieldUIPrefab;

	public void Configure(List<Field> MyFields, Pawn pawn)
    {
        foreach(Field F in MyFields)
        {
            var obj = Instantiate(SellingFieldUIPrefab);
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(-10, 0, 0);
            obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            obj.GetComponent<FieldSellingUI>().Configure(F, pawn);
            Refresh();
        }
    }

    public void Refresh()
    {
        var array = FindObjectsOfType<FieldSellingUI>();

        for(int i = 0; i < array.Count(); i++)
        {
            array[i].transform.localPosition = new Vector3(-10, (-30 * i), 0);
        }
    }
}
