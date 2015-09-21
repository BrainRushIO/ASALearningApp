using UnityEngine;
using System.Collections;

public class QuaternionLerp : MonoBehaviour {

	public Quaternion lerpStart, lerpEnd;
	public Vector3 lerpPosStart, lerpPosEnd;
	bool isLerping;
	float lerpTimer, lerpDuration;
	// Use this for initialization
	void Start () {
		lerpStart = transform.GetChild(0).rotation;
		lerpEnd = transform.GetChild(1).rotation;
		lerpPosStart = transform.GetChild(0).position;
		lerpPosEnd = transform.GetChild(1).position;
		transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, 0);
		transform.position = Vector3.Lerp(lerpPosStart, lerpPosEnd, 0);
	}
	
	// Update is called once per frame
	void Update () {

		if (isLerping) {
			float percentageLerp = (Time.time - lerpTimer)/lerpDuration;
			transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, percentageLerp);
			transform.position = Vector3.Lerp(lerpPosStart, lerpPosEnd, percentageLerp);
			if (percentageLerp > .9999) {
				transform.rotation = Quaternion.Lerp(lerpStart, lerpEnd, 1);
				isLerping = false;
			}
		}

	}

	public void StartLerp (float duration = 2f) {
		lerpDuration = duration;
		lerpTimer = Time.time;
		isLerping = true;
	}
}
