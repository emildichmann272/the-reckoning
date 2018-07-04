using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChunkType", menuName = "World Generation/New Chunk Type")]
public class Chunk : ScriptableObject
{
    public Tile[] tiles;

    public static int SIZE = 16;
    public static int tileSize = 10;
    public static int CHUNKSIZE = SIZE * tileSize;

    private static Transform[] nearbyChunks = new Transform[9];
    private static int currentX, currentY;

    private int[] generatedTiles = new int[(SIZE + 1) * (SIZE + 1)];

    public Chunk(){}

    public void PlaceChunk(int x, int z, Transform worldTrans, LayerMask parentMask, World world)
    {
        string chunkName = "chunk(" + x.ToString() + "-" + z.ToString() + ")";

        GameObject chunkObj = new GameObject() ;
        chunkObj.name = chunkName;
        chunkObj.transform.parent = worldTrans;
        chunkObj.layer = parentMask;

        FocusChunk(x, z, world);

        for (int i = 0; i < generatedTiles.Length; i++)
        {
            generatedTiles[i] = -1;
        }
        for (int zOffset = 0; zOffset < SIZE; zOffset++)
        {
            for (int xOffset = 0; xOffset < SIZE; xOffset++)
            {
                int generatedTileIndex = xOffset + zOffset * SIZE;
                int index = GetTile(xOffset , zOffset);
                Tile tile = tiles[index];

                bool[] nearbyTiles = new bool[9];

                nearbyTiles[0] = GetTile(xOffset - 1, zOffset - 1) == 1;
                nearbyTiles[1] = GetTile(xOffset, zOffset - 1) == 1;
                nearbyTiles[2] = GetTile(xOffset + 1, zOffset - 1) == 1;

                nearbyTiles[3] = GetTile(xOffset - 1, zOffset) == 1;
                // current tile
                nearbyTiles[5] = GetTile(xOffset + 1, zOffset) == 1;

                nearbyTiles[6] = GetTile(xOffset - 1, zOffset + 1) == 1;
                nearbyTiles[7] = GetTile(xOffset, zOffset + 1) == 1;
                nearbyTiles[8] = GetTile(xOffset + 1, zOffset + 1) == 1;

                int tx = x * CHUNKSIZE + xOffset * tileSize;
                int tz = z * CHUNKSIZE + zOffset * tileSize;

                GameObject newTile = tile.SpawnTile(tx, tz, nearbyTiles);

                // Used for debug wrong tile orientation
                if (newTile == null)
                {
                    nearbyTiles[0] = GetTileLog(xOffset - 1, zOffset - 1) == 1;
                    nearbyTiles[1] = GetTileLog(xOffset, zOffset - 1) == 1;
                    nearbyTiles[2] = GetTileLog(xOffset + 1, zOffset - 1) == 1;

                    nearbyTiles[3] = GetTileLog(xOffset - 1, zOffset) == 1;
                    // current tile
                    nearbyTiles[5] = GetTileLog(xOffset + 1, zOffset) == 1;

                    nearbyTiles[6] = GetTileLog(xOffset - 1, zOffset + 1) == 1;
                    nearbyTiles[7] = GetTileLog(xOffset, zOffset + 1) == 1;
                    nearbyTiles[8] = GetTileLog(xOffset + 1, zOffset + 1) == 1;
                    throw new System.NotImplementedException();
                }

                newTile.name = index.ToString();
                newTile.transform.parent = chunkObj.transform;
            }
        }
    }

    private static void FocusChunk(int x, int y, World world)
    {
        nearbyChunks[0] = world.GetChunk(x - 1, y - 1);
        nearbyChunks[1] = world.GetChunk(x, y - 1);
        nearbyChunks[2] = world.GetChunk(x + 1, y - 1);

        nearbyChunks[3] = world.GetChunk(x - 1, y);
        // current Chunk
        nearbyChunks[5] = world.GetChunk(x + 1, y);

        nearbyChunks[6] = world.GetChunk(x - 1, y + 1);
        nearbyChunks[7] = world.GetChunk(x, y + 1);
        nearbyChunks[8] = world.GetChunk(x + 1, y + 1);

        currentX = x * SIZE;
        currentY = y * SIZE;
    }

    private int GetTile(int x, int y)
    {
        int targetChunkX = 1;
        int targetChunkY = 1;

        // check if tile come from nearby chunk
        if (x < 0)
        {
            targetChunkX -= 1;
            x = SIZE - 1;
        }
        else if (x >= SIZE)
        {
            targetChunkX += 1;
            x = 0;
        }
        if (y < 0)
        {
            targetChunkY -= 1;
            y = SIZE-1;
        }
        else if (y >= SIZE)
        {
            targetChunkY += 1;
            y = 0;
        }
        int index = targetChunkX + targetChunkY * 3;
        if (index != 4)
        {
            // check if nearby chunk have been loaded
            if (nearbyChunks[index] != null)
            {
                return int.Parse(nearbyChunks[index].GetChild(x + y * SIZE).name);
            }
            return GetTileType(x + currentX + (targetChunkX - 1) * SIZE - 100, y + currentY + (targetChunkY - 1) * SIZE);
        }
        // test if tile have already been made
        int preMade = generatedTiles[x + y * SIZE];
        if (preMade != -1)
        {
            return preMade;
        }
        // create new tile
        int sample = GetTileType(x + currentX - 100, y + currentY);
        generatedTiles[x + y * SIZE] = sample;
        return sample;
    }

    private int GetTileLog(int x, int y)
    {
        int targetChunkX = 1;
        int targetChunkY = 1;

        // check if tile come from nearby chunk
        if (x < 0)
        {
            targetChunkX -= 1;
            x = SIZE - 1;
        }
        else if (x >= SIZE)
        {
            targetChunkX += 1;
            x = 0;
        }
        if (y < 0)
        {
            targetChunkY -= 1;
            y = SIZE - 1;
        }
        else if (y >= SIZE)
        {
            targetChunkY += 1;
            y = 0;
        }
        int index = targetChunkX + targetChunkY * 3;
        if (index != 4)
        {
            // check if nearby chunk have been loaded
            if (nearbyChunks[index] != null)
            {
                Transform child = nearbyChunks[index].GetChild(x + y * SIZE);
                child.position += new Vector3(0, 20, 0);
                int value = int.Parse(child.name);
                Debug.Log("from Chunk " + (currentX/SIZE + targetChunkX - 1).ToString() + "-" + (currentY/SIZE + targetChunkY - 1).ToString() + " pos: " + x.ToString() + " - " + y.ToString() + " value: " + value.ToString()) ;
                return value;
            }
            Debug.Log("generated" + x.ToString() + " - " + y.ToString());
            return GetTileType(x + currentX + (targetChunkX - 1) * SIZE - 100, y + currentY + (targetChunkY - 1) * SIZE);
        }
        // test if tile have already been made
        int preMade = generatedTiles[x + y * SIZE];
        if (preMade != -1)
        {
            Debug.Log("premade" + x.ToString() + " - " + y.ToString());
            return preMade;
        }
        // create new tile
        int sample = GetTileType(x + currentX - 100, y + currentY);
        generatedTiles[x + y * SIZE] = sample;
        Debug.Log("new" + x.ToString() + " - " + y.ToString());
        return sample;
    }


    private static int GetTileType(float x, float y)
    {
        //TODO: create world that fit intended play style
        float sample = (Mathf.PerlinNoise((x) / 4, y / 4 + 1000) > 0.3 ? 1 : 0) - (Mathf.PerlinNoise((x) / 12, y / 12 + 1000) > 0.3 ? 1 : 0);
        if (sample >= 0.2)
        {
            return 0;
        }
        return 1;
    }
}