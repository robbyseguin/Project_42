using UI;
using UnityEngine;
using Utilities;

namespace Levels.Interactable
{
    public interface ILootable : IToolTipInfo, IPoolable
    {
        public bool OnPickUp(GameObject target);
        public void OnDrop();
    }
}