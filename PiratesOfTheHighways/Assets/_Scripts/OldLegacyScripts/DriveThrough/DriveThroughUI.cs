using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class DriveThroughUI : MonoBehaviour {

    public static DriveThroughUI S;

    Dictionary<int, int> actualPlyToMiniGamePly = new Dictionary<int, int> (4);        // The relationship between actual and minigame player numbers

    [Header("Object References")]
    public GameObject[] orderSets;
    public List<GameObject> orderPrefabs;                   // They must be in order, so Fries, Burger, Nuggets, and Milkshake
    public List<Text> ordersTextReferences;                 // Follow same convention of, Fries, Burger, Nuggets, and Milkshake
    public List<AudioSource> playersAudioSources;           // The Audio Sources for Each Player;
    public GameObject startText;                            // Reference to the startText Gameobject
    public GameObject Popup;                                // Reference to the Popup Gameobject
    public Text popupText;                                  // Reference to the popupText
    public Text roundText;                                  // Reference to the roundText
    public Text NumOrdersPerRoundText;                      // The max number of orders per person text reference
    public Text TimerText;                                  // The timer text reference
    public GameObject scorePrefab;

    [Header("Player sprite References")]
    public List<GameObject> playerSprites;                  

    [Header("Timer Variables")]
    public float timeForPlayersToOrder;                     // The amount of time players 1-3 have to pick their orders
    public float timeForPlayerToCommitOrder;                // The amount of time player 4 has to commit the orders of his crew
    public float waitingTime;                               // This is the amount of time spent waiting so that the player that makes the order can prepare
    public float timeDisplayingItemsOrdered;                // This is the amount of time spent displaying the items orderd after the round is over
    public float initializationTime;                        // Time spend waiting at the beginning of the game

    [Header("Offsets for Orders")]
    public Vector3 ordersInitPos;                           // The initial offset of the first order object
    public float ordersOffset;                              // The Offset for each consecutive order

    [Header("Round Variables and Statistics")]
    public int totalNumberOfRounds = 5;
    public int currentRound = 1;
    public int maxOrdersPerPerson = 3;
    public List<int> currentRequestedItems = new List<int>();       // The items requested by each of the players (1-3)

    [Header("Sound References")]
    public List<AudioClip> itemsPurchaseSounds = new List<AudioClip>();    // The audio clips for purchasing items, int he order of fries, burger, nuggets and milkshake
    public AudioClip successCommitOrder;
    public AudioClip failCommitOrder;
    public AudioClip errorSound;

    //-- Private Variables --//
    private List<int> numberOfItemsOrdered = new List<int>();       // The current number of items EACH player has ordered
    private const int PlayerWhoMakesOrders = 4;                     // It is always player 4 (mini game numbering) that orders
    private int       ActualPlayerWhoMakesOrders = 0;               // This is the actual player number who makes orders so anything with (1-4)
    private bool timeToOrder = false;                               // Is it time to order? (set to true if that is the case)

    private List<string> requestStrings = new List<string>();       // An array strings of the order messages to be displayed
    private List<int> requestNumItemsOrdered = new List<int>();     // The amount of each item that the pirate who orders requested

    private bool TimerIsActive = false;                             // Set to false if the timer in the UI is inactive
    private bool PlayersOrdering = false;
    private bool PlayerCommitingOrders = false;
    private bool waitingForPopUp = false;                           // This is true as long as the popup is active on the UI

    private bool orderPlayersWon = false;
    private bool commitPlayerWon = false;

    private List<GameObject> objectsToDelete = new List<GameObject>(); // These should be all the icons that players 1-3 ordered and should be deleted before player 4 starts commiting his order

    void Awake() {
        S = this;
        InitializeDictionary();
        InitializePlayerSprites();
        InitializeRequestStrings();
        InitializeRequestNumItems();
        // Initialize numberOfItemsOrdered Lists to zeroes
        for (int i = 0; i < 3; ++i) {
            numberOfItemsOrdered.Add(0);
        }        
    }

    void Start() {
        string initialMessage = "Round " + currentRound + "\n\n<size=25>Max number of orders: " + maxOrdersPerPerson + "</size>\n\nGet Ready";
        StartCoroutine(EnablePopUpWithTimerInitial(initialMessage, timeForPlayersToOrder, initializationTime));
        PlayersOrdering = true;
    }

    void Update() {
        getInput();
        if (TimerIsActive) {
            string time = Timer.S.getCurrentTimeString();
            TimerText.text = "Time Remaining: " + time;
            if (Timer.S.currentTime <= 0) {
                if (PlayersOrdering) {
                    PlayersOrdering = false;
                    string message = "Player " + ActualPlayerWhoMakesOrders + "\n\nGet Ready to commit the order";
                    StartCoroutine(EnablePopUpWithTimer(message, timeForPlayerToCommitOrder, timeDisplayingItemsOrdered));
                    PlayerCommitingOrders = true;
                    timeToOrder = true;
                }
                else if (PlayerCommitingOrders) {
                    checkOrder(PlayerWhoMakesOrders);
                    //resetCommitedOrders();
                    //PlayerCommitingOrders = false;
                    //timeToOrder = false;
                    //if (currentRound != totalNumberOfRounds) {
                    //    string message1 = "Time is up!\n\nFailed to commit the order";
                    //    NextRound();
                    //    playersAudioSources[PlayerWhoMakesOrders - 1].PlayOneShot(failCommitOrder);
                    //    string message2 = "Round " + currentRound + "\n\n<size=25>Max number of orders: " + maxOrdersPerPerson + " </size>\n\nGet Ready";
                    //    StartCoroutine(Enable2PopUpsWithTimer(message1, message2, timeForPlayersToOrder));
                    //    PlayersOrdering = true;
                    //}
                    //else {
                    //    string message1 = "Time is up!\n\nFailed to commit the order\n\nEnd of Game";
                    //    EndGame(message1);
                    //}
                }
            }
        }
    }

    #region Initialization functions
    // Modify this accordingly, now it maps 1 to 1, 2 to 2 and etc. and player 4 makes orders
    void InitializeDictionary() {
        // Note it starts at 1
        for (int i = 1; i < 5; ++i) {
            actualPlyToMiniGamePly[i] = i;
        }
        actualPlyToMiniGamePly[2] = 4;
        actualPlyToMiniGamePly[4] = 2;

        for (int i = 1; i < 5; ++i) {
            if (actualPlyToMiniGamePly[i] == PlayerWhoMakesOrders) {
                ActualPlayerWhoMakesOrders = i;
                break;
            }
        }
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

    void InitializeRequestStrings() {
        for (int i = 0; i < 4; i++) {
            requestStrings.Add("");
            UpdateOrderStringToValue(i, 0);
        }
    }

    // Initializes requestNumItemsOrdered and currentRequestedItems to zero
    void InitializeRequestNumItems() {
        for (int i = 0; i < 4; ++i) {
            requestNumItemsOrdered.Add(0);
            currentRequestedItems.Add(0);
        }
    }
    #endregion

    void NextRound() {
        if (currentRound < totalNumberOfRounds) {
            currentRound++;
            maxOrdersPerPerson++;
            resetNumberOfItemsOrdered();
            resetOrderValues();
            roundText.text = "Round: " + currentRound;
            NumOrdersPerRoundText.text = "Max number of orders is: " + maxOrdersPerPerson;
        }
    }

    // Sets a specific order string with a particular value/amount
    void UpdateOrderStringToValue(int listIdx, int value) {
        if (listIdx == 0) {
            requestStrings[listIdx] = "Fries           x<color=green>" + value + "</color>";
        }
        if (listIdx == 1) {
            requestStrings[listIdx] = "Burgers      x<color=red>" + value + "</color>";
        }
        if (listIdx == 2) {
            requestStrings[listIdx] = "Nuggets      x<color=yellow>" + value + "</color>";
        }
        if (listIdx == 3) {
            requestStrings[listIdx] = "Milkshakes x<color=blue>" + value + "</color>";
        }
        ordersTextReferences[listIdx].text = requestStrings[listIdx];
    }

    // Resets all values in the order counters to zero
    void resetCommitedOrders() {
        for (int i = 0; i < 4; ++i) {
            requestNumItemsOrdered[i] = 0;
            UpdateOrderStringToValue(i, 0);
        }
    }

    void resetNumberOfItemsOrdered() {
        for (int i = 0; i < 3; ++i) {
            numberOfItemsOrdered[i] = 0;
        }
    }

    private void getInput() {
        for (int i = 1; i < 5; ++i) {
            if (Input.GetKeyDown("joystick " + i + " button 0")) {
                frenchFriesButton(actualPlyToMiniGamePly[i]);
                print("Player " + i + " Pressed A");
                continue;
            }
            if (Input.GetKeyDown("joystick " + i + " button 1")) {
                burgerButton(actualPlyToMiniGamePly[i]);
                print("Player " + i + " Pressed B");
                continue;
            }
            if (Input.GetKeyDown("joystick " + i + " button 2")) {
                milkshakeButton(actualPlyToMiniGamePly[i]);
                print("Player " + i + " Pressed Y");
                continue;
            }
            if (Input.GetKeyDown("joystick " + i + " button 3")) {
                chickenNuggetButton(actualPlyToMiniGamePly[i]);
                print("Player " + i + " Pressed X");
                continue;
            }
            if (Input.GetKeyDown("joystick " + i + " button 7")) {
                checkOrder(actualPlyToMiniGamePly[i]);
                print("Player " + i + " Pressed StartButton");
                continue;
            }
        }
    }

    #region Button functions
    public void frenchFriesButton(int num) {
        if (timeToOrder && num == PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderSummary(0);
            return;
        }
        else if (!timeToOrder && num != PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderVisual(0, num - 1);
            return;
        }
    }

    public void burgerButton(int num) {
        if (timeToOrder && num == PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderSummary(1);
            return;
        }
        else if (!timeToOrder && num != PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderVisual(1, num - 1);
            return;
        }
    }

    public void chickenNuggetButton(int num) {
        if (timeToOrder && num == PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderSummary(2);
            return;
        }
        else if (!timeToOrder && num != PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderVisual(2, num - 1);
            return;
        }
    }

    public void milkshakeButton(int num) {
        if (timeToOrder && num == PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderSummary(3);
            return;
        }
        else if (!timeToOrder && num != PlayerWhoMakesOrders && !waitingForPopUp) {
            updateOrderVisual(3, num - 1);
            return;
        }
    }

    // Pressing the start button and checking if the order is correct
    public void checkOrder(int num) {
        if (!timeToOrder) {
            print("Can't take orders yet");
            playersAudioSources[num - 1].PlayOneShot(errorSound);
            return;
        }
        if (num != PlayerWhoMakesOrders) {
            print("Player not in charge of order");
            playersAudioSources[num - 1].PlayOneShot(errorSound);
            return;
        }
        if (timeToOrder && num == PlayerWhoMakesOrders && !waitingForPopUp) {
            PlayerCommitingOrders = false;
            timeToOrder = false;
            for (int i = 0; i < currentRequestedItems.Count; ++i) {
                if (currentRequestedItems[i] != requestNumItemsOrdered[i]) {
                    if (currentRound != totalNumberOfRounds) {
                        string endGameMessage = "Incorrect Order\n\nEnd of Game";
                        orderPlayersWon = true;
                        EndGame(endGameMessage);

                        //// Uncomment the below lines if we want the game to continue even if a commit was wrong

                        //string message1 = "Incorrect Order";
                        //NextRound();
                        //string message2 = "Round " + currentRound + "\n\n<size=25>Max number of orders: " + maxOrdersPerPerson + " </size>\n\nGet Ready";
                        //playersAudioSources[num - 1].PlayOneShot(failCommitOrder);
                        //print("Incorrect order audio played");
                        //StartCoroutine(Enable2PopUpsWithTimer(message1, message2, timeForPlayersToOrder));
                        //PlayersOrdering = true;
                    }
                    else {
                        string message1 = "Incorrect Order\n\nEnd of Game";
                        playersAudioSources[num - 1].PlayOneShot(failCommitOrder);
                        orderPlayersWon = true;
                        EndGame(message1);
                    }
                    resetCommitedOrders();
                    print("incorect order");
                    return;
                }
            }
            if (currentRound != totalNumberOfRounds) {
                string message1 = "Correct Order!";
                NextRound();
                playersAudioSources[num - 1].PlayOneShot(successCommitOrder);
                string message2 = "Round " + currentRound + "\n\n<size=25>Max number of orders: " + maxOrdersPerPerson + " </size>\n\nGet Ready";
                StartCoroutine(Enable2PopUpsWithTimer(message1, message2, timeForPlayersToOrder));
                PlayersOrdering = true;
            }
            else {
                string message1 = "Correct Order\n\nEnd of Game";
                playersAudioSources[num - 1].PlayOneShot(successCommitOrder);
                commitPlayerWon = true;
                EndGame(message1);
            }
            resetCommitedOrders();
            print("Correct Order!");
        }
    }
    #endregion

    // Visually updates what each player has ordered (Parrent number is the player number in miniGame values, so it is a number 0-2)
    void updateOrderVisual(int itemCode, int parentNumber) {
        if (numberOfItemsOrdered[parentNumber] >= maxOrdersPerPerson) {
            print("Reached max number of items that you can order this round");
            playersAudioSources[parentNumber].PlayOneShot(errorSound);
            return;
        }
        Vector3 spawnLocation = ordersInitPos;
        spawnLocation.x += ordersOffset * numberOfItemsOrdered[parentNumber]++;
        GameObject orderItem = Instantiate(orderPrefabs[itemCode], Vector3.zero, Quaternion.identity) as GameObject;
        orderItem.transform.SetParent(orderSets[parentNumber].transform);
        orderItem.transform.localPosition = spawnLocation;
        orderItem.transform.localScale = new Vector3(1, 1, 1);
        objectsToDelete.Add(orderItem);

        updateOrderValues(itemCode);
        playersAudioSources[parentNumber].PlayOneShot(itemsPurchaseSounds[itemCode]);
    }

    // Updates the order values in the order summary
    void updateOrderSummary(int itemCode) {
        int valueToShow = ++requestNumItemsOrdered[itemCode];
        playersAudioSources[PlayerWhoMakesOrders - 1].PlayOneShot(itemsPurchaseSounds[itemCode]);
        UpdateOrderStringToValue(itemCode, valueToShow);
    }

    // They must be in order, so Fries(0), Burger(1), Nuggets(2), and Milkshake(3)
    private void updateOrderValues(int itemCode) {
        ++currentRequestedItems[itemCode];
    }

    // resets every order to zero
    private void resetOrderValues() {
        for (int i = 0; i < 4; ++i) {
            currentRequestedItems[i] = 0;
        }
    }

    // True to enable, false to disable
    private void PressStartDisplay(bool textEnabled) {
        if (textEnabled) {
            startText.SetActive(true);
        }
        else {
            startText.SetActive(false);
        }
    }

    private void SetPopUpText(string textToDisplay) {
        popupText.text = textToDisplay;
    }

    // Coroutine to display prompt
    private IEnumerator EnablePopUpWithTimerInitial(string textToDisplay, float timerAmount, float initialDelay) {
        waitingForPopUp = true;
        TimerIsActive = false;
        yield return new WaitForSeconds(initialDelay);
        TimerText.text = "Time Remaining: ";
        Popup.SetActive(true);
        SetPopUpText(textToDisplay);
        yield return new WaitForSeconds(waitingTime);
        Popup.SetActive(false);
        Timer.S.StartCountdownTimer(timerAmount);
        TimerIsActive = true;
        waitingForPopUp = false;
    }

    // Coroutine to display prompt with initial delay (Also deletes the objectsList)
    private IEnumerator EnablePopUpWithTimer(string textToDisplay, float timerAmount, float initialDelay) {
        waitingForPopUp = true;
        TimerIsActive = false;
        yield return new WaitForSeconds(initialDelay);
        deleteObjectsList();
        TimerText.text = "Time Remaining: ";
        Popup.SetActive(true);
        SetPopUpText(textToDisplay);
        yield return new WaitForSeconds(waitingTime);
        Popup.SetActive(false);
        Timer.S.StartCountdownTimer(timerAmount);
        TimerIsActive = true;
        waitingForPopUp = false;
    }

    // Used two display two promts one after another and then start a countdown
    private IEnumerator Enable2PopUpsWithTimer(string textToDisplay1, string textToDisplay2, float timerAmount) {
        waitingForPopUp = true;
        TimerIsActive = false;
        TimerText.text = "Time Remaining: ";
        Popup.SetActive(true);
        SetPopUpText(textToDisplay1);
        yield return new WaitForSeconds(waitingTime);
        SetPopUpText(textToDisplay2);
        yield return new WaitForSeconds(waitingTime);
        Timer.S.StartCountdownTimer(timerAmount);
        TimerIsActive = true;
        Popup.SetActive(false);
        waitingForPopUp = false;
    }

    private void EndGame(string message) {
        waitingForPopUp = true;
        TimerIsActive = false;
        Popup.SetActive(true);
        SetPopUpText(message);
        setScores();
        StartCoroutine(EndGameWaitRoutine(waitingTime));
    }

    private IEnumerator EndGameWaitRoutine(float waitAmount) {
        yield return new WaitForSeconds(waitAmount);
        ScoreKeeper.DisplayResultsScreen();
    }

    private void setScores() {
        string winMessage = "";

        if (orderPlayersWon) {
            winMessage = "Players ";
            List<int> playerNums = new List<int>();
            for (int i = 1; i < 5; ++i) {
                if (i != ActualPlayerWhoMakesOrders) {
                    playerNums.Add(i);
                    ScoreKeeper.roundScores[i] = 5;
                }
                else {
                    ScoreKeeper.roundScores[i] = 0;
                }
            }
            for (int i = 0; i < playerNums.Count; ++i) {
                if (i == playerNums.Count - 1) {
                    winMessage += "and " + i + " Won!";
                }
                else {
                    winMessage += playerNums[i] + ", ";
                }
            }
        }
        else if (commitPlayerWon) {
            winMessage = "Player " + ActualPlayerWhoMakesOrders + " Won!";
            for (int i = 1; i < 5; ++i) {
                if (i == ActualPlayerWhoMakesOrders) {
                    ScoreKeeper.roundScores[i] = 15;
                }
                else {
                    ScoreKeeper.roundScores[i] = 0;
                }
            }
        }
        print(winMessage);
        ScoreKeeper.winMessage = winMessage;
    }

    private void deleteObjectsList() {
        foreach (GameObject GO in objectsToDelete) {
            Destroy(GO);
        }
        objectsToDelete.Clear();
    }

}
