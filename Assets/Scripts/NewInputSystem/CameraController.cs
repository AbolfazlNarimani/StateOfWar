using Unity.Cinemachine;
using Unity.VisualScripting;
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
        [SerializeField] private float defaultZRotation = 35f;
        private float _rotationInput;
        private float _zoomInput;
        private float _rotationValue;

        private float _currentZoom = 10f;
        private const float ZoomSpeed = 5f;
        private const float MinZoom = 3f;
        private const float MaxZoom = 7f;

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
            HandleZoom();
        }

        private void HandleZoom()
        {
            // Get the Cinemachine Virtual Camera
            var cinemachineComponent = _cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            var framingTransposer = cinemachineComponent.GetComponent<CinemachineFollow>();


            // Get zoom input from mouse scroll
            _zoomInput = _gameInput.GetZoomVectorNormalized();
            

            // Adjust zoom smoothly
            _currentZoom += _zoomInput * Time.deltaTime * ZoomSpeed;

            // Clamp zoom to prevent extreme values
            _currentZoom = Mathf.Clamp(_currentZoom, MinZoom, MaxZoom);

            // Apply zoom to Follow Offset
            if (_currentZoom <= MaxZoom && _currentZoom >= MinZoom)
            {
                Vector3 offset = framingTransposer.FollowOffset;
                offset.y = _currentZoom;
                framingTransposer.FollowOffset = offset;
                
                    AdjustObjectZOffset();
                
            }
        }

        private void HandleRotation()
        {
            _rotationInput = _gameInput.GetRotationVectorNormalized();

            _rotationValue += _rotationInput * rotationSpeed * Time.deltaTime;

            transform.eulerAngles = new Vector3(defaultZRotation, _rotationValue * rotationSpeed, 0F);
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

        private void AdjustObjectZOffset()
        {
            // Define how much the Z position should change
            float minZOffset = 13f; // Closest position
            float maxZOffset = 35f; // Default position

            // Interpolate Z offset based on zoom level
            
            float zOffset = Mathf.Lerp(minZOffset, maxZOffset, (_currentZoom - MinZoom) / (MaxZoom - MinZoom));
            defaultZRotation = zOffset;
        }
    }
}