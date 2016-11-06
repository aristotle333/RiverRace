using UnityEngine;
using System.Collections;

public class Alligator : MonoBehaviour {

    public float speed;
    public float distance;
    Vector3 startPos;
    public bool vertical;

	// Use this for initialization
	void Start () {
        startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (!vertical)
        {
            float xPos = Mathf.Abs(startPos.x - this.transform.position.x);

            if (xPos >= distance)
            {
                speed *= -1;
            }
            //Change transform.position based on the axes
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;
        }
        else
        {
            float yPos = Mathf.Abs(startPos.y - this.transform.position.y);

            if (yPos >= distance)
            {
                speed *= -1;
            }
            //Change transform.position based on the axes
            Vector3 pos = transform.position;
            pos.y += speed * Time.deltaTime;
            transform.position = pos;
        }

    }
}
