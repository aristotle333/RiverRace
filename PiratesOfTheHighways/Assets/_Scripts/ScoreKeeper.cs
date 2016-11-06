using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour {

	public static ScoreKeeper MainScoreController;

	public static int[] gameScores;
	public static int[] roundScores;

	public static string winMessage;
	public static int lastNode; //tracks where the van is on the map. Range is zero to 14, I think.

	public bool started;

	void Awake() {
		if (GameObject.FindGameObjectsWithTag ("GameController").GetLength(0) > 1 && !started) {
			print("Deleted a superfluous scorekeeper that was making things ambiguous");
			Destroy (this.gameObject);
		}
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		if (!started){
			MainScoreController = this;
			lastNode = 0; //van is in Chicago

			gameScores = new int[5];
			roundScores = new int[5];
			gameScores[0] = gameScores[1] = gameScores[2] = gameScores[3] = gameScores[4] = 0;
			roundScores[0] = roundScores[1] = roundScores[2] = roundScores[3] = roundScores[4] = 0;

			winMessage = "I haven't been set yet";

			started = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.N)) {
			print (lastNode);
		}
	}


    //Ideally this should just happen in the raft script
	static public void DisplayResultsScreen() {
        LevelSelect.FromLevel = true;
        SceneManager.LoadScene ("mapSelectionScreen");
	}

	static public void ClearRoundScores() {
		for (int i = 0; i < 5; i++) {
			roundScores [i] = 0;
		}
	}


}
