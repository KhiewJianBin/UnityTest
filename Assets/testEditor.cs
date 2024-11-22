using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(test))]
[CanEditMultipleObjects]
public class testEditor : Editor
{
    bool[] a = {true,true,true,true,true};
    string[] t = {"h", "e","l", "l", "o" };
    void OnEnable()
    {
    }

    /*
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

       

        serializedObject.ApplyModifiedProperties();
    }

    //*/
}