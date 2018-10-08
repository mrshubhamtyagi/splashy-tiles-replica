using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float sideSpeed;
    public float forwardSpeed;
    public bool move = false;
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

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        //velocity.x = sideSpeed * inputHorizontal * Time.fixedDeltaTime;
        //velocity.y = rb.velocity.y;

        //if (move)
        //    velocity.z = rb.velocity.z * forwardSpeed * Time.fixedDeltaTime;
        //else
        //    velocity.z = rb.velocity.z;

        //rb.velocity = velocity;
        //rb.transform.position = new Vector3(Mathf.Camp(rb.transform.position.x, -3f, 3f), Mathf.Clamp(rb.transform.position.y, -10f, 2f), rb.velocity.z);
    }

    private void OnValidate()
    {
        Physics.gravity = gravity;
    }
}
