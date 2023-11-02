using UnityEngine;
using Utility;

namespace Datas
{
    [CreateAssetMenu(menuName = "Project 42/New Materials definition", fileName = "MD_Default")]
    public class MaterialsDefinition : ScriptableObject
    {
        [SerializeField] private Material[] _materials;
        public Material Material => _materials.RandomIndex();
        public Material[] MaterialsArray => _materials;
    }
}