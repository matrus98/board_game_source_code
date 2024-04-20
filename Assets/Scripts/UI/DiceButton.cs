using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceButton : MonoBehaviour {

    public void DisableButton(bool F)
    {
        GetComponent<Button>().enabled = !F;
        GetComponentInChildren<Image>().color = F ? Color.gray : Color.white;

        /*if (!F)
            Debug.Log("Enabled");
        else Debug.Log("Disabled");*/
    }
}
