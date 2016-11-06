using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountUp : MonoBehaviour {
	public int playerID;
	public Text text;
	public Text overallScore;
    public int score;
    int counter;
    int frames;
    string firstChar;
	public bool stillCountingUp;

	// Use this for initialization
	void Start () {
		
        string start = "+ 0";
        text.text = start;
        text.color = new Color(160, 160, 160);
        counter = 0;
        frames = 0;
        firstChar = "+ ";
		stillCountingUp = true;
	}

	void FixedUpdate () {
		if (frames == 0) { 
			ScoreKeeper.gameScores [playerID] = ScoreKeeper.gameScores [playerID] + ScoreKeeper.roundScores[playerID];
		}
        frames += 1;
        if(frames % 6 == 0 && stillCountingUp)
        {
            if (score < 0 && counter > score)
            {
                counter--;
                firstChar = "- ";
                text.color = new Color(160 + counter * 5, 160 + counter * 5, 160 + counter * 5);
            }
            else if(score > 0 && counter < score)
            {
                counter++;
                firstChar = "+ ";
                text.color = new Color(160 + counter * 5, 160 + counter * 5, 160 - counter * 10);
            }
            text.text = firstChar + Mathf.Abs(counter).ToString();
			if (counter == score) {
				stillCountingUp = false;
			}

        }
		if (frames % 6 == 0 && !stillCountingUp && Mathf.Abs(counter) > 0) {
			overallScore.text = (int.Parse (overallScore.text) + 1).ToString();
			if (counter > 0)
				counter--;
			else if (counter < 0)
				counter++;
			text.text = firstChar + Mathf.Abs (counter).ToString ();

		}
	
	}
		
}
