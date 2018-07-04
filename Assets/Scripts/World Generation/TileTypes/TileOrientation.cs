using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Orientation", menuName = "World Generation/New Tile Orientation")]
public class TileOrientation : TileOrientationAbstract
{

    public TileOrientation()
    {
        tileMask = new bool[9];
    }
}
