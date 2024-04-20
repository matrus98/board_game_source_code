using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectOnStart : MonoBehaviour {

    [SerializeField]
    public float EffectDuration = 2f;

    [SerializeField]
    Vector3 ScaleOfImage = new Vector3(10f, 8f, 0.1f);

    public bool textScaling = true;

	void Start () {
        StartCoroutine(FadeOutEffectCoroutine());
	}

	IEnumerator FadeOutEffectCoroutine()
    {
        var NetworkManager = FindObjectOfType<FindMe>();
        NetworkManager.gameObject.SetActive(false);

        Image image = gameObject.AddComponent<Image>();
        image.enabled = true;
        image.color = new Color(0, 0, 0, 1f);
        image.GetComponent<RectTransform>().localScale = ScaleOfImage;

        while (textScaling)
        {
            yield return new WaitForEndOfFrame();
        }

        while(image.color.a >= 0)
        {
            image.color -= new Color(0f, 0f, 0f, 1f) / EffectDuration * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
        NetworkManager.gameObject.SetActive(true);
    }
}
