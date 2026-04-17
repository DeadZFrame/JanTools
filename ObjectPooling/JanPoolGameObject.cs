using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Jan.Pool
{
    public static partial class JanPool
    {
        private static readonly Dictionary<string, Queue<GameObject>> GameObjectPools = new();

        private static void CreatePool(GameObject prefab) 
        {
            if (GameObjectPools.ContainsKey(prefab.name))
            {
                Debug.LogWarning($"Pool with key {prefab.name} already exists.");
                return;
            }

            GameObjectPools.Add(prefab.name, new Queue<GameObject>());
            GameObjectPools[prefab.name].Enqueue(CreateFunc(prefab));
        }
        
        public static T Spawn<T>(this T poolable, Transform parent = null) where T : MonoBehaviour
        {
            if (poolable == null)           
            {
                Debug.LogError("Poolable object must be a MonoBehaviour.");
                return default;
            }

            if (Pools.TryGetValue(poolable.name, out var pool))
            {
                if(!pool.TryDequeue(out var obj))
                {
                    obj = CreateFunc(poolable);
                }

                obj.transform.SetParent(parent);
                obj.gameObject.SetActive(true);

                return obj.GetComponent<T>();
            }
            else
            {
                CreatePool(poolable);
                
                var createdObj = Pools[poolable.name].Dequeue();

                createdObj.transform.SetParent(parent);
                createdObj.gameObject.SetActive(true);

                return createdObj.GetComponent<T>();
            }
        }

        public static T[] Spawn<T>(this T poolable, int count, Transform parent = null) where T : MonoBehaviour
        {
            if (poolable == null)           
            {
                return default;
            }

            T[] result = new T[count];

            for (int i = 0; i < count; i++)
             {
                if (!Pools.ContainsKey(poolable.name))
                {
                    CreatePool(poolable);
                }

                if(!Pools[poolable.name].TryDequeue(out var obj))
                {
                    obj = CreateFunc(poolable);
                }

                obj.transform.SetParent(parent);
                obj.gameObject.SetActive(true);

                result[i] = obj.GetComponent<T>();
             }
             
             return result;
        }

        public static T Spawn<T>(this T poolable, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour
        {
            var obj = Spawn(poolable, parent);
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        public static void Despawn(this MonoBehaviour poolable)
        {
            if (poolable == null)
            {
                Debug.LogError("Poolable object must be a MonoBehaviour.");
                return;
            }

            if (Pools.TryGetValue(poolable.name.Replace("(Clone)", ""), out var pool))
            {
                poolable.gameObject.SetActive(false);
                pool.Enqueue(poolable);
            }
            else
            {
                Debug.LogWarning($"Pool with key {poolable.name.Replace("(Clone)", "")} does not exist.");
            }
        }

        private static MonoBehaviour CreateFunc(MonoBehaviour prefab)
        {
            var poolable = UnityEngine.Object.Instantiate(prefab);
            return poolable;
        }

        public static void Dispose(this JanPoolAgent _)
        {
            Pools.Clear();
            GameObjectPools.Clear();
        }
    }
}