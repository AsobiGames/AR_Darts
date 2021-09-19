using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ARPlaneController : MonoBehaviour
{
    ARPlaneManager planeManager;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        PlaceObjectOnPlane.onObjectPlaced += DisablePlaneDetection;
    }
    void OnDisable()
    {
        PlaceObjectOnPlane.onObjectPlaced -= DisablePlaneDetection;
    }

    private void DisablePlaneDetection()
    {
        //planedetectionMessage = "Disable Plane Detection and Hide Existing"
        SetAllPlanesActive(false);
        planeManager.enabled = !planeManager.enabled;
    }

    ///<summary>
    ///Iterates over all the existing planes and activates
    ///or dectivates their <c>GameObject</c>s'.
    ///</summary>

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
