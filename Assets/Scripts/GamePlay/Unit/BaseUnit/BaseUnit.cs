using System;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.ActionSystem.MoveAction;
using GamePlay.GridSystem;
using GamePlay.Health;
using GridSystem;
using NewInputSystem.ActionSystem.SpinAction;
using UnityEngine;

namespace GamePlay.Unit.BaseUnit
{
    public abstract class BaseUnit : MonoBehaviour
    {
        protected GridPosition GridPosition;
        protected MoveAction MoveAction;
        protected SpinAction SpinAction;
        protected BaseAction[] BaseActionsArray;
        protected HealthSystem HealthSystem;
        [SerializeField] protected int actionPoints;
        protected int DefaultActionPoints;

        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;

        [SerializeField] protected bool isEnemy;

        protected virtual void Awake()
        {
            HealthSystem = GetComponent<HealthSystem>();
            MoveAction = GetComponent<MoveAction>();
            SpinAction = GetComponent<SpinAction>();
            BaseActionsArray = GetComponents<BaseAction>();
            DefaultActionPoints = actionPoints;
        }

        protected virtual void Start()
        {
            GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(GridPosition, this);
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnNumberChanged;
            HealthSystem.OnDead += HealthSystemOnDead;
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void HealthSystemOnDead(object sender, EventArgs e)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(GridPosition, this);
            Destroy(gameObject);
            
            OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnTurnNumberChanged(object sender, EventArgs e)
        {
            if ((IsEnemy() && !TurnSystem.TurnSystem.Instance.IsPlayerTurn()) ||
                (!IsEnemy() && TurnSystem.TurnSystem.Instance.IsPlayerTurn()))
            {
                actionPoints = DefaultActionPoints;
                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

            if (newGridPosition != GridPosition)
            {
                GridPosition oldGridPosition = GridPosition;
                GridPosition = newGridPosition;
                LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
            }
        }

        protected virtual void SpendActionPoints(int amount)
        {
            actionPoints -= amount;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (actionPoints >= baseAction.GetActionPointsCost())
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }

            return false;
        }

        // Common properties and methods
        public MoveAction GetMoveAction() => MoveAction;
        
        public int GetActionPoints() => actionPoints;
        public GridPosition GetGridPosition() => GridPosition;
        public BaseAction[] GetBaseActionArray() => BaseActionsArray;
        public int GetRemainingActionPoints() => actionPoints;
        public Vector3 GetWorldPosition() => transform.position;
        public bool IsEnemy() => isEnemy;
        public void Damage(int damageAmount) => HealthSystem.TakeDamage(damageAmount);

        public SpinAction GetSpinAction()
        {
            return SpinAction;
        }
    }
}