using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Raft : MonoBehaviour {
	public float victoryDelay;
	public int player1, player2;
	public PowerUp.PowerUpType power1, power2;
	public float currentScaling;
	public string left, right, backLeft, backRight, fireLeft, fireRight, fireTriggerLeft, fireTriggerRight;
	public float paddleForce;
	public int cooldownFrames;
	public int paddleFrames;
	public float spinOutSpeed;
	public float speedLimit;
	public int spinOutDuration;
    public float boostForce = 40f;
	public Raft otherRaftScript;
	public Rigidbody leftPaddle, rightPaddle;
	public Rigidbody raft;
	public Text finishText, winnerText;
	public SpriteRenderer leftPrompt, rightPrompt;
	public Image powerImage1, powerImage2;
	public Sprite cannonSprite, grappleSprite, sailSprite;
	public GameObject cannon1, cannon2, cannon1line, cannon2line, grapple1line, grapple2line, grapple1, grapple2, sail1, sail2, sailPole1, sailPole2;
	public GameObject paddle1, paddle2;
	public int lCool, rCool;
	int spinFrameCounter;
	public bool noControl;
	float victoryTime;
	bool winnerExists;
	bool tutorialActive;
	public KeyCode keyLeft, keyRight, keyBackLeft, keyBackRight, keyFireLeft, keyFireRight;
	public string leftAxisH, leftAxisV, rightAxisH, rightAxisV;
	public Sprite aButton, bButton;
    public GameObject TutorialGORef;
	public AudioSource powerupGrab, cannonFire, sailSound, grappleLaunch, winSound, forwardPaddleSound, whirlpoolSplash;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		sailSound.volume = 0f;
		cannon1.GetComponent<Cannon> ().enemyRaft = otherRaftScript.gameObject;
		cannon2.GetComponent<Cannon> ().enemyRaft = otherRaftScript.gameObject;
		grapple1.GetComponent<Grapple> ().enemyRaft = otherRaftScript.gameObject;
		grapple2.GetComponent<Grapple> ().enemyRaft = otherRaftScript.gameObject;
		grapple1.GetComponent<Grapple> ().thisRaft = this.gameObject;
		grapple2.GetComponent<Grapple> ().thisRaft = this.gameObject;
		power1 = power2 = PowerUp.PowerUpType.None;
		winnerExists = false;
		lCool = rCool = spinFrameCounter = 0;
		noControl = false;
        left = "joystick " + player1 + " button 0";
		right = "joystick " + player2 + " button 0";
		backLeft = "joystick " + player1 + " button 1";
		backRight = "joystick " + player2 + " button 1";
		fireLeft = "joystick " + player1 + " button 2";
		fireRight = "joystick " + player2 + " button 2";
        fireTriggerLeft = "Player" + player1 + "_RightTrigger";
        fireTriggerRight = "Player" + player2 + "_RightTrigger";
        leftAxisH = "Player" + player1 + "_axisX";
		rightAxisH = "Player" + player2 + "_axisX";
		leftAxisV = "Player" + player1 + "_axisY";
		rightAxisV = "Player" + player2 + "_axisY";
		raft.maxAngularVelocity = 4f;
	}

	// Update is called once per frame
	void Update () {

		//rotate the paddles appropriately!
		float lAnglePercent = (float)lCool / (float)paddleFrames ; //this is what percentage of a full paddle we've done!
		float rAnglePercent = (float)rCool / (float)paddleFrames;
		if (lAnglePercent < .2f && leftPrompt.sprite == aButton)
			lAnglePercent = 1f - (5f * lAnglePercent);
		if (rAnglePercent < .2f && rightPrompt.sprite == aButton)
			rAnglePercent = 1f - (5f * rAnglePercent);

		if (leftPrompt.sprite == bButton)
			lAnglePercent = 1f - lAnglePercent; //paddling backwards!
		if (rightPrompt.sprite == bButton)
			rAnglePercent = 1f - rAnglePercent;

		float angleToRotateLeftPaddle = lAnglePercent * 90f;
		float angleToRotateRightPaddle = rAnglePercent * 90f;

		Vector3 paddle1Angle = paddle1.transform.localEulerAngles;
		paddle1Angle.z = 80f -angleToRotateLeftPaddle;
		//print (paddle1Angle.z);
		paddle1.transform.localEulerAngles = paddle1Angle;

		Vector3 paddle2Angle = paddle2.transform.localEulerAngles;
		paddle2Angle.z = -80f + angleToRotateRightPaddle;
		//print (paddle1Angle.z);
		paddle2.transform.localEulerAngles = paddle2Angle;


		tutorialActive = TutorialGORef.GetComponent<RaftingTutorial>().tutorial.enabled;
		if (tutorialActive)
			return;
		

		//allow paddling!
		if ((Input.GetKeyDown(left) || Input.GetKeyDown(keyLeft) || Input.GetKeyDown(backLeft) || Input.GetKeyDown(keyBackLeft)) && lCool <= 0) {
			lCool = cooldownFrames;
			forwardPaddleSound.Play ();
		}
		if (Input.GetKeyDown(right) || Input.GetKeyDown(keyRight) || Input.GetKeyDown(backRight) || Input.GetKeyDown(keyBackRight) && rCool <= 0) {
			rCool = cooldownFrames;
			forwardPaddleSound.Play ();
			//rightPaddle.AddForce (rightPaddle.transform.up * paddleForce);
		}





		if (winnerExists && Time.time - victoryTime > victoryDelay) {
			ScoreKeeper.DisplayResultsScreen (); //leave the minigame
		}

		//Time to deal with powerups
		if (power1 == PowerUp.PowerUpType.None) {
			powerImage1.enabled = false;
		}
		if (power2 == PowerUp.PowerUpType.None) {
			powerImage2.enabled = false;
		}


		//Handle Cannon powerup
		if (power1 == PowerUp.PowerUpType.Cannon) {
			powerImage1.enabled = true;
			powerImage1.sprite = cannonSprite;
			cannon1.SetActive(true);
			//rotate the cannon to match the control stick
			float controlStickAngle = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis(leftAxisH), Input.GetAxis(leftAxisV));
			cannon1.transform.eulerAngles = new Vector3(0, 0, 180f-controlStickAngle);
			if (Mathf.Abs (Input.GetAxis (leftAxisH)) + Mathf.Abs (Input.GetAxis (leftAxisV)) < .1f) {
				cannon1.transform.localEulerAngles = new Vector3(0,0,180);
                //cannon1line.SetActive(false);
			}
            else {
                //cannon1line.SetActive(true);
            }
		} else {
			cannon1.SetActive(false);
		}
			
		if (power2 == PowerUp.PowerUpType.Cannon) {
			powerImage2.enabled = true;
			powerImage2.sprite = cannonSprite;
			cannon2.SetActive (true);
			//rotate the cannon to match the control stick
			float controlStickAngle = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis(rightAxisH), Input.GetAxis(rightAxisV));
			cannon2.transform.eulerAngles = new Vector3(0, 0, 180f-controlStickAngle);
			if (Mathf.Abs (Input.GetAxis (rightAxisH)) + Mathf.Abs (Input.GetAxis (rightAxisV)) < .2f) {
				cannon2.transform.eulerAngles = this.transform.eulerAngles;
                //cannon2line.SetActive(false);
			}
            else {
                //cannon2line.SetActive(true);
            }
		} else {
			cannon2.SetActive (false);
		}




		//allow player to fire powerups
		if (Input.GetKeyDown(fireLeft) || Input.GetKeyDown(keyFireLeft) ||
            Input.GetAxis(fireTriggerLeft) >= 0.9f) {
			if(power1 == PowerUp.PowerUpType.Cannon) {
				power1 = PowerUp.PowerUpType.None;
				cannon1.GetComponent<Cannon> ().fire ();
				cannon1line.SetActive (false);
				cannonFire.Play ();
			}
			if(power1 == PowerUp.PowerUpType.Grapple) {
				power1 = PowerUp.PowerUpType.None;
				grapple1.GetComponent<Grapple> ().fire ();
				grapple1line.SetActive (false);
				grappleLaunch.Play ();
			}
            if(power1 == PowerUp.PowerUpType.Sail)
            {
                power1 = PowerUp.PowerUpType.None;
				sailPole1.SetActive (false);
                StartCoroutine(Boost(sail1, sailPole1));
            }
		}
		if (Input.GetKeyDown(fireRight)|| Input.GetKeyDown(keyFireRight) ||
            Input.GetAxis(fireTriggerRight) >= 0.9f) {
			if(power2 == PowerUp.PowerUpType.Cannon) {
				power2 = PowerUp.PowerUpType.None;
				cannon2.GetComponent<Cannon> ().fire ();
				cannon2line.SetActive (false);
				cannonFire.Play ();
			}
			if(power2 == PowerUp.PowerUpType.Grapple) {
				power2 = PowerUp.PowerUpType.None;
				grapple2.GetComponent<Grapple> ().fire ();
				grapple2line.SetActive (false);
				grappleLaunch.Play ();
			}
            if (power2 == PowerUp.PowerUpType.Sail)
            {
                power2 = PowerUp.PowerUpType.None;
				sailPole2.SetActive (false);
                StartCoroutine(Boost(sail2, sailPole2));
            }
        }

		//Handle Grapple powerup stuff
		if (power1 == PowerUp.PowerUpType.Grapple) {
			grapple1.SetActive (true);
			powerImage1.enabled = true;
			powerImage1.sprite = grappleSprite;
			currentScaling = .1f; //for some reason having an active grapple makes you go way faster, so I manually nerfed it this way
			//rotate the grapple to match the control stick
			float controlStickAngle = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis(leftAxisH), Input.GetAxis(leftAxisV));
			grapple1.transform.eulerAngles = new Vector3(0, 0, 180f-controlStickAngle);
			if (Mathf.Abs (Input.GetAxis (leftAxisH)) + Mathf.Abs (Input.GetAxis (leftAxisV)) < .1f) {
				grapple1.transform.localEulerAngles = new Vector3(0,0,180);
			}
		} else {
			currentScaling = 1f; //un-nerf water speed
		}

		if (power2 == PowerUp.PowerUpType.Grapple) {
			grapple2.SetActive (true);
			powerImage2.enabled = true;
			powerImage2.sprite = grappleSprite;
			currentScaling = .1f; //for some reason grapple makes you flow way faster, so I had to manually nerf the current
			//rotate the cannon to match the control stick
			float controlStickAngle = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis(rightAxisH), Input.GetAxis(rightAxisV));
			grapple2.transform.eulerAngles = new Vector3(0, 0, 180f-controlStickAngle);
			if (Mathf.Abs (Input.GetAxis (rightAxisH)) + Mathf.Abs (Input.GetAxis (rightAxisV)) < .1f) {
				grapple2.transform.localEulerAngles = new Vector3(0,0,0);
			}
		} else {
			currentScaling = 1f; //un-nerf when you don't have a grapple anymore
		}

        //Handle Sail powerup stuff
		if (power1 == PowerUp.PowerUpType.Sail) {
			powerImage1.enabled = true;
			powerImage1.sprite = sailSprite;
			sailPole1.SetActive (true);
		}

        if (power2 == PowerUp.PowerUpType.Sail)
        {
            powerImage2.enabled = true;
            powerImage2.sprite = sailSprite;
			sailPole2.SetActive (true);
        }
    }
		
	void FixedUpdate () {
		if (tutorialActive) //tutorial is onscreen
			return;
		
		if (spinFrameCounter > 0) { //caught in a whirlpool
			raft.velocity = Vector3.zero;
			leftPrompt.enabled = false;
			rightPrompt.enabled = false;
			Quaternion spinRot = this.transform.rotation;
			spinRot.eulerAngles += new Vector3 (0, 0, spinOutSpeed);
			this.transform.rotation = spinRot;
			spinFrameCounter--;
			return; //whirlpool is a stun, so you don't get to do anything else
		} else {
			//this makes sure that everything is back to normal when we leave the whirlpool
			leftPrompt.enabled = true;
			rightPrompt.enabled = true;
		}
		if (noControl) //after finish line, you can't paddle any more, but you can still hit hazards, I guess, so keep this after spinframe if
			return;

		//limit maximum speed
		if (raft.velocity.magnitude > speedLimit) {
			raft.velocity = Vector3.Normalize (raft.velocity) * speedLimit;
		}
			
		//Confine boat angle to the 180 degrees facing forwards!
		float zRot = this.transform.rotation.eulerAngles.z;
		Quaternion newRot = this.transform.rotation;
		if (zRot < 269f && zRot >= 180f) {
			newRot.eulerAngles = new Vector3(0,0, 270);
			raft.angularVelocity = Vector3.zero;

		}
		if (zRot < 180f && zRot > 91f) {
			newRot.eulerAngles = new Vector3 (0, 0, 90);
			raft.angularVelocity = Vector3.zero;
		}
		raft.maxAngularVelocity = .4f;
		raft.angularVelocity = raft.angularVelocity * .9f;

		this.transform.rotation = newRot;


		//add force while paddle keys are down and cooldown is inactive
		//also, this part handles prompt sprites and colors according to the states.
		leftPrompt.sprite = aButton;
		leftPrompt.color = Color.gray;
		if (lCool > cooldownFrames - paddleFrames && (Input.GetKey (left) || Input.GetKey(keyLeft))) {
			leftPaddle.AddForce (leftPaddle.transform.up * paddleForce);
			leftPrompt.color = Color.white;
			leftPrompt.sprite = aButton;
		} else if (lCool > cooldownFrames - paddleFrames && (Input.GetKey (backLeft) || Input.GetKey(keyBackLeft))) {
			leftPaddle.AddForce (leftPaddle.transform.up * -paddleForce);
			leftPrompt.color = Color.white;
			leftPrompt.sprite = bButton;
		}  else if (lCool > cooldownFrames - paddleFrames) {
			lCool = cooldownFrames - paddleFrames;
		}
		rightPrompt.color = Color.gray;
		rightPrompt.sprite = aButton;
		if (rCool > cooldownFrames - paddleFrames && (Input.GetKey (right) || Input.GetKey(keyRight))) {
			rightPaddle.AddForce (rightPaddle.transform.up * paddleForce);
			rightPrompt.color = Color.white;
			rightPrompt.sprite = aButton;
		} else if (rCool > cooldownFrames - paddleFrames && (Input.GetKey (backRight) || Input.GetKey(keyBackRight))) {
			rightPaddle.AddForce (rightPaddle.transform.up * -paddleForce);
			rightPrompt.color = Color.white;
			rightPrompt.sprite = bButton;
		} else if (rCool > cooldownFrames - paddleFrames) {
			rCool = cooldownFrames - paddleFrames;


		}

		//tick down the cooldown for each side to paddle. As of 4/4/2016, the cooldown is irrelevant, but with some balance changes in the inspector this could become important.
		if (rCool > 0) {
			rCool--;
		} 
		if (lCool > 0) {
			lCool--;
		} 

	}

	void OnTriggerEnter(Collider coll) {
		GameObject other = coll.gameObject;
		if (other.tag == "Danger") { //whirlpool
			transform.position = other.transform.position;
			whirlpoolSplash.Play ();

            float distancebetween = otherRaftScript.transform.position.y - transform.position.y;    // How far ahead the otehr raft is
            if (distancebetween < 15f)
            {
                spinFrameCounter = spinOutDuration;
            }
            else if (distancebetween < 30f)
            {
                spinFrameCounter = spinOutDuration / 2;
                print("lagging a bit");
            }
            else
            {
                spinFrameCounter = spinOutDuration / 4;
                print("lagging a lot");
            }
			Destroy (other);
		}
		if (other.tag == "Finish") {
			finishText.enabled = true;
            winnerText.text = "Players " + player1.ToString() + " and " + player2.ToString() + " Win!";
            winnerText.enabled = true;
			winSound.Play ();
			Destroy (other); //break the finish line
			noControl = true;
			raft.angularDrag = 2f; //slow down a bit
			otherRaftScript.noControl = true;
			otherRaftScript.raft.angularDrag = 2f;
			prepVictory (); //trigger the countdown to results screen and score counting
		}
		if (other.tag == "PowerUp") {
			if (other.GetComponent<PowerUp> ().type == PowerUp.PowerUpType.Cannon) {
				getPowerUps (PowerUp.PowerUpType.Cannon);
			}
			if (other.GetComponent<PowerUp> ().type == PowerUp.PowerUpType.Grapple) {
				getPowerUps (PowerUp.PowerUpType.Grapple);
			}
            if (other.GetComponent<PowerUp>().type == PowerUp.PowerUpType.Sail)
            {
                getPowerUps(PowerUp.PowerUpType.Sail);
            }
			other.GetComponent<PowerUp> ().die ();
		}
			

	}

	void OnCollisionEnter(Collision coll) {
		GameObject other = coll.collider.gameObject;
		if (other.tag == "Cannonball") {
			if (other.GetComponent<Cannonball> ().enemyRaft == this.gameObject) {
				print ("I WAS HIT BY A CANNONBALL!");
				spinFrameCounter = spinOutDuration * 2; //get stunned just like with a whirlpool
				Destroy (other);
			}
		}
	}
		
	void OnTriggerStay(Collider coll) {
		GameObject other = coll.gameObject;
		if (other.tag == "Current" && !tutorialActive) {
			getPushedByCurrent(other.GetComponent<Current>().flowVector);
		}
	}

	void getPushedByCurrent(Vector3 waterFlow)
	{
		Vector3 velDiff = waterFlow - raft.velocity;
		raft.AddForce (velDiff  * currentScaling);
	}


	//allow the players to grab powerups, but don't overwrite old ones!
	void getPowerUps(PowerUp.PowerUpType incoming) {
		cannon1line.SetActive (true);
		cannon2line.SetActive (true);
		grapple1line.SetActive (true);
		grapple2line.SetActive (true);
		powerupGrab.Play ();
		if (power1 == PowerUp.PowerUpType.None)
			power1 = incoming;
		if (power2 == PowerUp.PowerUpType.None)
			power2 = incoming;
	}

	//tell the scoring system everything it needs to know a couple seconds before the game quits
	void prepVictory() {
		winnerExists = true;
		victoryTime = Time.time; //record the moment when you won so we know when to load the next scene
		ScoreKeeper.ClearRoundScores ();
		ScoreKeeper.roundScores [player1] = 12;
		ScoreKeeper.roundScores [player2] = 12;
		ScoreKeeper.winMessage = "PLAYERS " + player1.ToString () + " AND " + player2.ToString () + " WIN!";

	}

	private IEnumerator Boost(GameObject sail, GameObject sailPole) {
		sail.SetActive (true);
		sailSound.volume = 1f;
		sailPole.SetActive (true);
        for (int i = 0; i < 90f; ++i) {
			if (!noControl)
            	raft.AddForce(boostForce * transform.up);
            yield return new WaitForSeconds(0.01f);
        }
		sail.SetActive (false);
		sailPole.SetActive (false);
		sailSound.volume = 0f;
    }
}
