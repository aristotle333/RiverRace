using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {

    public GameObject[] carTypes;
    public float carSpawnRate;
    public float xSpawn;
    public float YSpawn;
    bool started = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (!GasGrab.tutorial && started == false)
        {
            SpawnCar();
            started = true;
        }
        
    }

    public void SpawnCar()
    {
        //Pick a car prefab to instantiate (it's not actually random)
        int ndx = Random.Range(0, carTypes.Length);
        GameObject go = Instantiate(carTypes[ndx]) as GameObject;

        //Position the Car either to the left or the right of the screen with xSpawn
        Vector3 pos = Vector3.zero;
        pos.x = xSpawn;
        //Position the Car at the right height based on what lane it should be in
        pos.y = YSpawn;
        go.transform.position = pos;
        //Call SpawnCar() again in a couple seconds
        Invoke("SpawnCar", carSpawnRate);
    }
}
