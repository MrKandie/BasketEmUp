using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, iTarget
{
    [Header("Components")]
    public Animator enemyAnim;
    public Animator HitAnim;
    public Transform hitEffetTransform;

    [Header("Settings")]
    public int HPmax = 100;
    public float invincibilityTime = 1f;

    [Header("Debug")]
    int HPcurrent;
    Rigidbody rb;
    protected NavMeshAgent agent;
    float invincibilityCD;
    public bool agentDisabled;
    public bool tryingToEnableAgent;

    [SerializeField]
    private Transform _targetedTransform;
    public Transform targetedTransform { get { return _targetedTransform; } set { _targetedTransform = value; } } //The position where the ball will land when someone shoot at this player

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = 1;
        HPcurrent = HPmax;
    }

    virtual protected void Update()
    {
        if (agentDisabled && tryingToEnableAgent)
        {
            if (IsGrounded())
            {
                EnableNavmeshAgent();
            }
        }
        if (invincibilityCD > 0)
        {
            invincibilityCD -= Time.deltaTime;
        }
        invincibilityCD = Mathf.Clamp(invincibilityCD, 0, Mathf.Infinity);

        Quaternion _hitEffetRotation = Quaternion.LookRotation(Camera.main.transform.position - hitEffetTransform.position);
        _hitEffetRotation.eulerAngles = new Vector3(0, _hitEffetRotation.eulerAngles.y, 0);
        hitEffetTransform.rotation = _hitEffetRotation;
    }

    virtual public void AddDamage(int amount)
    {
        if (invincibilityCD > 0) { return; } 
        invincibilityCD = invincibilityTime;
        HitAnim.SetTrigger("HitTrigger");
        enemyAnim.SetTrigger("HitTrigger");

        HPcurrent -= amount;
        HPcurrent = Mathf.Clamp(HPcurrent, 0, HPmax);

        if (HPcurrent <= 0)
        {
            Kill();
        }
    }

    //Disable the navmesh agent to allow pushing or moving the rigidbody, until the gameobject touches the ground
    virtual public void DisableNavmeshAgent()
    {
        agentDisabled = true;
        agent.enabled = false;
        tryingToEnableAgent = false;
        StartCoroutine(TryToGroundAgent_C());
    }

    virtual public void EnableNavmeshAgent()
    {
        tryingToEnableAgent = false;
        agentDisabled = false;
        agent.enabled = true;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position,
            Vector3.down,
            out hit,2f))
        {
            return true;
        } else
        {
            return false;
        }
    }

    //Push the enemy toward a direction
    public void Push(Vector3 direction, float magnitude)
    {
        DisableNavmeshAgent();
        direction = direction.normalized;
        direction = direction * magnitude;
        rb.AddForce(direction, ForceMode.Impulse);
    }

    public void Kill()
    {
        GameManager.i.levelManager.RemoveTargetFromList(this);
        Destroy(this.gameObject);
    }

    public void OnBallReceived(Ball ball)
    {
        AddDamage(GameManager.i.ballMovementManager.GetDamages());
        ball.BounceOnNearbyTargets();
    }

    public void OnTargetedBySomeone(Transform target)
    {

    }

    IEnumerator TryToGroundAgent_C()
    {
        tryingToEnableAgent = false;
        yield return new WaitForSeconds(1f);
        tryingToEnableAgent = true;
    }
}
