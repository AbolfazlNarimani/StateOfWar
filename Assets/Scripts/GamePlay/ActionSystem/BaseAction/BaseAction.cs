using System;
using System.Collections.Generic;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using UnityEngine;

namespace GamePlay.ActionSystem.BaseAction
{
    // you can use GetActionCost fun to define a cost for actions
    public abstract class BaseAction : MonoBehaviour
    {
        protected GamePlay.Unit.BaseUnit.BaseUnit Unit;
        protected bool IsActive;
        protected Action OnActionComplete;
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;
        

        protected virtual void Awake()
        {
            Unit = GetComponentInParent<GamePlay.Unit.BaseUnit.BaseUnit>();
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
            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
            this.OnActionComplete = OnActionComplete;
            
        }

        protected void ActionComplete()
        {
            IsActive = false;
            OnActionComplete();
            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public virtual int Damage()
        {
            return 0;
        }
        
        public virtual int GetActionNameFontSize()
        {
            return 0;
        }

        public BaseUnit GetUnit()
        {
            return Unit;
        }
    }
}
