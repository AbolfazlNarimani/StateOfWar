using System;
using System.Reflection;
using GridSystem;
using NewInputSystem;
using NewInputSystem.ActionSystem.BaseAction;
using NewInputSystem.ActionSystem.MoveAction;
using NewInputSystem.ActionSystem.SpinAction;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        private GridPosition _gridPosition;
        private MoveAction _moveAction;
        private SpinAction _spinAction;
        private BaseAction[] _baseActionsArray;
        [SerializeField] private int actionPoints;
        private int _defaultActionPoints;
        
        public static event EventHandler OnAnyActionPointsChanged;

        private void Awake()
        {
            _moveAction = GetComponent<MoveAction>();
            _spinAction = GetComponent<SpinAction>();
            _baseActionsArray = GetComponents<BaseAction>();
            _defaultActionPoints = actionPoints;
        }


        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnNumberChanged;
        }

        private void OnTurnNumberChanged(object sender, EventArgs e)
        {
            actionPoints = _defaultActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

            if (newGridPosition != _gridPosition)
            {
                //Unit Changed GridPosition
                LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
                _gridPosition = newGridPosition;
            }
        }

        private void SpendActionPoints(int amount)
        {
            actionPoints -= amount;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        
        private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
        {
            return actionPoints >= baseAction.GetActionPointsCost();
        }

        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (actionPoints >= baseAction.GetActionPointsCost())
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }
            return false;
        }

        public MoveAction GetMoveAction() => _moveAction;
        public SpinAction GetSpinAction() => _spinAction;
        public GridPosition GetGridPosition() => _gridPosition;
        public BaseAction[] GetBaseActionArray() => _baseActionsArray;
        
        public int GetRemainingActionPoints() => actionPoints;
    }
}