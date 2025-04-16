using System;
using System.Collections;
using GamePlay.GridSystem;
using GamePlay.Health;
using GridSystem;
using NewInputSystem.ActionSystem.MoveAction;
using NewInputSystem.ActionSystem.SpinAction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay.Unit
{
    public class Unit : BaseUnit.BaseUnit
    {
        private SpinAction _spinAction;

        protected override void Awake()
        {
            base.Awake();
            _spinAction = GetComponent<SpinAction>();
        }

        public SpinAction GetSpinAction() => _spinAction;
        
        
    }
}