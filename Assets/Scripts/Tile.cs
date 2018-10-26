using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tiles")]
public class Tile : ScriptableObject {

    public List<Helper.Directions> directions;

    public float extentSize;
    public GameObject way;
    public GameObject middle;
}
