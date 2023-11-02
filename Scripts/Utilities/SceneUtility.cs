using UnityEngine;

namespace Utility
{
    public static class SceneUtility
    {
        public static Transform CreateGameobjectFolder(string name, Transform parent = null)
        {
            Transform t = new GameObject(name).transform;
            t.parent = parent;

            return t;
        }
    }
}