using System;
using System.Collections.Generic;
using GridSystem;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace NewInputSystem.ActionSystem.BaseAction
{
    // you can use GetActionCost fun to define a cost for actions
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

        public abstract int GetActionPointsCost();

        protected void ActionStart(Action OnActionComplete)
        {
            IsActive = true;
            this.OnActionComplete = OnActionComplete;
        }

        protected void ActionComplete()
        {
            IsActive = false;
            OnActionComplete();
        }
           
        

        public virtual int GetActionNameFontSize()
        {
            return 0;
        }
    }
}
