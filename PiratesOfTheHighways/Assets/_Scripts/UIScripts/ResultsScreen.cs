using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour {
	public Text victoryMessage;
	public int SecondsBeforeYouCanLeave;
	public Text[] overallScores;
	public Text[] gameScores;
	public Button startButton;
	// Use this for initialization
	void Start () {
		if (ScoreKeeper.winMessage == null) //don't explode when loading scenes out of context
			return;
		victoryMessage.text = ScoreKeeper.winMessage;
		for (int i = 0; i < 4; i++) {
			overallScores [i].text = ScoreKeeper.gameScores [i + 1].ToString ();
			gameScores [i].GetComponent<CountUp>().score = ScoreKeeper.roundScores [i + 1];

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		if (Time.timeSinceLevelLoad > SecondsBeforeYouCanLeave) {
			startButton.interactable = true;
		}
	}

	public void ReturnToMap() {
		Application.LoadLevel (2);
	}

	public void ReturnToRapids() {
		Application.LoadLevel (6);
	}

	public void returnToMapSelection() {
		SceneManager.LoadScene ("mapSelectionScreen");
	}


}
