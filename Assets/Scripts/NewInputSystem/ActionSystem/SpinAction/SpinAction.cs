using System;
using System.Collections.Generic;
using GridSystem;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace NewInputSystem.ActionSystem.SpinAction
{
    public class SpinAction : BaseAction.BaseAction
    {
        private float _totalSpinAmount;
        private const string ActionName = "Spin";
        
        [SerializeField] private Sprite actionIcon;


        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            _totalSpinAmount += spinAddAmount;
            if (_totalSpinAmount >= 360f)
            {
                ActionComplete();
            }
        }

        //old Spin Function 
        public override Sprite GetActionIcon()
        {
            return actionIcon;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            ActionStart(onActionComplete);
            _totalSpinAmount = 0;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {

            GridPosition unitGridPosition = Unit.GetGridPosition();
            
            return new List<GridPosition> { unitGridPosition };
        }

        public override string GetActionName()
        {
            return ActionName;
        }

        public override int GetActionPointsCost()
        {
            return 2;
        }
    }
}