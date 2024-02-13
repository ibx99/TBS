using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    GridObject gridObject;

    public void SetGridObject(GridObject go)
    {
        gridObject = go;
    }

    private void Update()
    {
        GetComponentInChildren<TextMeshPro>().text
            = gridObject.ToString();
    }
}
