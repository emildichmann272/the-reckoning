using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : ScriptableObject
{
    public abstract GameObject SpawnTile(int x, int z, bool[] nearbyTiles);
}
