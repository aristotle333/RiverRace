using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    public static Timer S;

    // The time you initialized any timer
    public float startingTime;

    // This is the variable you want to observe or display (it is modified accordingly depending if you have normal timer or countdown timer active)
    public float currentTime;    

    public bool timerIsActive = false;              // Set to true if you have a normal timer active
    public bool countdownTimerIsActive = false;     // Set to true if you have a countdown timer active

    private float countDownOriginal;                // Used to get the time difference when we have a countdown timer

    void Awake() {
        S = this;
    }

	// Update is called once per frame
	void Update () {
	    if (timerIsActive) {
            currentTime = Time.time - startingTime;
            if (currentTime < 0f) {
                currentTime = 0f;
            }
        }
        if (countdownTimerIsActive) {
            currentTime = countDownOriginal - (Time.time - startingTime);
            if (currentTime < 0f) {
                currentTime = 0f;
            }
        }
	}

    // Use this function to Start the timer
    public void StartTimer() {
        countdownTimerIsActive = false;
        timerIsActive = true;
        startingTime = Time.time;
    }

    // Starts a countdown timer with timerAmount of time.
    public void StartCountdownTimer(float timerAmount) {
        timerIsActive = false;
        countdownTimerIsActive = true;
        currentTime = timerAmount;
        countDownOriginal = timerAmount;
        startingTime = Time.time;
    }

    // Simply stops all timers
    public void StopTimers() {
        timerIsActive = false;
        countdownTimerIsActive = false;
    }

    // returns a string value (in seconds) of the time in 2 decimal places
    public string getCurrentTimeString() {
        return currentTime.ToString("0.##");
    }
}
