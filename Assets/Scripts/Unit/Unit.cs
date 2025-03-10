using System;
using GridSystem;
using NewInputSystem;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        private const string IS_MOVING = "IsMoving";
        private Vector3 _targetPosition;
        private float _stoppingDistance;
        [SerializeField] private Animator animator;
        
        
        private GridPosition _gridPosition;

        private void Awake()
        {
            _targetPosition = transform.position;
        }

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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

            GridPosition newGridPosition =LevelGrid.Instance.GetGridPosition(transform.position);
            
            if (newGridPosition != _gridPosition)
            {
                //Unit Changed GridPosition
                LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
                _gridPosition = newGridPosition;
            }
        }

        public void MoveUnit(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }
    }
}