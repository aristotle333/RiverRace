using UnityEngine;
using System.Collections;

public class Cannonball : MonoBehaviour {
	public float speed;
	public GameObject enemyRaft;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += speed * Time.deltaTime * this.transform.up;
	}

	void OnCollisionEnter(Collision collision) {
		
		GameObject other = collision.collider.gameObject;
		print (other.name);
		if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
			Destroy(this.gameObject);
	}
}
