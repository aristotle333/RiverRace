using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetLongTermScore : MonoBehaviour {
	public Text[] players;
	public ScoreKeeper keeper;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; i++) {
			players[i].text = ScoreKeeper.gameScores[i + 1].ToString();
		}
	}
}
