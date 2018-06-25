using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Chunk[] chunkTypes;

	// Use this for initialization
	void Start () {
        for(int x = 0; x <= 3; x++) {
            for (int z = 0; z <= 0; z++) {
                CreateChunk(x, z);
            }
        }

    }

    public void CreateChunk (int x, int z)
    {
        chunkTypes[0].placeChunk(x, z, this.transform);
    }

    public void LoadChunks(float x1, float z1, float x2, float z2)
    {
        Debug.Log(x1 / (Chunk.WIDTH * Chunk.tileWidth));
        Debug.Log(Mathf.FloorToInt(x1 / (Chunk.WIDTH * Chunk.tileWidth)));
    }
}
