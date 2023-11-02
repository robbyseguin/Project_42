using UnityEngine;

namespace Utility
{
    public static class ArrayUtility
    {
        public static T RandomIndex<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
        
        public static T RandomIndex<T>(this T[] array, System.Random randomEngine)
        {
            return array[randomEngine.Next(array.Length)];
        }
    }
}