using System;
using NewInputSystem.ActionSystem.BaseAction;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.UnitActionSystemUI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField]private Transform actionButtonPrefab;
        [SerializeField]private Transform actionButtonContainerTransform;
        private Unit.Unit _selectedUnit;
        private void Start()
        {
            CreateUnitActionButtons();
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnUnitSelectionChanged;
        }

        private void OnUnitSelectionChanged(object sender, EventArgs e)
        {
            CreateUnitActionButtons();
        }

        private void CreateUnitActionButtons()
        {
            foreach (Transform button in actionButtonContainerTransform)
            {
                Destroy(button.gameObject);
            }
            _selectedUnit =  UnitActionSystem.Instance.GetSelectedUnit();
            foreach (BaseAction baseAction in _selectedUnit.GetBaseActionArray())
            {
               Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
               ActionButtonUI.ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI.ActionButtonUI>();
               actionButtonUI.SetBaseAction(baseAction);
            }
        }
    }
}
