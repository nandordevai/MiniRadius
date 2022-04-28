using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    ObjectPlacer op;

    void Start()
    {
        op = GameObject.Find("Interaction").GetComponent<ObjectPlacer>();
    }

    public void OnCubeClick()
    {
        Debug.Log("cube clicked");
        op.SetAreaType("cube");
    }

    public void OnCircleClick()
    {
        Debug.Log("circle clicked");
        op.SetAreaType("circle");
    }
}
