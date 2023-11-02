using UnityEngine;

namespace Managers
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T s_instance;
        private bool _isSingleton = false;
        
        public static T Instance 
        {
            get
            {
                if (!Exists)
                    s_instance = CreateSingleton();
                
                return s_instance;
            }
        }

        public bool IsSingleton => _isSingleton;
        public static bool Exists => s_instance != null;
        
        protected virtual void Awake()
        {
            if (Exists && !_isSingleton)
            {
#if UNITY_EDITOR
                Debug.LogWarning(
                    "Two singleton of type " + typeof(T).Name + " detected, deleting the second one.\n " +
                    "Click to focus the concerned object.", 
                    gameObject
                    );
#endif
                Destroy(this);
                return;
            }

            s_instance = this as T;
            _isSingleton = true;
        }

        private static T CreateSingleton()
        {
            return new GameObject(typeof(T).Name).AddComponent<T>();
        }
        
        public static void DestroySingleton()
        {
            if (Exists)
            {
                Destroy(s_instance);
                s_instance = null;
            }
        }
    }
}
