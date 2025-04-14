using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewInputSystem
{
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnMoveAction;
        public event EventHandler OnUnitSelect;


        public static GameInput Instance { get; private set; }
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            Instance = this;
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();

            _playerInputActions.Player.Move.performed += MoveOnPerformed;
            _playerInputActions.Player.SelectUnit.performed += SelectUnitPerformed;
        }

        private void SelectUnitPerformed(InputAction.CallbackContext obj)
        {
            OnUnitSelect?.Invoke(this, EventArgs.Empty);
        }

        public float GetRotationVectorNormalized()
        {
            float rotationVector = _playerInputActions.Player.CameraRotation.ReadValue<float>();
            return rotationVector;
        }

        public float GetZoomVectorNormalized()
        {
            float zoomVector = _playerInputActions.Player.CameraZoom.ReadValue<float>();
            return zoomVector;
        }


        public Vector3 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.Player.CameraMovement.ReadValue<Vector2>();

            return inputVector;
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