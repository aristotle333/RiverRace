using UnityEngine;
using System.Collections;

public class LoadLevelsFromMap : MonoBehaviour {
	int prevNode;
	// Use this for initialization
	void Start () {
		prevNode = -1;	
	}
	
	// Update is called once per frame
	void Update () {
		int node = this.gameObject.GetComponent<FollowPath> ().lastCityVisited;
		if (node == 2 && prevNode == 1){
			Application.LoadLevel (1); //Chug
		}
		if(node == 5 && prevNode == 4){
			Application.LoadLevel(7); //Tutorial for Drivethru
		}

		if (node == 7 && prevNode == 6) {
			Application.LoadLevel (5); //gasgrab
		}

		if (node == 11 && prevNode == 10) {
			Application.LoadLevel (6); //rafting
		}
			
		prevNode = node;
	
	}
}
