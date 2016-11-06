using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeclareOverallVictor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int maxIndex = -1;
		int topScore = 0;
		for(int i = 1; i < 5; i++) {
			if (ScoreKeeper.gameScores [i] > topScore) {
				topScore = ScoreKeeper.gameScores [i];
				maxIndex = i;
			}
		}
		this.gameObject.GetComponent<Text> ().text = "PLAYER " + maxIndex.ToString () + " WINS!";
	
	}
}
