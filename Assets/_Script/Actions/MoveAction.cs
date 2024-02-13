using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private int maxMoveDistance;
    [SerializeField] private Animator animator;
    private int walkingAnimationID = 0;
    Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    void Start()
    {
        walkingAnimationID = Animator.StringToHash("isWalking");
    }


    void Update()
    {
        if (!isActive) return;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float stoppingDistance = 0.1f;

        if (DistanceToTarget(targetPosition) > stoppingDistance)
        {
            // direction of travel
            float moveSpeed = 4.0f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            animator.SetBool(walkingAnimationID, true);
        }
        else
        {
            animator.SetBool(walkingAnimationID, false);
            isActive = false;
            onActionComplete();
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

    }


    public override void TakeAction(GridPosition mouseGridPosition, Action moveComplete)
    {
        this.onActionComplete = moveComplete;
        isActive = true;
        targetPosition = LevelGrid.Instance.GetWorldPosition(mouseGridPosition);
    }


    public override List<GridPosition> GetValidatedGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPos = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {       
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPos;

                // whether the grid pos is out of bounds
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                // whether the grid pos is the same as the one player is on
                if (unitGridPos == testGridPosition) continue;
                
                // whether the grid position is occupied
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    // since this method resides on move action
    public override string GetActionName() => "Move";


    #region Helper Methods

    private float DistanceToTarget(Vector3 target) => Vector3.Distance(transform.position, target);

    #endregion
}
