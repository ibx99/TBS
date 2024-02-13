using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{

    public static UnitActionSystem Instance { get; private set; }
    public EventHandler OnSelectedUnitChanged;
    public EventHandler OnSelectedActionChanged;
    public EventHandler<bool> OnBusyChanged;
    public EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    private bool isBusy = false;
    private BaseAction selectedAction;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance " + transform + " from " + Instance);
            Destroy(Instance.gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }


    private void Update()
    {
        // when already in action
        if (isBusy) return;

        // when when the mouse is on a ui element
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) 
        {
            return;
        }
        HandleSelectedAction();
    }


    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButton(0))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out RaycastHit hit, float.MaxValue, unitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    // already selected unit
                    if (unit == selectedUnit)
                        return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }


    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);            
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }


    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }


    public Unit GetSelectedUnit() => selectedUnit;


    public void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, true);
    } 


    public void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, false);
    }

    public BaseAction GetSelectedAction() => selectedAction;
}
