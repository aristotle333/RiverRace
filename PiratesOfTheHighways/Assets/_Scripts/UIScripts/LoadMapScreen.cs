using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMapScreen : MonoBehaviour {

	void Update () {
        //if (Input.GetAxis("AnyoneStart") == 1f) {
        //    LoadMap();
        //}
        if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Return)) {
            LoadMap();
        }
	}

    public void LoadMap() {
        StartCoroutine(LoadMapRoutine());
    }

    private IEnumerator LoadMapRoutine() {
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("raftSelectionScreen");
    }
}
