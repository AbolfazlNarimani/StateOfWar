using System;
using System.Collections.Generic;
using GamePlay.GridSystem;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using UnityEngine;

namespace GamePlay.ActionSystem.BaseShootAction.ShootAction
{
    public class ShootAction : BaseAction.BaseAction
    {
        // Configuration - set these in inspector per unit type
        [SerializeField] protected int maxShootDistance = 7;
        [SerializeField] protected int damageAmount = 50;
        [SerializeField] protected int actionPointCost = 1;
        [SerializeField] private Sprite shootSprite;

        // Rest of your existing ShootAction implementation...

        private enum State
        {
            Aiming,
            Shooting,
            CoolOff,
        }

        private State _state;
        private float _stateTimer;
        private bool _canShootBullet;
        private const string ActionName = "Shoot";

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            _stateTimer -= Time.deltaTime;
            switch (_state)
            {
                case State.Aiming:

                    Vector3 targetPosition = _targetUnit.GetWorldPosition();
                    Vector3 unitPosition = Unit.GetWorldPosition();

                    // Calculate direction with height adjustment
                    Vector3 aimDirection = (targetPosition - unitPosition).normalized;
                    aimDirection.y = 0; // Optional: keep rotation horizontal

                    // Smooth rotation with threshold
                    if (Vector3.Angle(transform.forward, aimDirection) > 5f)
                    {
                        float rotateSpeed = 20f; // Increased for responsiveness
                        transform.forward = Vector3.Lerp(transform.forward, aimDirection,
                            rotateSpeed * Time.deltaTime);
                    }

                    // Vector3 aimDirection = (_targetUnit.GetWorldPosition()).normalized;
                    //  float rotateSpeed = 10f;
                    // transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
                    break;
                case State.Shooting:
                    if (_canShootBullet)
                    {
                        Shoot();
                        _canShootBullet = false;
                    }

                    break;
            }

            if (_stateTimer <= 0)
            {
                NextState();
            }
        }

        private void NextState()
        {
            switch (_state)
            {
                case State.Aiming:
                    if (_stateTimer <= 0)
                    {
                        _state = State.Shooting;
                        float shootingStateTime = .1f;
                        _stateTimer = shootingStateTime;
                    }

                    break;
                case State.Shooting:
                    if (_stateTimer <= 0)
                    {
                        _state = State.CoolOff;
                        float coolOffStateTime = .5f;
                        _stateTimer = coolOffStateTime;
                    }

                    break;
                case State.CoolOff:
                    if (_stateTimer <= 0)
                    {
                        ActionComplete();
                    }

                    break;
            }
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validActionGridPositions = new List<GridPosition>();
            GridPosition unitGridPosition = Unit.GetGridPosition();

            // Create a square area around the unit
            for (int x = -maxShootDistance; x <= maxShootDistance; x++)
            {
                for (int z = -maxShootDistance; z <= maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    // Skip invalid grid positions
                    if (!LevelGrid.Instance.IsGridPositionValid(testGridPosition))
                        continue;

                    // Calculate actual distance (Manhattan distance)
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxShootDistance)
                        continue;

                    // Skip if no unit at position
                    if (!LevelGrid.Instance.HasAnyUnitAtGridPosition(testGridPosition))
                        continue;

                    BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    // CRITICAL FIX: Changed == to != for enemy check
                    if (targetUnit.IsEnemy() != Unit.IsEnemy())
                    {
                        validActionGridPositions.Add(testGridPosition);
                    }
                }
            }

            return validActionGridPositions;
        }

        public override string GetActionName()
        {
            return ActionName;
        }

        public override Sprite GetActionIcon()
        {
            return shootSprite;
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            _canShootBullet = true;


            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;
            ActionStart(OnActionComplete);
        }

        public override int GetActionPointsCost() => actionPointCost;


        // ... other fields ...

        private BaseUnit _targetUnit; // Changed from GamePlay.Unit.BaseUnit.BaseUnit

        public event EventHandler<OnShootEventArgs> OnUnitShoot;

        public class OnShootEventArgs : EventArgs
        {
            public BaseUnit TargetUnit; // Updated type
            public BaseUnit ShootingUnit; // Updated type and fixed casing
        }

        private void Shoot()
        {
            OnUnitShoot?.Invoke(this, new OnShootEventArgs
            {
                TargetUnit = _targetUnit,
                ShootingUnit = Unit // Now properly references the BaseUnit
            });
            _targetUnit.Damage(damageAmount);
        }
        
        public BaseUnit GetTargetUnit()
        {
            return _targetUnit;
        }

        public int  GetMaxShootDistance()
        {
            return maxShootDistance;
        }
    }
}