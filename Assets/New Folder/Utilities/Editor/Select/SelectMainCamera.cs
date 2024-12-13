using UnityEditor;
using UnityEngine;  
 
public class SelectMainCamera : ScriptableObject
{
    [MenuItem ("Utilities/Select/Main Camera")]
    static void Select()
    {
        if (Camera.main != null)
            Selection.activeObject = Camera.main.gameObject;
    }
}