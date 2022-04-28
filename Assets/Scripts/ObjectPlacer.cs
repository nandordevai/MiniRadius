// https://www.youtube.com/watch?v=Ml2UakwRxjk&list=WL&index=8

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject cursor;
    public GameObject circle;
    public GameObject cube;
    GameObject area;
    ARRaycastManager arOrigin;
    Pose placement;
    bool placementPoseIsValid = false;

    void Start()
    {
        area = circle;
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

    void PlaceObject()
    {
        area.transform.position = placement.position;
        area.transform.rotation = placement.rotation;
        if (!area.activeSelf) {
            area.SetActive(true);
        }
    }

    void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid) {
            cursor.SetActive(true);
            cursor.transform.SetPositionAndRotation(placement.position, placement.rotation);
        } else {
            cursor.SetActive(false);
        }
    }

    void UpdatePlacement()
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

    public void SetAreaType(string type)
    {
        if (area.activeSelf) {
            area.SetActive(false);
        }
        if (type == "cube") {
            area = cube;
        } else if (type == "circle") {
            area = circle;
        }
        if (area.activeSelf) {
            area.SetActive(true);
        }
    }
}
