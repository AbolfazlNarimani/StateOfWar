using System;
using System.Collections;
using GamePlay.Health;
using GamePlay.NewInputSystem.ActionSystem.BaseAction;
using GridSystem;
using NewInputSystem.ActionSystem.MoveAction;
using NewInputSystem.ActionSystem.SpinAction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay.Unit
{
    public class Unit : MonoBehaviour
    {
        private GridPosition _gridPosition;
        private MoveAction _moveAction;
        private SpinAction _spinAction;
        private BaseAction[] _baseActionsArray;
        private HealthSystem _healthSystem;
        [SerializeField] private int actionPoints;
        private int _defaultActionPoints;

        public static event EventHandler OnAnyActionPointsChanged;

        [SerializeField] private bool isEnemy;

        private void Awake()
        {
            _moveAction = GetComponent<MoveAction>();
            _spinAction = GetComponent<SpinAction>();
            _baseActionsArray = GetComponents<BaseAction>();
            _defaultActionPoints = actionPoints;
            _healthSystem = GetComponent<HealthSystem>();
        }


        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnNumberChanged;
            _healthSystem.OnDead += HealthSystemOnDead;
        }

        private void HealthSystemOnDead(object sender, EventArgs e)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
            Destroy(gameObject);
            
        }

        

        IEnumerator EnableRotationAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.freezeRotation = false;
                // Apply slight torque if needed
                rb.AddTorque(new Vector3(0, Random.Range(-1f, 1f), 0), ForceMode.Impulse);
            }
        }

        private void OnTurnNumberChanged(object sender, EventArgs e)
        {
            if ((IsEnemy() && !TurnSystem.TurnSystem.Instance.IsPlayerTurn()) ||
                (!IsEnemy() && TurnSystem.TurnSystem.Instance.IsPlayerTurn()))
            {
                actionPoints = _defaultActionPoints;
                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

            if (newGridPosition != _gridPosition)
            {
                //Unit Changed GridPosition
                LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
                _gridPosition = newGridPosition;
            }
        }

        private void SpendActionPoints(int amount)
        {
            actionPoints -= amount;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (actionPoints >= baseAction.GetActionPointsCost())
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }

            return false;
        }

        public MoveAction GetMoveAction() => _moveAction;
        public SpinAction GetSpinAction() => _spinAction;
        public GridPosition GetGridPosition() => _gridPosition;
        public BaseAction[] GetBaseActionArray() => _baseActionsArray;

        public int GetRemainingActionPoints() => actionPoints;

        public Vector3 GetWorldPosition() => transform.position;

        public bool IsEnemy()
        {
            return isEnemy;
        }

        public void Damage(int damageAmount)
        {
            _healthSystem.TakeDamage(damageAmount);
        }
    }
}