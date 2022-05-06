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
    float[] sizes;
    int selectedSizeItem = 2;

    void Start()
    {
        area = circle;
        arOrigin = FindObjectOfType<ARRaycastManager>();
        sizes = new float[] {
            10f,
            15f,
            30f,
            60f
        };
    }

    void Update()
    {
        UpdatePlacement();
        UpdateCursor();
        if (placementPoseIsValid
        && Input.touchCount > 0
        && Input.GetTouch(0).phase == TouchPhase.Began
        && (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null))
        {
            PlaceObject();
        }
    }

    Vector3 GetScale() {
        return Vector3.one * sizes[selectedSizeItem] / 30f;
    }

    void PlaceObject()
    {
        area.transform.localScale = GetScale();
        area.transform.position = placement.position;
        area.transform.rotation = placement.rotation;

        if (area.name == "Cube Container")
        {
            area.transform.Translate(Vector3.forward * sizes[selectedSizeItem] / 100 / 4);
        }
        else if (area.name == "Cone Container")
        {
            area.transform.Translate(Vector3.forward * sizes[selectedSizeItem] / 100 / 2);
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
        if (Camera.current == null) return;
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

    public void SetSize(int idx)
    {
        selectedSizeItem = idx;
        if (isActive)
        {
            PlaceObject();
        }
    }
}
