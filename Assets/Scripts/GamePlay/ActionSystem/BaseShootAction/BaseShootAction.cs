using System.Collections.Generic;
using GamePlay.GridSystem;
using GridSystem;
using UnityEngine;

namespace GamePlay.ActionSystem.BaseShootAction
{
    public abstract class BaseShootAction : BaseAction.BaseAction
    {
        [SerializeField] protected int shootRange = 3; // Default shoot range
    
      //  public override int GetActionRange() => shootRange;
    
        protected virtual bool IsValidTarget(GridPosition targetGridPosition)
        {
            // Basic validation that can be overridden
            GridPosition unitGridPosition = Unit.GetGridPosition();
            int testDistance = Mathf.Abs(targetGridPosition.x - unitGridPosition.x) + 
                               Mathf.Abs(targetGridPosition.z - unitGridPosition.z);
        
            return testDistance <= shootRange;
        }
    
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositions = new List<GridPosition>();
            GridPosition unitGridPosition = Unit.GetGridPosition();
        
            for (int x = -shootRange; x <= shootRange; x++)
            {
                for (int z = -shootRange; z <= shootRange; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                    if (!LevelGrid.Instance.IsGridPositionValid(testGridPosition))
                        continue;
                    
                    if (!IsValidTarget(testGridPosition))
                        continue;
                    
                    if (LevelGrid.Instance.HasAnyUnitAtGridPosition(testGridPosition))
                    {
                        validGridPositions.Add(testGridPosition);
                    }
                }
            }
        
            return validGridPositions;
        }
    
        public override string GetActionName() => "Shoot";
    }
}
