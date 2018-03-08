using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {
    [SerializeField]
   private Camera cam;





    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
	private Rigidbody rb;

	private Vector3 thrusterForce=Vector3.zero;

	private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    public void CameraRotate(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    //Physics iterations 
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();

    }
    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position+velocity*Time.deltaTime);
        }
		if (thrusterForce != Vector3.zero)
		{
		
			rb.AddForce (thrusterForce*Time.deltaTime,ForceMode.Acceleration);
		}
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation*Quaternion.EulerAngles(rotation));
        if (cam != null)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }



	public void applyThrusterForce(Vector3 _thrusterForce)
	{
		thrusterForce = _thrusterForce;
	}



}
