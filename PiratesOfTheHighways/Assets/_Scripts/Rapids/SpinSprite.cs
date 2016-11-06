using UnityEngine;
using System.Collections;

public class SpinSprite : MonoBehaviour {
	public SpriteRenderer sprend;
	public int spinEveryFrames;
	public int degreesToSpin;
	int counter;
	// Use this for initialization
	void Start () {
		counter = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		counter++;
		if (counter % spinEveryFrames == 0) {
			sprend.transform.Rotate (0, 0, -degreesToSpin);
		}
	}
}
