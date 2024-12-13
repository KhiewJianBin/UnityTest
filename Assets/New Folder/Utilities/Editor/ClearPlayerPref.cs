using UnityEditor;
using UnityEngine;

public class clearplayerpref
{
    [MenuItem("Utilities/Clear Player Pref")]
    public static void clearpref()
    {
        PlayerPrefs.DeleteAll();
            Debug.Log("Player Pref Deleted");
    }
}