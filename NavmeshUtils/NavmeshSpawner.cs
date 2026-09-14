using Jan.Core;
using Jan.Maths;
using Jan.Pool;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Jan.Navigation
{
    public class NavmeshSpawner : Singleton<NavmeshSpawner>
    {
        [SerializeField] private WeightedRNGItem<NavmeshItem>[] navmeshItems;
        [SerializeField] private float spawnRadius = 5f;
        [SerializeField] private int spawnCount;

        public void SpawnNavmeshItems()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var item = WeightedRNG.Instance.GetRandomItem(navmeshItems);
                var position = NavMeshUtils.GetRandomPointInCircle(transform.position, spawnRadius, item.NavmeshArea);
                JanPool.Spawn(item, position, quaternion.identity);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, spawnRadius);
        }
        #endif
    }
}