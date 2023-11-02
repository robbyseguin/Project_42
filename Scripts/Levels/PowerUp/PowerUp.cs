using Managers;
using UnityEngine;

namespace Levels.Interactable
{
    public abstract class PowerUp : MonoBehaviour, ILootable
    {
        [SerializeField] private Sprite _icon;

        public Sprite Icon => _icon;
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual Color MainColor => default;
        public virtual string[] Info => default;

        public abstract void OnDrop();
        public abstract bool OnPickUp(GameObject target);

        protected string GetLocalizedString(string fr, string en) => LocalizationManager.GetLocalizedString(fr, en);
    }
}