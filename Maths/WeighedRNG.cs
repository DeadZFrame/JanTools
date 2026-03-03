using System.Collections.Generic;
using System.Linq;
using Jan.Core;
using UnityEngine;

namespace Jan.Maths
{
    /// <summary>
    /// A static utility class for selecting a random item from a collection of weighted items.
    /// This class is used to perform weighted random selection, where each item's probability
    /// of being chosen is determined by its weight.
    /// </summary>
    public class WeightedRNG : Singleton<WeightedRNG>
    {
        [SerializeField] private bool debugMode = false;
        private readonly Dictionary<object, int> _spawnedObjects = new Dictionary<object, int>();

        /// <summary>
        /// Selects a random item from a collection of weighted items based on their weights.
        /// </summary>
        /// <typeparam name="T">The type of items in the weighted list.</typeparam>
        /// <param name="items">An array of items with associated weights.</param>
        /// <returns>Returns a randomly selected item of type T, or the default value of T if no item is selected.</returns>
        public T GetRandomItem<T>(WeightedRNGItem<T>[] items, T excludedItem = default)
        {
            // Calculate total weight
            float totalWeight = 0;
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (item.Item.Equals(excludedItem)) continue;

                if (_spawnedObjects.TryGetValue(item.Item, out int count) && item.LimitMaxSelections)
                {
                    if (count >= item.MaxSelections) continue;
                }

                totalWeight += item.Weight;
            }

            // Generate a random number between 0 and total weight
            float randomValue = UnityEngine.Random.Range(0, totalWeight);

            // Select the item
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                var exists = _spawnedObjects.TryGetValue(item.Item, out int count) && item.LimitMaxSelections;

                if (exists)
                {
                    if (count >= item.MaxSelections)
                    {
                        if(debugMode) Debug.Log($"GetRandomItemExcept: Skipping {item.Item} as it has reached its max selections of {item.MaxSelections}, current count: {count}");
                        continue;
                    }
                }

                randomValue -= item.Weight;
                if (randomValue <= 0)
                {
                    if (exists)
                    {
                        count++;
                        _spawnedObjects[item.Item] = count;
                    }
                    else if (item.LimitMaxSelections) _spawnedObjects.Add(item.Item, 1);

                    return item.Item;
                }
            }

            // Fallback in case something goes wrong
            return default;
        }

        public T GetRandomItem<T>(IWeightedRNG<T>[] items, T excludedItem = default)
        {
            WeightedRNGItem<T>[] weightedItems = new WeightedRNGItem<T>[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                weightedItems[i] = item.RNG;
            }

            return GetRandomItem(weightedItems, excludedItem);
        }

        /// <summary>
        /// Selects a random item from a collection of weighted items, excluding the specified item.
        /// </summary>
        /// <typeparam name="T">The type of items in the weighted list.</typeparam>
        /// <param name="items">An array of items with associated weights.</param>
        /// <param name="excludedItem">Item to exclude from the selection.</param>
        /// <returns>Returns a randomly selected item of type T, or the default value of T if no item is selected.</returns>
        public T GetRandomItemExcept<T>(WeightedRNGItem<T>[] items, T excludedItem)
        {
            // Calculate total weight
            float totalWeight = 0;
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                
                // Check if item should be excluded
                if (item.Item.Equals(excludedItem)) continue;

                if(_spawnedObjects.TryGetValue(item.Item, out int count) && item.LimitMaxSelections)
                {
                    if(count >= item.MaxSelections) continue;
                }
                
                totalWeight += item.Weight;
            }

            // Generate a random number between 0 and total weight
            float randomValue = UnityEngine.Random.Range(0, totalWeight);

            // Select the item
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                
                // Check if item should be excluded
                if (item.Item.Equals(excludedItem)) continue;
                
                var exists = _spawnedObjects.TryGetValue(item.Item, out int count) && item.LimitMaxSelections;
                if(exists)
                {
                    if (count >= item.MaxSelections)
                    {
                        if(debugMode) Debug.Log($"GetRandomItemExcept: Skipping {item.Item} as it has reached its max selections of {item.MaxSelections}, current count: {count}");
                        continue;
                    }   
                }

                randomValue -= item.Weight;
                if (randomValue <= 0)
                {
                    if(exists)
                    {
                        count++;
                        _spawnedObjects[item.Item] = count;
                    }                    
                    else if(item.LimitMaxSelections) _spawnedObjects.Add(item.Item, 1);

                    return item.Item;
                }
            }

            // Fallback in case something goes wrong
            return default;
        }

        /// <summary>
        /// Selects a random item from a collection of weighted items, excluding the specified item.
        /// </summary>
        /// <typeparam name="T">The type of items in the weighted list.</typeparam>
        /// <param name="items">An array of items implementing IWeightedRNG with associated weights.</param>
        /// <param name="excludedItem">Item to exclude from the selection.</param>
        /// <returns>Returns a randomly selected item of type T, or the default value of T if no item is selected.</returns>
        public T GetRandomItemExcept<T>(IWeightedRNG<T>[] items, T excludedItem)
        {
            WeightedRNGItem<T>[] weightedItems = new WeightedRNGItem<T>[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                weightedItems[i] = item.RNG;
            }

            return GetRandomItemExcept(weightedItems, excludedItem);
        }

        /// <summary>
        /// Despawns a MonoBehaviour object and updates the spawn count tracking.
        /// </summary>
        /// <typeparam name="T">Type of MonoBehaviour to despawn</typeparam>
        /// <param name="obj">The object to despawn</param>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>// <summary>
        public void Despawn<T>(T obj) where T : MonoBehaviour
        {
            foreach (var item in _spawnedObjects.ToArray())
            {
                if (item.Key is T)
                {
                    if (item.Value <= 0)
                    {
                        _spawnedObjects.Remove(item.Key);
                    }
                    else _spawnedObjects[item.Key] = item.Value - 1;
                }
            }

            //LeanPool.Despawn(obj);
        }
    }
}