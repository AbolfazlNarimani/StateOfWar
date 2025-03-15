using System;
using UnityEngine;

namespace NewInputSystem.ActionSystem.SpinAction
{
    public class SpinAction : BaseAction.BaseAction
    {
        
        private float _totalSpinAmount;
        private const string ActionName = "Spin";

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
                IsActive = false;
                OnActionComplete?.Invoke();
            }
        }

        public void Spin(Action onActionComplete)
        {
            this.OnActionComplete = onActionComplete;
            IsActive = true;
            _totalSpinAmount = 0;
        }

        public override string GetActionName()
        {
            return ActionName;
        }
    }
}