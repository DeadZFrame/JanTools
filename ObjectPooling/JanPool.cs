using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Jan.Pool
{
    public static partial class JanPool
    {
        private static readonly Dictionary<string, Queue<MonoBehaviour>> Pools = new();

        private static void CreatePool(MonoBehaviour prefab) 
        {
            if (Pools.ContainsKey(prefab.name))
            {
                Debug.LogWarning($"Pool with key {prefab.name} already exists.");
                return;
            }

            Pools.Add(prefab.name, new Queue<MonoBehaviour>());
            Pools[prefab.name].Enqueue(CreateFunc(prefab));
        }
        
        public static GameObject Spawn(this GameObject poolable, Transform parent = null)
        {
            if (poolable == null)           
            {
                Debug.LogError("Poolable object must be a GameObject.");
                return default;
            }

            if (GameObjectPools.TryGetValue(poolable.name, out var pool))
            {
                Debug.Log($"Spawning from pool: {poolable.name}, Pool Count: {pool.Count}");

                if(!pool.TryDequeue(out var obj))
                {
                    Debug.Log($"Pool with key {poolable.name} is empty. Instantiating new object.");
                    obj = CreateFunc(poolable);
                }

                obj.transform.SetParent(parent);
                obj.SetActive(true);

                return obj;
            }
            else
            {
                Debug.Log($"Pool with key {poolable.name} does not exist. Creating new pool.");
                CreatePool(poolable);
                
                var createdObj = GameObjectPools[poolable.name].Dequeue();

                createdObj.transform.SetParent(parent);
                createdObj.SetActive(true);

                return createdObj;
            }
        }

        public static GameObject[] Spawn(this GameObject poolable, int count, Transform parent = null)
        {
            if (poolable == null)           
            {
                Debug.LogError("Poolable object must be a GameObject.");
                return default;
            }

            GameObject[] result = new GameObject[count];

            for (int i = 0; i < count; i++)
             {
                if (!GameObjectPools.ContainsKey(poolable.name))
                {
                    CreatePool(poolable);
                }

                if(!GameObjectPools[poolable.name].TryDequeue(out var obj))
                {
                    Debug.Log($"Pool with key {poolable.name} is empty. Instantiating new object.");
                    obj = CreateFunc(poolable);
                }

                obj.transform.SetParent(parent);
                obj.SetActive(true);

                result[i] = obj;
             }
             
             return result;
        }

        public static GameObject Spawn(this GameObject poolable, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = Spawn(poolable, parent);
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        public static void Despawn(this GameObject poolable)
        {
            if (poolable == null)
            {
                Debug.LogError("Poolable object must be a GameObject.");
                return;
            }

            if (GameObjectPools.TryGetValue(poolable.name.Replace("(Clone)", ""), out var pool))
            {
                Debug.Log($"Despawning to pool: {poolable.name}, Pool Count Before: {pool.Count}");
                poolable.SetActive(false);
                pool.Enqueue(poolable);
            }
            else
            {
                CreatePool(poolable);
                poolable.SetActive(false);
            }
        }

        private static GameObject CreateFunc(GameObject prefab)
        {
            var poolable = UnityEngine.Object.Instantiate(prefab);
            return poolable;
        }
    }
}