using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Jan.Pool
{
    public static class JanPool
    {
        private static readonly Dictionary<Type, ObjectPool<IPoolable>> Pools = new();

        private static void CreatePool(MonoBehaviour prefab, int initialSize = 10) 
        {
            if (Pools.ContainsKey(prefab.GetType()))
            {
                Debug.LogWarning($"Pool with key {prefab.GetType()} already exists.");
                return;
            }

            var pool = new ObjectPool<IPoolable>(
                createFunc: () => CreateFunc(prefab),
                actionOnGet: obj => { },
                actionOnRelease: obj => { },
                actionOnDestroy: obj => UnityEngine.Object.Destroy(obj as UnityEngine.Object),
                collectionCheck: false,
                defaultCapacity: initialSize,
                maxSize: 100
            );
        
            Pools[prefab.GetType()] = pool;
        }
        
        public static T Spawn<T>(this T poolable, Transform parent = null) where T : IPoolable
        {
            var mono = poolable as MonoBehaviour;
            if (mono == null)           
            {
                Debug.LogError("Poolable object must be a MonoBehaviour.");
                return default;
            }

             if (Pools.TryGetValue(mono.GetType(), out var pool))
            {
                var obj = pool.Get();
                var objMono = obj as MonoBehaviour;
                objMono.transform.SetParent(parent);
                objMono.gameObject.SetActive(true);

                return (T)obj;
            }
            else
            {
                CreatePool(mono);
                var createdObj = Pools[mono.GetType()].Get();
                var createdMono = createdObj as MonoBehaviour;
                createdMono.transform.SetParent(parent);
                createdMono.gameObject.SetActive(true);

                return (T)createdObj;
            }
        }

        public static T[] Spawn<T>(this T poolable, int count, Transform parent = null) where T : IPoolable
        {
            var mono = poolable as MonoBehaviour;
            if (mono == null)           
            {
                Debug.LogError("Poolable object must be a MonoBehaviour.");
                return default;
            }

            T[] result = new T[count];

            for (int i = 0; i < count; i++)
             {
                if (!Pools.ContainsKey(mono.GetType()))
                {
                    CreatePool(mono);
                }

                var obj = Pools[mono.GetType()].Get();

                var objMono = obj as MonoBehaviour;
                objMono.transform.SetParent(parent);
                objMono.gameObject.SetActive(true);

                result[i] = (T)obj;
             }
             
             return result;
        }

        public static void Despawn<T>(this T poolable) where T : IPoolable
        {
            var poolKey = poolable as MonoBehaviour;
            if (poolKey == null)
            {
                Debug.LogError("Poolable object must be a MonoBehaviour.");
                return;
            }

             if (Pools.TryGetValue(poolKey.GetType(), out var pool))
            {
                poolKey.gameObject.SetActive(false);
                pool.Release(poolable);
            }
            else
            {
                Debug.LogWarning($"Pool with key {poolKey.GetType()} does not exist.");
            }
        }

        private static IPoolable CreateFunc(MonoBehaviour prefab)
        {
            var poolable = UnityEngine.Object.Instantiate(prefab).GetComponent<IPoolable>();
            return poolable;
        }

        public static void Dispose(this JanPoolAgent _)
        {
            foreach (var pool in Pools.Values)
            {
                pool.Dispose();
            }

            Pools.Clear();
        }
    }
}