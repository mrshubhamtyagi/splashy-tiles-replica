using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;

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

    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        float _x = speed * inputHorizontal * Time.fixedDeltaTime;
        velocity = new Vector3(_x, rb.velocity.y, rb.velocity.z);
        rb.velocity = velocity;
        rb.transform.position = new Vector3(Mathf.Clamp(rb.transform.position.x, -3f, 3f), Mathf.Clamp(rb.transform.position.y, -10f, 2f), rb.velocity.z);
    }
}
