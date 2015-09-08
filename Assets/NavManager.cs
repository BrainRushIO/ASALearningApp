using UnityEngine;
using System.Collections;

public class NavManager : MonoBehaviour {

	enum NavGameState {Intro, Gameplay};

	public GameObject[] navigationPoints;
	
	public static NavManager s_instance;
	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

	}
}
