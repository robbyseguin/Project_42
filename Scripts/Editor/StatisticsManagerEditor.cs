using System;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(StatisticsManager))]
    public class StatisticsManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            StatisticsManager sm = target as StatisticsManager;
            
            if(sm == null) 
                return;

            string[] _names = Enum.GetNames(typeof(StatisticsManager.GameStatistic));

            GUIEditorUtility.DrawTitle("Statistics");
            
            for (int i = 0; i < _names.Length - 1; i++)
            {
                EditorGUILayout.LabelField(_names[i], Application.isPlaying? sm._sessionStatistics[i].ToString(): "Not playing");
            }
        }
    }
}