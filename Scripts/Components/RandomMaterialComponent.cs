using Datas;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Components
{
    public class RandomMaterialComponent : MonoBehaviour
    {
        [SerializeField] private MaterialsDefinition _materialsDefinition;
        [SerializeField] private Renderer[] _meshRenderers;

        private void OnEnable()
        {
            Material mat = _materialsDefinition.Material;
            
            foreach (Renderer renderer in _meshRenderers)
                renderer.material = mat;
        }
#if UNITY_EDITOR
        [CustomEditor(typeof(RandomMaterialComponent))]
        private class RandomMaterialComponentEditor: Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                
                RandomMaterialComponent RMC = target as RandomMaterialComponent;

                if (GUILayout.Button("Get Renderers"))
                    RMC._meshRenderers = RMC.GetComponentsInChildren<Renderer>();

                if (GUILayout.Button("Randomize material"))
                    RMC.OnEnable();
            }
        }
        
#endif
    }
}