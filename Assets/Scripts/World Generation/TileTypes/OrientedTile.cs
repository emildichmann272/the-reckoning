using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Oriented Tile", menuName = "World Generation/New Oriented Tile")]
public class OrientedTile : Tile
{
    [SerializeField] public TileOrientationAbstract[] tiles;
    [SerializeField] public GameObject nullTile;

    public override GameObject SpawnTile(int x, int z, bool[] nearbyTiles)
    {
        int count = 0;
        for (int i = 0; i < nearbyTiles.Length; i++)
        {
            if (i == 4)
            {
                continue;
            }
            count += nearbyTiles[i] ? 1 : 0;
        }
        int connectionCount = count;
        GameObject tile = null;
        foreach (TileOrientationAbstract orientation in tiles)
        {
            if (connectionCount == orientation.GetConnectionCount())
            {
                tile = orientation.PlaceTile(nearbyTiles, x, z);
                if (tile == null && orientation.HasInverse())
                {
                    orientation.Inverse();
                    tile = orientation.PlaceTile(nearbyTiles, x, z);
                    if (tile != null)
                    {
                        tile.transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                if (tile != null)
                {
                    return tile;
                }
            }
        }
        string output = "";
        foreach(bool arg in nearbyTiles)
        {
            output += arg ? "1" : "0";
        }
        Debug.Log("Unimplemented rotation " + output);
        
        return Instantiate(nullTile, new Vector3(x, 0, z), Quaternion.identity); ;
    }
}
