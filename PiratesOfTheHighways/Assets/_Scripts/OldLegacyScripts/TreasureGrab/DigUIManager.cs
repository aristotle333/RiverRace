using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DigUIManager : MonoBehaviour {

    public static DigUIManager DigUIMan;

    public Text[] scoreTexts;
    public int[] playerScores;

    public Sprite[] TGSprites;

	// Use this for initialization
	void Start () {
        DigUIMan = this;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; i++)
        {
            scoreTexts[i].text = playerScores[i].ToString();
        }

        //--send final scores to results screen here when timer runs out?
	}
}
