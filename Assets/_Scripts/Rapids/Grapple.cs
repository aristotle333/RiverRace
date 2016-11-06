using UnityEngine;
using System.Collections;

public class Grapple : MonoBehaviour {

	public float hookVelocity;
	public float maxRange, minRange;
	public float hookDuration;
	public float forceAmount;
	public GameObject hook;
	public GameObject enemyRaft; //set by raft.cs, don't worry about this
	public GameObject thisRaft; //also set by raft.cs
	public LineRenderer lineRend;
	Rigidbody thisRigid;
	public Vector3 forceApplicationOffset;
	float currentHookSpeed;
	public bool fired;
	public bool connected;
	public GameObject ranInto;
	public Quaternion hookAngleOnCollision;
	public Vector3 targetOffset;
	public float timeOfCollision;
	public Vector3 hookLocalPosAtStart;
	public Quaternion hookLocalRotAtStart;

	void Start () {
		currentHookSpeed = 0f;
		hookLocalPosAtStart = hook.transform.localPosition;
		hookLocalRotAtStart = hook.transform.localRotation;
		lineRend.useWorldSpace = true;
		lineRend.enabled = false;
		lineRend.SetWidth (.1f, .1f);
		fired = connected = false;
	}
	
	void FixedUpdate () {
		thisRigid = thisRaft.GetComponent<Rigidbody> (); //this is in FixedUpdate() in order to kill a race condition
		if (!fired && !connected) {
			hook.transform.localRotation = hookLocalRotAtStart; //double check that the grapple is at its start position
			hook.transform.localPosition = hookLocalPosAtStart;
			forceApplicationOffset = this.transform.position - thisRaft.transform.position; //this keeps track of which part of Raft's rigidbody we will tug on
		}
		if (fired && !connected) {
			lineRend.enabled = true;
			hook.transform.position -= hook.transform.up * currentHookSpeed * Time.deltaTime; //move hook forward
			Vector3 pos = hook.transform.position;
			if (Vector3.Magnitude (pos - thisRaft.transform.position) > 1f) {
				pos.z = 0f; //put the hook at a z position where it's likely to hit any relevant objects
			} else {
				pos.z = -.3f;
			}
			hook.transform.position = pos;
			Vector3[] positions = { this.transform.position + new Vector3(0,0,-1f), hook.transform.position + new Vector3(0,0,-1f) + hook.transform.up * .3f};
			lineRend.SetPositions (positions);
            if (Vector3.Magnitude(hook.transform.position - this.transform.position) > maxRange) { //hook extended too far
				resetGrapple();
            }
		}
		if (connected) {
			float time = Time.time;
			if (time - timeOfCollision > hookDuration) {
				resetGrapple ();
			}
			hook.transform.rotation = hookAngleOnCollision;
			hook.transform.position = ranInto.transform.position - targetOffset;
			Vector3[] positions = { this.transform.position + new Vector3(0,0,-1f), hook.transform.position + hook.transform.up * -.7f + new Vector3(0,0, -1)};
			lineRend.SetPositions (positions);
			Vector3 forceDirection = Vector3.Normalize(positions [1] - positions [0]);
			thisRigid.AddForceAtPosition(forceDirection * forceAmount, this.transform.position);
			if (ranInto == enemyRaft) {
				enemyRaft.GetComponent<Rigidbody> ().AddForceAtPosition (-forceDirection * forceAmount, hook.transform.position);
			}
			if (Vector3.Magnitude(hook.transform.position - this.transform.position) > maxRange || Vector3.Magnitude(hook.transform.position - this.transform.position) < minRange && time - timeOfCollision > .5f) {
				resetGrapple ();
			}
		}
	}

	public void fire() {
		currentHookSpeed = hookVelocity;
		fired = true;
	}

	public void resetGrapple() {
		fired = false;
		connected = false;
		lineRend.enabled = false;
        ranInto = null;
		hook.transform.localRotation = hookLocalRotAtStart;
		hook.transform.localPosition = hookLocalPosAtStart;
		this.gameObject.SetActive(false);
	}

	//void OnTriggerEnter(Collider coll) {

	//	GameObject other = coll.gameObject;
	//	print (other.name);
	//	if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
	//		Destroy(this.gameObject);
	//}
}
