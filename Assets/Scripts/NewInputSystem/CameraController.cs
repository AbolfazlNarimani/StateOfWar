using Unity.Cinemachine;
using UnityEngine;

namespace NewInputSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineCamera _cinemachineCamera;
        private GameInput _gameInput;
        [SerializeField] private float moveSpeed = 7f;
        private readonly float _defaultZoom = 7;
        [SerializeField] private float rotationSpeed = 10f;
        private float _rotationInput;
        private float _zoomInput;
        private float _currentZoom;
        private float rotationValue;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _gameInput = GameInput.Instance;
            _currentZoom = _defaultZoom;
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleRotation()
        {
            _rotationInput = _gameInput.GetRotationVectorNormalized();

            rotationValue += _rotationInput * rotationSpeed * Time.deltaTime;
            
            transform.eulerAngles = new Vector3(35F, rotationValue * rotationSpeed, 0F);
        }


        private void HandleMovement()
        {
            Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

            // Calculate movement direction relative to the character's rotation
            Vector3 moveDir = transform.forward * inputVector.y + transform.right * inputVector.x;

            // Flatten the direction to the XZ plane (ignore Y-axis)
            moveDir.y = 0;

            // Normalize to prevent faster diagonal movement
            if (moveDir.magnitude > 0)
                moveDir.Normalize();

            float moveDistance = moveSpeed * Time.deltaTime;

            // Apply movement
            transform.position += moveDir * moveDistance;
        }
    }
}