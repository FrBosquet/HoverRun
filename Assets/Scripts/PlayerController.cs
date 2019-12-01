using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cam;
    public Rigidbody rb;

    public Vector3 velocity;
    public Vector3 transversalSpeed;
    public Vector3 longitudinalSpeed;

    public float transversalDrag;
    public float longitudinalDrag;

    public float camDistance = 3;

    public float thrustForce;
    public float backwardThrustForce;
    public float steerForce;


    private void FixedUpdate()
    {
        velocity = rb.velocity;

        longitudinalSpeed = Vector3.Project(velocity, transform.forward);
        transversalSpeed = Vector3.Project(velocity, transform.right);

        float thrust = Input.GetAxis("Thrust");
        float steer = Input.GetAxis("Steer");

        rb.AddForce(transform.forward * thrust * thrustForce * Time.deltaTime);
        rb.AddTorque(transform.up * steer * steerForce * Time.deltaTime);

        //Drag
        rb.AddForce(-longitudinalSpeed * longitudinalDrag * Time.deltaTime);
        rb.AddForce(-transversalSpeed * transversalDrag * Time.deltaTime);


        positionCamera();
    }

    private void positionCamera()
    {
        Vector3 camOffset = new Vector3(transform.forward.x * -1, camDistance / 10, transform.forward.z * -1).normalized * camDistance;

        cam.transform.position = transform.position + camOffset;
        cam.transform.LookAt(transform);
    }
}
