using UnityEngine;
using TMPro;

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

    public void OnSelect(TMP_Dropdown item)
    {
        op.SetSize(item.value);
    }
}
