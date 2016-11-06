using UnityEngine;
using System.Collections;

public enum Treasure { NONE, BOOT, COIN, BAG, CHEST };

public class Tile : MonoBehaviour {

    public bool used;
    public Treasure type;

	// Use this for initialization
	void Start () {

        float val = Random.value;

        if (val >= 0.98f)
        {
            type = Treasure.CHEST;
        }
        else if (val >= 0.83f)
        {
            type = Treasure.BAG;
        }
        else if (val >= 0.6f)
        {
            type = Treasure.COIN;
        }
        else if (val >= 0.3f)
        {
            type = Treasure.BOOT;
        }
        else
        {
            type = Treasure.NONE;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<DiggingPlayer>().digging)
        {
            return;
        }

        coll.gameObject.GetComponent<DiggingPlayer>().digging = true;

        GameObject subSprite = this.gameObject.transform.FindChild("Sprite").gameObject;
        subSprite.GetComponent<SpriteRenderer>().sprite = DigUIManager.DigUIMan.TGSprites[1];

        int val = 0;

        if (type == Treasure.NONE || type == Treasure.BOOT)
        {
            type = Treasure.NONE;
            return;
        }
        else if (type == Treasure.COIN)
        {
            val = 1;
        }
        else if (type == Treasure.BAG)
        {
            val = 3;
        }
        else if (type == Treasure.CHEST)
        {
            val = 10;
        }

        if (coll.tag == "Player")
        {
            print("player triggered a  tile");

            type = Treasure.NONE;

            int player = coll.gameObject.GetComponent<DiggingPlayer>().playerNum;

            DigUIManager.DigUIMan.playerScores[player-1] += val;
        }
    }
}
