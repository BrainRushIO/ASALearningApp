using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//Manages the Points of Sail Module
	public enum GameState {Idle, Review, Instructions, TestPage, Config, ImageLoad, Intro, SetRound, Playing, CheckAnswer, WrongAnswer, CorrectAnswer, WinScreen};
	public GameState gameState;
	public List<Term> listOfPOSTerms,tempListPointTerms,randomListPoints;
	List<pointOfSail> allPoints;
	bool userClickedStart = true;
	int requiredMastery = 2;
	int currIndex = 0;
	int totalMastery;
	float startTime, exitTime = 4f;
	float currMastery;
	public Vector3 directionOfWind = new Vector3 (1f,0,1f);
	public AudioSource wrong, correct, beep, waterPaddle;

	public GameObject IdlePage, ReviewPage,TestPage, InstructionsPage, GameplayPage, winPage;

	public TextAsset pointsOfSailTxt;
	public Slider masteryMeter;
	public Text currentQuestion;
	public Text wrongAnswerText;
	public Text correctText;
	public Timer1 timer;
	public string currAnimState;

	public static GameManager s_instance;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	public void MainMenu() {
		Application.LoadLevel(0);
	}

	void Update () 
	{
		switch (gameState) {
		case GameState.Idle :
			if (Input.GetKeyDown(KeyCode.Space)){
				IdlePage.SetActive(false);
				ReviewPage.SetActive(true);
				gameState = GameState.Review;
				beep.Play();
			}
			break;
		case GameState.Review :
			if (Input.GetKeyDown(KeyCode.Space)){
				ReviewPage.SetActive(false);
				TestPage.SetActive(true);
				gameState = GameState.TestPage;
				beep.Play();
			}
			break;
		case GameState.TestPage :
			if (Input.GetKeyDown(KeyCode.Space)){
				TestPage.SetActive(false);
				InstructionsPage.SetActive(true);
				gameState = GameState.Instructions;
				beep.Play();
			}
			break;
		case GameState.Instructions :
			if (Input.GetKeyDown(KeyCode.Space)){
				InstructionsPage.SetActive(false);
				GameplayPage.SetActive(true);
				gameState = GameState.Config;
				beep.Play();
				Camera.main.GetComponent<QuaternionLerp>().StartLerp(5f);
			}
			break;
		case GameState.Config :
			directionOfWind = new Vector3(0,0,1f);
			allPoints = new List<pointOfSail>();
			allPoints = TextParser.Parse(pointsOfSailTxt);
			listOfPOSTerms = new List<Term>();
			randomListPoints = new List<Term>();

			for (int i = 0; i < allPoints.Count; i++) { //fill out list of Term class instances
				Term tempTerm = new Term();
				tempTerm.initIndex = i;
				tempTerm.pointOfSailAnswer = allPoints[i].sailTitle;
				listOfPOSTerms.Add(tempTerm);
			}
			
			tempListPointTerms = new List<Term>(listOfPOSTerms);
			while (tempListPointTerms.Count > 0) //shuffle list
			{
				int randomIndex = Mathf.FloorToInt(Random.Range(0, tempListPointTerms.Count));//r.Next(0, inputList.Count); //Choose a random object in the list
				randomListPoints.Add(tempListPointTerms[randomIndex]); //add it to the new, random list
				tempListPointTerms.RemoveAt(randomIndex); //remove to avoid duplicates
			}

			totalMastery = requiredMastery * listOfPOSTerms.Count;
			gameState = GameState.Intro;
			break;
			
		case GameState.Intro : 
			if (userClickedStart) {
				gameState = GameState.SetRound;
			}
			break;
			
		case GameState.SetRound :
			CheckForSequenceMastery(); //eliminate mastered sequences
			InitiateTerm();
			gameState = GameState.Playing;
			break;
		case GameState.Playing :
			if (Input.GetKeyDown(KeyCode.Space)){ //when boat has been rotated
				gameState = GameState.CheckAnswer;
			}
			break;
			
		case GameState.CheckAnswer :
			if (Checker()){
				gameState = GameState.CorrectAnswer;
			}
			else {
				gameState = GameState.WrongAnswer;
			}
			break;
			
		case GameState.CorrectAnswer :
			if (AnswerCorrect()){
				WinRound();
				gameState = GameState.WinScreen;
			}
			else {
				gameState = GameState.SetRound;
			}
			break;
			
		case GameState.WrongAnswer :
			AnswerWrong();
			gameState = GameState.Playing;
			break;
			
		case GameState.WinScreen :
			GameplayPage.SetActive(false);
			winPage.SetActive(true);
//			if ((Time.time - startTime) > exitTime) {
//				GotoNextModule();
//			}
			break;
		}
	}

	bool Checker() {
		if (randomListPoints[currIndex].pointOfSailAnswer.Replace("|"," ") == currAnimState) {
			return true;
		}
		else {
			return false;
		}
	}
	void InitiateTerm(){
		currentQuestion.text = "Rotate the boat to " + randomListPoints[currIndex].pointOfSailAnswer.Replace("|"," ");
		timer.Reset(25f);
	}
	void WinRound(){
		startTime = Time.time;

	}
	bool AnswerCorrect(){
		correct.Play ();
		wrongAnswerText.enabled = false;
		correctText.enabled = true;
		correctText.GetComponent<Fader>().StartFadeOut();
		AdjustMasteryMeter(true);
		if (masteryMeter.value > .97f ) {
			return true;
		} else { 
			return false;
		}

	}
	void AnswerWrong(){
		wrong.Play ();
		AdjustMasteryMeter(false);
		timer.timesUp = true;
		timer.pause = true;
		DisplayFeedbackText();
	}

	void DisplayFeedbackText () {
		//Nope, you selected this position, try again
		wrongAnswerText.enabled = true;
		wrongAnswerText.text = "Incorrect, you selected: " + currAnimState;

	}
	void GotoNextModule(){
		Application.LoadLevel(1);
	}

	void AdjustMasteryMeter(bool didAnswerCorrect) {
		
		if (didAnswerCorrect && !timer.timesUp) {
			
			listOfPOSTerms[randomListPoints[currIndex].initIndex].mastery += 1;
			print ("mastery added");
		}
		
		else if (!didAnswerCorrect) {
			if (listOfPOSTerms[randomListPoints[currIndex].initIndex].mastery > 0) {
				
				listOfPOSTerms[randomListPoints[currIndex].initIndex].mastery -= 1;
			}
		}
		SetMastery();
		
	}
	
	void SetMastery() {
		currMastery = 0;
		foreach (Term x in listOfPOSTerms) {
			currMastery+=x.mastery;
		}
		masteryMeter.value = (float)(currMastery)/totalMastery;
		masteryMeter.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "Mastery: " + Mathf.FloorToInt(masteryMeter.value*100f).ToString()+"%";
	}


	public void CheckForSequenceMastery() {
		if (randomListPoints.Count == 0) {
			WinRound();
			return;
		}
		else {
			for (int i = 0; i < randomListPoints.Count; i++) {
				if (randomListPoints[i].mastery == requiredMastery) { //skip over completed 
					randomListPoints.Remove(randomListPoints[i]);
				}
			}
			if (randomListPoints.Count > currIndex+1) {
				currIndex++;
			}
			else {
				currIndex = 0;
			}
		}
	}

}
