using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveState
{
    Idle,
    Walk,
    Blocked,
}

public class PlayerController : MonoBehaviour, iTarget
{

    static public PlayerController instance;

    [Header("Components")]
    public Transform self;
    public Rigidbody body;
    public float deadzone = 0.2f;
    public Transform hand;
    public Animator playerAnim;
    public ParticleSystem[] handoffEffects;
    public Camera cam;
    public GameObject groundDunkEffect;
    public ParticleSystem ballReceivedParticleSystem;
    public PlaneBetweenPlayers planeBetweenPlayers;

    [SerializeField]
    private Transform _targetedTransform;
    public Transform targetedTransform { get { return _targetedTransform; } set { _targetedTransform = value; } } //The position where the ball will land when someone shoot at this player

    [Space(2)]
    [Header("General settings")]
    public int inputIndex;
    public Color playerColor;
    public float playerHeight = 2.2f;
    public int MaxHP;

    [Space(2)]
    [Header("Movement settings")]
    public MoveState moveState;
    public AnimationCurve accelerationCurve;


    [Tooltip("Minimum required speed to go to walking state")] public float minWalkSpeed = 0.1f;
    public float maxSpeedMin = 9;
    public float maxSpeedMax = 11;
    public float maxAcceleration = 10;

    [Space(2)]
    public float movingDrag = .4f;
    public float idleDrag = .4f;
    public float onGroundGravityMultiplyer;

    [Space(2)]
    [Range(0.01f, 1f)]
    public float turnSpeed = .25f;
    public AnimationCurve walkAnimationSpeedCurve;

    [Space(2)]
    [Header("Dunk settings")]
    public float dunkExplosionRadius = 10;
    public float dunkExplosionForce = 5000;
    public int dunkExplosionDamage = 20;

    public float dunkJumpHeight; //In meters
    public float dunkJumpDistance; //In meters
    public float dunkJumpSpeed; //In m/s
    public float dunkTime; //In seconds 
    public float dunkClimaxTime = 0.3f; //In seconds
    [MinMaxSlider(0, 1)]
    public Vector2 dunkTreshold; //The moment when the player can receive the ball and do a dunk
    public AnimationCurve dunkJumpSpeedCurve;
    public AnimationCurve dunkJumpMovementCurve;

    [Space(2)]
    [Header("Pass settings")]
    [Range(0, 180f)]
    [Tooltip("angle treshold to target something, big values mean it's easier to target something")] public float targetAngleTreshold = 30;

    [Space(2)]
    [Header("Debug")]
    public int currentHP;
    Vector3 speedVector;
    float accelerationTimer;
    Vector3 lastVelocity;
    Vector3 input;
    Quaternion turnRotation;
    float distance;
    bool inputDisabled;
    GameObject highlighter;
    [HideInInspector] public Ball possessedBall;
    public iTarget target; //The object targeted by this player
    Coroutine dunkJumpCoroutine;
    float speed;
    float customDrag;
    float customGravity;
    float maxSpeed;
    [HideInInspector] public GameObject targetedBy; //The object targeting this player
    [HideInInspector] public bool doingHandoff;
    [HideInInspector] public Transform handoffTarget;
    bool isJumping;

    private void Awake()
    {
        GenerateHighlighter().SetActive(false);
        customGravity = onGroundGravityMultiplyer;
        customDrag = idleDrag;
        currentHP = MaxHP;
        maxSpeed = maxSpeedMin;
    }

    void Update()
    {
        GetInput();
        if (inputDisabled) { return; }
        HighlightTarget();
        UpdateMaxSpeed();
    }

    private void FixedUpdate()
    {
        CheckMoveState();
        Rotate();
        if (input.magnitude != 0)
        {
            accelerationTimer += Time.fixedDeltaTime;
            Accelerate();
        }
        else
        {
            accelerationTimer = 0;
        }
        Move();
        ApplyDrag();
        ApplyCustomGravity();
        UpdateAnimatorBlendTree();
    }

    #region Input
    void GetInput()
    {
        if (inputDisabled) { input = Vector3.zero; return; }


        GeneralInput();
        if (HasGamepad())
        {
            GamepadInput();
        }
        else
        {
            KeyboardInput();
        }
    }

    void GeneralInput()
    {
        if (isJumping) { return; }
        if (Input.GetButtonDown("Dunk_" + inputIndex.ToString()) && dunkJumpCoroutine == null)
        {
            StartDunk();
        }
        if (Input.GetButtonDown("Handoff_" + inputIndex.ToString()))
        {
            playerAnim.SetTrigger("HandoffTrigger");
            List<iTarget> ally = new List<iTarget>();
            foreach (iTarget target in GameManager.i.levelManager.GetTargetableAllies())
            {
                ally.Add(target);
            }
            ally.Remove(this);
            target = ally[0];
            PassBall(target, GameManager.i.momentumManager.momentum);
            target = null;
        }
        if (Input.GetMouseButtonDown(1))
        {
            TakeBall(GameManager.i.levelManager.activeBall, 0.1f);
        }
    }

    void GamepadInput()
    {
        Vector3 _inputX = Input.GetAxisRaw("Horizontal_" + inputIndex.ToString()) * cam.transform.right;
        Vector3 _inputZ = Input.GetAxisRaw("Vertical_" + inputIndex.ToString()) * cam.transform.forward;
        input = _inputX + _inputZ;
        input.y = 0;
        input = input.normalized * ((input.magnitude - deadzone) / (1 - deadzone));
        Debug.DrawLine(transform.position, transform.position + input * 10);
    }

    void KeyboardInput()
    {
        Vector3 _inputX = Input.GetAxisRaw("Horizontal_" + inputIndex.ToString()) * cam.transform.right;
        Vector3 _inputZ = Input.GetAxisRaw("Vertical_" + inputIndex.ToString()) * cam.transform.forward;
        input = _inputX + _inputZ;
        input.y = 0;
        input.Normalize();
        Debug.DrawLine(transform.position, transform.position + input * 10);
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
        if (moveState == MoveState.Blocked) { return; }

        else if (body.velocity.magnitude <= minWalkSpeed)
        {
            if (moveState != MoveState.Idle)
            {
                body.velocity = new Vector3(0, body.velocity.y, 0);
            }
            customDrag = idleDrag;
            moveState = MoveState.Idle;
        }
    }

    void Rotate()
    {
        if (doingHandoff && handoffTarget != null)
            turnRotation = Quaternion.LookRotation(handoffTarget.position - self.position);
        else if (targetedBy != null)
            turnRotation = Quaternion.LookRotation(targetedBy.transform.position - self.position);
        else if (input.magnitude >= 0.1f)
            turnRotation = Quaternion.Euler(0, Mathf.Atan2(input.x, input.z) * 180 / Mathf.PI, 0);

        self.rotation = Quaternion.Slerp(transform.rotation, turnRotation, turnSpeed);
    }

    void Accelerate()
    {
        if (moveState == MoveState.Blocked) { return; }
        body.AddForce(input * (accelerationCurve.Evaluate(body.velocity.magnitude / maxSpeed) * maxAcceleration), ForceMode.Acceleration);
        customDrag = movingDrag;
    }

    void Move()
    {
        if (moveState == MoveState.Blocked) { speed = 0; return; }
        Vector3 myVel = body.velocity;
        myVel.y = 0;
        myVel = Vector3.ClampMagnitude(myVel, maxSpeed);
        myVel.y = body.velocity.y;
        body.velocity = myVel;
        speed = body.velocity.magnitude;
    }

    void ApplyDrag()
    {
        Vector3 myVel = body.velocity;
        myVel.x *= 1 - customDrag;
        myVel.z *= 1 - customDrag;
        body.velocity = myVel;
    }

    void ApplyCustomGravity()
    {
        body.AddForce(new Vector3(0, -9.81f* customGravity, 0));
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
            return new Vector2(Input.GetAxisRaw("Mouse X_" + inputIndex.ToString()), Input.GetAxisRaw("Mouse Y_" + inputIndex.ToString()));
        } else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector2 mousePosition = new Vector2(hit.point.x, hit.point.z);
                mousePosition -= new Vector2(self.position.x, self.position.z);
                //shootQuadTransform.rotation = Quaternion.LookRotation((new Vector3(mousePosition.x, 0, mousePosition.y) + self.position) - self.position);
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
            MonoBehaviour targetScript = target as MonoBehaviour;
            GameObject targetGameObject = targetScript.gameObject;
            HighlightGameObject(targetGameObject);
        }
    }

    //Returns the nearest object to the mouse position
    public iTarget GetTargetedObject(List<iTarget> potentialTargets)
    {
        if (HasGamepad() && GetMouseDirection().magnitude == 0) { return null; }
        Vector2 positionVec2 = new Vector2(self.position.x, self.position.z);
        float mouseAngle = GetAngle(positionVec2, positionVec2 + GetMouseDirection());

        List<iTarget> acceptedTargets = new List<iTarget>();

        //Get the targets in the correct direction
        foreach (iTarget target in potentialTargets)
        {
            Vector2 targetPositionVec2 = new Vector2(target.targetedTransform.position.x, target.targetedTransform.position.z);
            float targetAngle = GetAngle(positionVec2, targetPositionVec2);
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(targetAngle, mouseAngle));
            if (angleDifference <= targetAngleTreshold)
            {
                acceptedTargets.Add(target);
            }
        }

        //Get the nearest target between all the correct targets
        if (acceptedTargets.Count > 0)
        {
            iTarget nearestTarget = acceptedTargets[0];
            float nearestDistance = Vector3.Distance(nearestTarget.targetedTransform.position, self.transform.position);
            foreach (iTarget acceptedTarget in acceptedTargets)
            {
                if (Vector3.Distance(acceptedTarget.targetedTransform.position, self.transform.position) < nearestDistance)
                {
                    nearestTarget = acceptedTarget;
                    nearestDistance = Vector3.Distance(acceptedTarget.targetedTransform.position, self.transform.position);
                }
            }
            return nearestTarget;
        }
        else
        {
            return null;
        }
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void AddDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        if (currentHP <= 0)
        {
            //Kill(); (Not for debug)
        }
    }

    public void Push(Vector3 direction, float magnitude)
    {
        direction = direction.normalized;
        direction = direction * magnitude;
        body.AddForce(Vector3.up * 2, ForceMode.Impulse);
        body.AddForce(direction, ForceMode.Impulse);
    }

    //Jump to start the dunk
    public void StartDunk()
    {
        if (possessedBall == null)
        {
            dunkJumpCoroutine = StartCoroutine(StartDunk_C());
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

    public void CancelHandoff()
    {
        handoffTarget = null;
        doingHandoff = false;
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
    public void PassBall(iTarget target, float momentum)
    {
        //Conditions
        MonoBehaviour targetScript = target as MonoBehaviour;
        if (targetScript == this) { Debug.LogWarning("Can't pass ball to yourself"); return; }
        if (possessedBall == null) { return; }
        if (targetScript.GetType() == typeof(PlayerController)) { possessedBall.triggerEnabled = true; }

        //Function
        StartCoroutine(PassBall_C(possessedBall, target, momentum));
        DropBall();
    }

    //Returns the angle between two positions
    public float GetAngle(Vector2 initial, Vector2 target)
    {
        Vector2 dir = initial - target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = - angle;
        return angle;
    }
    #endregion

    #region Coroutines 
    IEnumerator StartDunk_C()
    {
        isJumping = true;
        customGravity = 0;

        Vector3 startPosition = self.position;
        Vector3 endPosition = GameManager.i.GetGroundPosition(self.position + (self.forward * dunkJumpDistance)) + new Vector3(0,playerHeight/2f,0);
        float dunkJumpTime = Vector3.Distance(startPosition, endPosition) / dunkJumpSpeed;

        playerAnim.SetTrigger("PrepareDunkTrigger");

        for (float i = 0; i < dunkJumpTime; i+= Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            self.position = Vector3.Lerp(startPosition, endPosition, dunkJumpSpeedCurve.Evaluate(i / dunkJumpTime));
            float height = dunkJumpMovementCurve.Evaluate(i / dunkJumpTime) * dunkJumpHeight;
            self.position = new Vector3(self.position.x, self.position.y + height, self.position.z);
            if (possessedBall == true)
            {
                playerAnim.SetTrigger("DunkTrigger");
                StartCoroutine(Dunk_C(endPosition));
                StopCoroutine(dunkJumpCoroutine);
            }
        }
        isJumping = false;
        playerAnim.SetTrigger("DunkMissedTrigger");
        customGravity = onGroundGravityMultiplyer;
        dunkJumpCoroutine = null;
    }

    IEnumerator Dunk_C(Vector3 position)
    {
        moveState = MoveState.Blocked;
        for (float i = 0; i < dunkClimaxTime; i+= Time.deltaTime)
        {
            self.position = self.position;
            yield return new WaitForEndOfFrame();
        }
        Vector3 startPosition = self.position;
        for (float i = 0; i < dunkTime; i+=Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            self.position = Vector3.Lerp(startPosition, position, i / dunkTime);
        }
        isJumping = false;
        possessedBall.transform.localPosition = Vector3.zero;
        GameManager.i.soundManager.PlaySound(GameManager.i.soundManager.dunk, false);
        GenerateDunkExplosion(position, dunkExplosionRadius, dunkExplosionForce, dunkExplosionDamage);
        self.position = position;
        customGravity = onGroundGravityMultiplyer;
        dunkJumpCoroutine = null;
        moveState = MoveState.Blocked;
        GameManager.i.momentumManager.DecrementMomentum(GameManager.i.momentumManager.momentum);
        yield return null;
    }

    IEnumerator TakeBall_C(Ball ball, float time)
    {
        ball.SetState(BallMoveState.Moving);
        ball.StopAllCoroutines();
        Vector3 startPosition = ball.transform.position;
        Vector3 endPosition = hand.transform.position;
        for (float i = 0; i < time; i+=Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, i / time);
        }
        ball.transform.position = endPosition;
        possessedBall = ball;
        ball.holder = this;
        ball.transform.SetParent(hand.transform);
        ball.transform.localPosition = Vector3.zero;
        GameManager.i.soundManager.PlayRandomSound(GameManager.i.soundManager.ballCatch, true);
        yield return null;
    }

    IEnumerator PassBall_C(Ball ball, iTarget target, float momentum)
    {
        target.OnTargetedBySomeone(self.transform);
        doingHandoff = true;
        Vector3 startPosition = ball.transform.position;
        Vector3 endPosition = target.targetedTransform.position;
        handoffTarget = target.targetedTransform;
        GameManager.i.soundManager.PlayRandomSound(GameManager.i.soundManager.whoosh, true);

        //Rotate players towards target and play particles
        self.rotation = Quaternion.LookRotation(handoffTarget.position - self.position);
        for (int i = 0; i < handoffEffects.Length; i++)
        {
            handoffEffects[i].Play();
        }

        float passSpeed = GameManager.i.ballManager.GetPassSpeed();
        float passTime = Vector3.Distance(startPosition, endPosition) / passSpeed;
        AnimationCurve speedCurve = GameManager.i.ballManager.passMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballManager.passAngleCurve;

        ball.direction = endPosition - startPosition;
        for (float i = 0; i < passTime; i+=Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            //Apply speed curve
            ball.transform.position = Vector3.Lerp(startPosition, target.targetedTransform.position, speedCurve.Evaluate(i / passTime));

            //Apply angle curve
            ball.transform.position = new Vector3(
                    ball.transform.position.x, 
                    ball.transform.position.y + (angleCurve.Evaluate(i / passTime) * GameManager.i.ballManager.GetPassHeight()), 
                    ball.transform.position.z
                );
        }
        ball.transform.position = target.targetedTransform.position;
        ball.triggerEnabled = false;
        target.OnBallReceived(ball);
        ball.direction = Vector3.zero;
        CancelHandoff();
        yield return null;
    }
    #endregion

    #region Private functions
    private void UpdateMaxSpeed()
    {
        maxSpeed = Mathf.Lerp(maxSpeedMin, maxSpeedMax, GameManager.i.momentumManager.momentum);
    }

    private void GenerateDunkExplosion(Vector3 position, float radius, float power, int damages)
    {
        float trueRadius = radius * GameManager.i.momentumManager.momentum;
        float truePower = power * GameManager.i.momentumManager.momentum;
        int trueDamages = Mathf.RoundToInt(damages * GameManager.i.momentumManager.momentum);

        Collider[] colliders = Physics.OverlapSphere(position, trueRadius);
        foreach (Collider hit in colliders)
        {
            Enemy potentialEnemy = hit.gameObject.GetComponent<Enemy>();
            if (potentialEnemy != null)
            {
                potentialEnemy.DisableNavmeshAgent();
                potentialEnemy.AddDamage(trueDamages);
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    //rb.AddForce(new Vector3(0, 5000, 0));
                    //rb.AddExplosionForce(truePower, position + new Vector3(0, -2, 0), trueRadius, 3.0F);
                }
            }
        }

        Vector3 spawnPosition = new Vector3(transform.position.x, 0.05f, transform.position.z) + transform.forward *2;
        spawnPosition.y = 0.05f;
        GameObject dunkFXRef = Instantiate(groundDunkEffect, spawnPosition, Quaternion.Euler(90, 0, 0));
        dunkFXRef.transform.localScale = new Vector3(trueRadius, trueRadius, trueRadius);
        Destroy(dunkFXRef, 2);
    }

    private GameObject GenerateHighlighter()
    {
        if (highlighter != null) { return highlighter; }
        highlighter = Instantiate(GameManager.i.library.highlighter, self.transform, false);
        highlighter.transform.Find("Visuals").GetComponent<SpriteRenderer>().color = playerColor;
        return highlighter;
    }
    private void UpdateAnimatorBlendTree()
    {
        playerAnim.SetFloat("IdleRunningBlend", speed / maxSpeed);
    }
    public void Freeze()
    {
        moveState = MoveState.Blocked;
    }

    public void UnFreeze()
    {
        moveState = MoveState.Idle;
    }

    public void OnBallReceived(Ball ball)
    {
        targetedBy = null;
        TakeBall(ball, 0);
        ballReceivedParticleSystem.Play();
    }

    public void OnTargetedBySomeone(Transform target)
    {
        targetedBy = target.gameObject;
    }

    #endregion
}
