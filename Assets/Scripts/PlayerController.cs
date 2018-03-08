using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    private float horizontalLookSensitivity = 0.01f;
	private float verticalLookSensitivity = 1f;

	[SerializeField]
	private float thrusterForce = 1000f;

	[Header("Spring Settings:")]
	[SerializeField]
	private JointDriveMode jointMode=JointDriveMode.Position;
	[SerializeField]
	private float jointSpring=20.0f;
	[SerializeField]
	private float jointMaxForce=40.0f;

	private ConfigurableJoint joint;






	private PlayerMotor motor;


    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint> ();
		setJointSettings (jointSpring);
    }


    private void Update()
    {
        //Calculate movement velocity as 3D vector
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _zMove;


        //final movement vector
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        motor.Move(_velocity);
        //Calculate rotation as  3D vector
        float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0,_yRot,0)*horizontalLookSensitivity;
        motor.Rotate(_rotation);



        float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(_xRot, 0, 0) *verticalLookSensitivity;

        motor.CameraRotate(_cameraRotation);

		//Apply thruster force
		Vector3 _thrusterForce = Vector3.zero;
		if (Input.GetKey (KeyCode.Space)) {
			_thrusterForce = Vector3.up * thrusterForce;
			//	setJointSettings (0);
			joint = null;
		} else {
			if (joint == null) {
				joint = GetComponent<ConfigurableJoint> ();
				setJointSettings (jointSpring);
			}
		}
	
		//Motor will apply force 

		motor.applyThrusterForce (_thrusterForce);
    }

	private void setJointSettings(float _jointSpring)
	{
		joint.yDrive = new JointDrive { mode=jointMode,
			positionSpring=jointSpring,
			maximumForce=jointMaxForce
		};
		

	}

}
