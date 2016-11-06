using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DriveThroughTutorial : MonoBehaviour {

    Dictionary<int, int> actualPlyToMiniGamePly = new Dictionary<int, int>(4);

    public int numberOfTexts = 2;
    [Header("Sound References")]
    public AudioClip clickSound;

    [Header("Object References")]
    public List<GameObject> tutorialTexts;
    public List<GameObject> arrowsForTexts;
    public Text textCommitOrder;

    [Header("Player sprite References")]
    public List<GameObject> playerSprites;                  // The sprites of each player

    public GameObject orderSummaryGoRef;                    // Reference of the order summary gameobject

    private string      miniGameScene = "DriveThrough";
    private int         activeTextNum = 0;                  // What is our current tutorial text on display (default should be zero)
    private int         playerWhoCommitsOrder;

    void Start() {
        Initialize();
    }
    #region Initialization Functions used only at Start
    // Runs all initialization functions
    private void Initialize() {
        InitializeTextArrowVisibility();
        InitializeDictionary();
        FindPlayerWhoCommitsOrder();
        InitializePlayerSprites();
        InitializeTexts();
    }

    private void InitializeTextArrowVisibility() {
        tutorialTexts[activeTextNum].SetActive(true);
        arrowsForTexts[activeTextNum].SetActive(true);
        if (tutorialTexts.Count > 1) {
            for (int i = 1; i < tutorialTexts.Count; ++i) {
                tutorialTexts[i].SetActive(false);
            }
        }
        if (arrowsForTexts.Count > 1) {
            for (int i = 1; i < arrowsForTexts.Count; ++i) {
                arrowsForTexts[i].SetActive(false);
            }
        }
    }

    // Modify this accordingly
    private void InitializeDictionary() {
        for (int i = 1; i < 5; ++i) {
            actualPlyToMiniGamePly[i] = i;
        }
        actualPlyToMiniGamePly[2] = 4;
        actualPlyToMiniGamePly[4] = 2;
    }

    // Initializes the player number and sprites
    private void InitializePlayerSprites() {
        for (int i = 1; i < 5; ++i) {
            // Change player sprite here

            int player_num = 0;
            // Loop through dictionary to find correct actual player_num
            for (int j = 1; j < 5; ++j) {
                if (actualPlyToMiniGamePly[j] == i) {
                    player_num = j;
                    break;
                }
            }

            playerSprites[i - 1].transform.GetChild(0).GetComponent<Text>().text = "Player " + player_num;
        }
    }

    private void FindPlayerWhoCommitsOrder() {
        for (int i = 1; i <= actualPlyToMiniGamePly.Count; ++i) {
            if (actualPlyToMiniGamePly[i] == 4) {
                playerWhoCommitsOrder = i;
                break;
            }
        }
    }

    private void InitializeTexts() {
        textCommitOrder.text = "When time runs out <color=red>Player " + playerWhoCommitsOrder + "</color> you need to remember the total quatinty of each item ordered and press 		     to commit your order\n\nPress            to go to previous hint\n\nPress              to start the game";
        orderSummaryGoRef.SetActive(false);
    }

    #endregion

    void Update() {
        getInput();
    }

    private void getInput() {
        for (int i = 1; i < 5; ++i) {
            if (Input.GetKeyDown("joystick " + i + " button 0")) {
                getNextTutorialText();
                this.GetComponent<AudioSource>().PlayOneShot(clickSound);
                print("Player " + i + " Pressed A");
                break;
            }
            if (Input.GetKeyDown("joystick " + i + " button 1")) {
                getPreviousTutorialText();
                this.GetComponent<AudioSource>().PlayOneShot(clickSound);
                print("Player " + i + " Pressed B");
                break;
            }
        }
    }

    private void getNextTutorialText() {
        tutorialTexts[activeTextNum].SetActive(false);
        arrowsForTexts[activeTextNum].SetActive(false);
        activeTextNum++;
        if (activeTextNum == numberOfTexts) {
            print("nextScene");
            this.GetComponent<SceneFadeInOut>().EndScene(miniGameScene);
            //SceneManager.LoadScene(miniGameScene);
            return;
        }
        tutorialTexts[activeTextNum].SetActive(true);
        arrowsForTexts[activeTextNum].SetActive(true);

        if (activeTextNum == 1) {
            orderSummaryGoRef.SetActive(true);
        }
        else {
            orderSummaryGoRef.SetActive(false);
        }
    }

    private void getPreviousTutorialText() {
        if (activeTextNum == 0) {
            print("This is the first tutorial text; Can't go back further");
            return;
        }
        tutorialTexts[activeTextNum].SetActive(false);
        arrowsForTexts[activeTextNum].SetActive(false);
        activeTextNum--;
        tutorialTexts[activeTextNum].SetActive(true);
        arrowsForTexts[activeTextNum].SetActive(true);
    }
}
