using System;
using System.Collections.Generic;
using GamePlay.GridSystem;
using GridSystem;
using UnityEngine;

namespace GamePlay.NewInputSystem.ActionSystem.ShootAction
{
    public class ShootAction : BaseAction.BaseAction
    {
        private enum State
        {
            Aiming,
            Shooting,
            CoolOff,
        }

        private State _state;
        [SerializeField] private Sprite shootSprite;
        private const string ActionName = "Shoot";
        private readonly int _maxShootDistance = 7;
        private float _stateTimer;
        private GamePlay.Unit.Unit _targetUnit;
        private bool _canShootBullet;

        public event EventHandler<OnShootEventArgs> OnUnitShoot;

        public class OnShootEventArgs : EventArgs
        {
            public GamePlay.Unit.Unit TargetUnit;
            public GamePlay.Unit.Unit shootingUnit;
        }
         

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
                    Vector3 aimDirection = (_targetUnit.GetWorldPosition()).normalized;
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
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

        private void Shoot()
        {
            OnUnitShoot?.Invoke(this, new OnShootEventArgs{TargetUnit = _targetUnit , shootingUnit = Unit});
            _targetUnit.Damage(50);
           
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

            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsGridPositionValid(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxShootDistance)
                    {
                        continue;
                    }


                    if (!LevelGrid.Instance.HasAnyUnitAtGridPosition(testGridPosition))
                    {
                        //grid position is empty
                        continue;
                    }


                    GamePlay.Unit.Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    if (targetUnit.IsEnemy() == Unit.IsEnemy())
                    {
                        // Both Units Are on the same team
                        continue;
                    }


                    validActionGridPositions.Add(testGridPosition);
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
           
            ActionStart(OnActionComplete);
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            _canShootBullet = true;
            
            
            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;
        }

        public override int GetActionPointsCost()
        {
            return 1;
        }
    }
}