using UnityEngine;
using System.Collections;

public class RopeManager : MonoBehaviour {

    [Header("Group A Transform References")]
    public Transform groupA1;
    public Transform groupA2;

    [Header("Group B Transform References")]
    public Transform groupB1;
    public Transform groupB2;

    [Header("Rope References")]
    public GameObject groupARope;           // Reference of Group A's rope
    public GameObject groupBRope;

    void Update() {
        UpdateRopes();
    }


    // Updates locations, size and rotation of both ropes for both teams
    private void UpdateRopes() {
        float diffXA = Mathf.Abs(groupA1.position.x - groupA2.position.x);
        float diffYA = Mathf.Abs(groupA1.position.y - groupA2.position.y);
        float angleA = Mathf.Atan(diffYA / diffXA) * Mathf.Rad2Deg;
        float sizeA = Vector3.Distance(groupA1.position, groupA2.position);

        float diffXB = Mathf.Abs(groupB1.position.x - groupB2.position.x);
        float diffYB = Mathf.Abs(groupB1.position.y - groupB2.position.y);
        float angleB = Mathf.Atan(diffYB / diffXB) * Mathf.Rad2Deg;
        float sizeB = Vector3.Distance(groupB1.position, groupB2.position);

        SetRopePosition(diffXA, diffYA, groupA1.position, groupA2.position, ref groupARope);
        SetRopePosition(diffXB, diffYB, groupB1.position, groupB2.position, ref groupBRope);

        groupARope.transform.rotation = Quaternion.Euler(0, 0, angleA);
        groupBRope.transform.rotation = Quaternion.Euler(0, 0, angleB);
        groupARope.transform.localScale = new Vector3(sizeA, 1, 0);
        groupBRope.transform.localScale = new Vector3(sizeB, 1, 0);
    }

    private void SetRopePosition(float dx, float dy, Vector3 pos1, Vector3 pos2, ref GameObject ropeRef) {
        Vector3 ropPos = new Vector3(0, 0, 0);
        if (pos1.x < pos2.x) {
            ropPos.x = pos1.x + 0.5f * dx;
        }
        else {
            ropPos.x = pos2.x + 0.5f * dx;
        }

        if (pos1.y < pos2.y) {
            ropPos.y = pos1.y + 0.5f * dy;
        }
        else {
            ropPos.y = pos2.y + 0.5f * dy;
        }
        ropeRef.transform.position = ropPos;
    }

}
