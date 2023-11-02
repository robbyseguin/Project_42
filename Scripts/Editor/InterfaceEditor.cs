using Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class InterfaceEditor
    {
        public static void DrawIDamageable(IDamageable target, ref int hitPoint)
        {
            GUIEditorUtility.DrawTitle("IDamageable (Only in Playmode)");

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            {
                EditorGUILayout.LabelField("Health", Application.isPlaying? target.Health.ToString():"Not playing");
                hitPoint = EditorGUILayout.IntField("Hit Point", hitPoint);
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Heal"))
                        target.Heal(hitPoint);

                    if (GUILayout.Button("Damage"))
                        target.TakeDamage(hitPoint);
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Kill"))
                    target.TakeDamage(target.Health);
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawIWarpable(IWarpable target, ref Vector3 position)
        {
            GUIEditorUtility.DrawTitle("IWarpable (Only in Playmode)");
            
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            {
                position = EditorGUILayout.Vector3Field("Target", position);
                
                if(GUILayout.Button("Warp"))
                        target.WarpTo(position,quaternion.identity);
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawInteractable(IInteractable target)
        {
            GUIEditorUtility.DrawTitle("IInteractable (Only in Playmode)");
            
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Perform"))
                        target.InteractionStarted();

                    if (GUILayout.Button("Cancel"))
                        target.InteractionCanceled();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}