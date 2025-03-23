using System;
using System.Collections.Generic;
using NewInputSystem.ActionSystem.BaseAction;
using NUnit.Framework.Internal;
using TMPro;
using Unit;
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
            UnitActionSystem.Instance.OnActionStarted += OnActionStarted;
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnNumberChanged;
            Unit.Unit.OnAnyActionPointsChanged += OnAnyActionPointsChanged;
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
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