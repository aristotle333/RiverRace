using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

    public static ScreenFade S;

    public Image FadeObject;               // This is the gameObject (usually a black image) that is used to fade in and out to make screen transitions
    public float timeToFade;               // How many seconds it takes to fade the screen


    private float maxAlpha = 255f;
    private float minAlpha = 0f;
    private float waitTime;
    private float rateOfChangeFloat;

    void Awake() {
        S = this;
        rateOfChangeFloat = maxAlpha / 100f;
        waitTime = timeToFade / 100f;
    }

    // Increases the alpha channel of the screen to 255 (Fully transparent)
    public IEnumerator DarkenScreen() {
        while (FadeObject.color.a <255) {
            Color temp = FadeObject.color;
            temp.a += rateOfChangeFloat;
            FadeObject.color += temp;
            yield return new WaitForSeconds(waitTime);
        }
        print("screenIsDark");
    }


    // Decreases the alpha channel of the screen to 0 (Fully transparent)
    public IEnumerator BrightenScreen() {
        while (FadeObject.color.a > 0) {
            Color32 temp = FadeObject.color;
            temp.a -= (byte)rateOfChangeFloat;
            print(temp.a);
            FadeObject.color += temp;
            StartCoroutine(BrightenScreen());
            yield return new WaitForSeconds(waitTime);
        }
        StopCoroutine(BrightenScreen());
        print("screenIsWhite");
    }


}
