using System;
using System.Collections.Generic;
using GridSystem;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace NewInputSystem.ActionSystem.BaseAction
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit.Unit Unit;
        protected bool IsActive;
        protected Action OnActionComplete;
        

        protected virtual void Awake()
        {
            Unit = GetComponentInParent<Unit.Unit>();
        }

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }
        public abstract List<GridPosition> GetValidActionGridPositionList();
        public abstract string GetActionName();

        public abstract Sprite GetActionIcon();
        public abstract void TakeAction(GridPosition gridPosition, Action OnActionComplete);
    }
}
