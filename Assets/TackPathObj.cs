using UnityEngine;
using System.Collections;

public class TackPathObj : MonoBehaviour {

	public bool reverseDirection;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (reverseDirection) {
			transform.Translate(-Vector3.forward*.3f);
		}
		else {
			transform.Translate(Vector3.forward*.3f);

		}
	}
}
