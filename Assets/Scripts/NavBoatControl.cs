using UnityEngine;
using System.Collections;

public class NavBoatControl : MonoBehaviour {

	Rigidbody body;
	public float currThrust = 500;
	public enum BoatSideFacingWind {Port, Starboard};
	float angleToAdjustTo;
	float angleWRTWind, lastAngleWRTWind;
	Quaternion lerpStart, lerpEnd;
	float lerpTimer, lerpDuration=1f;
	Vector3 boatDirection;
	bool isJibing = false;
	float turnStrength = 100f, weakTurnStrength = 100f, strongTurnStrength = 100f;
	public static NavBoatControl s_instance;
	bool isNoSailZone;
	public bool canMove = false;


	Vector3 directionWindComingFrom = new Vector3(0f,0f,1f);
	public GameObject mast;
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

	// Update is called once per frame
	void Update () {
		lastAngleWRTWind = angleWRTWind;
		directionWindComingFrom = WindManager.s_instance.directionOfWind;
		

		//figure out angular relation ship between boat and wind
		//isNegative lets us know port vs starboard
//		float isNegative = Mathf.Atan2(localTarget.x, localTarget.z)/Mathf.Abs(Mathf.Atan2(localTarget.x, localTarget.z));
		boatDirection = transform.forward;

		angleWRTWind = Vector3.Angle(boatDirection,directionWindComingFrom);
		if (transform.rotation.eulerAngles.y > 180f ) {
			angleWRTWind = 360-angleWRTWind;
		}


		if (float.IsNaN(angleWRTWind)) {
			angleWRTWind=0;
		}
		if ((angleWRTWind > 180 && lastAngleWRTWind < 180 
		    && angleWRTWind <190)) {
			if (lastAngleWRTWind!=0){
				Jibe (-1f);
			}
		}
		if(angleWRTWind < 180 && lastAngleWRTWind > 180
		    && angleWRTWind > 170) {
			if (lastAngleWRTWind!=0){
				Jibe (1f);
			}
		}

		if (!isJibing) {
			mast.transform.rotation = Quaternion.Lerp(Quaternion.identity, transform.rotation, 0.5f);
		}
		else if (isJibing) {
//			if ((angleWRTWind > 0 && lastAngleWRTWind < 0 && Mathf.Abs(lastAngleWRTWind) >150f) || (angleWRTWind < 0 && lastAngleWRTWind > 0) && Mathf.Abs(lastAngleWRTWind) >150f) {
//				Jibe ();
//			}

			print (lerpStart.eulerAngles + " lerpStart" + lerpEnd.eulerAngles +  " lerpEnd");
			float percentageLerp = (Time.time - lerpTimer)/lerpDuration;
			mast.transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, percentageLerp);
			if (percentageLerp > .98) {
				mast.transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, 1);
				isJibing = false;

			}

		}


		if (Mathf.Abs(Vector3.Angle(WindManager.s_instance.directionOfWind, transform.forward)) < 30f) {
			isNoSailZone = true;
			turnStrength = weakTurnStrength;
		}
		else {
			isNoSailZone = false;
			turnStrength = strongTurnStrength;
		}

	}

	void FixedUpdate () {
		if (canMove) {
		if(Input.GetKey(KeyCode.LeftArrow)) {
			//todo put this in a function that gets called in fixedUpdate, also add in rudder steering
			body.AddRelativeTorque (-Vector3.up*turnStrength);
			
		}
		
		if(Input.GetKey(KeyCode.RightArrow)) {
			body.AddRelativeTorque (Vector3.up*turnStrength);
			
		}
		//make thrust proportionate to dist WRT to wind
		if (!isNoSailZone) {
			body.AddForce (transform.forward * ReturnCurrentThrust());
		}
		}
	}

	void Jibe(float negative) {
		isJibing = true;
		lerpTimer = Time.time;
		lerpStart = mast.transform.rotation;
		lerpEnd = mast.transform.rotation * Quaternion.Euler(0,negative*180f,0);

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "NavTarget" && other.name == NavManager.s_instance.ReturnCurrNavPointName() && Vector3.Distance(transform.position, other.transform.position) <10f) {
			NavManager.s_instance.SwitchNavigationPoint();
		}
	}

	float ReturnCurrentThrust() {
		return currThrust;
	}
}
