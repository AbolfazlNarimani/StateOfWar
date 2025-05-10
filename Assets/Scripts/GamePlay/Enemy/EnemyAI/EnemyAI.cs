/*using System;
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
}*/


/*using System;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using NewInputSystem.ActionSystem.SpinAction;
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
                _timer = 0.5f; // Short delay before first enemy acts
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
                        TryNextEnemyAction();
                    }

                    break;

                case State.Busy:
                    // Waiting for current action to complete
                    break;
            }
        }

        private void TryNextEnemyAction()
        {
            var enemyUnits = UnitManager.Instance.GetEnemyUnitList();

            // If we've processed all enemies, end turn
            if (_currentEnemyIndex >= enemyUnits.Count && !_hasEnemyActedThisCycle)
            {
                if (_hasEnemyActedThisCycle)
                {
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

            // Get current enemy
            BaseUnit currentEnemy = enemyUnits[_currentEnemyIndex];

            if (currentEnemy.GetActionPoints() == 0)
            {
                // Skip dead enemies or those with no AP
                _currentEnemyIndex++;
                _timer = 0.1f;
                return;
            }

            // making enemies do all thy can 
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

            if (enemyUnit.GetActionPoints() >= spinAction.GetActionPointsCost())
            {
                spinAction.TakeAction(actionGridPosition, onEnemyAiActionComplete);
                return true;
            }

            return false;
        }

        private void OnEnemyActionComplete()
        {
            // Action complete, move to next enemy
            _currentEnemyIndex++;
            _timer = 0.5f; // Delay before next enemy acts
            state = State.TakingTurn;
        }
    }
}*/

using System;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using GridSystem;
using NewInputSystem.ActionSystem.SpinAction;
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
            SpinAction spinAction = enemyUnit.GetSpinAction();
            GridPosition actionGridPosition = enemyUnit.GetGridPosition();
            
            if (!spinAction.IsValidActionGridPosition(actionGridPosition))
                return false;

            if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
                return false;

            spinAction.TakeAction(actionGridPosition, onEnemyAiActionComplete);
            return true;
        }

        private void OnEnemyActionComplete()
        {
            // Action complete, allow next enemy to act
            _timer = 0.1f;
            state = State.TakingTurn;
        }
    }
}