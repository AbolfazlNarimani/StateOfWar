using System;
using System.ComponentModel.Design.Serialization;
using GridSystem;
using NewInputSystem;
using NewInputSystem.ActionSystem.BaseAction;
using UnityEngine;

namespace Unit
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;
        private GameInput _gameInput;
        private BaseAction _selectedAction;

        public event EventHandler OnSelectedUnitChanged;
        public event EventHandler OnSelectedActionChanged;

        public static UnitActionSystem Instance { get; private set; }

        private bool _isBusy;

        private void Start()
        {
            Instance = this;
            _gameInput = GameInput.Instance;
            _gameInput.OnMoveAction += OnMoveAction;
            _gameInput.OnUnitSelect += OnUnitSelected;
            SetSelectedUnit(selectedUnit);
        }


        private void OnUnitSelected(object sender, EventArgs e)
        {
            HandleUnitSelection();
        }

        private void OnMoveAction(object sender, EventArgs e)
        {
            if (_isBusy) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());


            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition) && !_isBusy)
            {
                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }

        private void HandleUnitSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: float.MaxValue, unitLayerMask))
            {
                // selectedUnit = hit.collider.GetComponent<Unit>();
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (selectedUnit != unit)
                    {
                        SetSelectedUnit(unit);
                    }
                }
            }
        }

        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;

            SetSelectedAction(unit.GetMoveAction());

            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetBusy()
        {
            _isBusy = true;
        }

        private void ClearBusy()
        {
            _isBusy = false;
        }

        public Unit GetSelectedUnit()
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