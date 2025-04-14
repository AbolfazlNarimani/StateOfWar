using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.TurnSystemUI
{
    public class TurnSystemUI : MonoBehaviour
    {
        private TurnSystem.TurnSystem _turnSystem;
        [SerializeField] private Button nextTurnButton;
        [SerializeField] private TextMeshProUGUI turnNumberText;
        [SerializeField] private GameObject enemyTurnVisualGameObject;
        [SerializeField] private GameObject nextTurnButtonBackGround;

        private void Start()
        {
            _turnSystem = TurnSystem.TurnSystem.Instance;
            nextTurnButton.onClick.AddListener((() => _turnSystem.NextTurn()));
            UpdateTurnNumber();
            _turnSystem.OnTurnNumberChanged += OnTurnChanged;
            UpdateEnemyTurnVisual();
            UpdateEndTurnButtonVisibility();
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            UpdateTurnNumber();
            UpdateEnemyTurnVisual();
            UpdateEndTurnButtonVisibility();
        }


        private void UpdateTurnNumber()
        {
            turnNumberText.text = $"TURN : {_turnSystem.GetTurnNumber()}";
        }

        private void UpdateEnemyTurnVisual()
        {
            enemyTurnVisualGameObject.SetActive(!TurnSystem.TurnSystem.Instance.IsPlayerTurn());
        }

        private void UpdateEndTurnButtonVisibility()
        {
            nextTurnButton.gameObject.SetActive(TurnSystem.TurnSystem.Instance.IsPlayerTurn());
            nextTurnButtonBackGround.SetActive(TurnSystem.TurnSystem.Instance.IsPlayerTurn());
        }
    }
}
