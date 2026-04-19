using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlockManager))]
public class BlockManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BlockManager blockManager = target as BlockManager;

        if (GUILayout.Button("SetNextBlocksMaterial")) {
            blockManager.SetNextBlocksMaterial();
            EditorUtility.SetDirty(blockManager);
        }
        //GUILayout.Label("Material index = " + blockManager.CurrentMaterialIndex.ToString());

    }

}
