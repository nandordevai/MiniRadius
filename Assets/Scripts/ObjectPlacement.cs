// https://www.youtube.com/watch?v=Ml2UakwRxjk&list=WL&index=8

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject cursor;
    public GameObject radius;
    private ARRaycastManager arOrigin;
    private Pose placement;
    private bool placementPoseIsValid = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacement();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        radius.transform.position = placement.position;
        radius.transform.rotation = placement.rotation;
        if (!radius.activeSelf) {
            radius.SetActive(true);
        }
        // Instantiate(radius, placement.position, placement.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid) {
            cursor.SetActive(true);
            cursor.transform.SetPositionAndRotation(placement.position, placement.rotation);
        } else {
            cursor.SetActive(false);
        }
    }

    private void UpdatePlacement()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, .5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid) {
            placement = hits[0].pose;
            var camForward = Camera.current.transform.forward;
            var camBearing = new Vector3(camForward.x, 0, camForward.z).normalized;
            placement.rotation = Quaternion.LookRotation(camBearing);
        }
    }
}
