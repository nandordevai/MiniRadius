using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameObject op;

    public void OnClick(string area)
    {
        op.GetComponent<ObjectPlacer>().SetAreaType(area);
    }
}
