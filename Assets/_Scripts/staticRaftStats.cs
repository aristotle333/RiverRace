using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class staticRaftStats : MonoBehaviour {

    public static staticRaftStats S;

    [Header("Prefabs of ships")]
    public List<GameObject> ships = new List<GameObject>();
    [Header("Color List")]
    public List<Color> colors = new List<Color>();     // A list with the available colors to choose from

    [Header("Mesh List")]
    public List<Mesh> shipMeshes = new List<Mesh>();

    [Header("Ship Material List")]
    public List<Material> shipMaterials = new List<Material>();

    [Header("Current stats List")]
    public static int team1ShipNum;      // The index of team 1 ship in the ships prefab list
    public static int team2ShipNum;      // The index of team 2 ship in the ships prefab list
    public static int team1ShipColor;       // The index of the Ship color for team 1 from the color list
    public static int team2ShipColor;       // The index of the Ship color for team 2 from the color list

    void Awake() {
        S = this;
    }

    void Start() {
        print(" team 1 type: " + team1ShipNum + " team 2 type: " + team2ShipNum +
              " team 1 color: " + team1ShipColor + " team 2 coloor: " + team2ShipColor);
    }

    public void SetShipCapsuleColliders(GameObject boat, int shipType) {

        CapsuleCollider colliderRef = boat.GetComponent<CapsuleCollider>();

        switch (shipType) {
            // Collider for OldWoddenBoat
            case 0:
                colliderRef.center = new Vector3(-0.03f, 0.15f, -0.5f);
                colliderRef.radius = 0.45f;
                colliderRef.height = 2.4f;
                colliderRef.direction = 1;     // y-Axis
                break;
            // Collider for NewWoodenBoat
            case 1:
                colliderRef.center = new Vector3(0f, -0.1f, -0.5f);
                colliderRef.radius = 0.4f;
                colliderRef.height = 2.3f;
                colliderRef.direction = 1;     // y-Axis
                break;
            // Collider for AluminumBoat
            case 2:
                colliderRef.center = new Vector3(-0.03f, 0.15f, -0.5f);
                colliderRef.radius = 0.45f;
                colliderRef.height = 2.4f;
                colliderRef.direction = 1;     // y-Axis
                break;
            default:
                print("Wrong Ship Type");
                break;
        }
    }

}
