using System;
using GridSystem;
using NewInputSystem;
using NewInputSystem.BaseActions;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        private GridPosition _gridPosition;
        private MoveAction _moveAction;

        private void Awake()
        {
            _moveAction = GetComponent<MoveAction>();
        }


        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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

        public MoveAction GetMoveAction() => _moveAction;
        public GridPosition GetGridPosition() => _gridPosition;
    }
}