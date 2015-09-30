using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cinematographer : MonoBehaviour {

	/*

	Cinematographer takes a list of gameobjects, vector3s, floats
	
	and uses these to move a camera as it transitions between looking at different objects

	it also keeps text or UI panels that are associated with each 


	 */

	Quaternion lerpStart, lerpEnd;
	public List<Quaternion> quaternions;
	public List<GameObject> textUIObjects;
	public List<Vector3> positions;
	public List<float> timeAtEachObject;
	int currentIndex = 0;
	public bool hasStarted;
	float pauseTimer;

	//lerp
	float lerpTimer;
	float lerpDuration = 5f;
	bool isLerping;

	public void RollCamera () {
		pauseTimer = Time.time;
		Camera.main.transform.rotation = quaternions[currentIndex];
		hasStarted = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (hasStarted) {
			if (Time.time - pauseTimer > timeAtEachObject[currentIndex]) {
				GotoNextPosition();
			}


			if (isLerping) {
				float fraction = Time.time - lerpTimer / lerpDuration;
				Camera.main.transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, fraction);
				if (fraction > .9999f) {
					Camera.main.transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, 1f);
					isLerping = false;
				}
			}

		}
	}

	void GotoNextPosition () {
		pauseTimer = Time.time;
		currentIndex++;
		lerpEnd = quaternions[currentIndex];
		lerpStart = Camera.main.transform.rotation;
		isLerping = true;

	}
}
