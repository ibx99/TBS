using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float spinAmount;

    void Update()
    {
        if (!isActive) return;
        
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0f, spinAddAmount, 0f);
        spinAmount += spinAddAmount;
        
        if (spinAmount >= 360f) 
        {
            isActive = false; 
            onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        spinAmount = 0f;
        isActive = true;
    }

    public override List<GridPosition> GetValidatedGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>() { unitGridPosition };
    }

    public override int GetActionPointsCost() => 2;

    // since this resides on spin action
    public override string GetActionName() => "Spin";
}
