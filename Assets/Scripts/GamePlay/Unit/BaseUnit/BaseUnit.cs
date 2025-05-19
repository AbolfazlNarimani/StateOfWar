using System;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.ActionSystem.BaseShootAction.ShootAction;
using GamePlay.ActionSystem.MoveAction;
using GamePlay.ActionSystem.SpinAction;
using GamePlay.GridSystem;
using GamePlay.Health;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.Unit.BaseUnit
{
    public abstract class BaseUnit : MonoBehaviour
    {
        protected GridPosition GridPosition;
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

        public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (baseAction.GetActionPointsCost() <= actionPoints)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (BaseAction baseAction in BaseActionsArray)
            {
                if (baseAction is T)
                {
                    return (T)baseAction;
                }
            }

            return null;
        }

        // Common properties and methods
        public float GetHealthNormalized() => HealthSystem.GetHealthNormalized();

        public int GetActionPoints() => actionPoints;
        public GridPosition GetGridPosition() => GridPosition;
        public BaseAction[] GetBaseActionArray() => BaseActionsArray;
        public int GetRemainingActionPoints() => actionPoints;
        public Vector3 GetWorldPosition() => transform.position;
        public bool IsEnemy() => isEnemy;
        public void Damage(int damageAmount) => HealthSystem.TakeDamage(damageAmount);
    }
}