/*
 *	Created by:  Peter @sHTiF Stefcek
 */

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BinaryEgo.Voxelizer.Editor
{
    [CustomEditor(typeof(Voxelizer))]
    public class VoxelizerInspector : UnityEditor.Editor
    {
        public static GUISkin Skin => (GUISkin)Resources.Load("Skins/VoxelizerEditorSkin");
        
        private Voxelizer voxelizer => (target as Voxelizer);

        
        public override void OnInspectorGUI()
        {
            var voxelizer = (target as Voxelizer);
            
            GUILayout.Label("<color=#FF8800>VOXELIZER</color>", Skin.GetStyle("editor_title"), GUILayout.Height(24));
            GUILayout.Label("VERSION "+Voxelizer.VERSION, Skin.GetStyle("editor_version"), GUILayout.Height(16));

            EditorGUI.BeginChangeCheck();

            GUILayout.Space(4);
            
            DrawSourceSection();
            
            GUILayout.Space(2);

            DrawVoxelSection();
            
            GUILayout.Space(2);
            
            DrawAdditionalSection();
            
            GUILayout.Space(2);
            
            if (EditorGUI.EndChangeCheck())
            {
                if (voxelizer.autoVoxelize)
                {
                    Voxelize();
                }
            }

            GUI.color = new Color(0.9f, .5f, 0);
            
            if (GUIUtils.DrawButton("VOXELIZE"))
            {
                Voxelize();
            }

            GUI.color = Color.white;
        }

        private void Voxelize()
        {
            voxelizer.Voxelize();
            EditorUtility.SetDirty(voxelizer);
            SceneView.lastActiveSceneView?.Repaint();
        }

        public void DrawSourceSection()
        {
            if (!GUIUtils.DrawMinimizableSectionTitle("SOURCE SETTINGS", ref voxelizer.sourceSectionMinimized))
                return;
            
            voxelizer.sourceTransform =
                (Transform)EditorGUILayout.ObjectField("Source", voxelizer.sourceTransform,
                    typeof(Transform), true);
            voxelizer.sourceLayerMask = EditorGUILayout.MaskField("Source Mask", voxelizer.sourceLayerMask, InternalEditorUtility.layers);

            GUI.enabled = voxelizer.sourceTransform != null;
            
            if (voxelizer.sourceTransform != null && GUIUtils.DrawButton(voxelizer.sourceTransform.gameObject.activeSelf ? "HIDE SOURCE" : "SHOW SOURCE"))
            {
                voxelizer.sourceTransform.gameObject.SetActive(!voxelizer.sourceTransform.gameObject.activeSelf);
            }
        }

        public void DrawVoxelSection()
        {
            if (!GUIUtils.DrawMinimizableSectionTitle("VOXEL SETTINGS", ref voxelizer.voxelSectionMinimized))
                return;
            
            GUI.enabled = voxelizer.sourceTransform != null;

            voxelizer.autoVoxelize = EditorGUILayout.Toggle("Auto Voxelize", voxelizer.autoVoxelize);

            voxelizer.voxelizationType =
                (VoxelizationType)EditorGUILayout.EnumPopup("Voxelization", voxelizer.voxelizationType);

            voxelizer.voxelSizeType = (VoxelSizeType)EditorGUILayout.EnumPopup("Voxel Size Type", voxelizer.voxelSizeType);

            switch (voxelizer.voxelSizeType)
            {
                case VoxelSizeType.ABSOLUTE:
                    voxelizer.voxelSize = EditorGUILayout.FloatField("Voxel Size", voxelizer.voxelSize);
                    break;
                case VoxelSizeType.RELATIVE:
                    voxelizer.voxelDensityType =
                        (VoxelDensityType)EditorGUILayout.EnumPopup("Density Type", voxelizer.voxelDensityType);
                    voxelizer.voxelDensity = EditorGUILayout.IntSlider("Voxel Density", voxelizer.voxelDensity, 1, 100);
                    break;
            }
            
            voxelizer.voxelTransformBakeType = (VoxelTransformBakeType)EditorGUILayout.EnumPopup("Voxel Bake Transform", voxelizer.voxelTransformBakeType);

            if (voxelizer.voxelTransformBakeType == VoxelTransformBakeType.NONE)
            {
                voxelizer.enableVoxelCache = EditorGUILayout.Toggle("Enable Voxel Cache", voxelizer.enableVoxelCache);
            }
            else
            {
                GUI.enabled = false;
                voxelizer.enableVoxelCache = false;
                voxelizer.enableVoxelCache = EditorGUILayout.Toggle("Enable Voxel Cache", voxelizer.enableVoxelCache);
                GUI.enabled = true;
            }

            voxelizer.enableColorSampling = EditorGUILayout.Toggle("Enable Color Sampling", voxelizer.enableColorSampling);
        }

        public void DrawAdditionalSection()
        {
            var voxelizer = (target as Voxelizer);

            if (!GUIUtils.DrawMinimizableSectionTitle("ADDITIONAL SETTINGS", ref voxelizer.additionalSectionMinimized))
                return;
            
            voxelizer.generateMesh = EditorGUILayout.Toggle("Generate Unity Mesh", voxelizer.generateMesh);
        }
    }
}