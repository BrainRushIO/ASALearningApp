using UnityEngine;
using System.Collections;

public class midPointLine : MonoBehaviour {

	public GameObject targetStart;
	public GameObject targetEnd;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pos = new Vector3 (targetStart.transform.position.x, (targetEnd.transform.position.y + targetStart.transform.position.y) / 2, targetEnd.transform.position.z);
		transform.position = pos;

	}
}
