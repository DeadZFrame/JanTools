using System;
using Jan.Tasks;
using UnityEngine;

namespace Jan.Maths
{
    /// <summary>
    /// Provides extension methods for performing various mathematical and utility operations.
    /// </summary>
    public static class ValueExtensions
    {
        /// <summary>
        /// Returns a randomized float value within a specified range, clamped by a minimum and maximum.
        /// </summary>
        /// <param name="value">The base value to randomize.</param>
        /// <param name="range">The range within which to randomize the value.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>A randomized float within the range of [value - range, value + range], clamped to [min, max].</returns>
        public static float Randomize(this float value, float range, float min, float max)
        {
            float randomizedValue = UnityEngine.Random.Range(value - range, value + range);
            return Mathf.Clamp(randomizedValue, min, max);
        }

        /// <summary>
        /// Returns a random integer from an array of integers.
        /// </summary>
        /// <param name="values">An array of integers to select a random value from.</param>
        /// <returns>A randomly selected integer from the provided array.</returns>
        public static int GetRandomFromValues(params int[] values)
        {
            var rand = UnityEngine.Random.Range(0, values.Length);
            return values[rand];
        }

        /// <summary>
        /// Determines if a given value is within the specified range defined by two values.
        /// </summary>
        /// <param name="value">The value to check if it's within the range.</param>
        /// <param name="x">The lower bound of the range.</param>
        /// <param name="y">The upper bound of the range.</param>
        /// <returns>True if the value is greater than x and less than y; otherwise, false.</returns>
        public static bool IsValueInRange(this float value, float x, float y) => value < x && value > y;

        public static int ToInt(this float value)
        {
            return Mathf.RoundToInt(value);
        }
    }
}