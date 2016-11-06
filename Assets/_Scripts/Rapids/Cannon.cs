using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	public GameObject enemyRaft;
	public UnityEngine.Object cannonballPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void fire() {
		print ("BOOM!");
		GameObject firedObject = (GameObject)Instantiate (cannonballPrefab, this.transform.position, 
			Quaternion.Euler (this.transform.eulerAngles + new Vector3 (0, 0, 180f)));
		firedObject.GetComponent<Cannonball> ().enemyRaft = enemyRaft;
	}
}
