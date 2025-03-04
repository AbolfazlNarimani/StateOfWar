using System;
using NewInputSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        private const string IS_MOVING = "IsMoving";
        private Vector3 _targetPosition;
        private float _stoppingDistance;
        [FormerlySerializedAs("_animation")] [SerializeField] private Animator _animator;
   

        private void Start()
        {
            GameInput.Instance.OnMoveAction += OnMoveAction;
        }

        private void OnMoveAction(object sender, EventArgs e)
        {
            MoveUnit(MouseWorld.GetMouseWorldPosition());
        
        }

        void Update()
        {
            float moveSpeed = 4f;
            _stoppingDistance = .1f;

            if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
            {
                Vector3 moveDirection = (_targetPosition - transform.position).normalized;
                _animator.SetBool(IS_MOVING, true);
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
                float rotateSpeed = 10f;
                transform.forward =  Vector3.Lerp(transform.forward,moveDirection , rotateSpeed * Time.deltaTime);
            }
            else
            {
                _animator.SetBool(IS_MOVING, false);
            }
        }

        private void MoveUnit(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }
    }
}