using UnityEngine;
using System.Collections;

public class Pursuer : MonoBehaviour
{
    public Vector3 startPos;

    Transform Raft1;
    Transform Raft2;
    public float moveSpeed;      //move speed
    public float rotationSpeed;  //speed of turning
    public float chaseDist;

    GameObject target;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;

        Raft1 = GameObject.FindWithTag("Raft1").transform;
        Raft2 = GameObject.FindWithTag("Raft2").transform;
        print(this.GetComponent<Rigidbody>().velocity);

    }

    // Update is called once per frame

    /*
	void Update () {

        float dist1 = Mathf.Sqrt(Mathf.Pow(Raft1.position.x - transform.position.x, 2) + Mathf.Pow(Raft1.position.y - transform.position.y, 2));
        float dist2 = Mathf.Sqrt(Mathf.Pow(Raft2.position.x - transform.position.x, 2) + Mathf.Pow(Raft2.position.y - transform.position.y, 2));

        if (dist1 <= dist2)
        {
            if (dist1 < chaseDist)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Raft1.position - transform.position), rotationSpeed * Time.deltaTime);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;              
            }
            else
            {
                if(transform.position != startPos)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position), rotationSpeed * Time.deltaTime);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    
                }
            }
        }
        else if (dist2 < dist1)
        {
            if (dist2 < chaseDist)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Raft2.position - transform.position), rotationSpeed * Time.deltaTime);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                if (transform.position != startPos)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position), rotationSpeed * Time.deltaTime);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                }
            }
        }
    }
    */

    void Update()
    {

        float dist1 = Mathf.Sqrt(Mathf.Pow(Raft1.position.x - startPos.x, 2) + Mathf.Pow(Raft1.position.y - startPos.y, 2));
        float dist2 = Mathf.Sqrt(Mathf.Pow(Raft2.position.x - startPos.x, 2) + Mathf.Pow(Raft2.position.y - startPos.y, 2));

        if (dist1 <= dist2)
        {
            target = Raft1.gameObject;
        }
        else
        {
            target = Raft2.gameObject;
        }
        if (dist1 < chaseDist || dist2 < chaseDist)
        {
            Vector3 direction = (target.transform.position - this.transform.position).normalized;
            this.GetComponent<Rigidbody>().velocity = direction * moveSpeed;
        }
        else
        {
            Vector3 direction = (startPos - this.transform.position).normalized;
            this.GetComponent<Rigidbody>().velocity = direction * moveSpeed;
        }

    }
}