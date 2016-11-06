using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public enum PowerUpType{None, Cannon, Grapple, Sail};
	public PowerUpType type;
	public float currentScaling, respawnTime;
	public float deadTime;

	void OnTriggerStay(Collider coll) {
		GameObject other = coll.gameObject;
		if (other.tag == "Current") {
			getPushedByCurrent(other.GetComponent<Current>().flowVector);
		}
	}

	void Update(){
		if (deadTime > 0) {
			this.gameObject.GetComponent<SphereCollider> ().enabled = false;
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			deadTime -= Time.deltaTime;
		} else {
			this.gameObject.GetComponent<SphereCollider> ().enabled = true;
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	void getPushedByCurrent (Vector3 waterFlow)
	{
		Vector3 velDiff = waterFlow - this.GetComponent<Rigidbody>().velocity;
		this.GetComponent<Rigidbody>().AddForce (velDiff * currentScaling);
	}

	public void die()
	{
		deadTime = respawnTime;
	}
}
