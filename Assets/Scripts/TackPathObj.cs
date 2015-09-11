using UnityEngine;
using System.Collections;

public class TackPathObj : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (transform.forward);
		print (transform.forward);
	}

}
