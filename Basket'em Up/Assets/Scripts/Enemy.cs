using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, iTarget
{
    [Header("Components")]
    public Animator HitAnim;

    [Header("Settings")]
    public int HPmax = 100;
    public float invincibilityTime = 1f;

    [Header("Debug")]
    int HPcurrent;
    Rigidbody rb;
    float invincibilityCD;

    [SerializeField]
    private Transform _targetedTransform;
    public Transform targetedTransform { get { return _targetedTransform; } set { _targetedTransform = value; } } //The position where the ball will land when someone shoot at this player

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HPcurrent = HPmax;
    }

    private void Update()
    {
        if (invincibilityCD > 0)
        {
            invincibilityCD -= Time.deltaTime;
        }
        invincibilityCD = Mathf.Clamp(invincibilityCD, 0, Mathf.Infinity);
    }

    public void AddDamage(int amount)
    {
        if (invincibilityCD > 0) { return; } 
        invincibilityCD = invincibilityTime;
        HitAnim.SetTrigger("HitTrigger");

        HPcurrent -= amount;
        HPcurrent = Mathf.Clamp(HPcurrent, 0, HPmax);

        if (HPcurrent <= 0)
        {
            Kill();
        }
    }

    //Push the enemy toward a direction
    public void Push(Vector3 direction, float magnitude)
    {
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
        ball.BounceOnNearbyTargets();
    }

    public void OnTargetedBySomeone(Transform target)
    {

    }
}
