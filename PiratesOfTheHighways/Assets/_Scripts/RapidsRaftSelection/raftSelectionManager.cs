using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class raftSelectionManager : MonoBehaviour {

    [Header("Types of ships")]
    public List<GameObject>     shipTypes = new List<GameObject>();

    [Header("Colors available to choose from")]
    public List<Color>          colorList = new List<Color>();     // A list with the available colors to choose from

    [Header("Button colors and sizing")]
    public Color selectedColor;             // The color of the buttons whene selected;
    public Color unselectedColor;           // The color of the buttons when not selected
    public Vector2  selectedSize;           // The size of thhe button when selected (X width, Y height)
    public Vector2  unselectedSize;         // The size of the button when not selected (X width, Y height)

    [Header("GameObject References")]
    public GameObject[] buttonReferences;       // The order is from top to bottom (team 1 type, team 1 color, team 2 type, team 2 color)
    public GameObject[] TeamReadyReferences;    // Reference to the team ready gameObject
    public GameObject[] TeamTextReferences;     // Reference to the team Text gameObjects
    public GameObject   countDownReference;     // Reference to the countdown text Object
    public GameObject   pressStartText;         // Reference to the pressStart text tutorial

    [Header("Arrow References")]
    public GameObject[] rightArrows;
    public GameObject[] leftArrows;
    public Color ArrowOriginalColor;
    public Color ArrowSelectedColor;

    [Header("SoundClip References")]
    public AudioClip StartSound;
    public AudioClip ArrowMoveSound;
    public AudioClip ArrowErrorSound;
	public float errorVolume;
	public float moveVolume;
	public float startVolume;

    //---PRIVATE VARIABLES---//

    // input variables
    private string[] xAxisNames = new string[4];
    private string[] yAxisNames = new string[4];
    private string[] startButtonNames = new string[4];
    private string[] BButtonNames = new string[4];

    // state variables
    private bool[] xAxisInUse = new bool[4];           // Set to true when x axis of a player is used
    private bool[] yAxisInUse = new bool[4];           // Set to true when y axis of a player is used

    private int []currentActiveButton = new int[2];    // currentActiveButton[team_number], the value is either 0 or 1 (for type or color respecively)
    private int []currentShipType = new int[2];        // currentShipType[team_number]
    private int []currentShipColor = new int[2];       // currentShipColor[team_number]

    private GameObject boatTeam1;
    private GameObject boatTeam2;

    private Vector3 boatTeam1Pos = new Vector3(-21f, -6f, 0);
    private Vector3 boatTeam2Pos = new Vector3(21f, -6f, 0);
    private Vector3 boatEulerRotation = new Vector3(270f, 0f, 0f);

    private bool readyToStart = false;
    private bool countdownStarted = false;
    private bool selectionLocked = false;

    Coroutine co;   // Coroutine reference

    void Start() {
        InitializeStateValues();
        InitializeBoats();
        UpdateButtons();
        InitializeNamesAndState();
        checkArrows();
    }

    void Update() {
        if (!selectionLocked) {
            checkIfGameReadyToStart();
            getInput();
        }
    }

    private void getInput() {
        getTeamInput(0);
        getTeamInput(1);
        UpdateAxisUsage();
    }

    private void getTeamInput(int teamNum) {
        int player1 = teamNum * 2;
        int player2 = teamNum * 2 + 1;

        // B button check
        /*
        if (TeamReadyReferences[teamNum].activeSelf == true
            && (Input.GetButtonDown(BButtonNames[player1]) == true || Input.GetButtonDown(BButtonNames[player2]) == true)) {
            TeamReadyReferences[teamNum].SetActive(false);
            resetCountdown();
            print("team:" + teamNum + " pressed back");
        }
        */

        // exit all input if team is ready
        if (TeamReadyReferences[teamNum].activeSelf == true) {
            return;
        }

        // Start button check
        if (Input.GetButtonDown(startButtonNames[0]) == true) {
            print("team:" + teamNum + " pressed start");
            TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(StartSound, startVolume);
            TeamReadyReferences[0].SetActive(true);
            TeamReadyReferences[1].SetActive(true);
            return;
        }

        // Positive X
        if ((xAxisInUse[player1] == false && Input.GetAxis(xAxisNames[player1]) > 0f && Mathf.Abs(Input.GetAxis(yAxisNames[player1])) < 0.1f) || 
            (xAxisInUse[player2] == false && Input.GetAxis(xAxisNames[player2]) > 0f && Mathf.Abs(Input.GetAxis(yAxisNames[player1])) < 0.1f)) {
            print("team:" + teamNum + " move right");
            horizontalSlider(teamNum, 1);
            checkArrows();
            forwardArrowAnimate(teamNum);
            return;
        }

        // Negative X
        if ((xAxisInUse[player1] == false && Input.GetAxis(xAxisNames[player1]) < 0f && Mathf.Abs(Input.GetAxis(yAxisNames[player1])) < 0.1f) ||
            (xAxisInUse[player2] == false && Input.GetAxis(xAxisNames[player2]) < 0f && Mathf.Abs(Input.GetAxis(yAxisNames[player1])) < 0.1f)) {
            print("team:" + teamNum + " move left");
            horizontalSlider(teamNum, -1);
            checkArrows();
            backwardArrowAnimate(teamNum);
            return;
        }

        // Positive Y
        if ((yAxisInUse[player1] == false && Input.GetAxis(yAxisNames[player1]) > 0f && Mathf.Abs(Input.GetAxis(xAxisNames[player1])) < 0.1f) ||
            (yAxisInUse[player2] == false && Input.GetAxis(yAxisNames[player2]) > 0f && Mathf.Abs(Input.GetAxis(xAxisNames[player1])) < 0.1f)) {
            print("team:" + teamNum + " move up");
            verticalSlider(teamNum, 1);
            checkArrows();
            return;
        }

        // Negative Y
        if ((yAxisInUse[player1] == false && Input.GetAxis(yAxisNames[player1]) < 0f && Mathf.Abs(Input.GetAxis(xAxisNames[player1])) < 0.1f) ||
            (yAxisInUse[player2] == false && Input.GetAxis(yAxisNames[player2]) < 0f && Mathf.Abs(Input.GetAxis(xAxisNames[player1])) < 0.1f)) {
            print("team:" + teamNum + " move down");
            verticalSlider(teamNum, -1);
            checkArrows();
            return;
        }
    }

    private void horizontalSlider(int teamNum, int amount) {
        // If boat type button is active check
        if (currentActiveButton[teamNum] == 0) {
            int shipIndexRequested = currentShipType[teamNum] + amount;
            if (shipIndexRequested >= shipTypes.Count) {
                TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(ArrowErrorSound, errorVolume);
                print("No more ships");
                return;
            }
            else if (shipIndexRequested < 0) {
                TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(ArrowErrorSound, errorVolume);
                print("This is the first ship");
                return;
            }
            else {
                currentShipType[teamNum] = shipIndexRequested;
                setShip(teamNum, shipIndexRequested);
                TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(ArrowMoveSound, moveVolume);
                return;
            }
        }

        // If color button is active
        if (currentActiveButton[teamNum] == 1) {
            int colorIndexRequested = currentShipColor[teamNum] + amount;
            if (colorIndexRequested >= colorList.Count) {
                TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(ArrowErrorSound, errorVolume);
                print("No more colors");
                return;
            }
            else if (colorIndexRequested < 0) {
                TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(ArrowErrorSound, errorVolume);
                print("This is the first color");
                return;
            }
            else {
                currentShipColor[teamNum] = colorIndexRequested;
                setColor(teamNum, colorIndexRequested);
                TeamTextReferences[teamNum].GetComponent<AudioSource>().PlayOneShot(ArrowMoveSound, moveVolume);
                return;
            }
        }
    }

    private void verticalSlider(int teamNum, int amount) {
        if (amount < 0) {
            if (currentActiveButton[teamNum] == 0) {
                currentActiveButton[teamNum] = 1;
                UpdateButtons();
            }
            else if (currentActiveButton[teamNum] == 1) {
                print("Cannot go down, reached lowest button");
            }
        }
        if (amount > 0) {
            if (currentActiveButton[teamNum] == 1) {
                currentActiveButton[teamNum] = 0;
                UpdateButtons();
            }
            else if (currentActiveButton[teamNum] == 0) {
                print("Cannot go up, reached highest button");
            }
        }
    }

    // Sets the visual representation of the selected ship in the Scene
    private void setShip(int teamNum, int index) {
        Quaternion rotation = Quaternion.Euler(boatEulerRotation);
        if (teamNum == 0) {
            Destroy(boatTeam1);
            boatTeam1 = Instantiate(shipTypes[index], boatTeam1Pos, rotation) as GameObject;
            setColor(teamNum, currentShipColor[teamNum]);
        }
        else if (teamNum == 1) {
            Destroy(boatTeam2);
            boatTeam2 = Instantiate(shipTypes[index], boatTeam2Pos, rotation) as GameObject;
            setColor(teamNum, currentShipColor[teamNum]);
        }
        else {
            print("Error, wrong team number");
        }
    }

    // Sets the color of the selected ship in the Scene
    private void setColor(int teamNum, int index) {
        if (teamNum == 0) {
            boatTeam1.GetComponent<Renderer>().material.color = colorList[index];
        }
        else if (teamNum == 1) {
            boatTeam2.GetComponent<Renderer>().material.color = colorList[index];
        }
        else {
            print("Error, wrong team number");
        }
    }

    private void UpdateButtons() {
        // Team 1 update
        for (int i = 0; i < 2; ++i) {
            if (currentActiveButton[i] == 0) {
                updateButton(true, 2 * i);
                updateButton(false, 1 + 2 * i);
            }
            else {
                updateButton(false, 2 * i);
                updateButton(true, 1 + 2 * i);
            }
        }
    }

    private void updateButton(bool isActive, int index) {
        if (isActive) {
            buttonReferences[index].GetComponent<RectTransform>().sizeDelta = selectedSize;
            buttonReferences[index].GetComponent<Image>().color = selectedColor;
        }
        else {
            buttonReferences[index].GetComponent<RectTransform>().sizeDelta = unselectedSize;
            buttonReferences[index].GetComponent<Image>().color = unselectedColor;
        }
    }

    #region Initialization functions

    private void InitializeStateValues() {
        currentActiveButton[0] = 0;
        currentActiveButton[1] = 0;
        currentShipType[0] = 0;
        currentShipType[1] = 0;
        currentShipColor[0] = 0;
        currentShipType[1] = 0;
    }

    // Sets the names of all axis and their state to false(not in use)
    private void InitializeNamesAndState() {
        for (int i = 1; i < 5; ++i) {
            xAxisNames[i - 1] = "Player" + i + "_axisX";
            yAxisNames[i - 1] = "Player" + i + "_axisY";
            startButtonNames[i - 1] = "Player" + i + "_buttonStart";
            BButtonNames[i - 1] = "Player" + i + "_buttonB";
            xAxisInUse[i - 1] = false;
            yAxisInUse[i - 1] = false;
        }
    }

    private void InitializeBoats() {
        Quaternion rotation = Quaternion.Euler(boatEulerRotation);
        boatTeam1 = Instantiate(shipTypes[currentShipType[0]], boatTeam1Pos, rotation) as GameObject;
        boatTeam2 = Instantiate(shipTypes[currentShipType[1]], boatTeam2Pos, rotation) as GameObject;
    }

    #endregion

    // Updates the Axis usage of all players accordingly
    private void UpdateAxisUsage() {
        for (int i = 0; i < 4; ++i) {
            if (Input.GetAxis(xAxisNames[i]) != 0f) {
                xAxisInUse[i] = true;
            }
            else {
                xAxisInUse[i] = false;
            }

            if (Input.GetAxis(yAxisNames[i]) != 0f) {
                yAxisInUse[i] = true;
            }
            else {
                yAxisInUse[i] = false;
            }
        }
    }

    // checks if it is time to start the game
    private void checkIfGameReadyToStart() {
        if (TeamReadyReferences[0].activeSelf == true && TeamReadyReferences[1].activeSelf == true) {
            readyToStart = true;
            if (!countdownStarted) {
                countdownStarted = true;
                co = StartCoroutine(displayText());
            }
        }
    }

    private IEnumerator displayText() {

        //// --- Uncomment to display Countdown text --- ////

        //Text textRef = countDownReference.GetComponent<Text>();
        //countDownReference.SetActive(true);

        //// Increase 3
        //for (int i = 0; i < 70; ++i) {
        //    textRef.fontSize += 2;
        //    yield return new WaitForSeconds(0.015f);
        //}

        //// change to 2 (small size) and increase size
        //textRef.fontSize = 50;
        //textRef.text = "2";
        //for (int i = 0; i < 70; ++i) {
        //    textRef.fontSize += 2;
        //    yield return new WaitForSeconds(0.015f);
        //}
        //// change to 1 (small size) and increase size
        //textRef.fontSize = 50;
        //textRef.text = "1";
        //for (int i = 0; i < 70; ++i) {
        //    textRef.fontSize += 2;
        //    yield return new WaitForSeconds(0.015f);
        //}

        //setStaticVariables();

        //// Lock selection
        //selectionLocked = true;
        //TeamReadyReferences[0].SetActive(false);
        //TeamReadyReferences[1].SetActive(false);
        //boatTeam1.GetComponent<moveSelectionScreenRaftForward>().active = true;
        //boatTeam2.GetComponent<moveSelectionScreenRaftForward>().active = true;
        //yield return new WaitForSeconds(2.5f);
        //// Load next scene here
        //SceneManager.LoadScene("mapSelectionScreen");

        setStaticVariables();
        selectionLocked = true;

        yield return new WaitForSeconds(0.5f);
        boatTeam1.GetComponent<moveSelectionScreenRaftForward>().active = true;
        boatTeam2.GetComponent<moveSelectionScreenRaftForward>().active = true;
        yield return new WaitForSeconds(2.5f);
        // Load next scene here
        SceneManager.LoadScene("mapSelectionScreen");
    }

    // Called when someone presses B during loading
    private void resetCountdown() {
        print("reset countdown ran");
        if (co != null) {
            StopCoroutine(co);
        }
        Text textRef = countDownReference.GetComponent<Text>();
        countDownReference.SetActive(false);
        textRef.text = "3";
        textRef.fontSize = 50;
        readyToStart = false;
        countdownStarted = false;
    }

    private void setStaticVariables() {
        staticRaftStats.team1ShipNum = currentShipType[0];
        staticRaftStats.team2ShipNum = currentShipType[1];

        staticRaftStats.team1ShipColor = currentShipColor[0];
        staticRaftStats.team2ShipColor = currentShipColor[1];
    }

    #region Arrow Check functions

    private void checkArrows() {
        for (int teamNum = 0; teamNum < 2; ++teamNum) {
            if (currentActiveButton[teamNum] == 0) {
                rightArrows[teamNum * 2 + 1].SetActive(false);
                leftArrows[teamNum * 2 + 1].SetActive(false);
                checkForward(teamNum * 2, 0, teamNum);
                checkBackward(teamNum * 2, 0, teamNum);
            }
            else if (currentActiveButton[teamNum] == 1) {
                rightArrows[teamNum * 2].SetActive(false);
                leftArrows[teamNum * 2].SetActive(false);
                checkForward(teamNum * 2 + 1, 1, teamNum);
                checkBackward(teamNum * 2 + 1, 1, teamNum);
            }
        }
    }

    private void checkForward(int index, int activeButton, int teamNum) {
        // Check shipType
        if (activeButton == 0) {
            if (currentShipType[teamNum] == (shipTypes.Count - 1)) {
                rightArrows[index].SetActive(false);
            }
            else {
                rightArrows[index].SetActive(true);
            }
            return;
        }
        
        // Check shipColor
        if (activeButton == 1) {
            if (currentShipColor[teamNum] == (colorList.Count - 1)) {
                rightArrows[index].SetActive(false);
            }
            else {
                rightArrows[index].SetActive(true);
            }
            return;
        }
        print("Check Forward Failed");
    }

    private void checkBackward(int index, int activeButton, int teamNum) {
        // Check shipType
        if (activeButton == 0) {
            if (currentShipType[teamNum] == 0) {
                leftArrows[index].SetActive(false);
            }
            else {
                leftArrows[index].SetActive(true);
            }
            return;
        }

        // Check shipColor
        if (activeButton == 1) {
            if (currentShipColor[teamNum] == 0) {
                leftArrows[index].SetActive(false);
            }
            else {
                leftArrows[index].SetActive(true);
            }
            return;
        }
        print("Check Backward Failed");
    }

    private void forwardArrowAnimate(int teamNum) {
        int index = 2 * teamNum;
        if (currentActiveButton[teamNum] == 0) {
            if (currentShipType[teamNum] != (shipTypes.Count - 1)) {
                StartCoroutine(ChangeArrowColor(rightArrows[index]));
            }
        }
        else if (currentActiveButton[teamNum] == 1) {
            index++;
            if (currentShipColor[teamNum] != (colorList.Count - 1)) {
                StartCoroutine(ChangeArrowColor(rightArrows[index]));
            }
        }
    }

    private void backwardArrowAnimate(int teamNum) {
        int index = 2 * teamNum;
        if (currentActiveButton[teamNum] == 0) {
            if (currentShipType[teamNum] != 0) {
                StartCoroutine(ChangeArrowColor(leftArrows[index]));
            }
        }
        else if (currentActiveButton[teamNum] == 1) {
            index++;
            if (currentShipColor[teamNum] != 0) {
                StartCoroutine(ChangeArrowColor(leftArrows[index]));
            }
        }
    }

    private IEnumerator ChangeArrowColor(GameObject go) {
        print("run coroutine");
        go.GetComponent<Image>().color = ArrowSelectedColor;
        yield return new WaitForSeconds(0.2f);
        go.GetComponent<Image>().color = ArrowOriginalColor;
    }

    #endregion
}
