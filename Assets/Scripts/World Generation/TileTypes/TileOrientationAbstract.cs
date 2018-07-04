using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOrientationAbstract : ScriptableObject{

    public bool[] tileMask;
    public bool hasInverse;
    private bool inversed;
    private int connectionCount;
    public GameObject[] tiles = new GameObject[0];

    private int rotations = 0;

    public bool HasInverse()
    {
        return hasInverse;
    }

    public int GetConnectionCount()
    {
        return connectionCount;
    }

    public void addTile(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        GameObject[] newTiles = new GameObject[tiles.Length + 1];
        tiles.CopyTo(newTiles, 0);
        newTiles[tiles.Length] = obj;

        tiles = newTiles;
    }

    public GameObject PlaceTile(bool[] nearbyTiles, float x, float z)
    {
        GameObject tile = null;
        for (int rotation = 0; rotation < 360 - 1; rotation += 90)
        {
            bool same = true;
            for (int i = 0; i < nearbyTiles.Length; i++)
            {
                if (i == 4) { continue; }
                if (nearbyTiles[i] != tileMask[i])
                {
                    same = false;
                    break;
                }
            }
            if (same)
            {
                tile = Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(x, 0, z), Quaternion.Euler(0, 180 - rotation, 0));
                break;
            }
            RotateTileMask();
        }
        Reset();
        return tile;
    }

    public void FindConnectionCount()
    {
        if (tileMask == null)
        {
            return;
        }
            connectionCount = 0;
        int i = 0;
        foreach (bool t in tileMask)
        {
            if (t && i != 4)
            {
                connectionCount += 1;
            }
        }
    }

    public void OnEnable()
    {
        FindConnectionCount();
    }

    public void RotateTileMask()
    {
        bool temp = tileMask[0];
        tileMask[0] = tileMask[6];
        tileMask[6] = tileMask[8];
        tileMask[8] = tileMask[2];
        tileMask[2] = temp;

        temp = tileMask[1];
        tileMask[1] = tileMask[3];
        tileMask[3] = tileMask[7];
        tileMask[7] = tileMask[5];
        tileMask[5] = temp;

        rotations = (rotations + 1) % 4;
    }

    public void Inverse()
    {
        inversed = !inversed;
        bool temp = tileMask[0];
        tileMask[0] = tileMask[2];
        tileMask[2] = temp;

        temp = tileMask[3];
        tileMask[3] = tileMask[5];
        tileMask[5] = temp;

        temp = tileMask[6];
        tileMask[6] = tileMask[8];
        tileMask[8] = temp;
    }

    public void Reset()
    {
        while (rotations != 0)
        {
            RotateTileMask();
        }
        if (inversed)
        {
            Inverse();
        }
    }
}
