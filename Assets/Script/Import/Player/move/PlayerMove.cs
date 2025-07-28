using Fusion;
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
    [SerializeField] private float gravityValue = -9.8f;
    public float _speedRun = 15f;
    public float _speedWalk = 7f;

    private Camera _mainCamera;
    private CharacterController _player;
    private Vector3 _movementDirection;


    public void Move(float delta )
    {
        MoveThePlayer(delta);
        TurnThePlayer();
    }

    private void MoveThePlayer(float delta)
    {
        Vector3 gravity = Vector3.zero;

        Vector3 movement = CameraDirection(_movementDirection) * _movementSpeed;
       
        if (_player.isGrounded || gravity.y < 0)
        {
            gravity.y = 0;
        }

        movement.y += gravityValue * delta;
        _player.Move(movement * delta);

        //_player.Move(gravity * delta);
    }

    private void TurnThePlayer()
    {
        if (_movementDirection.sqrMagnitude > 0.01f)
        {

            Quaternion rotation = Quaternion.Slerp(transform.rotation,
                                                 Quaternion.LookRotation(CameraDirection(_movementDirection)),
                                                 _turnSpeed);
           transform.rotation = rotation;

            //Quaternion rotation = Quaternion.Slerp(_playerRigidbody.rotation,
            //                                     Quaternion.LookRotation(CameraDirection(_movementDirection)),
            //                                     _turnSpeed);

            //_playerRigidbody.MoveRotation(rotation);
        }
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        _movementDirection = newMovementDirection;
    }

    public void SetupPlayerMove()
    {
        _player = GetComponent<CharacterController>();

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
