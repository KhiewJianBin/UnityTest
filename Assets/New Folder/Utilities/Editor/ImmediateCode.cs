using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides an immediate window where the user can input C# code and run it directly from within the unity editor.
/// </summary>
public class ImmediateCode : EditorWindow
{
    // script text
    string scriptText = string.Empty;

    // script we wrap around user entered code
    static readonly string scriptFormat = @"
                using UnityEngine;
                using UnityEditor;
                using System.Collections;
                using System.Collections.Generic;
                using System.Text;
                using System;
                public static class ImmediateWindowCodeWrapper
                {{
                    public static void PerformAction()
                    {{
                        // user code goes here
                        {0};
                    }}
                }}";

    // cache of last method we compiled so repeat executions only incur a single compilation
    MethodInfo lastScriptMethod;

    // position of scroll view
    Vector2 scrollPosition;

    void ValidateScript()
    {
        // create and configure the code provider
        CSharpCodeProvider codeProvider = new CSharpCodeProvider();
        CompilerParameters options = new CompilerParameters();
        options.GenerateInMemory = true;
        options.GenerateExecutable = false;

        // bring in system libraries
        options.ReferencedAssemblies.Add("System.dll");
        options.ReferencedAssemblies.Add("System.Core.dll");

        // bring in Unity assemblies
        options.ReferencedAssemblies.Add(typeof(EditorWindow).Assembly.Location);
        options.ReferencedAssemblies.Add(typeof(Transform).Assembly.Location);

        // compile an assembly from our source code
        CompilerResults result = codeProvider.CompileAssemblyFromSource(options, string.Format(scriptFormat, scriptText));

        // log any errors we got
        if (result.Errors.Count > 0)
        {
            foreach (CompilerError error in result.Errors)
            {
                // the magic -11 on the line is to compensate for usings and class wrapper around the user script code.
                // subtracting 11 from it will give the user the line numbers in their code.
                Debug.LogError(string.Format("Immediate Compiler Error ({0}): {1}", error.Line - 11, error.ErrorText));
            }
            lastScriptMethod = null;
        }

        // otherwise use reflection to pull out our action method so we can invoke it
        else
        {
            Type type = result.CompiledAssembly.GetType("ImmediateWindowCodeWrapper");
            lastScriptMethod = type.GetMethod("PerformAction", BindingFlags.Public | BindingFlags.Static);
        }
    }


    void HandleValidateAndExecuteButtons()
    {
        GUILayout.BeginHorizontal();

        // show the execute button
        if (GUILayout.Button("Validate"))
        {
            // if our script method needs compiling
            ValidateScript();
            if (lastScriptMethod != null) Debug.Log("No script errors! :D");
        }

        // show the execute button
        if (GUILayout.Button("Execute"))
        {
            // if we have a compiled method, invoke it
            ValidateScript();
            if (lastScriptMethod != null) lastScriptMethod.Invoke(null, null);
        }

        GUILayout.EndHorizontal();
    }

    void HandleSaveLoadButtons()
    {
        EditorGUILayout.BeginHorizontal();
        // show the execute button
        if (GUILayout.Button("Save"))
        {
            string fileName = EditorUtility.SaveFilePanel("Save Snippet", "", "", "txt");
            if (fileName.Length > 0) System.IO.File.WriteAllText(fileName, scriptText);
        }

        // show the execute button
        if (GUILayout.Button("Load"))
        {
            string fileName = EditorUtility.OpenFilePanel("Load Snippet", "", "txt");
            if (fileName.Length > 0)
            {
                scriptText = System.IO.File.ReadAllText(fileName);
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Called by unity to draw the window ui.
    /// </summary>
    void OnGUI()
    {
        // start the scroll view
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // show the script field
        scriptText = EditorGUILayout.TextArea(scriptText);
        EditorGUILayout.EndScrollView();

        HandleValidateAndExecuteButtons();
        HandleSaveLoadButtons();
    }

    [MenuItem("Utilities/ImmediateCode")]
    public static void ShowImmediateWindow()
    {
        // get the window, show it, and hand it focus
        try
        {
            ImmediateCode code = GetWindow<ImmediateCode>();
            code.titleContent.text = "Immediate";
            code.Show();
            code.Focus();
            code.Repaint();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}