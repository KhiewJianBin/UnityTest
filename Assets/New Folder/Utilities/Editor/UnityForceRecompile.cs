using UnityEditor;
using UnityEngine;

public static class UnityForceRecompile
{
    [MenuItem("Utilities/Recompile")]
    public static void Recompile()
    {
        AssetDatabase.Refresh();
        Debug.Log("Recompiled");
    }
}
