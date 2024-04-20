using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUI : MonoBehaviour {

	public void SetDefaultPositionOfCamera()
    {
        FindObjectOfType<Camera>().SetDefaultPosition();
        Destroy(gameObject);
    }

}
