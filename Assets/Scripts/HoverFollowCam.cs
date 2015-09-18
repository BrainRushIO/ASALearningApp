using UnityEngine;
using System.Collections;

public class HoverFollowCam : MonoBehaviour
{
	Transform player, camPos;
	float camDistanceToCamPos;
	float smoothRate = 8f;
	float verticalLookOffset = 3f;
	public enum CameraMode {stationary, follow, lerpToDestination};
	public CameraMode thisCameraMode = CameraMode.stationary;
	public Vector3 panAwayPosition, startPosition;
	float lerpTimer, lerpDuration = 5f;
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

		if (isPanningOut) {
			float fraction;
			fraction = (Time.time - lerpTimer) / lerpDuration;
			camPos.position = Vector3.Lerp(startPosition, panAwayPosition, fraction);
			if (fraction >= .99f) {
				isPanningOut = false;
			}
		}
	}

	void FixedUpdate() {
		switch (thisCameraMode) {
		case CameraMode.follow :
			transform.position -= (transform.position - camPos.position) * smoothRate *Time.deltaTime;
			break;
		case CameraMode.stationary :
			break;
		case CameraMode.lerpToDestination :

			break;
		}
//		transform.position = Vector3.SmoothDamp(transform.position, camPos.position, ref refVelocity, smoothTime);
	}

	public void PanOut() {
		lerpTimer = Time.time;
		isPanningOut = true;
		startPosition = camPos.position;
		panAwayPosition = new Vector3(camPos.position.x + 100f, camPos.position.y + 100f, camPos.position.z + 100f);
	}

	//We have two orders of business, firstly we must have the camera always be moving to camPos, this will be triggered when it is a certain distance away from camPos
	//Further down the line, we will have to deal with walls and things, how do we avoid the camera getting thrown behind a wall, for this we will have to write a detection system on the camera for when it is approaching 
}
