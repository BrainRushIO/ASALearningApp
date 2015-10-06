using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class posAnimState : MonoBehaviour {

	public Animator boatAnim;
	public GameManager gameManager;
	public Text displayPOS; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		//Cast ray at point of mouse click
		if (Input.GetButtonDown("Fire1")) 
		{
			Ray ray;
			RaycastHit hit;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)){

				//checks which point of sail object was clicked for animation states
				switch(hit.collider.gameObject.name){

				case "Irons":
					boatAnim.SetTrigger("Irons");
					gameManager.currAnimState = "In Irons";
					displayPOS.text = "In Irons - the boat is head to wind (0 degrees)";
					break;
				case "PTCloseHaul":
					boatAnim.SetTrigger("PTCloseHaul");
					gameManager.currAnimState = "Close Hauled Port Tack";
					displayPOS.text = "Close Hauled - the boat is sailing as close to the wind as possible (~45 degrees)";

					break;
				case "PTCloseReach":
					boatAnim.SetTrigger("PTCloseReach");
					gameManager.currAnimState = "Close Reach Port Tack";
					displayPOS.text = "Close Reach - the point of sail between close-hauled and beam reach. (~60 degrees)";

					break;

				case "PTBeamReach":
					boatAnim.SetTrigger ("PTBeamReach");
					gameManager.currAnimState = "Beam Reach Port Tack";
					displayPOS.text = "Beam Reach - the wind is abeam of the boat (~90 degrees)";

					break;

				case "PTBroadReach":
					boatAnim.SetTrigger("PTBroadReach");
					gameManager.currAnimState = "Broad Reach Port Tack";
					displayPOS.text = "Broad Reach - the point of sail between a beam reach and a run (~135 degrees)";

					break;
				case "Run":
					boatAnim.SetTrigger("Run");
					gameManager.currAnimState = "Run";
					displayPOS.text = "Run - the point of sail on which the wind is aft (180 degrees)";

					break;
				case "STBroadReach":
					boatAnim.SetTrigger("STBroadReach");
					gameManager.currAnimState = "Broad Reach Starboard Tack";
					displayPOS.text = "Broad Reach - the point of sail between a beam reach and a run (~135 degrees)";
					break;
				case "STBeamReach":
					boatAnim.SetTrigger("STBeamReach");
					gameManager.currAnimState = "Beam Reach Starboard Tack";
					displayPOS.text = "Beam Reach - the wind is abeam of the boat (~90 degrees)";

					break;
				case "STCloseReach":
					boatAnim.SetTrigger("STCloseReach");
					gameManager.currAnimState = "Close Reach Starboard Tack";
					displayPOS.text = "Close Reach - the point of sail between close-hauled and beam reach. (~60 degrees)";
					break;
				case "STCloseHaul":
					boatAnim.SetTrigger("STCloseHaul");
					gameManager.currAnimState = "Close Hauled Starboard Tack";
					displayPOS.text = "Close Hauled - the boat is sailing as close to the wind as possible (~45 degrees)";
					break;


				}

			}


		
		}



	}
}
