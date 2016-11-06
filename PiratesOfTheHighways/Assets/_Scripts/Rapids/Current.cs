using UnityEngine;
using System.Collections;

public class Current : MonoBehaviour {
	public Vector3 flowVector = new Vector3(0f, 0f, 0f);

    public float maximumCurrent;            // The maximum allowed current as a vector3 magnitude

    [Header("Set this to true if this current is able to change")]
    public bool     isChangingCurrent = false;
    public float    minimumCurrent;            // Only set that if isChangingCurrent is true

    private bool    coroutineIsActive = false;
    private Vector3 originalFlowVector;


    public void updateflowVector(Vector3 vec) {
        vec.Normalize();
        flowVector = vec * maximumCurrent;
        originalFlowVector = flowVector;
    }

    void Update() {
        if (isChangingCurrent) {
            if (!coroutineIsActive) {
                StartCoroutine(changeCurrent());
            }
        }
    }

    private IEnumerator changeCurrent() {
        coroutineIsActive = true;

        float divisions = 50f;
        //float speedChange = (maximumCurrent - minimumCurrent) / divisions;
        float speedChange = (minimumCurrent / maximumCurrent) / divisions;
        float currentMult = 1f;

        // Decreasing the speed
        for (int i = 0; i < divisions; ++i) {
            flowVector = (currentMult - i * speedChange) * originalFlowVector;
            yield return new WaitForSeconds(0.05f);
        }

        // Stay at lowest speed for some time
        yield return new WaitForSeconds(1.5f);

        // Increment the speed
        for (int i = (int)divisions; i > 0; --i) {
            flowVector = (currentMult - i * speedChange) * originalFlowVector;
            yield return new WaitForSeconds(0.05f);
        }

        // Stay at maximum speed for some time
        flowVector = originalFlowVector;
        yield return new WaitForSeconds(1.5f);

        coroutineIsActive = false;
    }

}
