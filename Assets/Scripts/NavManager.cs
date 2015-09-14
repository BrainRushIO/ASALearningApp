using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NavManager : MonoBehaviour {

	enum GameState {Idle, Review, Instructions, Gameplay, Win};
	GameState gameState = GameState.Idle;
	public GameObject[] navigationPoints;
	public GameObject reviewPage, idlePage, instructionsPage, gamePlayPage, winPage;
	public static NavManager s_instance;
	public bool hasReachedAllTargets;
	int currNavPoint = 0;
	public Text currTarget;
	public AudioSource beep;
	float startTime, elapsedTime;
	public Text timeText;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	public void SwitchNavigationPoint() {
		navigationPoints[currNavPoint].transform.GetChild(0).gameObject.SetActive(false);
		currNavPoint++;
		if (currNavPoint < navigationPoints.Length) {
			navigationPoints[currNavPoint].transform.GetChild(0).gameObject.SetActive(true);
		}
		else {
			hasReachedAllTargets= true;
			winPage.SetActive(true);
			gamePlayPage.SetActive(false);
			NavBoatControl.s_instance.transform.GetChild(0).gameObject.SetActive(false);
		}

	}

	public string ReturnCurrNavPointName() {
		if (currNavPoint<navigationPoints.Length) {
			return navigationPoints[currNavPoint].name;
		}
		else {
			return "";
		}
	}
	// Update is called once per frame
	void Update () {
		switch (gameState) {
		case GameState.Idle :
			if (Input.GetKeyDown(KeyCode.Space)){
				idlePage.SetActive(false);
				reviewPage.SetActive(true);
				gameState = GameState.Review;
				beep.Play();

			}
			break;
		case GameState.Review :
			if (Input.GetKeyDown(KeyCode.Space)){
				reviewPage.SetActive(false);
				instructionsPage.SetActive(true);
				gameState = GameState.Instructions;
				beep.Play();

			}
			break;
		case GameState.Instructions :
			if (Input.GetKeyDown(KeyCode.Space)){
				instructionsPage.SetActive(false);
				gameState = GameState.Gameplay;
				NavBoatControl.s_instance.canMove = true;
				gamePlayPage.SetActive(true);
				beep.Play();
				StartClock();
			}
			break;
		case GameState.Gameplay :
			if (hasReachedAllTargets) {
				gameState = GameState.Win;
				NavBoatControl.s_instance.canMove = false;
				break;
			}
			elapsedTime = Time.time - startTime;
			currTarget.text = "Your current destination is: " + navigationPoints[currNavPoint].name;
			timeText.text = "Elapsed time: " + elapsedTime.ToString("F2") + "s";
			                                   
			break;
		case GameState.Win :
			break;
		}

	}

	void StartClock() {
		startTime = Time.time;
	}
}
