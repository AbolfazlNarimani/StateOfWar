using System;
using System.Collections.Generic;
using GamePlay.ActionSystem.BaseAction;
using GamePlay.Unit;
using GamePlay.Unit.BaseUnit;
using NUnit.Framework.Internal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.UnitActionSystemUI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform actionButtonPrefab;
        [SerializeField] private Transform actionButtonContainerTransform;
        [SerializeField] private TextMeshProUGUI actionPoints;
        private GamePlay.Unit.BaseUnit.BaseUnit _selectedUnit;
        private List<ActionButtonUI.ActionButtonUI> _actionButtonList;

        private void Awake()
        {
            _actionButtonList = new List<ActionButtonUI.ActionButtonUI>();
        }

        private void Start()
        {
            // Ensure instance exists first
            if (UnitActionSystem.Instance == null)
            {
                Debug.LogError("UnitActionSystem instance not found!");
                return;
            }
            // Null-check the prefab
            if (actionButtonPrefab == null)
            {
                Debug.LogError("Action button prefab not assigned!");
                return;
            }
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnUnitSelectionChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted += OnActionStarted;
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnNumberChanged;
            BaseUnit.OnAnyActionPointsChanged += OnAnyActionPointsChanged;
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
            
            /*// Initialize with current selection
            _selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            if (_selectedUnit != null)
            {
                CreateUnitActionButtons();
                UpdateActionPoints();
            }*/
            
        }

        private void OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void OnTurnNumberChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void OnActionStarted(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void OnUnitSelectionChanged(object sender, EventArgs e)
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
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
                Button button = actionButtonPrefab.GetComponent<Button>();
                TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
                
                if (baseAction.GetActionNameFontSize() != 0)
                {
                    textMesh.fontSizeMax = baseAction.GetActionNameFontSize();
                }

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

        private void UpdateActionPoints()
        {
            int actionPointsLeft = _selectedUnit.GetRemainingActionPoints();
            actionPoints.text = $"ActionPointsLeft: {actionPointsLeft}";
        }
    }
}