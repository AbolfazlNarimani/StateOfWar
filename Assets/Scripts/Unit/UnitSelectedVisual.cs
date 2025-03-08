using System;
using UnityEngine;

namespace Unit
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        private MeshRenderer _meshRenderer;
        private UnitActionSystem _unitActionSystem;
        private Unit _currentUnit;

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
    }
}