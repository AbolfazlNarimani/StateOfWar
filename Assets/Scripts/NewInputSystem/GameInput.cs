using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewInputSystem
{
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnMoveAction;
        public static GameInput Instance { get; private set; }
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            Instance = this;
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();

            _playerInputActions.Player.Move.performed += MoveOnPerformed;
        }

        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            OnMoveAction?.Invoke(this, EventArgs.Empty);
        }


        private void OnDestroy()
        {
            _playerInputActions.Player.Disable();
        }
    }
}
