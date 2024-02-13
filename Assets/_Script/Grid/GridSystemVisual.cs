using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance {get; private set;}

    [SerializeField] private Transform gridSystemVisualSingle;
    [SerializeField] private Transform gridVisualSingleParent;

    GridSystemVisualSingle[,] gridSystemVisualSinglesArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance: " + transform + " - " + this.gameObject);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Start()
    {
        gridSystemVisualSinglesArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetGridWidth(),
            LevelGrid.Instance.GetGridHeight()
        ];

        for (int i = 0; i < LevelGrid.Instance.GetGridWidth(); i++)
        {
            for (int j = 0; j < LevelGrid.Instance.GetGridHeight(); j++)
            {
                GridPosition gridPostion = new GridPosition(i, j);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSingle, 
                            LevelGrid.Instance.GetWorldPosition(gridPostion), 
                            Quaternion.identity, 
                            gridVisualSingleParent);
                
                gridSystemVisualSinglesArray[i, j] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        for (int i = 0; i < LevelGrid.Instance.GetGridWidth(); i++)
        {
            for (int j = 0; j < LevelGrid.Instance.GetGridHeight(); j++)
            {
                gridSystemVisualSinglesArray[i, j].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSinglesArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        HideAllGridPositions();
        ShowGridPositionList(selectedAction.GetValidatedGridPositionList());
    }
}