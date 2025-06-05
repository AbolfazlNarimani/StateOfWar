using System;
using System.Collections.Generic;
using GamePlay.Enemy.EnemyAI;
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
        [SerializeField] private LayerMask obstaclesLayerMask;

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
        private  string _actionName = "Shoot";

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
            GridPosition unitGridPosition = Unit.GetGridPosition();
            return GetValidActionGridPositionList(unitGridPosition);
        }

        public override string GetActionName()
        {
            return _actionName;
        }

        public void SetActionNameForChildActions(string altActionName)
        {
            _actionName = altActionName;
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

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            targetUnit.GetHealthNormalized();
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
            };
        }


        // ... other fields ...

        private BaseUnit _targetUnit; // Changed from GamePlay.Unit.BaseUnit.BaseUnit

        public static event EventHandler OnAnyShoot;
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
            
            OnAnyShoot?.Invoke(this, EventArgs.Empty);
            
            _targetUnit.Damage(damageAmount);
        }

        public BaseUnit GetTargetUnit()
        {
            return _targetUnit;
        }

        public int GetMaxShootDistance()
        {
            return maxShootDistance;
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }

        public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validActionGridPositions = new List<GridPosition>();

            for (int x = -maxShootDistance; x <= maxShootDistance; x++)
            {
                for (int z = -maxShootDistance; z <= maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsGridPositionValid(testGridPosition))
                        continue;

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxShootDistance)
                        continue;

                    if (!LevelGrid.Instance.HasAnyUnitAtGridPosition(testGridPosition))
                        continue;

                    BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    // Check if target is enemy before doing line-of-sight check
                    if (targetUnit.IsEnemy() != Unit.IsEnemy())
                    {
                        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                        Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(testGridPosition);
                        Vector3 shootDir = (targetWorldPosition - unitWorldPosition).normalized;

                        float unitShoulderHeight = 1.7f;

                        if (!Physics.Raycast(
                                unitWorldPosition + Vector3.up * unitShoulderHeight,
                                shootDir,
                                Vector3.Distance(unitWorldPosition, targetWorldPosition),
                                obstaclesLayerMask))
                        {
                            validActionGridPositions.Add(testGridPosition);
                        }
                    }
                }
            }

            return validActionGridPositions;
        }

        /*public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validActionGridPositions = new List<GridPosition>();


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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = _targetUnit.GetWorldPosition() - unitWorldPosition.normalized;
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, Vector3.Distance(unitWorldPosition, _targetUnit.GetWorldPosition()),obstaclesLayerMask))
                {
                    // we are blocked
                    continue;
                }


                BaseUnit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                // CRITICAL FIX: Changed == to != for enemy check
                if (targetUnit.IsEnemy() != Unit.IsEnemy())
                {
                    validActionGridPositions.Add(testGridPosition);
                }
            }
        }

        return validActionGridPositions;
    }*/
    }
}