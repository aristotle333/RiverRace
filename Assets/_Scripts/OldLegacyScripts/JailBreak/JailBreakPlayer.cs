using UnityEngine;
using System.Collections;

public class JailBreakPlayer : MonoBehaviour {
    [Header("Player Stats References")]
    public int      playerNum;                  // The number of the player 1-4 inclusive
    public float    speed;                      // How fast the player can move
    public float    maxSeperation; 
    public GameObject partner;                  // Gameobject reference of the player's partner
    

    public Bounds bounds;

    public bool Locked = false;

    // Axis names
    private string xAxisName;
    private string yAxisName;

    void Awake() {
        xAxisName = "Player" + playerNum + "_axisX";
        yAxisName = "Player" + playerNum + "_axisY";
    }

    void Update() {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        getInput();
    }

    private void getInput() {
        float xAxis = Input.GetAxis(xAxisName);
        float yAxis = Input.GetAxis(yAxisName);

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        checkPartnerDistance(ref pos);
        this.transform.position = pos;

        bounds.center = transform.position;

        //Keep the player contrained to the screen bounds
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero) {
            pos -= off;
            transform.position = pos;
        }
    }
    
    // Checks if the partner is too far away and corrects their distance accordingly
    private void checkPartnerDistance(ref Vector3 newPos) {
        float distanceToPartner = Vector3.Distance(newPos, partner.transform.position);
        if (distanceToPartner > maxSeperation) {
            Vector3 temp = newPos - partner.transform.position;
            temp = Vector3.Normalize(temp) * maxSeperation;
            newPos = partner.transform.position + temp;
        }
    }

}
