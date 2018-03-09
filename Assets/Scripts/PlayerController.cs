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


	[SerializeField]
	private float thrusterPower = 3.0f;


	private PlayerMotor motor;


	private void Awake()
	{


	}

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint> ();

	//	setJointSettings (jointSpring);
    }


    private void Update()
    {
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _zMove;


    
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        motor.Move(_velocity);
      
        float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0,_yRot,0)*horizontalLookSensitivity;
        motor.Rotate(_rotation);



        float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(_xRot, 0, 0) *verticalLookSensitivity;

        motor.CameraRotate(_cameraRotation);


		Vector3 _thrusterForce = Vector3.zero;
		/*if (Input.GetKey (KeyCode.Space)) {
			if (thrusterPower >= 0) {
				_thrusterForce = Vector3.up * thrusterForce;
				gameObject.GetComponent<ConfigurableJoint> ().yDrive = new JointDrive{ positionSpring=jointSpring,maximumForce=0};
				thrusterPower -= Time.deltaTime;
				
			
			} else {
				gameObject.GetComponent<ConfigurableJoint> ().yDrive = new JointDrive{ positionSpring=jointSpring,maximumForce=1000};
				if (thrusterPower <= 3f) {
					thrusterPower += Time.deltaTime;
				}
			}
		
		} else {
			gameObject.GetComponent<ConfigurableJoint> ().yDrive = new JointDrive {positionSpring=jointSpring,maximumForce=1000};
		}*/
		if (Input.GetKey (KeyCode.Space)) 
		{
			_thrusterForce = Vector3.up * thrusterForce;


		}


		motor.applyThrusterForce (_thrusterForce);
    }

	private void setJointSettings(float _jointSpring)
	{
		joint.yDrive = new JointDrive {
			positionSpring=jointSpring,
			maximumForce=jointMaxForce
		};
		

	}

}
