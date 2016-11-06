using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {

    public static AudioHandler S;

    static bool AudioBegin = false;
    void Awake()
    {
        S = this;
        if (!AudioBegin)
        {
            this.GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }

    public void DisableMe()
    {
        if (AudioHandler.S == null) {
            AudioBegin = false;
            return;
        }
        AudioBegin = false;
        Destroy(this.gameObject);
    }
}
