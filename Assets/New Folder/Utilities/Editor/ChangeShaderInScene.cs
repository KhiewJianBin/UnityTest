using UnityEngine;
using UnityEditor;

public class ChangeShaderInScene : EditorWindow
{
    Shader s;
    bool changeChild;

    int prevselectedcount = 0;
    Transform[] selectedobj;

    [MenuItem("Utilities/ChangeShaderInScene")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ChangeShaderInScene));
    }
    void Update()
    {
        selectedobj = Selection.transforms;
        if (selectedobj.Length != prevselectedcount)
        {
            Focus();
        }
    }
    void OnGUI()
    {
        prevselectedcount = selectedobj.Length;
        if (selectedobj.Length == 0)
        {
            EditorGUILayout.TextField("Please Select Object in Scene");
            return;
        }
        GUILayout.BeginVertical();
        {
            changeChild = EditorGUILayout.Toggle("changeChild",changeChild);

            s = (Shader)EditorGUILayout.ObjectField(s, typeof(Shader), true);

            if (GUILayout.Button("ChangeShader")) // when this button is clicked
            {
                if (changeChild)
                {
                    foreach (Transform parentObject in selectedobj)
                    {
                        MeshRenderer[] children;
                        children = parentObject.GetComponentsInChildren<MeshRenderer>(); // get all children's renderer component
                        
                        foreach (MeshRenderer rend in children)
                        {
                            Undo.RecordObject(rend, "Changed Shader");
                            for (int j = 0; j < rend.sharedMaterials.Length; j++) // for each material in each children component, change
                            {
                                rend.sharedMaterials[j].shader = s;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Transform parentObject in selectedobj)
                    {
                        MeshRenderer parent = parentObject.GetComponent<MeshRenderer>();
                        if (parent == null) return;
                        Undo.RecordObject(parent, "Changed Shader");
                        for (var j = 0; j < parent.sharedMaterials.Length; j++) // for each material in each children component, change
                        {
                            parent.sharedMaterials[j].shader = s;
                        }
                    }
                }
            }
        }
        GUILayout.EndVertical();
    }
}