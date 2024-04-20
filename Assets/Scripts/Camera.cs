using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour {

    [SerializeField]
    Transform ObjectToTrack;
    Transform PawnToTrack;

    [SerializeField]
    GameObject ReturnCameraUIPrefab;
    private bool IsSetTrackingOnPawn = false;

    [SerializeField]
    Vector3 Delta = new Vector3(0, 10, 10);

    [Range(1,10)]
    [SerializeField]
    float SensitiveOfCamera = 3f;

    private float axisX;
    private float axisY;
    private float axisScrollWheel; // axis Z

    private void Awake()
    {
        FindObjectOfType<GameManager>().OnPlayerTurnChanged += Name =>
        {
            if (IsSetTrackingOnPawn)
            {
                PawnToTrack = FindObjectOfType<GameManager>().WhoseTurn().transform;
            }
        };
    }

    void Start () {
		
	}

	void Update ()
    {
        if (!IsSetTrackingOnPawn)
            LookAtMiddleOfTheBoard();
        else TrackPawn();

    }

    private void UpdateDelta()
    {
        if (IsSetTrackingOnPawn)
        {
            Delta.y = 3;
            Delta.z = 3;
        }
    }

    public void SetDefaultPosition()
    {
        IsSetTrackingOnPawn = false;
    }

    public void SetPawnAsObjectToTrack(Transform pawn)
    {
        if(FindObjectsOfType<CameraUI>().Count() <= 0)
            Instantiate(ReturnCameraUIPrefab);

        PawnToTrack = pawn;
        IsSetTrackingOnPawn = true;
    }


    private void TrackPawn()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateAxises();
            UpdatePosition(PawnToTrack);
        }
        MoveTowardAndAway(PawnToTrack);
    }

    private void LookAtMiddleOfTheBoard()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateAxises();
            UpdatePosition(ObjectToTrack);
        }
        MoveTowardAndAway(ObjectToTrack);
    }

    /// <summary>
    /// Pobiera Informacje o ruchach myszki i zapisuje na osiach
    /// </summary>
    private void UpdateAxises()
    {
        axisX += Input.GetAxis("Mouse X") * SensitiveOfCamera;
        axisY += Input.GetAxis("Mouse Y") * SensitiveOfCamera;

        axisY = Mathf.Clamp(axisY, -40, 40);
    }

    /// <summary>
    /// Aktualizuje pozycje kamery w zależności od ruchu myszki
    /// </summary>
    private void UpdatePosition(Transform ObjectITrack)
    {
        UpdateDelta();
        transform.position = ObjectITrack.position + Quaternion.Euler(-axisY, axisX, 0) * Delta;
        transform.LookAt(ObjectITrack);
    }

    private void MoveTowardAndAway(Transform ObjectITrack)
    {
        axisScrollWheel = (Input.GetAxis("Mouse ScrollWheel") * SensitiveOfCamera) * (-1);

        Delta.z += axisScrollWheel;
        Delta.y += axisScrollWheel;

        Delta.z = Mathf.Clamp(Delta.z, 3f, 10f);
        Delta.y = Mathf.Clamp(Delta.y, 3f, 10f);

        UpdatePosition(ObjectITrack);
    }

    public bool IsThisPawnTracked(Transform pawn)
    {
        if (PawnToTrack == pawn && IsSetTrackingOnPawn)
            return true;

        else return false;
    }
}
