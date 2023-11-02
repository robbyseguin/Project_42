using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(menuName = "Project 42/New World Settings", fileName = "WS_Default")]
    public class WorldSettings : ScriptableObject
    {
        [SerializeField] private Color _duskColor;
        [SerializeField] private Color _dawnColor;
        [SerializeField] private float _timeScale = 0.5f;

        public Color DuskColor => _duskColor;
        public Color DawnColor => _dawnColor;
        public float TimeScale => _timeScale;
    }
}