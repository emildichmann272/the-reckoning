using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour {

    public Transform backPlane;
    public World worldController;

	// Use this for initialization
	void Start () {
        Vector3 bPos = backPlane.position;
        float width = 10f * 10f;
        float height = 10f * 13f;
        float x1 = bPos.x - width / 2f;
        float z1 = bPos.z - height / 2f;
        worldController.LoadChunks(x1, z1, width, height);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
