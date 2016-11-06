using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonImageDisplay : MonoBehaviour {

    public int num;
    public Image RiverRide;
    public Image WhirlpoolCanyon;
    public Image SeasideScramble;
    public List<GameObject> LevelDescriptions = new List<GameObject>();


    public void DisplayImage() {
        if (num == 1) {
            RiverRide.enabled = true;
            WhirlpoolCanyon.enabled = false;
            SeasideScramble.enabled = false;
            EnableLevelDescription(0);
        }
        else if(num == 2) {
            RiverRide.enabled = false;
            WhirlpoolCanyon.enabled = false;
            SeasideScramble.enabled = true;
            EnableLevelDescription(1);
        }
        else if(num == 3) {
            RiverRide.enabled = false;
            WhirlpoolCanyon.enabled = true;
            SeasideScramble.enabled = false;
            EnableLevelDescription(2);
        }
        else {
            RiverRide.enabled = false;
            WhirlpoolCanyon.enabled = false;
            SeasideScramble.enabled = false;
            EnableLevelDescription(-1);     // should set every levelDescription to inactive
        }
    }

    public void EnableLevelDescription(int levelNum) {
        for (int i = 0; i < LevelDescriptions.Count; ++i) {
            if (i == levelNum) {
                LevelDescriptions[i].SetActive(true);
            }
            else {
                LevelDescriptions[i].SetActive(false);
            }
        }
    }
}
