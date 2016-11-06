using UnityEngine;
using System.Collections;

public class GasPlayer : MonoBehaviour {
    public int PlayerNum;           //1 for Player1, 2 for Player2, ect.
    public float speed;
    public bool gotGas = false;
    public int numPoints = 0;        //Keeps track of players score
    float RespawnTime = 40f;
    float RespawnLeft = 0;
    public Vector3 RespawnPos;

    public AudioClip pickupSound;
    public AudioClip scorePoint;

    public Bounds bounds;

    GameObject gas_can;

    //Tutorial stuff

    bool DriveBy = false;
    public GameObject DriveByCar;
    public float waitTime = 180f;
    public float waitLeft;


    // Use this for initialization
    void Start () {

        gas_can = gameObject.transform.GetChild(0).gameObject;

        //Sets the location for a player to respawn at
        RespawnPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (!GasGrab.tutorial)
        {
            if (RespawnLeft == 0)
            {
                //Pull in information from the Input class
                float xAxis = Input.GetAxis("Player" + (PlayerNum) + "_axisX");
                float yAxis = Input.GetAxis("Player" + (PlayerNum) + "_axisY");

                //Change transform.position based on the axes
                Vector3 pos = transform.position;
                pos.x += xAxis * speed * Time.deltaTime;
                pos.y += yAxis * speed * Time.deltaTime;
                transform.position = pos;

                bounds.center = transform.position;

                //Keep the player contrained to the screen bounds
                Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
                if (off != Vector3.zero)
                {
                    pos -= off;
                    transform.position = pos;
                }

            }
            else
            {
                RespawnLeft--;
            }
        }
        else
        {
            if (gotGas == false && DriveBy == false) {
                TutorialMoves1();
            }
            else if (gotGas == true && transform.position.y > 2.25)
            {
                TutorialMoves2();
            }
            else if (gotGas == true && transform.position.y <= 2.25)
            {
                TutorialMoves3();
                if (DriveBy == false && PlayerNum == 1)
                {
                    GameObject go = Instantiate(DriveByCar) as GameObject;
                    Vector3 pos = Vector3.zero;
                    pos.x = 9.75f;
                    pos.y = 2.15f;
                    go.transform.position = pos;
                }
                DriveBy = true;
                waitLeft = waitTime;
            }
            else
            {
                if (waitLeft != 0)
                {
                    waitLeft--;
                }
                else
                {
                    GasGrab.tutorial = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Car")
        {
            Death();
        }

        else if (coll.gameObject.tag == "GasZone")
        {
            if (!gotGas)
            {
                gotGas = true;
                gas_can.SetActive(true);
                this.GetComponent<AudioSource>().PlayOneShot(pickupSound);
            }
        }
        else if (coll.gameObject.tag == "ReturnZone")
        {
            if (gotGas)
            {
                this.GetComponent<AudioSource>().PlayOneShot(scorePoint);
                if (PlayerNum == 1)
                {
                    GasGrab.PlayerScores[0]++;
                }
                else if (PlayerNum == 2)
                {
                    GasGrab.PlayerScores[1]++;
                }
                else if (PlayerNum == 3)
                {
                    GasGrab.PlayerScores[2]++;
                }
                else
                {
                    GasGrab.PlayerScores[3]++;
                }
                numPoints++;
                gas_can.SetActive(false);
                gotGas = false;
            }
        }
    }

    void Death()
    {
        this.transform.position = RespawnPos;
        gotGas = false;
        gas_can.SetActive(false);
        RespawnLeft = RespawnTime;
    }

    void TutorialMoves1()
    {

        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += 0 * speed * Time.deltaTime;
        pos.y += .8f * speed * Time.deltaTime;
        transform.position = pos;

    }

    void TutorialMoves2()
    {
        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += 0 * speed * Time.deltaTime;
        pos.y += -.5f * speed * Time.deltaTime;
        transform.position = pos;
    }

    void TutorialMoves3()
    {
        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += 0 * speed * Time.deltaTime;
        pos.y += 0 * speed * Time.deltaTime;
        transform.position = pos;
    }
}
