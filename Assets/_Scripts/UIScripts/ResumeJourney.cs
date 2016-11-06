using UnityEngine;
using System.Collections;

public class ResumeJourney : MonoBehaviour {
	public GameObject camera, van;
	// Use this for initialization
	void Start () {
		camera.GetComponent<FollowPath> ().lastCityVisited = ScoreKeeper.lastNode;
		van.GetComponent<FollowPath> ().lastCityVisited = ScoreKeeper.lastNode;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
