using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageScale : MonoBehaviour {
    void Start() {
        ResizeImage();
    }

    // This function will scale the image (most likely a background image in a scene) to the size of our camera taking into account any aspect ratio.
    // This function also assumes that there is a MainCamera in the scene
    void ResizeImage() {
        Image im = this.GetComponent<Image>();
        if (im.sprite == null) {
            print("No Image Found");
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);

        float width = im.sprite.bounds.size.x;
        float height = im.sprite.bounds.size.y;


        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;
        //transform.localScale.y = worldScreenHeight / height;
    }
}
