using UnityEngine;
using System.Collections;

public class RaftCamera : MonoBehaviour {
	public float maxYDist, maxXDist; //max distance camera will allow the raft to go above/below it in meters
	public GameObject Raft;
    public float dampTime = 0.15f;

    private Camera currentCamera;
    private Transform RaftTransform;
    private Rigidbody RaftRigidbody;
    private Vector3 zeroVel = Vector3.zero;
    private float yVelocity;

    void Awake() {
        RaftRigidbody = Raft.GetComponent<Rigidbody>();
        RaftTransform = Raft.GetComponent<Transform>();
        currentCamera = this.GetComponent<Camera>();
    }

	void FixedUpdate () {
        yVelocity = Mathf.Abs(RaftRigidbody.velocity.y);

        if (RaftTransform) {
            Vector3 point = currentCamera.WorldToViewportPoint(RaftTransform.position);
            float posChange = yVelocity / 55f;
            Vector3 delta = RaftTransform.position - currentCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.3f - posChange, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref zeroVel, dampTime);
        }

        //float yOffset = Raft.transform.position.y - this.transform.position.y;
        //float xOffset = Raft.transform.position.x - this.transform.position.x;

        //if (Mathf.Abs(yOffset) > maxYDist) {
        //    Vector3 pos = this.transform.position;
        //    pos.y = Raft.transform.position.y - maxYDist;
        //    transform.position = .08f * pos + .92f * transform.position;
        //}
        //if (Mathf.Abs(xOffset) > maxYDist) {
        //    Vector3 pos = this.transform.position;
        //    pos.x = Raft.transform.position.x - maxXDist;
        //    transform.position = .08f * pos + .92f * transform.position;
        //}


    }

    /*
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    // Update is called once per frame
    void Update() {
        if (target) {
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }
    */
}
