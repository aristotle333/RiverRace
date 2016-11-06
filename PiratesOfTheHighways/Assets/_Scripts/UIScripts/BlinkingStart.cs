using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkingStart : MonoBehaviour {
    public Button blinkingText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Mathf.FloorToInt(Time.timeSinceLevelLoad * 1.4f) % 2 == 1)
        {
			blinkingText.image.color = Color.white;
        }
        else
        {
			blinkingText.image.color = Color.gray;
        }
	
	}
}
