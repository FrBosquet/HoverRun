using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public GameObject cam;
  public Rigidbody rb;
  public CheckpointManager checkpointManager;

  public float transversalDrag;
  public float longitudinalDrag;

  public bool isGrounded = false;
  public float camDistance = 3;

  public float maxSpeed = 5;
  public float forwardThrustForce;
  public float backwardThrustForce;
  public float steerForce;

  public float minSteerPercentage = 0.5f;

  private int nextCheckpoint = 0;

  private void FixedUpdate()
  {
    Vector3 velocity = rb.velocity;
    Vector3 longitudinalSpeed = Vector3.Project(velocity, transform.forward);
    Vector3 transversalSpeed = Vector3.Project(velocity, transform.right);
    float speed = velocity.magnitude;
    float speedRate = speed / maxSpeed;
    float limitSteer = 1 - (1 - minSteerPercentage) * (speedRate > 1 ? 1 : speedRate);

    float thrust = Input.GetAxis("Thrust");
    float steer = limitSteer * Input.GetAxis("Steer");

    float thrustForce = thrust > 0 ? forwardThrustForce : backwardThrustForce;

    if (velocity.magnitude > maxSpeed) thrustForce = 0;

    Vector3 longitudinalForce = transform.forward * thrust * thrustForce - longitudinalSpeed * longitudinalDrag;
    Vector3 transversalForce = -transversalSpeed * transversalDrag;
    Vector3 angularForce = transform.up * steer * steerForce;

    if (isGrounded)
    {
      rb.AddForce(longitudinalForce * Time.deltaTime);
      rb.AddForce(transversalForce * Time.deltaTime);
      transform.Rotate(angularForce * Time.deltaTime);

      positionCamera();
    }
  }

  private void positionCamera()
  {
    Vector3 camOffset = new Vector3(transform.forward.x * -1, camDistance / 10, transform.forward.z * -1).normalized * camDistance;

    cam.transform.position = transform.position + camOffset;
    cam.transform.LookAt(transform);
  }

  private void OnCollisionExit(Collision other)
  {
    if (other.gameObject.CompareTag("Ground") && isGrounded)
    {
      isGrounded = false;
    }
  }

  private void OnCollisionStay(Collision other)
  {
    if (other.gameObject.CompareTag("Ground"))
    {
      isGrounded = true;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Checkpoint"))
    {
      int index = checkpointManager.IndexOf(other.gameObject);

      if (index == nextCheckpoint)
      {
        nextCheckpoint++;

        Checkpoint checkpoint = other.GetComponent<Checkpoint>();

        other.gameObject.SetActive(false);

        if (checkpoint.Use())
        {
          Debug.Log("Checkpoint!");
        }
      }

    }
  }
}
