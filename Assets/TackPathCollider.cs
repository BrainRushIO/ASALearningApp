using UnityEngine;
using System.Collections;

public class TackPathCollider : MonoBehaviour {

	public GameObject redPathCube, greenPathCube;
	public bool isRed;
	public bool isPlayer;
	Vector3 currentIntersectionPointRed, currentIntersectionPointGreen;
	float spawnTimer, spawnFrequency = 5f;
	LayerMask currLayerMask;


	void Start () {
		spawnTimer = Time.time;
		if (isPlayer) {
			currLayerMask = 7;
		}
		else {
			currLayerMask = 8;
		}


	}



	void Update () {

		//shoot 4 raycasts in the directions of 45 + 90 + 90 + 90 to the direction of wind
		WindManager.s_instance.directionOfWind

		if (Time.time - spawnTimer > spawnFrequency) {
			GameObject temp;
			if (isRed) {
				temp = Instantiate(redPathCube) as GameObject;
				temp.transform.LookAt(currentIntersectionPointGreen);

			}
			else {
				temp = Instantiate(greenPathCube) as GameObject;
				temp.transform.LookAt(currentIntersectionPointRed);
			}
			spawnTimer = Time.time;
		}
	}


}
