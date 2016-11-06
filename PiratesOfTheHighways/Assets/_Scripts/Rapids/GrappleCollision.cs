using UnityEngine;
using System.Collections;

public class GrappleCollision : MonoBehaviour {

    public GameObject grappleParentObject;

    private bool collided = false;          // This is used to prevent double colission

    void OnTriggerEnter(Collider coll) {
		if (grappleParentObject.GetComponent<Grapple> ().ranInto != null) //prevents hook from getting brushed off on a rock
			return;
		if (!grappleParentObject.GetComponent<Grapple> ().fired) //prevents hooking on to things without firing
			return;
        GameObject other = coll.gameObject;
		if (other.tag == "Danger" || other.tag == "PowerUp" || other.tag == "Finish")
			return;
		GameObject hook = grappleParentObject.GetComponent<Grapple> ().hook;
        print(other.name);
		if (other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject == grappleParentObject.GetComponent<Grapple>().enemyRaft && !collided) {
            collided = true;
            //grappleParentObject.SetActive(false);
			print(other.gameObject.name);
			grappleParentObject.GetComponent<AudioSource> ().Play (); //play grapple collide sound

			grappleParentObject.GetComponent<Grapple> ().ranInto = other.gameObject;
			grappleParentObject.GetComponent<Grapple>().connected = true;
			grappleParentObject.GetComponent<Grapple> ().targetOffset = other.transform.position - hook.transform.position + hook.transform.up * .1f;
			grappleParentObject.GetComponent<Grapple> ().hookAngleOnCollision = hook.transform.rotation;
			float time = Time.time;
			grappleParentObject.GetComponent<Grapple> ().timeOfCollision = time;
			//grappleParentObject.transform.SetParent(other.gameObject.transform);
        }
    }

}
