using UnityEngine;
using System.Collections;

public class loadRaftModel : MonoBehaviour {

    [Header("Team Number (either 1 or 2)")]
    public int teamNum;

    [Header("Reference to the parent Gameobject, so the Raft")]
    public GameObject parentRaftObject;

    void Start() {
        LoadMesh();
        AssignMaterialAndColor();
        //SetRaftCollider();
    }

    private void LoadMesh() {
        if (teamNum == 1) {
            this.gameObject.GetComponent<MeshFilter>().mesh = staticRaftStats.S.shipMeshes[staticRaftStats.team1ShipNum];
            return;
        }
        if (teamNum == 2) {
            this.gameObject.GetComponent<MeshFilter>().mesh = staticRaftStats.S.shipMeshes[staticRaftStats.team2ShipNum];
            return;
        }
        print("Team Number is incorrect, please check team number");
    }

    private void AssignMaterialAndColor() {
        if (teamNum == 1) {
            this.GetComponent<Renderer>().material = staticRaftStats.S.shipMaterials[staticRaftStats.team1ShipNum];
            this.GetComponent<Renderer>().material.color = staticRaftStats.S.colors[staticRaftStats.team1ShipColor];
            return;
        }
        if (teamNum == 2) {
            this.GetComponent<Renderer>().material = staticRaftStats.S.shipMaterials[staticRaftStats.team2ShipNum];
            this.GetComponent<Renderer>().material.color = staticRaftStats.S.colors[staticRaftStats.team2ShipColor];
            return;
        }
        print("Team Number is incorrect, please check team number");
    }

    private void SetRaftCollider() {
        if (teamNum == 1) {
            staticRaftStats.S.SetShipCapsuleColliders(parentRaftObject, staticRaftStats.team1ShipNum);
            return;
        }
        if (teamNum == 2) {
            staticRaftStats.S.SetShipCapsuleColliders(parentRaftObject, staticRaftStats.team2ShipNum);
            return;
        }
        print("Team Number is incorrect, please check team number");
    }

}
