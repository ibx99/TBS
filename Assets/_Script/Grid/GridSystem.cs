using System;
using UnityEngine;

public class GridSystem 
{
    private int height, width;
    private float cellSize;
    GridObject[,] gridObjectArray;

    public GridSystem(int height, int width, float cellSize)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPos = new GridPosition(x, z);
                GridObject gridObject = new GridObject(gridPos, this);
                gridObjectArray[x, z] = gridObject;
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPos)
    {
        return new Vector3(gridPos.x, 0, gridPos.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugObject, Transform parent)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GridPosition gridPos = new GridPosition(i, j);
                Transform gridDebugObjectTransform = GameObject.Instantiate(debugObject, 
                    GetWorldPosition(gridPos), Quaternion.identity, parent);
                gridDebugObjectTransform.GetComponent<GridDebugObject>().SetGridObject(GetGridObject(gridPos));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPos)
    {
        return gridObjectArray[gridPos.x, gridPos.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return (gridPosition.x >= 0 && gridPosition.z >= 0 &&
                gridPosition.x < height && gridPosition.z < width);
    }

    internal int GetHeight() => this.height;

    internal int GetWidth() => this.width;
}
