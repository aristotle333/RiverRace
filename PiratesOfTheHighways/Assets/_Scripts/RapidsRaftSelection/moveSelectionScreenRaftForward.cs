using UnityEngine;
using System.Collections;

public class moveSelectionScreenRaftForward : MonoBehaviour {
    public bool active = false;

    private Vector3 vec = new Vector3(0f, 11f, 0f);

    void Update() {
        if (active) {
            this.GetComponent<Rigidbody>().velocity = vec;
        }
    }
}