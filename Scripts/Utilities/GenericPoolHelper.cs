using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class GenericPoolHelper
    {
        public static Transform PoolFolder { get; }
        private static Dictionary<int, CheckedOutItem> _checkedOutObjects = new Dictionary<int, CheckedOutItem>();

        static GenericPoolHelper()
        {
            PoolFolder = new GameObject("Pools").transform;
            GameObject.DontDestroyOnLoad(PoolFolder);
            PoolFolder.gameObject.SetActive(false);
        }

        public static void Destroy<T>(this T obj) where T : IPoolable
        {
            MonoBehaviour mb = obj as MonoBehaviour;
            
            if (!mb)
                return;

            if (!_checkedOutObjects.TryGetValue(mb.GetInstanceID(), out CheckedOutItem item))
            {
                Object.Destroy(mb.gameObject);
                return;
            }

            item.Return();
        }

        internal static void AddCheckedOutObject(this GenericPool pool, MonoBehaviour obj)
        {
            _checkedOutObjects.TryAdd(obj.GetInstanceID(), new CheckedOutItem(pool, obj));
        }

        private struct CheckedOutItem
        {
            public GenericPool Pool;
            public MonoBehaviour Self;

            public CheckedOutItem(GenericPool pool, MonoBehaviour self)
            {
                Pool = pool;
                Self = self;
            }

            public void Return()
            {
                Pool.ReturnObject(Self);
            }
        }
    }
}