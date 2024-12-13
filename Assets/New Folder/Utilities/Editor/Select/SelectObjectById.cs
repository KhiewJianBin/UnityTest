using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides a window where you can select objects by typing in there Instance ID.
/// </summary>
public class SelectObjectById : EditorWindow
{
    string instanceIDs = string.Empty;

    /// <summary>
    /// Called by unity to draw the window ui.
    /// </summary>
    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUI.SetNextControlName("txtIds");
        instanceIDs = GUILayout.TextArea(instanceIDs, int.MaxValue, GUILayout.ExpandHeight(true));
        GUILayout.BeginHorizontal();

        GUI.SetNextControlName("btnPaste");

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select", GUILayout.Width(125)))
        {
            // get ids
            try
            {
                IEnumerable<int> data =
                    from x in instanceIDs.Trim().Split(new[] {",", "\n"}, StringSplitOptions.RemoveEmptyEntries)
                    select int.Parse(x.Trim());
                Selection.objects = data.Select(EditorUtility.InstanceIDToObject).ToArray();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    [MenuItem("Utilities/Select/Select Object by ID")]
    public static void ShowWindow()
    {
        // get the window, show it, and hand it focus
        SelectObjectById window = GetWindow<SelectObjectById>();
        window.titleContent.text = "Select Object by ID";
        window.Show();
        window.Focus();
        window.Repaint();
    }
}