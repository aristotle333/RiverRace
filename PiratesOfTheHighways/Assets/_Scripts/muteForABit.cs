using UnityEngine;
using System.Collections;

public class muteForABit : MonoBehaviour {
	// Use this for initialization
	public float muteTime;
	void Start () {
		this.gameObject.GetComponent<AudioSource> ().mute = true;
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > muteTime) {
			this.gameObject.GetComponent<AudioSource> ().mute = false;
		}
	}
}
