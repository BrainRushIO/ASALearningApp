﻿using UnityEngine;
using System.Collections;

public class SailHackCorrection : MonoBehaviour {
	public Transform imitate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation = imitate.transform.localRotation;
	}
}
