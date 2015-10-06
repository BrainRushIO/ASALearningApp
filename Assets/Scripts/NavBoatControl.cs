using UnityEngine;
using System.Collections;

public class NavBoatControl : BoatBase {

	Rigidbody body;
	public enum BoatSideFacingWind {Port, Starboard};
	public static NavBoatControl s_instance;
	public ParticleSystem left, right;
	float currThrust = 0f;
	float weakThrust = 150f, strongThrust = 1500f;
	float angleToAdjustTo;
	float turnStrength = 5f, weakTurnStrength = 3f, strongTurnStrength = 5f;
	float turningRate = 60f;
	float rudderRotation =40f;
	float deadZone = 20f;

	Quaternion comeAboutStart, comeAboutEnd;
	Quaternion targetRudderRotation = Quaternion.identity;

	public bool canMove = false;
	public AudioSource correct;
	public Animator boatKeel;
	public GameObject arrow;
	public GameObject rudderR, rudderL;
	public GameObject redNavObj, greenNavObj;
	public Transform red1,red2,green1,green2;

	void Start () {
		body = GetComponent<Rigidbody>();
	}

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	void Update () {

		MastRotation();

		//add keeling into the boat rotation
		float animatorBlendVal;

		if (angleWRTWind < 360f && angleWRTWind > 180f) {
			animatorBlendVal = (angleWRTWind-180f)/360f;
		}
		else {
			animatorBlendVal = (angleWRTWind/360f + .5f);
		}

		boatKeel.SetFloat("rotation", animatorBlendVal);

		if (Mathf.Abs(Vector3.Angle(WindManager.s_instance.directionOfWind, transform.forward)) < deadZone) {
			if(currThrust > 0) {
				currThrust -= 10f;
			}
			else {
				currThrust = 0;
				left.enableEmission = false;
				right.enableEmission = false;

			}
			turnStrength = weakTurnStrength;

		}
	
		else {
			left.enableEmission = true;
			right.enableEmission = true;
			if (currThrust < strongThrust) {
				currThrust += 10f;
			}
			else {
				currThrust = strongThrust;
			}
			turnStrength = strongTurnStrength;

		}
		print (currThrust + "CT");
		if (NavManager.s_instance.gameState == NavManager.GameState.Win) {
			arrow.SetActive(false);
		}
	}

	void FixedUpdate () {
	
		if (canMove) {
			if(Input.GetKey(KeyCode.LeftArrow)) {
				body.AddRelativeTorque (-Vector3.up*turnStrength);
				targetRudderRotation = Quaternion.Euler(0, rudderRotation,0);
			}
			
			else if(Input.GetKey(KeyCode.RightArrow)) {
				body.AddRelativeTorque (Vector3.up*turnStrength);
				targetRudderRotation = Quaternion.Euler(0, -rudderRotation,0);

			}
			else {
				targetRudderRotation = Quaternion.identity;
	
			}

			rudderR.transform.localRotation = Quaternion.RotateTowards(rudderR.transform.localRotation, targetRudderRotation, turningRate * Time.deltaTime);
			rudderL.transform.localRotation = Quaternion.RotateTowards(rudderL.transform.localRotation, targetRudderRotation, turningRate * Time.deltaTime);

			body.AddForce (transform.forward * currThrust);
		}
	}



	void OnTriggerEnter(Collider other) {
//		print (other.tag + " " + NavManager.s_instance.ReturnCurrNavPointName());
		if (other.tag == "NavTarget" && other.name == NavManager.s_instance.ReturnCurrNavPointName() && Vector3.Distance(transform.position, other.transform.position) <100f) {
			NavManager.s_instance.SwitchNavigationPoint();
			correct.Play();
		}

		if (other.tag == "CollisionObject") {
			body.AddForce (transform.forward * -1 * currThrust);
		}
	
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "collisionObject") {
			body.AddForce(transform.forward * -1 * currThrust);
		}
	}

}
