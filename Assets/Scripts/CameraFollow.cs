using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 distanceFromTarget;
    public float followSpeed = 0.5f;

    public bool isStatic = false;

    private void Start()
    {
        transform.position = distanceFromTarget;
    }

    private void FixedUpdate()
    {
        if (!isStatic)
        {
            Vector3 targetpos = target.localPosition + distanceFromTarget;
            targetpos.y = distanceFromTarget.y;
            transform.position = Vector3.Lerp(transform.position, targetpos, 0.2f);
        }
    }

}
