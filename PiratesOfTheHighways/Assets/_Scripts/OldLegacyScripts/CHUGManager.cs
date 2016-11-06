using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CHUGManager : MonoBehaviour {
	public bool tutorialOn;
	public float minTutTime;
	public float tutorialScreenTime;
    public float readygoDelay;
	public int victoryAmount;
    public int numPlayers = 2;
    public int[] roundScores;
    public Text[] scoreTexts;
    public bool[] drinking;
    public bool[] spilled;
    public bool[] drinkUp;
    public bool[] resetDrink;
    public int[] PresetDelay;
    public int[] tutResetDelay;
    public int resetDelay = 30;
    public float[] drinkLevel;
    public float[] chugBars;

    public GameObject beerPrefab;
    public GameObject[] beers;
    public Sprite[] beerAnimation;

    public GameObject meterPrefab;
    public GameObject[] allMeters;

    public GameObject bartenderPrefab;
    public GameObject[] barkeeps;

    public GameObject spillPrefab;
    public GameObject[] spills;

    public GameObject aButtonPrefab;
    public GameObject[] aButtons;

	public Canvas tutorialCanvas;
	public Button tutorialButton;

    public GameObject readyBanner;
    public GameObject goBanner;

    //--THIS IS CURRENTLY A BALL OF COMMENTS AND POLISH THAT HAS TO GET ADDED

	// Use this for initialization
	void Start () {
        //ability to downscale number of players to <4?
		tutorialOn = true;
        roundScores = new int[numPlayers];
        drinking = new bool[numPlayers];
        spilled = new bool[numPlayers];
        drinkUp = new bool[numPlayers];
        resetDrink = new bool[numPlayers];
        PresetDelay = new int[numPlayers];
        tutResetDelay = new int[numPlayers];
        drinkLevel = new float[numPlayers];
        chugBars = new float[numPlayers];

        beers = new GameObject[numPlayers];
        allMeters = new GameObject[numPlayers];
        barkeeps = new GameObject[numPlayers];
        spills = new GameObject[numPlayers];
        aButtons = new GameObject[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            drinking[i] = false;
            spilled[i] = false;
            drinkUp[i] = false;
            resetDrink[i] = false;
            PresetDelay[i] = 0;
            drinkLevel[i] = 1;
            chugBars[i] = 0;

            beers[i] = Instantiate(beerPrefab, new Vector3(-6f + 4 * i, -1.65f, -1 + 0.1f * i), Quaternion.identity) as GameObject;
            allMeters[i] = Instantiate(meterPrefab, new Vector3(-7.75f + 4 * i, 0f, 0), Quaternion.identity) as GameObject;

            spills[i] = Instantiate(spillPrefab, new Vector3(-6f + 4 * i, 0.5f, -2f), Quaternion.identity) as GameObject;
            spills[i].GetComponent<SpriteRenderer>().enabled = false;

            aButtons[i] = Instantiate(aButtonPrefab, new Vector3(-7f + 4 * i, 2f, 0f), Quaternion.identity) as GameObject;
            aButtons[i].GetComponent<SpriteRenderer>().enabled = false;
        }


	}

	public void closeTutorial() {
		tutorialOn = false;
		tutorialCanvas.enabled = false;

        for (int i = 0; i < 4; i++){
            drinking[i] = false;
            spilled[i] = false;
            drinkUp[i] = false;
            resetDrink[i] = false;
            PresetDelay[i] = 0;
            drinkLevel[i] = 1;
            chugBars[i] = 0;
            roundScores[i] = 0;

            beers[i].transform.position = new Vector3(-6f + 4 * i, -1.65f, -1 + 0.1f * i);
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[1];
        }

        readygoDelay = 0;
	}
	
	// Update is called once per frame
	void Update () {

        readygoDelay++;

		if (tutorialOn) {
			tutorialScreenTime = Time.timeSinceLevelLoad;
			if (tutorialScreenTime > minTutTime) {
				tutorialButton.interactable = true;
			}
            if (tutorialButton.interactable && (Input.GetAxisRaw("Player1_buttonStart") > 0.2f || Input.GetAxisRaw("Player2_buttonStart") > 0.2f || Input.GetAxisRaw("Player3_buttonStart") > 0.2f || Input.GetAxisRaw("Player4_buttonStart") > 0.2f)) {
                closeTutorial();
            }
		}
	    for (int i = 0; i < numPlayers; i++)
        {
			//check for victory!
			if (!tutorialOn && roundScores [i] >= victoryAmount) {
				for (int j = 0; j < 4; j++) {
					ScoreKeeper.roundScores [j + 1] = 0;
				}
				ScoreKeeper.roundScores [i + 1] = 10;
				ScoreKeeper.winMessage = "PLAYER " + (i + 1).ToString() + " WINS!";
				ScoreKeeper.DisplayResultsScreen ();
			}


            scoreTexts[i].text = roundScores[i].ToString();

            if (!drinking[i] && !resetDrink[i]){
            	//--if not chugging, and the initiate button is pressed swap sprite to chugging one and initialize bar
            	//--display waiting sprite (hand out to catch sliding beer)
            	
                if (drinkUp[i])
                {
                    if (Input.GetAxisRaw("Player" + (i+1) + "_buttonA") > .6f)
                    {
                        //--need to make an animation controller
                        //--this is where the slam would happen
				        activateReset(i);
                    }
                    else if (tutorialOn && drinkLevel[i] <= 0)
                    {
                        tutResetDelay[i]++;
                        if (tutResetDelay[i] >= 60)
                        {
                            activateReset(i);
                            tutResetDelay[i] = 0;
                        }
                    }
                }
                else
                {
                    if (!tutorialOn && readygoDelay < 200)
                    {
                        if (readygoDelay < 125)
                        {
                            readyBanner.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else if (readygoDelay < 185)
                        {
                            readyBanner.GetComponent<SpriteRenderer>().enabled = false;
                            goBanner.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            readyBanner.GetComponent<SpriteRenderer>().enabled = false;
                            goBanner.GetComponent<SpriteRenderer>().enabled = false;
                        }
                        return;
                    }

                    //--*hums "raise your glass"*
                    //--pick it up to the drink position
                    drinking[i] = true;
                    drinkUp[i] = true;
                    beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[1];
                    Vector3 pos = beers[i].transform.position;
                    pos.y += 2;
                    beers[i].transform.position = pos;
                }
            }
            else if (drinking[i]){
            	updateBarandBeer(i);
            }
            else if (resetDrink[i])
            {
                //Half second period to refill glass before allowing more chugging
                //--need a pour sound
                //--need a slam sound
                //--need a barkeep back and front sprite
                //--need to animate walking to and from bar
                //--need to make flashing slam A button
                animateReset(i);

            }
        }
	}

    void updateBarandBeer(int i){
        //--sprite now is raised glass (need to determine the stages od drinking sprites later on)
        //Hitting "up" increases the chug level, over maxing bar causes spill (no points)
        //Bar drains over time
        //--not expressly a mashing game, more of a balance game in this state
        if (tutorialOn)
        {
            chugBars[0] += .0015f;
            if (chugBars[0] >= 1.1f)
            {
                chugBars[0] = 1.1f;
                drinkLevel[0] = 0;
            }
            chugBars[1] += .0015f;
            if (chugBars[1] >= .9f)
            {
                chugBars[1] = .9f;
            }
            chugBars[2] += .0015f;
            if (chugBars[2] >= .6f)
            {
                chugBars[2] = .6f;
            }
            chugBars[3] += .0015f;
            if (chugBars[3] >= .3f)
            {
                chugBars[3] = .3f;
            }
        }
        else
        {
            float chugAdd = Input.GetAxis("Player" + (i+1) + "_axisY");
            if (chugAdd > 0){
                chugBars[i] += chugAdd*0.05f;
            }

            //--difficulty increase/wide variation the more you've had to drink?
            //Bar automatically drains a little
    
            chugBars[i] -= .01f;
        }

        if (chugBars[i] < 0)
        {
            chugBars[i] = 0;
        }

        //--bar should flash is close to spill?
        if (chugBars[i] > 1){
            //spill
            //--should break or something so it doesnt give player points despite "empty glass"
            drinkLevel[i] = 0f;
            spilled[i] = true;
            spills[i].GetComponent<SpriteRenderer>().enabled = true;
            
        } else {
            //drink
            //2.0833x3 - 1.875x2 + 0.7917x
            float x = chugBars[i];
            drinkLevel[i] -= 0.01f*(2.0833f*(x*x*x) - 1.875f*(x*x) + 0.7917f*x);
        }

        animateBeers(i);

        //--slam mechanic/button?
        //--currently you just drop empty glass behind bar and can grab the next one
        //Reset if glass empty
        if (drinkLevel[i] <= 0f){

            aButtons[i].GetComponent<SpriteRenderer>().enabled = true;

            chugBars[i] = 0;
            //--slam/need a ui notification so you know youre done
            //--press a to slam        
            drinking[i] = false;

            //--need sprites to set glass back down
            //--might need some sort of reset period?
        }
    }

    void animateBeers(int i){

        Vector3 meterPos = new Vector3(-7.75f + 4 * i, (-2f + 4f * chugBars[i]), -0.1f);
        allMeters[i].gameObject.transform.FindChild("Slider").transform.position = meterPos;

        if (drinkLevel[i] >= 0.925f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[9];
        }
        else if (drinkLevel[i] >= 0.775f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[8];   
        }
        else if (drinkLevel[i] >= 0.65f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[7];
        }
        else if (drinkLevel[i] >= 0.45f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[6];
        }
        else if (drinkLevel[i] >= 0.3f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[5];
        }
        else if (drinkLevel[i] >= 0.15f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[4];
        }
        else if (drinkLevel[i] >= 0.05f)
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[3];
        }
        else
        {
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[2];
        }
    }

    void animateReset(int i){

        //--decrease chug bar to 0 over the course of the refill?
        PresetDelay[i]++;
                
        if (PresetDelay[i] == 25)
        {
            barkeeps[i] = Instantiate(bartenderPrefab, new Vector3(-6f + 4*i, -2f, -2f), Quaternion.identity)as GameObject;
        }
        else if (PresetDelay[i] == 40)
        {
            Destroy(barkeeps[i].gameObject);
            beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[1];
        }
        else if (PresetDelay[i] >= 60)
        {
            //--maybe have this be where the beer lifts up so that when done resetting it tips and you start drinking
            drinking[i] = false;
            drinkUp[i] = false;
            resetDrink[i] = false;
            PresetDelay[i] = 0;
            drinkLevel[i] = 1;
            chugBars[i] = 0;        
        }
    }

    void activateReset(int i){

        spills[i].GetComponent<SpriteRenderer>().enabled = false;
        aButtons[i].GetComponent<SpriteRenderer>().enabled = false;

        if (!spilled[i]){
            roundScores[i]++;
        }
        drinking[i] = false;
        spilled[i] = false;
        resetDrink[i] = true;
        PresetDelay[i] = 0;
        beers[i].GetComponent<SpriteRenderer>().sprite = beerAnimation[0];
        Vector3 pos = beers[i].transform.position;
        pos.y -= 2;
        beers[i].transform.position = pos;
    }

}
