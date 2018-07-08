using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOrientationAbstract : ScriptableObject{

    public bool[] tileMask;
    public bool[] floorMask;
    public bool hasInverse;
    private bool inversed;
    private int tileConnectionCount;
    private int floorConnectionCount;
    private int totalConnectionCount;
    public GameObject[] tiles = new GameObject[0];

    private int rotations = 0;

    public bool HasInverse()
    {
        return hasInverse;
    }

    public int GetConnectionCount()
    {
        return tileConnectionCount;
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
            int similar = 0;
            for (int i = 0; i < nearbyTiles.Length; i++)
            {
                if (i == 4) { continue; }
                if (nearbyTiles[i] == true && tileMask[i] == true)
                {
                    similar++;
                }
                if (nearbyTiles[i] == false && floorMask[i] == true)
                {
                    similar++;
                }
            }
            if (similar == totalConnectionCount)
            {
                tile = Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(x, 0, z), Quaternion.Euler(0, 180 - rotation, 0));
                break;
            }
            RotateMask();
        }
        Reset();
        return tile;
    }

    public void FindConnectionCount()
    {
        if (tileMask == null && floorMask == null)
        {
            return;
        }
        tileConnectionCount = 0;
        int i = 0;
        foreach (bool t in tileMask)
        {
            if (t && i != 4)
            {
                tileConnectionCount++;
            }
        }
        i = 0;
        floorConnectionCount = 0;
        foreach (bool f in floorMask)
        {
            if (f && i != 4)
            {
                floorConnectionCount++;
            }
        }
        totalConnectionCount = tileConnectionCount + floorConnectionCount;
    }

    public void OnEnable()
    {
        FindConnectionCount();
    }

    public void RotateMask()
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

        temp = floorMask[0];
        floorMask[0] = floorMask[6];
        floorMask[6] = floorMask[8];
        floorMask[8] = floorMask[2];
        floorMask[2] = temp;

        temp = floorMask[1];
        floorMask[1] = floorMask[3];
        floorMask[3] = floorMask[7];
        floorMask[7] = floorMask[5];
        floorMask[5] = temp;

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


        temp = floorMask[0];
        floorMask[0] = floorMask[2];
        floorMask[2] = temp;

        temp = floorMask[3];
        floorMask[3] = floorMask[5];
        floorMask[5] = temp;

        temp = floorMask[6];
        floorMask[6] = floorMask[8];
        floorMask[8] = temp;
    }

    public void Reset()
    {
        while (rotations != 0)
        {
            RotateMask();
        }
        if (inversed)
        {
            Inverse();
        }
    }
}
