using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject opo;
    ObjectPlacer op;

    void Start()
    {
        op = opo.GetComponent<ObjectPlacer>();
    }

    public void OnClick(string area)
    {
        op.SetAreaType(area);
    }

    public void OnValueChanged(float value)
    {
        op.SetSize((int)value);
    }
}
