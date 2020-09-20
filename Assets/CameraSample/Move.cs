using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    public float speed;

    private Vector3 direction;
    private Vector3 forward;
    private Vector3 right;

    private Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        forward = this.transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
        right = this.transform.TransformDirection(Vector3.right) * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddForce(forward, ForceMode.VelocityChange);
            direction = forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddForce(-forward, ForceMode.VelocityChange);
            direction = -forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddForce(-right, ForceMode.VelocityChange);
            direction = -right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(right, ForceMode.VelocityChange);
            direction = right;
        }
    }
}
