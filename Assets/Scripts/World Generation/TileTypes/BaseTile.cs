using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Tile", menuName = "World Generation/New Base Tile")]
public class BaseTile : Tile{
    [SerializeField] GameObject tile;

    public override GameObject SpawnTile(int x, int z, bool[] nearbyTiles)
    {
        return Instantiate(tile, new Vector3(x, 0, z), Quaternion.identity);
    }
}
