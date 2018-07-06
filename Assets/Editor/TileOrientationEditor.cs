using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileOrientation))]
public class TileOrientationEditor : Editor {

    private TileOrientation script;
    private GUILayoutOption[] toggleMenuOptions = new GUILayoutOption[]
    {
        GUILayout.Width(200),
        GUILayout.MaxWidth(200)
    };

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);
        script = (TileOrientation)target;


        EditorGUILayout.PrefixLabel("Tile Mask");
        for (int i = 0; i < 9; i++)
        {
            int rowIndex = i % 3;
            if(rowIndex == 0)
            {
                EditorGUILayout.BeginHorizontal(toggleMenuOptions);
            }
            if (i != 4)
            {
                script.tileMask[i] = EditorGUILayout.Toggle(script.tileMask[i]);
            }
            else
            {
                EditorGUILayout.Toggle(false);
            }
            if (rowIndex == 2)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.PrefixLabel("Has inverse");
        script.hasInverse = EditorGUILayout.Toggle(script.hasInverse);

        EditorGUILayout.LabelField("Tiles");
        bool[] markedForDeletion = new bool[script.tiles.Length];
        int index = 0;
        int marked = 0;
        foreach (GameObject prefab in script.tiles)
        {
            Object obj = EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
            if(obj == null)
            {
                markedForDeletion[index] = true;
                marked++;
            }
            index++;
        }
        if (marked != 0)
        {
            GameObject[] newTiles = new GameObject[script.tiles.Length - marked];
            int ci = 0;
            for(int i = script.tiles.Length - 1; i >= 0; i--)
            {
                if (!markedForDeletion[i])
                {
                    newTiles[ci] = script.tiles[i];
                    ci++;
                }
            }
            script.tiles = newTiles;
        }
        EditorGUILayout.PrefixLabel("Add new tile");
        script.addTile((GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false));
    }

}
