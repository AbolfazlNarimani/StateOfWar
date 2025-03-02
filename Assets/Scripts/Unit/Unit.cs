using System;
using NewInputSystem;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 _targetPosition;
    private float _stoppingDistance;
   

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
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
    }

    private void MoveUnit(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}