using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public List<Transform> targets; //All targets that must be on the camera


    Vector3 centerPosition;

    private void Start()
    {
        //Updates the center position
        InvokeRepeating("UpdateCenterPosition", 0.1f, 0.1f);
    }

    private void Update()
    {
        //Move to the center position every seconds
        if (centerPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, centerPosition, Time.deltaTime * moveSpeed);
        }
    }

    public void UpdateCenterPosition()
    {
        centerPosition = GetCenterPosition();
    }

    //Returns the center position for the camera
    public Vector3 GetCenterPosition()
    {
        Vector2 targetMidPosition = GetTargetsMidPosition(targets);
        Vector3 center = new Vector3(targetMidPosition.x, transform.position.y, targetMidPosition.y);
        return center;
    }

    //Returns the position (x and z) between all the targets
    public Vector2 GetTargetsMidPosition(List<Transform> targets)
    {
        float totalX = 0;
        float totalZ = 0;
        foreach (Transform target in targets)
        {
            totalX += target.transform.position.x;
            totalZ += target.transform.position.z;
        }
        float centerX = totalX / targets.Count;
        float centerZ = totalZ / targets.Count;

        return new Vector2(centerX, centerZ);
    }

}
