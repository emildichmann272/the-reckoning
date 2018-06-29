using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New chunk", menuName = "Chunk Types")]
public class Chunk : ScriptableObject
{
    public GameObject[] tilePrefabs;
    public GameObject chunkPrefab;

    public static int SIZE = 16;
    public static int tileSize = 10;

    public static int CHUNKSIZE = SIZE * tileSize;

    public Chunk(){}

    public void placeChunk(int x, int z, Transform worldTrans, LayerMask parentMask, GameObject chunkPre)
    {
        GameObject chunkObj = new GameObject() ;
        chunkObj.name = "chunk(" + x.ToString() + "-" + z.ToString() + ")";
        chunkObj.transform.parent = worldTrans;
        chunkObj.layer = parentMask;

        for (int xOffset = 0; xOffset <= SIZE; xOffset++)
        {
            for (int zOffset = 0; zOffset <= SIZE; zOffset++)
            {
                GameObject tile = tilePrefabs[getRandomTileIndex()];
                GameObject newTile = Instantiate(tile, new Vector3(x* CHUNKSIZE + xOffset*tileSize, 0 , z* CHUNKSIZE + zOffset*tileSize), Quaternion.identity);

                newTile.layer = parentMask;

                newTile.transform.parent = chunkObj.transform;
            }
        }
    }

    private int getRandomTileIndex()
    {
        return Random.Range(0, tilePrefabs.Length);
    }

}