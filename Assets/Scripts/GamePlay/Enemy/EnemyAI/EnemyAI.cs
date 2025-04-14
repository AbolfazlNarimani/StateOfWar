using System;
using UnityEngine;

namespace Enemy.EnemyAI
{
    public class EnemyAI : MonoBehaviour
    {
        private float _timer;

        private void Start()
        {
            TurnSystem.TurnSystem.Instance.OnTurnNumberChanged += OnTurnChanged;
        }

        private void OnTurnChanged(object sender, EventArgs e)
        {
            _timer = 2f;
        }

        private void Update()
        {
            if (TurnSystem.TurnSystem.Instance.IsPlayerTurn()) return;
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                TurnSystem.TurnSystem.Instance.NextTurn();
            }
            
        }
    }
}
