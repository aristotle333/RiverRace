using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public static bool FromLevel;

    public AudioSource menuMusic;

    void Awake()
    {
        print("fromLevel is: " + FromLevel);
        if (FromLevel == true)
        {
            print("Playing music");
            menuMusic.enabled = true;
        }
    }

	public void loadLevel1(){
        FromLevel = false;
        AudioHandler.S.DisableMe();
		SceneManager.LoadScene ("BetaRiverRide");
	}

	public void loadLevel2() {
        FromLevel = false;
        AudioHandler.S.DisableMe();
        SceneManager.LoadScene("BetaSeasideScramble");
	}

	public void loadLevel3() {
        FromLevel = false;
        AudioHandler.S.DisableMe();
        SceneManager.LoadScene("ShowWhirlpoolCanyon");
	}

	public void backToMainMenu() {
        if (AudioHandler.S == null) {
            print("Audio Handler is null");
            LevelSelect.FromLevel = false;
            SceneManager.LoadScene("startScreen");
            return;
        }
        LevelSelect.FromLevel = false;
        AudioHandler.S.DisableMe();
        SceneManager.LoadScene ("startScreen");
	}
}
