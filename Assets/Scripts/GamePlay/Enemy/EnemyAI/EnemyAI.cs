using System;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.ActionSystem.SpinAction;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using UnityEngine;

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
        private float _timer;
        private int _currentEnemyIndex;
        private bool _hasEnemyActedThisCycle;

        private void Awake()
        {
            state = State.WaitingEnemyTurn;
            _currentEnemyIndex = 0;
        }

        private void Start()
        {
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnChanged;
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            if (!TurnSystem.TurnSystem.Instance.IsPlayerTurn())
            {
                state = State.TakingTurn;
                _currentEnemyIndex = 0;
                _timer = 0.5f;
                _hasEnemyActedThisCycle = false;
            }
        }

        private void Update()
        {
            if (TurnSystem.TurnSystem.Instance.IsPlayerTurn()) return;

            switch (state)
            {
                case State.WaitingEnemyTurn:
                    break;

                case State.TakingTurn:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        ProcessEnemyActions();
                    }

                    break;

                case State.Busy:
                    // Waiting for current action to complete
                    break;
            }
        }

        private void ProcessEnemyActions()
        {
            var enemyUnits = UnitManager.Instance.GetEnemyUnitList();

            // If we've processed all enemies, check if any can still act
            if (_currentEnemyIndex >= enemyUnits.Count)
            {
                if (_hasEnemyActedThisCycle)
                {
                    // Some enemies may still have AP left, restart cycle
                    _currentEnemyIndex = 0;
                    _hasEnemyActedThisCycle = false;
                    _timer = 0.1f;
                    return;
                }
                else
                {
                    // No enemies can act anymore, end turn
                    TurnSystem.TurnSystem.Instance.NextTurn();
                    state = State.WaitingEnemyTurn;
                    return;
                }
            }

            BaseUnit currentEnemy = enemyUnits[_currentEnemyIndex];

            if (currentEnemy.GetActionPoints() == 0)
            {
                // Skip dead enemies or those with no AP
                _currentEnemyIndex++;
                _timer = 0.1f;
                return;
            }

            if (TryTakeEnemyAIAction(currentEnemy, OnEnemyActionComplete))
            {
                state = State.Busy;
                _hasEnemyActedThisCycle = true;
            }
            else
            {
                // This enemy can't act right now, try next one
                _currentEnemyIndex++;
                _timer = 0.1f;
            }
        }

        private bool TryTakeEnemyAIAction(BaseUnit enemyUnit, Action onEnemyAiActionComplete)
        {
            EnemyAIAction bestEnemyAIAction = null;
            BaseAction bestBaseAction = null;
            foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
            {
                if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
                {
                    // enemy cannot afford this action
                    continue;
                }
                else
                {
                    if (bestEnemyAIAction == null)
                    {
                        bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                        bestBaseAction = baseAction;
                    }
                    else
                    {
                        EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                        if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                        {
                            bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                            bestBaseAction = baseAction;
                        }
                    }
                }
            }
            if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
            {
                bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAiActionComplete);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnEnemyActionComplete()
        {
            // Action complete, allow next enemy to act
            _timer = 0.1f;
            state = State.TakingTurn;
        }
    }
}