using System;
using System.Collections.Generic;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.GridSystem;
using GridSystem;
using UnityEngine;

namespace NewInputSystem.ActionSystem.MoveAction
{
    public class MoveAction : BaseAction
    {
        [SerializeField] private int maxMoveDistance = 4;
        
        private const string ActionName = "Move";
        private Vector3 _targetPosition;
        private float _stoppingDistance;
        [SerializeField] private Sprite actionIcon;
        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;

        protected override void Awake()
        {
            base.Awake();
            _targetPosition = transform.position;
        }

        public override string GetActionName()
        {
            return ActionName;
            ;
        }

        public override Sprite GetActionIcon()
        {
            return actionIcon;
        }


        void Update()
        {
            if (!IsActive)
            {
                return;
            }

            float moveSpeed = 4f;
            _stoppingDistance = .1f;
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
            {
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            }
            else
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }

        // old moveUnit function
        public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
        {
            OnStartMoving?.Invoke(this, EventArgs.Empty);
            ActionStart(onActionComplete);
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        }

        public override int GetActionPointsCost()
        {
            return 1;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validActionGridPositions = new List<GridPosition>();

            GridPosition unitGridPosition = Unit.GetGridPosition();

            for (int X = -maxMoveDistance; X <= maxMoveDistance; X++)
            {
                for (int Z = -maxMoveDistance; Z <= maxMoveDistance; Z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(X, Z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsGridPositionValid(testGridPosition))
                    {
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        // this is where unit is at
                        continue;
                    }

                    if (LevelGrid.Instance.HasAnyUnitAtGridPosition(testGridPosition))
                    {
                        //grid position is occupied with another unit
                        continue;
                    }

                    validActionGridPositions.Add(testGridPosition);
                }
            }

            return validActionGridPositions;
        }


    }
}