using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject cursor;
    public GameObject circle;
    public GameObject cube;
    public GameObject cone;

    GameObject area;
    ARRaycastManager arOrigin;
    Pose placement;
    bool placementPoseIsValid = false;
    bool isActive = false;

    void Start()
    {
        area = circle;
        arOrigin = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacement();
        UpdateCursor();
        if (placementPoseIsValid
        && Input.touchCount > 0
        && Input.GetTouch(0).phase == TouchPhase.Began
        && EventSystem.current.currentSelectedGameObject == null)
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        area.transform.position = placement.position;
        if (area.name == "Cube Container")
        {
            area.transform.Translate(Vector3.left * .075f);
        }
        area.transform.rotation = placement.rotation;
        if (area.name == "Cone Container")
        {
            area.transform.Translate(Vector3.down * .15f);
            var rot = Quaternion.Euler(90f, 0f, 180f) * placement.rotation.eulerAngles;
            area.transform.RotateAround(placement.position, rot, 90);
        }
        if (!area.activeSelf)
        {
            area.SetActive(true);
        }
        if (!isActive)
        {
            isActive = true;
        }
    }

    void UpdateCursor()
    {
        if (placementPoseIsValid)
        {
            cursor.SetActive(true);
            cursor.transform.SetPositionAndRotation(placement.position, placement.rotation);
        }
        else
        {
            cursor.SetActive(false);
        }
    }

    void UpdatePlacement()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, .5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placement = hits[0].pose;
            var camForward = Camera.current.transform.forward;
            var camBearing = new Vector3(camForward.x, 0, camForward.z).normalized;
            placement.rotation = Quaternion.LookRotation(camBearing);
        }
    }

    public void SetAreaType(string type)
    {
        cube.SetActive(false);
        circle.SetActive(false);
        cone.SetActive(false);

        if (type == "cube")
        {
            area = cube;
        }
        else if (type == "circle")
        {
            area = circle;
        }
        else if (type == "cone")
        {
            area = cone;
        }

        if (isActive)
        {
            PlaceObject();
        }
    }
}
