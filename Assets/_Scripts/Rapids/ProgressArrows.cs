using UnityEngine;
using System.Collections;

public class ProgressArrows : MonoBehaviour {

	public GameObject raftObject;
    public float xpos;
    public RectTransform Arrow;
    Transform Raft;
    Transform Finish;
    float totaldistance;
    float startposition;

    // Use this for initialization
    void Start () {
		Raft = raftObject.transform;
        Finish = GameObject.FindWithTag("Finish").transform;

        totaldistance = Finish.position.y - Raft.position.y;
        startposition = Raft.position.y;
    }
	
	// Update is called once per frame
	void Update () {
		if (raftObject.GetComponent<Raft> ().noControl)
			Arrow.gameObject.SetActive(false);
		else
			Arrow.gameObject.SetActive(true);
		
        float distance = Raft.position.y;

        float percentage = (distance - startposition) / totaldistance;

        if (percentage < 0f)
        {
            percentage = 0f;
        }

        float y = -210f + (420f * percentage);

        Arrow.localPosition = new Vector3(xpos, y, 0f);
	
	}
}
