using System;
using UnityEngine;

namespace GamePlay.Unit
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private GamePlay.Unit.BaseUnit.BaseUnit unit;
        private MeshRenderer _meshRenderer;
        private UnitActionSystem _unitActionSystem;
        private GamePlay.Unit.BaseUnit.BaseUnit _currentUnit;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            _unitActionSystem = UnitActionSystem.Instance;
            _unitActionSystem.OnSelectedUnitChanged += OnUnitSelectionChanged;
            UpdateVisual();
        }

        private void OnUnitSelectionChanged(object sender, EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            _meshRenderer.enabled = _unitActionSystem.GetSelectedUnit() == unit;
        }

        private void OnDestroy()
        {
            _unitActionSystem.OnSelectedUnitChanged -= OnUnitSelectionChanged;
        }
    }
}
