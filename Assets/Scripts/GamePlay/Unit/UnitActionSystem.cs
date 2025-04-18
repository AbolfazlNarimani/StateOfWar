using System;
using System.Linq;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.GridSystem;
using GamePlay.NewInputSystem;
using GridSystem;
using NewInputSystem;
using UnityEngine;

namespace GamePlay.Unit
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private GamePlay.Unit.BaseUnit.BaseUnit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;
        private GameInput _gameInput;
        private BaseAction _selectedAction;

        public event EventHandler OnSelectedUnitChanged;
        public event EventHandler OnSelectedActionChanged;
        public event EventHandler OnActionStarted;
        public event EventHandler<bool> OnBusyChanged;


        public static UnitActionSystem Instance { get; private set; }

        private bool _isBusy;
        
        private void Start()
        {
            Instance = this;
            _gameInput = GameInput.Instance;
            _gameInput.OnMoveAction += OnMoveAction;
            _gameInput.OnUnitSelect += OnUnitSelected;
            
            // Initialize with no selection
            selectedUnit = null;
            _selectedAction = null;
    
            // Find first player unit if none is assigned
            if (selectedUnit == null)
            {
                var playerUnits = FindObjectsOfType<BaseUnit.BaseUnit>()
                    .Where(unit => !unit.IsEnemy())
                    .ToList();
            
                if (playerUnits.Count > 0)
                {
                    SetSelectedUnit(playerUnits[0]);
                }
            }
            else if (!selectedUnit.IsEnemy()) // Only auto-select if it's a player unit
            {
                SetSelectedUnit(selectedUnit);
            }
        }


        private void OnUnitSelected(object sender, EventArgs e)
        {
            HandleUnitSelection();
        }

        private void OnMoveAction(object sender, EventArgs e)
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
            
            if (_isBusy) return;
            if (selectedUnit == null) return; // Add null check
            if (!TurnSystem.TurnSystem.Instance.IsPlayerTurn()) return;
            if (_selectedAction == null) return; // Add null check
            
            if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if (!selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;
            
            SetBusy();
            _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
            
        }

        private void HandleUnitSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: float.MaxValue, unitLayerMask))
            {
                // selectedUnit = hit.collider.GetComponent<Unit>();
                if (hit.transform.TryGetComponent<GamePlay.Unit.BaseUnit.BaseUnit>(out GamePlay.Unit.BaseUnit.BaseUnit unit) && selectedUnit != unit && !unit.IsEnemy())
                {
                    SetSelectedUnit(unit);
                }
            }
        }

        private void SetSelectedUnit(GamePlay.Unit.BaseUnit.BaseUnit unit)
        {
            selectedUnit = unit;

            SetSelectedAction(unit.GetMoveAction());

            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetBusy()
        {
            _isBusy = true;
            OnBusyChanged?.Invoke(this, true);
        }

        private void ClearBusy()
        {
            _isBusy = false;
            OnBusyChanged?.Invoke(this, false);
        }

        public GamePlay.Unit.BaseUnit.BaseUnit GetSelectedUnit()
        {
            return selectedUnit;
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            _selectedAction = baseAction;
            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }

        public BaseAction GetSelectedAction() => _selectedAction;
        
    }
    
    
}