using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RaftingTutorial : MonoBehaviour {

    public static RaftingTutorial S;

	public Canvas               tutorial;
	public Button               button;
    public GameObject           countDownReference;
    public GameObject           parent;
	public AudioClip beepSound;
	AudioSource beeper;

    void Awake() {
        S = this;

	}
	
	void Update() {
		if (Time.timeSinceLevelLoad > 1f) {
			button.interactable = true;
			button.Select ();
		}
	}

	public void endTutorial() {
        StartCoroutine(DisplayCountdown());
	}

    public IEnumerator DisplayCountdown() {
		beeper = this.gameObject.AddComponent<AudioSource> ();
		beeper.clip = beepSound;
        parent.SetActive(false);

        Text textRef = countDownReference.GetComponent<Text>();
        countDownReference.SetActive(true);
		beeper.Play ();
        // Increase 3
        for (int i = 0; i < 70; ++i) {
            textRef.fontSize += 2;
            yield return new WaitForSeconds(0.015f);
        }
		beeper.Play ();
        // change to 2 (small size) and increase size
        textRef.fontSize = 50;
        textRef.text = "2";
        for (int i = 0; i < 70; ++i) {
            textRef.fontSize += 2;
            yield return new WaitForSeconds(0.015f);
        }
		beeper.Play ();
        // change to 1 (small size) and increase size
        textRef.fontSize = 50;
        textRef.text = "1";
        for (int i = 0; i < 70; ++i) {
            textRef.fontSize += 2;
            yield return new WaitForSeconds(0.015f);
        }
		beeper.pitch = 2f;
		beeper.volume = 2f;
		beeper.Play ();
        tutorial.enabled = false;
    }
}
