using System;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using NewInputSystem.ActionSystem.SpinAction;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Enemy.EnemyAI
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WaitingEnemyTurn,
            TakingTurn,
            Busy,
        }

        private State state;

        private void Awake()
        {
            state = State.WaitingEnemyTurn;
        }

        private float _timer;

        private void Start()
        {
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnChanged;
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            if (!TurnSystem.TurnSystem.Instance.IsPlayerTurn())
            {
                state = State.Busy;
                _timer = 2f;
            }
        }


        private void Update()
        {
            if (TurnSystem.TurnSystem.Instance.IsPlayerTurn()) return;
            state = State.TakingTurn;
            switch (state)
            {
                case State.WaitingEnemyTurn:
                    break;
                case State.TakingTurn:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        if (  TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            //enemy's have action
                            state = State.Busy;
                        }
                        else
                        {
                            // no more enemy's have action
                            TurnSystem.TurnSystem.Instance.NextTurn();
                            
                        }

                      
                    }

                    break;
                case State.Busy:
                    break;
            }
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAiActionComplete)
        {
            foreach (BaseUnit enemyUnit in UnitManager.Instance.GetEnemyUnitList() )
            {
                if ( TryTakeEnemyAIAction(enemyUnit, onEnemyAiActionComplete))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryTakeEnemyAIAction(BaseUnit enemyUnit, Action onEnemyAiActionComplete)
        {
            SpinAction spinAction = enemyUnit.GetSpinAction();
            GridPosition actionGridPosition = enemyUnit.GetGridPosition();
            if (!spinAction.IsValidActionGridPosition(actionGridPosition))
            {
                return false;
            }

            if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
            {
                return false;
            }
            
            spinAction.TakeAction(actionGridPosition,onEnemyAiActionComplete);
            return true;
        }

        private void SetStateTakingTurn()

        {
            _timer -= Time.deltaTime;
            state = State.TakingTurn;
        }
    }
}