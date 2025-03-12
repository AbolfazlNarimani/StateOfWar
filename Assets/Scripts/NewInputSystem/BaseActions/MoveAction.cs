using System;
using System.Collections.Generic;
using GridSystem;
using NUnit.Framework;
using UnityEngine;

namespace NewInputSystem.BaseActions
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private int maxMoveDistance = 4;
        private const string IS_MOVING = "IsMoving";
        private Vector3 _targetPosition;
        private float _stoppingDistance;
        private Unit.Unit _unit;
        [SerializeField] private Animator animator;


        private void Awake()
        {
            _unit = GetComponentInParent<Unit.Unit>();
            _targetPosition = transform.position;
        }


        void Update()
        {
            float moveSpeed = 4f;
            _stoppingDistance = .1f;

            if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
            {
                Vector3 moveDirection = (_targetPosition - transform.position).normalized;
                animator.SetBool(IS_MOVING, true);
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(IS_MOVING, false);
            }
        }

        public void MoveUnit(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        public List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validActionGridPositions = new List<GridPosition>();
            
            GridPosition unitGridPosition = _unit.GetGridPosition();

            for (int X = -maxMoveDistance ; X <= maxMoveDistance ; X++)
            {
                for (int Z = -maxMoveDistance; Z < maxMoveDistance; Z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(X,Z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                }
            }
            
            
            
            
            return validActionGridPositions;
        }
    }
}