using UnityEngine;
using System.Collections;

public class posAnimState : MonoBehaviour {

	public Animator boatAnim;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Fire1")) 
		{
			Ray ray;
			RaycastHit hit;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f)){


				switch(hit.collider.gameObject.name){

				case "Irons":
					boatAnim.SetTrigger("Irons");
					break;
				case "PTCloseHaul":
					boatAnim.SetTrigger("PTCloseHaul");
					break;
				case "PTCloseReach":
					boatAnim.SetTrigger("PTCloseReach");
					break;

				case "PTBeamReach":
					break;
				case "PTBroadReach":
					break;
				case "Run":
					break;
				case "STBroadReach":
					break;
				case "STBeamReach":
					break;
				case "STCloseReach":
					break;
				case "STCloseHaul":
					break;


				}

			}


		
		}



	}
}
