using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class GUIEditorUtility
    {
        private static readonly Color InspectorBackgroundColor = new Color(56,56,56,255);
        private static readonly Color LineColor = Color.white;
        
        public static void DrawGUIRectBorder(Rect rect, float thickness, Color cBorder, Color cFill)
        {
            EditorGUI.DrawRect(rect, cBorder);
            rect.x += thickness;
            rect.y += thickness;
            rect.width -= thickness * 2;
            rect.height -= thickness * 2;
            EditorGUI.DrawRect(rect, cFill);
        }
    
        public static  void DrawGUIRectBorder(Rect rect, float thickness, Color cBorder)
        {
            DrawGUIRectBorder(rect,thickness,cBorder,InspectorBackgroundColor);
        }
    
        public static  void DrawGUIRectBorder(Rect rect, float thickness)
        {
            DrawGUIRectBorder(rect,thickness, LineColor);
        }
        
        public static  void DrawGUILine(int thickness, Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, thickness);
            rect.height = thickness;
            EditorGUI.DrawRect(rect, color);
        }
        
        public static  void DrawGUILine(int thickness)
        {
            DrawGUILine(thickness, LineColor);
        }

        public static void DrawTitle(string label)
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            DrawGUILine(2);
            GUILayout.Space(2);
        }
    }
}
