using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class World : MonoBehaviour {

    public Chunk[] chunkTypes;

    public GameObject chunkBase;

    public bool randomGeneration = true;

    private LayerMask ground;

    // Use this for initialization
    void Start () {
        ground = LayerMask.NameToLayer("Ground");
        AddNavMeshData();
        BuildMask = ~0;
        NullMask = 0;
    }

    public void LoadChunks(float x1, float z1, float w, float h)
    {
        if (!randomGeneration)
        {
            return;
        }
        int x0 = Mathf.FloorToInt(x1 / Chunk.CHUNKSIZE);
        int w0 = Mathf.CeilToInt((x1 + w) / Chunk.CHUNKSIZE) - x0;
        int z0 = Mathf.FloorToInt(z1 / Chunk.CHUNKSIZE);
        int h0 = Mathf.CeilToInt((z1 + h) / Chunk.CHUNKSIZE) - z0;

        for (int x = -1; x <= w0; x++)
        {
            for (int z = -1; z <= h0; z++)
            {
                Transform chunk = this.gameObject.transform.Find("chunk(" + (x + x0).ToString() + "-" + (z + z0).ToString() + ")");
                if (chunk == null)
                {
                    chunkTypes[0].placeChunk(x + x0, z + z0, this.transform, ground, chunkBase);
                }
            }
        }
    }

    // Nav mesh builder
    Vector3 BoundsCenter = Vector3.zero;
    Vector3 BoundsSize = new Vector3(999999f, 4000f, 999999f);

    LayerMask BuildMask;
    LayerMask NullMask;

    NavMeshData NavMeshData;
    NavMeshDataInstance NavMeshDataInstance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Build " + Time.realtimeSinceStartup.ToString());
            Build();
            Debug.Log("Build finished " + Time.realtimeSinceStartup.ToString());
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Update " + Time.realtimeSinceStartup.ToString());
            UpdateNavmeshData();
        }
    }

    void AddNavMeshData()
    {
        if (NavMeshData != null)
        {
            if (NavMeshDataInstance.valid)
            {
                NavMesh.RemoveNavMeshData(NavMeshDataInstance);
            }
            NavMeshDataInstance = NavMesh.AddNavMeshData(NavMeshData);
        }
    }

    void UpdateNavmeshData()
    {
        StartCoroutine(UpdateNavmeshDataAsync());
    }

    IEnumerator UpdateNavmeshDataAsync()
    {
        AsyncOperation op = NavMeshBuilder.UpdateNavMeshDataAsync(
            NavMeshData,
            NavMesh.GetSettingsByID(0),
            GetBuildSources(BuildMask),
            new Bounds(BoundsCenter, BoundsSize));
        yield return op;

        AddNavMeshData();
        Debug.Log("Update finished " + Time.realtimeSinceStartup.ToString());
    }

    void Build()
    {
        NavMeshData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            GetBuildSources(NullMask),
            new Bounds(BoundsCenter, BoundsSize),
            Vector3.zero,
            Quaternion.identity);
        AddNavMeshData();
    }

    List<NavMeshBuildSource> GetBuildSources(LayerMask mask)
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(
            new Bounds(BoundsCenter, BoundsSize),
            mask,
            NavMeshCollectGeometry.RenderMeshes,
            0,
            new List<NavMeshBuildMarkup>(),
            sources);
        Debug.Log("Sources found: " + sources.Count.ToString());
        return sources;
    }
}
