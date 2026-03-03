using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jan.Maths
{
    // Define a struct for items and their weights
    /// <summary>
    /// Represents a weighted item for use in random selection processes.
    /// </summary>
    /// <typeparam name="T">The type of the item to be weighted and selected.</typeparam>
    /// <remarks>
    /// This structure is commonly used in scenarios where multiple items have associated weights,
    /// and one item needs to be randomly selected based on probability determined by the weights.
    /// Weights should be non-negative values, and higher weights increase the likelihood of selection.
    /// </remarks>
    [Serializable]
    public struct WeightedRNGItem<T>
    {
        /// <summary>
        /// Represents the data item associated with a weighted random selection logic.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <remarks>
        /// This property is used as the primary value in a weighted random generator logic.
        /// The associated weight of the item determines the probability of its selection during randomization.
        /// </remarks>
        [field: SerializeField] public T Item { get; private set; }

        /// <summary>
        /// Represents the weight of an item in a weighted random number generator system.
        /// </summary>
        /// <remarks>
        /// The weight determines the likelihood of selecting a specific item.
        /// Higher weights increase the probability of selection, while lower weights decrease it.
        /// </remarks>
        [field: Header("RNG Settings")]
        [field: Tooltip("The weight of the item in the weighted random selection process.")]
        [field: Range(0f, 100f), SerializeField] public float Weight { get; private set; }
        [field: Tooltip("Indicates whether the item has a limit on the number of selections.")]
        [field: SerializeField, FormerlySerializedAs("LimitMaxSpawns")] public bool LimitMaxSelections { get; private set; }
        [field: Tooltip("The maximum number of times the item can be selected if a limit is imposed.")]
        [field: Range(0, 1000), SerializeField, ShowIf(nameof(LimitMaxSelections)), FormerlySerializedAs("MaxSpawns")] public int MaxSelections { get; private set; }

        /// <summary>
        /// Represents an item with an associated weight that can be used in randomized selection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the item being represented.
        /// </typeparam>
        /// <remarks>
        /// Items with higher weights have a greater chance of being selected during the randomization process.
        /// The weight value is expected to be a non-negative floating point number.
        /// </remarks>
        public WeightedRNGItem(T item, float weight, int maxSelections)
        {
            Item = item;
            Weight = weight;
            MaxSelections = maxSelections;
            LimitMaxSelections = false;
        }
    }
}
