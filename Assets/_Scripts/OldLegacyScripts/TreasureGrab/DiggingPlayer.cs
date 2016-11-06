using UnityEngine;
using System.Collections;

public class DiggingPlayer : MonoBehaviour {

    public int playerNum;

    public float speed;
    public bool digging;
    public float digDelay;

    //--this still needs literally a shit ton of pirate sprites to work with

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //If player is currently digging, no other actions are allowed
        if (digging)
        {
            digDelay -= Time.deltaTime;

            //--need incremental hole creation
            //--need to move player to center of tile that got activated

            if (digDelay <= 0.3)
            {
                //--this would be when to display the sprite of what you dug up
                
            }

            if (digDelay <= 0)
            {
                digging = false;

                Vector3 rise = new Vector3(0, 0, 0);

                this.GetComponent<SphereCollider>().center = rise;
            }

            return;
        }

        //Pull in information from the Input class
        float xAxis = Input.GetAxis("Player" + playerNum + "_axisX");
        float zAxis = Input.GetAxis("Player" + playerNum + "_axisY");

        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.z += zAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Force the player to stay on the map
        if (transform.position.x < -12f)
        {
            pos.x = -12;
            transform.position = pos;
        }
        else if (transform.position.x > 11)
        {
            pos.x = 11;
            transform.position = pos;
        }

        if (transform.position.z < -8f)
        {
            pos.z = -8;
            transform.position = pos;
        }
        else if (transform.position.z > 7)
        {
            pos.z = 7;
            transform.position = pos;
        }

        if (Input.GetAxisRaw("Player" + playerNum + "_buttonA") > .6f)
        {
            digDelay = 0.5f;

            //--bug with this when youre in between squares
            Vector3 drop = new Vector3(0, -0.5f, 0);

            this.GetComponent<SphereCollider>().center = drop;
        }
    }
}
