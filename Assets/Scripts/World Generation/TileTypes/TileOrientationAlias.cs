using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Alias", menuName = "World Generation/New Alias")]
public class TileOrientationAlias : TileOrientationAbstract
{
    public TileOrientation origin;

    public void AddOrigin(TileOrientation origin)
    {
        if (origin == null)
        {
            return;
        }
        this.origin = origin;
        tiles = origin.tiles;
    }
    public TileOrientationAlias()
    {
        tileMask = new bool[9];
    }
}
