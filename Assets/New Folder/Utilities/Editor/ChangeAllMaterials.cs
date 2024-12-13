using UnityEngine;
using UnityEditor;

public class ChangeAllMaterials : EditorWindow
{
    GameObject parentObject;
    Material changeToMat;

    [MenuItem("Utilities/ChangeAllMaterials")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ChangeAllMaterials));
    }
    void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            parentObject = (GameObject)EditorGUILayout.ObjectField(parentObject, typeof(GameObject), true);

            changeToMat = (Material)EditorGUILayout.ObjectField(changeToMat, typeof(Material), true);

            if (parentObject != null && changeToMat != null)
            {
                if (GUILayout.Button("ChangeMaterial"))
                {
                    Renderer[] children;
                    children = parentObject.GetComponentsInChildren<Renderer>();
                    foreach (Renderer rend in children)
                    {
                        var mats = new Material[rend.sharedMaterials.Length];
                        for (var j = 0; j < rend.sharedMaterials.Length; j++)
                        {
                            mats[j] = changeToMat;
                        }
                        rend.sharedMaterials = mats;
                    }
                }
            }
        }
        GUILayout.EndVertical();
    }
}