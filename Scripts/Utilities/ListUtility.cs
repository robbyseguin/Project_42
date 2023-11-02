using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class ListUtility
    {
        public static T RandomIndex<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        
        public static T RandomIndex<T>(this List<T> list, System.Random randomEngine)
        {
            return list[randomEngine.Next(list.Count)];
        }
        
        public static T RandomIndexRemove<T>(this List<T> list)
        {
            int index = Random.Range(0, list.Count);
            T t = list[index];
            list.RemoveAt(index);

            return t;
        }
    }
}