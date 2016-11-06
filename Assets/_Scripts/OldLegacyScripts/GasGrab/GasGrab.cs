using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GasGrab : MonoBehaviour{

    public static int[] PlayerScores;

    public static bool tutorial = true;
    bool tutorialend = false;

    public Text P1score;
    public Text P2score;
    public Text P3score;
    public Text P4score;
    public Text timertext;

    public static GasGrab instance;

    // Use this for initialization
    void Start()
    {
        PlayerScores = new int[4];      //Needs to be modified if alllowing for less players
        tutorial = true;

    }

    // Update is called once per frame
    void Update()
    {
        P1score.text = PlayerScores[0].ToString();
        P2score.text = PlayerScores[1].ToString();
        P3score.text = PlayerScores[2].ToString();
        P4score.text = PlayerScores[3].ToString();
        timertext.text = Timer.S.getCurrentTimeString();

        if(tutorial == false && tutorialend == false)
        {
            Timer.S.StartCountdownTimer(45);
            tutorialend = true;
        }

        if (Timer.S.currentTime == 0f && tutorial == false)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        print("it's over");

        //Transition to results with necessary values
        int winners = 0;
        int highscore = 0;
        bool[] didwin = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            didwin[i] = false;
        }

        for (int i = 0; i < 4; i++)
        {
            if (PlayerScores[i] > highscore)
            {
                highscore = PlayerScores[i];
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (PlayerScores[i] == highscore)
            {
                didwin[i] = true;
                winners++;
            }
        }

        int winnercount = winners;
        ScoreKeeper.winMessage = "";
        for (int i = 0; i < 4; i++)
        {
            if (winners > 1)
            {
                if (didwin[i])
                {
                    if (winnercount > 1)
                    {
                        if (winnercount == winners)
                        {
                            ScoreKeeper.winMessage += "PLAYERS " + (i + 1).ToString() + ", ";
                        }
                        else
                        {
                            ScoreKeeper.winMessage += (i + 1).ToString() + ", ";
                        }
                        winnercount--;
                    }
                    else
                    {
                        ScoreKeeper.winMessage += (i + 1).ToString() + " WIN!";
                    }
                }
            }
            else
            {
                if (didwin[i])
                {
                    ScoreKeeper.winMessage = "PLAYER " + (i + 1).ToString() + " WINS!";
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (didwin[i])
            {
                if (winners == 4)
                {
                    ScoreKeeper.roundScores[i + 1] = 5;
                }
                if (winners == 3)
                {
                    ScoreKeeper.roundScores[i + 1] = 10;
                }
                if (winners == 2)
                {
                    ScoreKeeper.roundScores[i + 1] = 15;
                }
                if (winners == 1)
                {
                    ScoreKeeper.roundScores[i + 1] = 20;
                }
            }
        }

        ScoreKeeper.DisplayResultsScreen();

    }


}