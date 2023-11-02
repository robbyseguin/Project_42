using Entities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Entity), true)]
    public class EntityEditor : UnityEditor.Editor
    {
        private Vector3 _position = Vector3.zero;
        
        public override void OnInspectorGUI()
        {
            Entity entity = target as Entity;

            if(entity == null) return;

            //InterfaceEditor.DrawIDamageable(entity,ref _hitPoint);
            InterfaceEditor.DrawIWarpable(entity, ref _position);

            GUIEditorUtility.DrawTitle("Entity");
            EditorGUILayout.LabelField("Player GUID", Entity.PlayerGuid.ToString());
            base.OnInspectorGUI();
        }
    }
}