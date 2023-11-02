using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
    public abstract class GenericPool
    {
        public abstract void ReturnObject(MonoBehaviour obj);

        public virtual T GetObject<T>() where T : MonoBehaviour, IPoolable
        {
            return default;
        }
    }
    
    public class GenericPool<T> : GenericPool where T : MonoBehaviour, IPoolable
    {
        private int _timeOverflown = 0;
        private string _name;
        private Transform _poolFolder { get; }
        private T _prefab;
        
        private T[] _pool;
        private int _atPool;

        public GenericPool(T prefab, int poolSize)
        {
            _prefab = prefab;
            _name = typeof(T).Name + "_" + prefab.name + "_Pool";
            _poolFolder = new GameObject(_name).transform;
            
            _pool = new T[poolSize];
            _atPool = 0;
            
            for (int i = 0; i < poolSize; i++)
                _pool[i] = Object.Instantiate(_prefab, _poolFolder);
        
            _poolFolder.SetParent(GenericPoolHelper.PoolFolder, false);
        }
        
        public T GetObject()
        {
            if (_atPool >= _pool.Length)
            {
                ++_timeOverflown;
                
#if UNITY_EDITOR
                Debug.LogWarning(
                    "Pool \"" + _name + "\" has overflown for " + _timeOverflown + " time.\n " + 
                    "Solution: Change the initial amount of " + _pool.Length + " for a bigger number."
                    );
#endif
                
                return Object.Instantiate(_prefab, null);
            }
            
            if (_pool[_atPool].IsDestroyed())
            {
#if UNITY_EDITOR
                Debug.LogError(
                    "Pool \"" + _name + "\" has a destroyed object inside the pool.\n " + 
                    "This should not be happening."
                );
#endif
                
                //_pool[_atPool] = Object.Instantiate(_prefab, _poolFolder);
            }
            
            this.AddCheckedOutObject(_pool[_atPool]);
            
            _pool[_atPool].transform.SetParent(null,false);
            return _pool[_atPool++];
        }
        
        public override TV GetObject<TV>()
        {
            return GetObject() as TV;
        }

        public override void ReturnObject(MonoBehaviour obj)
        {
            ReturnObject(obj as T);
        }
        
        public void ReturnObject(T obj)
        {
            if(obj.IsDestroyed())
                return;

            if (_pool.Contains(obj) && obj.transform.parent != _poolFolder)
            {
                obj.transform.SetParent(_poolFolder,false);
                --_atPool;

                for (int i = 0; i <= _atPool; i++)
                {
                    if (_pool[i].GetInstanceID() != obj.GetInstanceID()) 
                        continue;
                    
                    (_pool[i], _pool[_atPool]) = (_pool[_atPool], _pool[i]);
                    break;
                }
                
                return;
            }
            
            Object.Destroy(obj.gameObject);
        }
    }
}
