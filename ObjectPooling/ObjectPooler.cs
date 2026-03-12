// using System;
// using UnityEngine.Pool;

// namespace Jan.Pool
// {
//     public static class ObjectPooler
//     {
//         private static readonly Dictionary<string, IObjectPool> pools = new Dictionary<string, IObjectPool>();

//         public static void CreatePool<T>(string poolName, Func<T> objectGenerator, int initialSize = 0) where T : IPoolable
//         {
//             if (!pools.ContainsKey(poolName))
//             {
//                 var pool = new ObjectPool<T>(objectGenerator, initialSize);
//                 pools.Add(poolName, pool);
//             }
//         }

//         public static T GetFromPool<T>(string poolName) where T : IPoolable
//         {
//             if (pools.TryGetValue(poolName, out var pool))
//             {
//                 return ((ObjectPool<T>)pool).Get();
//             }
//             throw new Exception($"Pool with name {poolName} does not exist.");
//         }

//         public static void ReturnToPool<T>(string poolName, T obj) where T : IPoolable
//         {
//             if (pools.TryGetValue(poolName, out var pool))
//             {
//                 ((ObjectPool<T>)pool).Return(obj);
//             }
//             else
//             {
//                 throw new Exception($"Pool with name {poolName} does not exist.");
//             }
//         }
//     }
// }