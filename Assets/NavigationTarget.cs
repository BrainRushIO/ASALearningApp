using UnityEngine;
using System.Collections;

public class NavigationTarget : MonoBehaviour {

	bool isInFront, isToTheLeft;
	float spawnTimer, spawnTimerDuration=2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//find if boat is front or back, left or right


		if (spawnTimer > spawnTimerDuration) {
			SpawnNavObjs();
		}
	}

	//render mesh vertex to create red and green lines TODO

	void SpawnNavObjs() {

	}
}
