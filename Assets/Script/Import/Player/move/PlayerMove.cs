using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed
    {
        get { return _movementSpeed; }
        set
        {
            if (value < 0) value = 1;

            _movementSpeed = value;
        }
    }

    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _turnSpeed = 0.1f;
    public float _speedRun = 15f;
    public float _speedWalk = 7f;

    private Camera _mainCamera;
    private Rigidbody _playerRigidbody;
    private Vector3 _movementDirection;


    public void Move(float deltaTime)
    {
        MoveThePlayer(deltaTime);
        TurnThePlayer();
    }

    private void MoveThePlayer(float deltaTime)
    {
        Vector3 movement = CameraDirection(_movementDirection) * _movementSpeed * deltaTime;
        _playerRigidbody.MovePosition(transform.position + movement);
    }

    private void TurnThePlayer()
    {
        if (_movementDirection.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.Slerp(_playerRigidbody.rotation,
                                                 Quaternion.LookRotation(CameraDirection(_movementDirection)),
                                                 _turnSpeed);
            _playerRigidbody.MoveRotation(rotation);
        }
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        _movementDirection = newMovementDirection;
    }

    public void SetupPlayerMove()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    private Vector3 CameraDirection(Vector3 movementDirection)
    {
        if (_mainCamera == null) return Vector3.zero;
        var cameraForward = _mainCamera.transform.forward;
        var cameraRight = _mainCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        return cameraForward * movementDirection.z + cameraRight * movementDirection.x;
    }
}
