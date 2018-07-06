using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TileOrientationAlias))]
public class TileOrientationAliasEditor : Editor {

    private TileOrientationAlias script;
    private GUILayoutOption[] toggleMenuOptions = new GUILayoutOption[]
    {
        GUILayout.Width(200),
        GUILayout.MaxWidth(200)
    };
    public void OnEnable()
    {
        script = (TileOrientationAlias)target;
    }

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);
        script = (TileOrientationAlias)target;

        EditorGUILayout.PrefixLabel("Tile Mask");
        for (int i = 0; i < 9; i++)
        {
            int rowIndex = i % 3;
            if (rowIndex == 0)
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

        EditorGUILayout.PrefixLabel("origin");

        script.AddOrigin((TileOrientation)EditorGUILayout.ObjectField(script.origin, typeof(TileOrientation), false));

    }
}
