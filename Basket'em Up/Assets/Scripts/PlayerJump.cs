using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    private GameObject self;
    private Transform selfTransform;
    private Rigidbody selfRig;
    private Vector3 jumpDirection;
    private float downModificator;
    private float distanceToGround;

    [Header("Jump settings")]
    public float jumpForce = 25f; //In meters
    public float minimalHeight = 1.2f;

    [Header("Jump variables")]
    public bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        self = gameObject;
        if (self.GetComponent<Rigidbody>()) { selfRig = self.GetComponent<Rigidbody>(); }
        selfTransform = transform;
        downModificator = jumpForce / 2;
    }

    private void Update()
    {
        RaycastHit hit;

        Physics.Raycast(selfTransform.position, Vector3.down, out hit, Mathf.Infinity);
        distanceToGround = ( hit.point - selfTransform.position).magnitude;
        Debug.DrawLine(selfTransform.position, hit.point, Color.blue, 1f);
    }

    public IEnumerator Jump()
    {
        if (isJumping) { StopAllCoroutines(); }
        isJumping = true;

        jumpDirection = (selfTransform.forward + Vector3.up * 3f).normalized;

        Debug.DrawLine(selfTransform.position, selfTransform.position+jumpDirection, Color.green, 1f);

        selfRig.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        float t = 0;
        while (distanceToGround > minimalHeight && isJumping)
        {
            float forceDown = distanceToGround * Mathf.Lerp(0, downModificator, t);
            print("force down " + forceDown);
            selfRig.AddForce(Vector3.down* forceDown, ForceMode.Force);
            t += Time.deltaTime;
            yield return null;
        }

        if (distanceToGround <= minimalHeight)
        {
            t = 0;
            isJumping = false;
        }

        //Physics.gravity = GameManager.i.baseGravity;
        
    }
}
