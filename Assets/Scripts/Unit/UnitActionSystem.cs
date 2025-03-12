using System;
using System.ComponentModel.Design.Serialization;
using NewInputSystem;
using NewInputSystem.BaseActions;
using UnityEngine;

namespace Unit
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;
        private GameInput _gameInput;

        public event EventHandler OnSelectedUnitChanged;

        public static UnitActionSystem Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            _gameInput = GameInput.Instance;
            _gameInput.OnMoveAction += OnMoveAction;
            _gameInput.OnUnitSelect += OnUnitSelected;
        }

        private void OnUnitSelected(object sender, EventArgs e)
        {
            HandleUnitSelection();
        }

        private void OnMoveAction(object sender, EventArgs e)
        {
            selectedUnit.GetMoveAction().MoveUnit(MouseWorld.GetMouseWorldPosition());
        }

        private void HandleUnitSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: float.MaxValue, unitLayerMask))
            {
                // selectedUnit = hit.collider.GetComponent<Unit>();
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnit(unit);
                }
            }
        }

        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }
    }
}