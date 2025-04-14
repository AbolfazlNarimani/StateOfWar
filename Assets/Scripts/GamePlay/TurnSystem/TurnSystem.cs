using System;
using UnityEngine;

namespace TurnSystem
{
    public class TurnSystem : MonoBehaviour
    {
        private int _turnNumber = 1;
        public event EventHandler OnTurnNumberChanged;

        public static TurnSystem Instance { get; private set; }
        
        private bool _isPlayerTurn = true;

        private void Awake()
        {
            Instance = this;
        }

        public void NextTurn()
        {
            _turnNumber++;
            _isPlayerTurn = !_isPlayerTurn;
            OnTurnNumberChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetTurnNumber()
        {
            return _turnNumber;
        }
        
        public bool IsPlayerTurn() => _isPlayerTurn;
    }
}