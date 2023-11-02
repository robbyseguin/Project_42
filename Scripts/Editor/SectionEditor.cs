using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Levels;
using Levels.Sections;
using Unity.AI.Navigation;
using Unity.AI.Navigation.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Section))]
    public class SectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Section section = target as Section;

            Dictionary<Type, int> c = new Dictionary<Type, int>();

            foreach (SectionComponent go in section.SectionComponents)
            {
                if(go == null)
                    continue;
                
                SectionComponent isc = go.GetComponent<SectionComponent>();
                
                if (!c.TryAdd(isc.GetType(), 1))
                    c[isc.GetType()]++;
            }
            
            GUIEditorUtility.DrawTitle("Section settings");
            EditorGUI.indentLevel++;
            base.OnInspectorGUI();
            EditorGUI.indentLevel--;

            GUIEditorUtility.DrawTitle("Portals");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Entries", section.Entries.Length.ToString());
            EditorGUILayout.LabelField("Exits", section.Exits.Length.ToString());
            EditorGUI.indentLevel--;
            
            GUIEditorUtility.DrawTitle("Section components (" + section.SectionComponents.Length + ")");
            EditorGUI.indentLevel++;
            foreach (var v in c)
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(v.Key.ToString(), new GUIStyle("Label")))
                        SelectSectionComponent(section.SectionComponents,v.Key.ToString());

                    GUILayout.Label(v.Value.ToString());
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
            
            GUIEditorUtility.DrawTitle("Nav Meshes");
            EditorGUI.indentLevel++;
            {
                foreach (NavMeshSurface surface in section.GetComponentsInChildren<NavMeshSurface>())
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(surface.gameObject.name, new GUIStyle("Label")))
                            Selection.objects = new[] { surface };

                        GUILayout.Label(surface.navMeshData.ToString());
                    }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
            bool IsPrefabOpen = PrefabStageUtility.GetPrefabStage(section.gameObject) != null;

            EditorGUI.BeginDisabledGroup(!IsPrefabOpen);
            {
                if (GUILayout.Button("Bake section"))
                {
                    UpdateSectionData(section);
                    EditorUtility.SetDirty(section);
                }
            }
            EditorGUI.EndDisabledGroup();

            if (!IsPrefabOpen)
            {
                EditorGUILayout.HelpBox("Open prefab to bake section", MessageType.Info);
            }

            if (section.Entries.Length <= 0 || section.Exits.Length <= 0)
            {
                EditorGUILayout.HelpBox("At least 1 entry and 1 exit must be present.", MessageType.Error);
            }
        }

        private void SelectSectionComponent(SectionComponent[] components, string name)
        {
            List<GameObject> list = new List<GameObject>();
            foreach (SectionComponent component in components)
            {
                if (component.GetType().ToString() == name)
                    list.Add(component.gameObject);
            }
            Selection.objects = list.ToArray();
        }
        
        private void OnSceneGUI()
        {
            Section section = target as Section;

            Handles.color = Color.green;
            foreach (Portal portal in section.Entries)
                Handles.DrawWireDisc(portal.transform.position,Vector3.up, 1, 2);
            
            Handles.color = Color.red;
            foreach (Portal portal in section.Exits)
                Handles.DrawWireDisc(portal.transform.position,Vector3.up, 1, 2);
        }

        private void UpdateSectionData(Section section)
        {
            section.BakeSection();
            foreach (NavMeshSurface surface in section.GetComponentsInChildren<NavMeshSurface>())
            {
                if(surface.navMeshData)
                    surface.UpdateNavMesh(surface.navMeshData);
                else 
                    NavMeshAssetManager.instance.StartBakingSurfaces(section.GetComponentsInChildren<NavMeshSurface>());
            }
        }
    }
}