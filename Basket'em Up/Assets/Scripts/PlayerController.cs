using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum MoveState
{
    Idle,
    Walk,
    Steer
}

public class PlayerController : MonoBehaviour
{

    static public PlayerController instance;

    [Header("Components")]
    public Transform self;
    public Rigidbody body;
    public float deadzone = 0.2f;

    [Space(2)]
    [Header("Controls")]
    public MoveState moveState;
    [SerializeField] private float speed;

    [Tooltip("Minimum required speed to go to walking state")] public float minWalkSpeed = 0.1f;
    public float maxSpeed = 10;
    public float maxAcceleration = 10;
    public AnimationCurve accelerationCurve;

    [Space(2)]
    public float movingDrag = .4f;
    public float idleDrag = .4f;

    [Space(2)]
    [Range(0.01f, 1f)]
    public float turnSpeed = .25f;
    public AnimationCurve walkAnimationSpeedCurve;


    Vector3 speedVector;
    float accelerationTimer;
    Vector3 lastVelocity;
    Vector3 input;
    Quaternion turnRotation;
    float distance;
    bool inputDisabled;

    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        CheckMoveState();
        if (input.magnitude != 0)
        {
            accelerationTimer += Time.fixedDeltaTime;
            Rotate();
            Accelerate();
        }
        else
        {
            accelerationTimer = 0;
        }
        Move();

    }

    #region Input
    void GetInput()
    {
        if (inputDisabled) { input = Vector3.zero; return; }
        if (HasGamepad())
        {
            GamepadInput();
        }
        else
        {
            KeyboardInput();
        }
    }

    void GamepadInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        input = input.normalized * ((input.magnitude - deadzone) / (1 - deadzone));
    }

    void KeyboardInput()
    {

        int _horDir = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _horDir--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _horDir++;
        }

        int _vertDir = 0;
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _vertDir--;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _vertDir++;
        }
        input = new Vector3(_horDir, 0, _vertDir);
        input.Normalize();
    }

    bool HasGamepad()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Movement

    void CheckMoveState()
    {
        if (body.velocity.magnitude <= minWalkSpeed)
        {
            if (moveState != MoveState.Idle)
            {
                body.velocity = Vector3.zero;
            }
            body.drag = idleDrag;
            moveState = MoveState.Idle;
        }
        else if (moveState != MoveState.Steer)
        {
            if (input == Vector3.zero)
            {
                body.drag = idleDrag;
            }
            else
            {
                body.drag = movingDrag;
            }
            moveState = MoveState.Walk;
        }
    }

    void Rotate()
    {
        turnRotation = Quaternion.Euler(0, Mathf.Atan2(input.x, input.z) * 180 / Mathf.PI, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, turnSpeed);
    }

    void Accelerate()
    {
        body.AddForce(input * (accelerationCurve.Evaluate(body.velocity.magnitude / maxSpeed) * maxAcceleration), ForceMode.Acceleration);
        body.drag = movingDrag;
    }

    void Move()
    {
        body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);
        speed = body.velocity.magnitude;
    }
    #endregion

    #region Functions
    public void DisableInput()
    {
        inputDisabled = true;
    }

    public void EnableInput()
    {
        inputDisabled = false;
    }
    #endregion
}
