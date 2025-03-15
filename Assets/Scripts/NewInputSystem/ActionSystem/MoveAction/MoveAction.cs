using System;
using System.Collections.Generic;
using GridSystem;
using UnityEngine;

namespace NewInputSystem.ActionSystem.MoveAction
{
    public class MoveAction : BaseAction.BaseAction
    {
        [SerializeField] private int maxMoveDistance = 4;
        private const string IsMoving = "IsMoving";
        private const string ActionName = "Move";
        private Vector3 _targetPosition;
        private float _stoppingDistance;
        [SerializeField] private Sprite actionIcon;
        [SerializeField] private Animator animator;

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
                animator.SetBool(IsMoving, true);
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(IsMoving, false);
                IsActive = false;
                OnActionComplete?.Invoke();
            }

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }

        // old moveUnit function
        public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
        {
            this.OnActionComplete = onActionComplete;
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
            IsActive = true;
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