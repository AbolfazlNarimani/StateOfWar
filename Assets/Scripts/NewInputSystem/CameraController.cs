using Unity.Cinemachine;
using UnityEngine;

namespace NewInputSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineCamera _cinemachineCamera;
        private GameInput _gameInput;
        [SerializeField] private float moveSpeed = 7f;
        private float _defaultZoom = 7;
        [SerializeField] private float rotationSpeed = 10f;
        private float _rotationInput;
        private float _zoomInput;
        private float _currentZoom; 

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _gameInput = GameInput.Instance;
            _currentZoom = _defaultZoom;
            SetFollowOffset(_defaultZoom);
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();
        }

        private void HandleRotation()
        {
            _rotationInput = _gameInput.GetRotationVectorNormalized();

            transform.Rotate(Vector3.up, _rotationInput * rotationSpeed * Time.deltaTime);
        }

        public void SetFollowOffset(float offsetInput)
        {
            // Get the component that handles position adjustments
            var bodyComponent = _cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

            if (bodyComponent != null)
            {
                // Assuming you are using the Transposer body (or similar)
                var follow = bodyComponent as CinemachineFollow;
                if (follow != null)
                {
                    _currentZoom += offsetInput;

                    _currentZoom = Mathf.Clamp(_currentZoom, 3f, 10f);

                    follow.FollowOffset = new Vector3(0F, _currentZoom, 0F); // Set the offset value
                    Debug.Log("Follow offset set to: " + _currentZoom);
                }
                else
                {
                    Debug.LogError("Body component is not a CinemachineTransposer!");
                }
            }
            else
            {
                Debug.LogError("CinemachineBody component not found.");
            }
        }


        private void HandleZoom()
        {
            _zoomInput = _gameInput.GetZoomVectorNormalized();
            SetFollowOffset(_zoomInput);
        }


        private void HandleMovement()
        {
            Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(inputVector.x, 0F, inputVector.y);


            // float playerSize = .2F;
            var playerHight = .7F;
            //float playerRadious = 1F;
            var turningSpeed = 10f;
            var moveDistance = moveSpeed * Time.deltaTime;
            var position = transform.position;
            //moveDir.x != 0 &&
            // must inmplement logic for camera world border
            var canMove = true;

            if (!canMove)
            {
                // Cannot move towards moveDir
                // Attempt only x movement
                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                var position1 = transform.position;
                canMove = moveDir.x is < -0.5f or > +0.5F && !Physics.CapsuleCast(position1,
                    (position1 + Vector3.up * playerHight),
                    playerHight, moveDirX, moveDistance);
                if (canMove)
                {
                    // only can move on X
                    moveDir = moveDirX;
                }
                else
                {
                    // we Cannot move on the X
                    // Attempt only z movement
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    var position2 = transform.position;
                    canMove = moveDir.z is < -0.5f or > +0.5F && !Physics.CapsuleCast(position2,
                        (position2 + Vector3.up * playerHight),
                        playerHight, moveDirZ, moveDistance);
                    if (canMove)
                    {
                        // we only can move on z 
                        moveDir = moveDirZ;
                    }
                    else
                    {
                        // we Cannot move in any direction
                    }
                }
            }


            if (canMove)
            {
                transform.position += moveDir * (moveDistance);
            }


            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * turningSpeed);
        }
    }
}