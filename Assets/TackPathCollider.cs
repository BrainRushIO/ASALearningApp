using UnityEngine;
using System.Collections;

public class TackPathCollider : MonoBehaviour {

	public GameObject redPathCube, greenPathCube;
	public bool isRed;
	public bool isPlayer;
	Vector3 currentIntersectionPointRed, currentIntersectionPointGreen;
	float spawnTimer, spawnFrequency = .5f;
	int currLayerMask;


	void Start () {
		spawnTimer = Time.time;
		if (isPlayer) {
			currLayerMask = 1 << LayerMask.NameToLayer("Player");
		}
		else {
			currLayerMask = 1 << LayerMask.NameToLayer("Target");
		}
		currLayerMask = ~currLayerMask;

	}



	void Update () {

		//shoot 4 raycasts in the directions of 45 + 90 + 90 + 90 to the direction of wind
		float yRotationWindValue = Vector3.Angle (Vector3.forward, WindManager.s_instance.directionOfWind);




		for (int i = 0; i < 4; i++) {

			Vector3 dir = Quaternion.AngleAxis(yRotationWindValue + 45f + (90f * i), Vector3.up) * Vector3.forward;
			RaycastHit hit;
			if (Physics.Raycast(transform.position, dir, out hit, 500000f, currLayerMask)) {
				if (hit.collider.tag == "green") {
					currentIntersectionPointGreen = hit.point;
				}
				else if (hit.collider.tag == "red") {
					currentIntersectionPointRed = hit.point;

				}
			}
		}
		if (Time.time - spawnTimer > spawnFrequency) {
			GameObject temp;
			if (isRed) {
				temp = Instantiate(redPathCube,transform.position, Quaternion.identity) as GameObject;
				temp.transform.LookAt(currentIntersectionPointGreen);

			}
			else {
				temp = Instantiate(greenPathCube,transform.position, Quaternion.identity) as GameObject;
				temp.transform.LookAt(currentIntersectionPointRed);
			}
			spawnTimer = Time.time;
		}
	}


}
