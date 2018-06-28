using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Chunk[] chunkTypes;

    public float chunkSize = Chunk.WIDTH * Chunk.tileWidth;

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

    public void LoadChunks(float x1, float z1, float w, float h)
    {
        int x0 = Mathf.FloorToInt(x1 / chunkSize);
        int w0 = Mathf.CeilToInt((x1 + w) / chunkSize) - x0;
        int z0 = Mathf.FloorToInt(z1 / chunkSize);
        int h0 = Mathf.CeilToInt((z1 + h) / chunkSize) - z0;
        for (int x = -1; x <= w0; x++)
        {
            for (int z = -1; z <= h0; z++)
            {
                Transform chunk = this.gameObject.transform.Find("chunk(" + (x + x0).ToString() + "-" + (z + z0).ToString() + ")");
                if (chunk == null)
                {
                    chunkTypes[0].placeChunk(x + x0, z + z0, this.transform);
                }
            }
        }
    }
}
