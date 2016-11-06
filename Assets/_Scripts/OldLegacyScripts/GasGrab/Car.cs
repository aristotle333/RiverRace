using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

    public float speed = 6f;
    public float xOffset = 0;


	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update () {
        Move();
        CheckOffScreen();
	}

    public void Move()
    {
        Vector3 tempPos = this.transform.position;
        tempPos.x += speed * Time.deltaTime;
        this.transform.position = tempPos;
    }

    public void CheckOffScreen()
    {
        xOffset = this.transform.position.x - Camera.main.transform.position.x;
        if (xOffset > 17 || xOffset < -17)
        {
            Destroy(this.gameObject);
        }
    }
}
