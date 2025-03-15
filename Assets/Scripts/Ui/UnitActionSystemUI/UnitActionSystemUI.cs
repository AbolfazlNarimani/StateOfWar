using System;
using System.Collections.Generic;
using NewInputSystem.ActionSystem.BaseAction;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.UnitActionSystemUI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform actionButtonPrefab;
        [SerializeField] private Transform actionButtonContainerTransform;
        private Unit.Unit _selectedUnit;
        private List<ActionButtonUI.ActionButtonUI> _actionButtonList;

        private void Awake()
        {
            _actionButtonList = new List<ActionButtonUI.ActionButtonUI>();
        }

        private void Start()
        {
          
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnUnitSelectionChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
            CreateUnitActionButtons();
            UpdateSelectedVisual();
        }

        private void OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void OnUnitSelectionChanged(object sender, EventArgs e)
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
        }

        private void CreateUnitActionButtons()
        {
            foreach (Transform button in actionButtonContainerTransform)
            {
                Destroy(button.gameObject);
            }

            _actionButtonList.Clear();
            _selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            foreach (BaseAction baseAction in _selectedUnit.GetBaseActionArray())
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI.ActionButtonUI actionButtonUI =
                    actionButtonTransform.GetComponent<ActionButtonUI.ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
                _actionButtonList.Add(actionButtonUI);
            }
        }

        private void UpdateSelectedVisual()
        {
            foreach (ActionButtonUI.ActionButtonUI actionButtonUI in _actionButtonList)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }
    }
}