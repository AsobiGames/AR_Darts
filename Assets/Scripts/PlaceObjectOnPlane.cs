using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private Pose placementPose;
    private Transform placementTransform;
    private bool placementPoseIsValid = false;
    private bool isObjectPlaced = false;
    private TrackableId placePlaneId = TrackableId.invalidId;

    ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public static event Action onObjectPlaced;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!isObjectPlaced)
        {
            UpdatePlacementPosition();
            UpdatePlacementIndicator();

            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

  

    private void UpdatePlacementIndicator()
    {
        var screenCentre = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (raycastManager.Raycast(screenCentre, hits, TrackableType.PlaneWithinPolygon)) 
        {
            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid) 
            {
                placementPose = hits[0].pose;
                placePlaneId = hits[0].trackableId;

                var planeManager = GetComponent<ARPlaneManager>();
                ARPlane arPlane = planeManager.GetPlane(placePlaneId);
                placementTransform = arPlane.transform;

            }
        }
    }

    private void UpdatePlacementPosition()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementTransform.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementTransform.rotation);
        onObjectPlaced?.Invoke();
        isObjectPlaced = true;
        placementIndicator.SetActive(false);
    }
}
