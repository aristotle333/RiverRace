using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowPath : MonoBehaviour {
	public Text victoryText;
    public GameObject[] spotsToVisit;
    public int lastCityVisited;
    public int legFrameCount;
    int currCount;
    float percentThere;
    public float height;

	// Use this for initialization
	void Start () {
        this.transform.position = spotsToVisit[0].transform.position;
        //lastCityVisited = 0;
        currCount = 0;
        percentThere = .001f;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        
        if (lastCityVisited >= spotsToVisit.Length - 1)
        {
			victoryText.GetComponent<Text>().enabled = true;
            return;
        }
        if (currCount == legFrameCount)
        {
            lastCityVisited++;
			ScoreKeeper.lastNode = lastCityVisited;
            currCount = 0;
        }
        else
        {
            Vector3 pos = Vector3.zero;
            
            percentThere = (float)currCount / legFrameCount;
            if (percentThere > 1f)
                percentThere = 1f;
            pos = spotsToVisit[lastCityVisited].transform.position * (1 - percentThere) + spotsToVisit[lastCityVisited + 1].transform.position * (percentThere);
            pos.z = height;
            if(percentThere > 1f)
                print(percentThere);
            this.transform.position = pos;
            currCount++;

        }
    }
}
