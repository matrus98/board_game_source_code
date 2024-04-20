using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOnStart : MonoBehaviour {

    [SerializeField]
    float EffectDuration = 2f;

    [SerializeField]
    string Destription;

    void Start () {
        StartCoroutine(TextEffectCoroutine());
    }

    IEnumerator TextEffectCoroutine()
    {
        var text = GetComponent<Text>();
        yield return new WaitForEndOfFrame();
        text.enabled = true;
        text.text = Destription;

        while (text.color.a <= 1f)
        {
            text.color += new Color(0f, 0f, 0f, 1f) / EffectDuration * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        while (text.color.a >= 0)
        {
            text.color -= new Color(0f, 0f, 0f, 1f) / EffectDuration * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        text.enabled = false;
        FindObjectOfType<EffectOnStart>().textScaling = false;
    }
}
