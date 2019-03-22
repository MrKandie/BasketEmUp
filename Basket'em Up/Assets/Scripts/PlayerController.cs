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
    public Transform hand;

    [Space(2)]
    [Header("General settings")]
    public Color playerColor;

    [Space(2)]
    [Header("Movement settings")]
    public MoveState moveState;
    public float speed;
    public AnimationCurve accelerationCurve;

    [Tooltip("Minimum required speed to go to walking state")] public float minWalkSpeed = 0.1f;
    public float maxSpeed = 10;
    public float maxAcceleration = 10;

    [Space(2)]
    public float movingDrag = .4f;
    public float idleDrag = .4f;

    [Space(2)]
    [Range(0.01f, 1f)]
    public float turnSpeed = .25f;
    public AnimationCurve walkAnimationSpeedCurve;

    [Space(2)]
    [Header("Pass settings")]
    [Range(0, 180f)]
    [Tooltip("angle treshold to target something, big values mean it's easier to target something")] public float targetAngleTreshold = 30;

    [Space(2)]
    [Header("Other settings")]


    Vector3 speedVector;
    float accelerationTimer;
    Vector3 lastVelocity;
    Vector3 input;
    Quaternion turnRotation;
    float distance;
    bool inputDisabled;
    GameObject highlighter;
    Ball possessedBall;
    GameObject target;

    private void Awake()
    {
        GenerateHighlighter().SetActive(false); ;
    }

    void Update()
    {
        GetInput();
        if (inputDisabled) { return; }
        target = GetTargetedObject();
        HighlightTarget();
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

    void OnGUI()
    {
        if (inputDisabled) { return; }
        //Displays the mouse angle on the GUI for the active player
        GUI.contentColor = new Color(0, 0, 0, 1);
        GUI.Label(new Rect(25, 25, 200, 40), "Mouse angle " + GetAngle(new Vector2(self.transform.position.x, self.transform.position.z), new Vector2(self.transform.position.x, self.transform.position.z) + GetMouseDirection()));
    }

    #region Input
    void GetInput()
    {
        if (inputDisabled) { input = Vector3.zero; return; }

        if (Input.GetMouseButtonDown(0) && target != null && target.GetComponent<PlayerController>() != null)
        {
            PassBall(target.GetComponent<PlayerController>(), GameManager.i.momentumManager.momentum);
        }
        if (Input.GetMouseButtonDown(1))
        {
            TakeBall(FindObjectOfType<Ball>(), 1);
        }

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
        string[] names = Input.GetJoystickNames();
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Length > 0)
            {
                return true;
            }
        }
        return false;
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

    #region Public functions
    public void DisableInput()
    {
        inputDisabled = true;
    }

    public void EnableInput()
    {
        inputDisabled = false;
    }

    //Highlight the target with the correspunding player color
    public void HighlightGameObject(GameObject target)
    {
        highlighter.transform.SetParent(target.transform, false);
        highlighter.SetActive(true);
    }

    public void StopHighlight()
    {
        highlighter.transform.SetParent(self, false);
        highlighter.SetActive(false);
    }

    //Returns the mouse direction vector
    public Vector2 GetMouseDirection()
    {
        if (HasGamepad())
        {
            return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        } else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector2 mousePosition = new Vector2(hit.point.x, hit.point.z);
                mousePosition -= new Vector2(self.position.x, self.position.z);
                return mousePosition;
            }
        }
        return Vector2.zero;
    }

    public void HighlightTarget()
    {
        if (target == null || possessedBall == null)
        {
            StopHighlight();
        }
        else
        {
            HighlightGameObject(target);
        }
    }

    //Returns the nearest object to the mouse position
    public GameObject GetTargetedObject()
    {
        if (HasGamepad() && GetMouseDirection().magnitude == 0) { return null; }
        Vector2 positionVec2 = new Vector2(self.position.x, self.position.z);
        float mouseAngle = GetAngle(positionVec2, positionVec2 + GetMouseDirection());

        List<GameObject> potentialTargets = new List<GameObject>();
        List<GameObject> acceptedTargets = new List<GameObject>();

        //Generates the potential target list
        for (int i = 0; i < GameManager.i.playerList.Count; i++)
        {
            if (GameManager.i.playerList[i] != this)
            {
                potentialTargets.Add(GameManager.i.playerList[i].self.gameObject);
            }
        }

        //Get the targets in the correct direction
        foreach (GameObject obj in potentialTargets)
        {
            Vector2 objPositionVec2 = new Vector2(obj.transform.position.x, obj.transform.position.z);
            float targetAngle = GetAngle(positionVec2, objPositionVec2);
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(targetAngle, mouseAngle));
            if (angleDifference <= targetAngleTreshold)
            {
                acceptedTargets.Add(obj);
            }
        }

        //Get the nearest target between all the correct targets
        if (acceptedTargets.Count > 0)
        {
            GameObject nearestObject = acceptedTargets[0];
            float nearestDistance = Vector3.Distance(nearestObject.transform.position, self.transform.position);
            foreach (GameObject acceptedTarget in acceptedTargets)
            {
                if (Vector3.Distance(acceptedTarget.transform.position, self.transform.position) < nearestDistance)
                {
                    nearestObject = acceptedTarget;
                    nearestDistance = Vector3.Distance(acceptedTarget.transform.position, self.transform.position);
                }
            }
            return nearestObject;
        }
        else
        {
            return null;
        }
    }

    //Drops the ball
    public void DropBall()
    {
        if (possessedBall == null) { return; }
        possessedBall.transform.SetParent(null);
        possessedBall.holder = null;
        possessedBall = null;
    }

    //Gives the ball to the player
    public void TakeBall(Ball ball, float time)
    {
        if (ball.holder != null)
        {
            ball.holder.DropBall();
        }
        StartCoroutine(TakeBall_C(ball, time));
    }

    //Pass the ball to a specific player
    public void PassBall(PlayerController player, float momentum)
    {
        //Conditions
        if (player == this) { Debug.LogWarning("Can't pass ball to yourself"); return; }
        if (possessedBall == null) { return; }

        //Function
        StartCoroutine(PassBall_C(possessedBall, player, momentum));
        DropBall();
    }

    //Returns the angle between two positions
    public float GetAngle(Vector2 initial, Vector2 target)
    {
        Vector2 dir = initial - target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
    #endregion

    #region Coroutines 
    IEnumerator TakeBall_C(Ball ball, float time)
    {
        Vector3 startPosition = ball.transform.position;
        for (float i = 0; i < time; i+=Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            ball.transform.position = Vector3.Lerp(startPosition, hand.transform.position, i / time);
        }
        possessedBall = ball;
        ball.holder = this;
        ball.transform.SetParent(hand.transform);
        yield return null;
    }

    IEnumerator PassBall_C(Ball ball, PlayerController player, float momentum)
    {
        Vector3 startPosition = ball.transform.position;
        Vector3 endPosition = player.hand.transform.position;

        float passSpeed = GameManager.i.momentumManager.GetPassSpeed();
        float passTime = Vector3.Distance(startPosition, endPosition) / passSpeed;
        AnimationCurve speedCurve = GameManager.i.momentumManager.passMovementCurve;
        AnimationCurve angleCurve = GameManager.i.momentumManager.passAngleCurve;

        ball.direction = endPosition - startPosition;
        for (float i = 0; i < passTime; i+=Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            //Apply speed curve
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, speedCurve.Evaluate(i / passTime));

            //Apply angle curve
            ball.transform.position = new Vector3(
                    ball.transform.position.x, 
                    startPosition.y + (angleCurve.Evaluate(i / passTime) * GameManager.i.momentumManager.GetPassHeight()), 
                    ball.transform.position.z
                );
        }
        ball.direction = Vector3.zero;
        player.TakeBall(ball, 0);
        yield return null;
    }
    #endregion

    #region Private functions
    private GameObject GenerateHighlighter()
    {
        if (highlighter != null) { return highlighter; }
        highlighter = Instantiate(GameManager.i.library.highlighter, self.transform, false);
        highlighter.transform.Find("Visuals").GetComponent<SpriteRenderer>().color = playerColor;
        return highlighter;
    }

    #endregion
}
