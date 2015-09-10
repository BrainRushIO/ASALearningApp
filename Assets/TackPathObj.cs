using UnityEngine;
using System.Collections;

public class TackPathObj : MonoBehaviour {

	public bool reverseDirection;
	public Vector3 target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward);
	}
	
}
