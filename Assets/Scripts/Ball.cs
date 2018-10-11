using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float sideSpeed;
    public float forwardSpeed;
    public bool isStatic = false;
    public Vector3 gravity = Vector3.zero;

    private Rigidbody rb;
    private float inputHorizontal;

    private Vector3 velocity;

    private void Awake()
    {
        if (GetComponent<Rigidbody>())
            rb = GetComponent<Rigidbody>();
        else
            Debug.LogError("RigidBody is mising");
    }

    void Start()
    {
        Physics.gravity = gravity;
    }

    private void FixedUpdate()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        velocity.x = sideSpeed * inputHorizontal * Time.deltaTime;
        velocity.y = rb.velocity.y;

        if (!isStatic)
            velocity.z = forwardSpeed * Time.deltaTime;
        else
            velocity.z = rb.velocity.z;
        rb.velocity = velocity;
    }

    private void OnValidate()
    {
        Physics.gravity = gravity;
    }
}
