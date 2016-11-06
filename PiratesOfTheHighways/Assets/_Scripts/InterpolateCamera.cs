using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CameraTarget {
    public GameObject       targetObject;

    // The intermediate list of GO that that the camera will interpolate to.
    // The first object must the starting location, in between intermediate locations
    // and last object in the list must be targetObject
    public List<GameObject> interpolGOList;     
}

public class InterpolateCamera : MonoBehaviour {

    public static InterpolateCamera S;

    [Header("Current Interpolation value---MUST BE SET TO ZERO---")]
    public float u = 0f;                            // A value between 0 to 1 used in interpolation
    [Header("Speed of the preview camera")]
    public float speed = 0.2f;                      // Speed of camera in m/s
    [Header("The position where the preview camera starts")]
    public GameObject originPos;                    // The location where the camera starts
    [Header("When enabled preview will loop its path")]
    public bool loopPreview = true;

    [Header("List of the camera Targets")]
    public List<CameraTarget> cameraTargets;

    [Header("Reference to the UI prompt")]
    public GameObject UIPromptRef;

    [Header("References of Object to be hidden when preview camera is active")]
    public List<GameObject> objectsToHide = new List<GameObject>();

    //----Private Variables----//
    private float   distanceToTarget = 0f;         // The distance to the next target location
    private float   timeToReachTarget = 0f;        // Time required for the camera to reach to desired target from its starting target
    private float   uIncrement = 0f;               // The increment of u per fixed update
    private int     currTarget = 0;                // The index of the current target for the camera

    void Awake() {
        S = this;
        HideObjects();
    }

    void Start() {
        this.transform.position = originPos.transform.position;
        distanceToTarget = Vector3.Distance(originPos.transform.position, cameraTargets[0].targetObject.transform.position);
        timeToReachTarget = distanceToTarget / speed;
        uIncrement = 1 / timeToReachTarget;
    }

    void Update() {
        if (Input.GetButtonDown("AnyoneStart") == true) {
            StartCoroutine(UnHideObjects());
        }

        if (u >= 1f) {
            UpdateTarget();
        }
        else {
            u += uIncrement;
            Vector3 p = Interp(u, ref cameraTargets[currTarget].interpolGOList);
            this.transform.position = p;
        }
    }

    //void FixedUpdate() {
    //    if (u >= 1f) {
    //        UpdateTarget();
    //    }
    //    else {
    //        u += uIncrement;
    //        Vector3 p = Interp(u, ref cameraTargets[currTarget].interpolGOList);
    //        this.transform.position = p;
    //    }
    //}

    private void UpdateTarget() {
        // Check if we reached the last target
        if (currTarget >= cameraTargets.Count - 1) {
            if (loopPreview) {
                distanceToTarget = Vector3.Distance(cameraTargets[currTarget].targetObject.transform.position, cameraTargets[0].targetObject.transform.position);
                timeToReachTarget = distanceToTarget / speed;
                uIncrement = 1 / timeToReachTarget;
                u = 0f;
                currTarget = 0;
                print("Performed preview loop");
                return;
            }
            else {
                // Set an invalid large value for u
                u = 10f;
                return;
            }
        }

        distanceToTarget = Vector3.Distance(cameraTargets[currTarget].targetObject.transform.position, cameraTargets[++currTarget].targetObject.transform.position);
        timeToReachTarget = distanceToTarget / speed;
        uIncrement = 1 / timeToReachTarget;
        // Reset u to zero
        u = 0f;
        print("Changing target");
    }

    // Recursive Interpolation function
    private Vector3 Interp(float u, ref List<GameObject> pS, int i0 = 0, int i1 = -1) {
        if (i1 == -1) {
            i1 = pS.Count - 1;
        }
        if (i0 == i1) return pS[i0].transform.position;
        Vector3 pL = Interp(u, ref pS, i0, i1 - 1);
        Vector3 pR = Interp(u, ref pS, i0 + 1, i1);
        Vector3 pLR = (1 - u) * pL + u * pR;
        return pLR;
    }


    public void HideObjects() {
        for (int i = 0; i < objectsToHide.Count; ++i) {
            objectsToHide[i].SetActive(false);
        }
    }

    // UnhidesObjects and sets the preview camera to inactive
    public IEnumerator UnHideObjects() {
        print("run unhide");
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        UIPromptRef.SetActive(false);
        for (int i = 0; i < objectsToHide.Count; ++i) {
            objectsToHide[i].SetActive(true);
        }
        this.gameObject.SetActive(false);
    }

}