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
	private float thrusterPower = 1.0f;


	private Ray ray;

	private PlayerMotor motor;
	private RaycastHit hit;

	private void Awake()
	{


	}

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint> ();
		ray.origin = gameObject.transform.position;
		ray.direction = Vector3.down;
		setJointSettings (jointSpring);
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
		float yRotation = _yRot * horizontalLookSensitivity;
	
		Vector3 _rotation = new Vector3(0,yRotation,0);
        motor.Rotate(_rotation);



        float _xRot = Input.GetAxisRaw("Mouse Y");
		float _xRotation = _xRot * verticalLookSensitivity;

		if (_xRotation >= 85f) {
			_xRotation = 85f;
		}
		if (_xRotation <= -85f) {
			_xRotation = 85f;
		}
		Vector3 _cameraRotation = new Vector3(_xRotation, 0, 0) ;

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
		ray.origin = gameObject.transform.position;
		ray.direction = Vector3.down;
		if (Physics.Raycast (ray, out hit,100f)) 
		{
			joint.targetPosition = new Vector3 (0, -(hit.point.y)+0.5f, 0);

			Debug.Log (hit.collider.name);
			Debug.DrawRay (gameObject.transform.position,Vector3.down);
		}
		else 
		{
			joint.targetPosition = Vector3.zero;
		}
		if (Input.GetKey (KeyCode.Space) && thrusterPower>=0) 
		{
			
			_thrusterForce = Vector3.up * thrusterForce;
			gameObject.GetComponent<ConfigurableJoint> ().yDrive = new JointDrive{ 
				positionSpring=0,
				maximumForce=jointMaxForce
			};
			thrusterPower -= 2*Time.deltaTime;	
			//setJointSettings (0);

		}
		else 
		{	
			thrusterPower += Time.deltaTime;
			if (thrusterPower >= 1f) {
				thrusterPower = 1f;
			}
			_thrusterForce = Vector3.zero;
			gameObject.GetComponent<ConfigurableJoint> ().yDrive = new JointDrive {
				positionSpring = jointSpring,
				maximumForce = jointMaxForce
			};
			setJointSettings (jointSpring);
		}


		motor.applyThrusterForce (_thrusterForce);
    }

	private void setJointSettings(float _jointSpring)
	{
		joint.yDrive = new JointDrive 
			{
			
			positionSpring=jointSpring,
			maximumForce=jointMaxForce
		};
		

	}

}
