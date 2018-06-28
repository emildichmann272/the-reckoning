using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New chunk", menuName = "Chunk Types")]
public class Chunk : ScriptableObject
{
    public GameObject[] tilePrefabs;

    private static NavMeshData navData;

    public static int WIDTH = 5;
    public static int HEIGHT = 5;
    public static Vector2[] chunks;

    public static int tileWidth = 10;
    public static int tileHeight = 10;


    public Chunk(){}

    public void placeChunk(int x, int z, Transform worldTrans)
    {
        GameObject chunkObj = new GameObject("chunk(" + x.ToString() + "-" + z.ToString() + ")");
        chunkObj.transform.parent = worldTrans;

        for (int xOffset = 0; xOffset <= WIDTH; xOffset++)
        {
            for (int zOffset = 0; zOffset <= HEIGHT; zOffset++)
            {
                GameObject tile = tilePrefabs[getRandomTileIndex()];
                GameObject newTile = World.Instantiate(tile, new Vector3(x*WIDTH*tileWidth + xOffset*tileWidth, 0 , z*HEIGHT*tileHeight + zOffset*tileHeight), Quaternion.identity);
                NavMeshSurface surf = newTile.GetComponent<NavMeshSurface>();
                
                newTile.transform.parent = chunkObj.transform;
            }
        }
    }

    private int getRandomTileIndex()
    {
        return Random.Range(0, tilePrefabs.Length);
    }

}