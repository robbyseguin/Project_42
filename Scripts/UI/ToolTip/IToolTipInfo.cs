using UnityEngine;

namespace UI
{
    public interface IToolTipInfo
    {
        public bool UpdateAction => false;
        public Color MainColor => Color.gray;
        public Sprite Icon => default;
        public string Name { get; }
        public string Description => default;
        public string Action => default;
        public string[] Info => default;
        public Sprite ImageListOverlay => default;
        public Color ImageListOverlayColor => Color.white;
        public Sprite[] ImageList => default;
        public float Loading => 0.0f;
    }
}
