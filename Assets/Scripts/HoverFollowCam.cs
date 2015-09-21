using UnityEngine;
using System.Collections;

public class HoverFollowCam : MonoBehaviour
{
	Transform player, camPos;
	float camDistanceToCamPos;
	float smoothRate = 8f;
	float verticalLookOffset = 5f;
	public enum CameraMode {stationary, follow, lerpToDestination};
	public CameraMode thisCameraMode = CameraMode.stationary;
	public Vector3 panAwayPosition, startPosition;
	float lerpTimer, lerpDuration = 15f;
	//switches
	bool isAdjustingToCamPos;
	bool isPanningOut;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		camPos = GameObject.FindGameObjectWithTag("CamPos").transform;
	}

	
	void Update()
	{
		transform.LookAt(new Vector3(player.position.x, player.position.y+verticalLookOffset, player.position.z));


	}

	void FixedUpdate() {
		switch (thisCameraMode) {
		case CameraMode.follow :
			transform.position -= (transform.position - camPos.position) * smoothRate *Time.deltaTime;
			break;
		case CameraMode.stationary :
			break;
		case CameraMode.lerpToDestination :
			if (isPanningOut) {
				float fraction;
				fraction = (Time.time - lerpTimer) / lerpDuration;
				print ("LERPING " + fraction);
				transform.position = Vector3.Lerp(startPosition, panAwayPosition, fraction);
				if (fraction >= .99f) {
					isPanningOut = false;
				}
			}
			break;
		}
//		transform.position = Vector3.SmoothDamp(transform.position, camPos.position, ref refVelocity, smoothTime);
	}

	public void PanOut() {
		lerpTimer = Time.time;
		isPanningOut = true;
		startPosition = transform.position;
		panAwayPosition = new Vector3(transform.position.x + 500f, transform.position.y + 100f, transform.position.z + 100f);
		thisCameraMode = CameraMode.lerpToDestination;
	}

	//We have two orders of business, firstly we must have the camera always be moving to camPos, this will be triggered when it is a certain distance away from camPos
	//Further down the line, we will have to deal with walls and things, how do we avoid the camera getting thrown behind a wall, for this we will have to write a detection system on the camera for when it is approaching 
}
