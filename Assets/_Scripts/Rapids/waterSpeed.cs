using UnityEngine;
using System.Collections;

public class waterSpeed : MonoBehaviour {

    //this.GetComponent<Renderer>().material.SetVector("WaveSpeed", new Vector4(-40f, -30f, 0f, 0f));
    [Header("Wave Speed properties")]
    public GameObject waterCurrentColliderRef;          // Reference to the accociated water current gameobject of the specific wave
    public float waveSpeed;                             // The speed of the wave (this is the magnitude of the wave speed vector)


    //public bool waveSpeedIsChanging = false;            // Set this to true if we want the wave's speed to be changing

    //[Header("The minimum wave speed during an oscilation --Modify this only if waveSpeedIsChanging is true--")]
    //public float minimumSpeed;

    /* 
    The direction of the wave is handled by the rotation of the wave
    The speed of map2 = 2 * map1_speed in order to give a nice movement effect
    */
    private Vector4 waterSpeedMap = new Vector4(0f, 0f, 0f, 0f);    // (x-direction of map1, y-direction of map1, x-direction of map2, y-direction of map2)

    private const float angleOffset = 270f;                         // Angle offset used to find the speed of the wave
    private bool coroutineIsActive = false;                         // Check if the coroutine is running, this prevents many instances of coroutines at the same tme   

    void Start() {
        setWaveSpeed();

        // Update the current vector associated with this wave
        Vector3 vectorToCurrent = new Vector3(-waterSpeedMap.x, -waterSpeedMap.y, 0f);
        waterCurrentColliderRef.GetComponent<Current>().updateflowVector(vectorToCurrent);
    }

    //void Update() {
    //    if (waveSpeedIsChanging) {
    //        if (!coroutineIsActive) {
    //            StartCoroutine(waveSpeedChangeCoroutine());
    //        }
    //    }
    //}

    private void setWaveSpeed() {
        float xRot = this.transform.rotation.eulerAngles.x;
        float yRot = this.transform.rotation.eulerAngles.y;

        float angle = 0f;                 // Then angle of the orientation of the wave relative to the +y axis
        if (yRot < 90f) {
            angle = xRot - angleOffset;
        }
        else {
            angle = -(xRot - angleOffset);
        }
        angle = angle * Mathf.Deg2Rad;      // Convert angle to radians

        float map1Y = -1 * waveSpeed * Mathf.Cos(angle);
        float map1X = -1 * waveSpeed * Mathf.Sin(angle);

        float map2Y = 2 * map1Y;
        float map2X = 2 * map1X;

        waterSpeedMap = new Vector4(map1X, map1Y, map2X, map2Y);
        this.GetComponent<Renderer>().material.SetVector("WaveSpeed", waterSpeedMap);
    }


    // THIS IS NOT WORKING PROPERLY


    //// Always start at maximum speed and return back to a maximum speed state to complete a full cycle
    //private IEnumerator waveSpeedChangeCoroutine() {
    //    print(waterSpeedMap);
    //    coroutineIsActive = true;

    //    float multiple = waveSpeed / minimumSpeed;
    //    Vector4 newSpeed = multiple * waterSpeedMap;
    //    this.GetComponent<Renderer>().material.SetVector("WaveSpeed", newSpeed);
    //    yield return new WaitForSeconds(2f);

    //    this.GetComponent<Renderer>().material.SetVector("WaveSpeed", waterSpeedMap);
    //    yield return new WaitForSeconds(2f);
    //    coroutineIsActive = false;

    //    //float divisions = 4f;
    //    //float speedChange = (minimumSpeed / waveSpeed) / divisions;
    //    //float currentMult = 1f;

    //    //// Decreasing the speed
    //    //for (int i = 0; i <= divisions; ++i) {
    //    //    Vector4 newSpeed = (currentMult - i * speedChange) * waterSpeedMap;
    //    //    print(currentMult - i * speedChange);
    //    //    this.GetComponent<Renderer>().material.SetVector("WaveSpeed", newSpeed);
    //    //    yield return new WaitForSeconds(1f);
    //    //}

    //    //// Stay at lowest speed for some time
    //    //yield return new WaitForSeconds(1f);

    //    //// Increment the speed
    //    //for (int i = (int)divisions; i >= 0; --i) {
    //    //    Vector4 newSpeed = (currentMult - i * speedChange) * waterSpeedMap;
    //    //    print(currentMult - i * speedChange);
    //    //    this.GetComponent<Renderer>().material.SetVector("WaveSpeed", newSpeed);
    //    //    yield return new WaitForSeconds(1f);
    //    //}

    //    //// Stay at maximum speed for some time
    //    //this.GetComponent<Renderer>().material.SetVector("WaveSpeed", waterSpeedMap);
    //    //yield return new WaitForSeconds(1f);

    //    //coroutineIsActive = false;
    //}

}
