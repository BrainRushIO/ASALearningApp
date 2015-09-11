using UnityEngine;
using System.Collections;

public class RotationLock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float angleOfWindWRTIdentity;
		angleOfWindWRTIdentity = Vector3.Angle(Vector3.forward, WindManager.s_instance.directionOfWind);
		transform.rotation = Quaternion.Euler(new Vector3(0, angleOfWindWRTIdentity, 0) + new Vector3(0,45f,0));
	}
}
