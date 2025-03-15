using System;
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
        public abstract string GetActionName();
    }
}
