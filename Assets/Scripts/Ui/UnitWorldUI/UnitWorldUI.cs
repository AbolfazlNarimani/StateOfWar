using System;
using GamePlay.Health;
using GamePlay.Unit.BaseUnit;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.UnitWorldUI
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointsText;
        [SerializeField] private BaseUnit baseUnit;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSystem healthSystem;

        private void Start()
        {
            UpdateActionPointsText();
            BaseUnit.OnAnyActionPointsChanged += BaseUnitOnAnyActionPointsChanged;
            healthSystem.OnDamaged += HealthSystemOnDamaged;
            UpdateHealthBar();
        }

        private void HealthSystemOnDamaged(object sender, EventArgs e)
        {
            UpdateHealthBar(); 
        }

        private void BaseUnitOnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPointsText();
        }

        private void UpdateActionPointsText()
        {
            actionPointsText.text = baseUnit.GetActionPoints().ToString();
        }

        private void UpdateHealthBar()
        {
            healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }
    }
}