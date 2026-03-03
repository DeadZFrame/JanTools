using System;

namespace Jan.Core
{
    public static class ArrayExtensions
    {
        
        /// <summary>
        /// Checks whether the specified <paramref name="index"/> is within the bounds of <paramref name="array"/>
        /// and returns the element at that index via the <paramref name="item"/> out parameter when valid.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The array to validate against.</param>
        /// <param name="index">The index to check.</param>
        /// <param name="item">When the method returns true, this out parameter contains the element at <paramref name="index"/>; otherwise the default value.</param>
        /// <returns>
        /// True when <paramref name="index"/> is within [0, array.Length) and the element at that index is not null; otherwise false.
        /// </returns>
        /// <remarks>
        /// Note: For value types T the null check will always be false, so the method effectively only validates the index for value types.
        /// </remarks>
        public static bool IsIndexValid<T>(this T[] array, int index, out T item)
        {
            item = default;
            if (index < 0 || index >= array.Length) return false;

            item = array[index];
            return item != null;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="array"/> contains the
        /// given <paramref name="item"/> using <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <typeparam name="T">Element type of the array.</typeparam>
        /// <param name="array">Array to search.</param>
        /// <param name="item">Item to locate in the array.</param>
        /// <returns>True if an element equal to <paramref name="item"/> is found; otherwise false.</returns>
        /// <remarks>Null elements in the array may cause <see cref="NullReferenceException"/> when calling <c>Equals</c>.</remarks>
        public static bool Contains<T>(this T[] array, T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item)) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a random element from the array excluding the first occurrence of <paramref name="except"/>.
        /// If <paramref name="except"/> is found the method removes that occurrence locally and then returns a random item
        /// from the remaining elements.
        /// </summary>
        /// <typeparam name="T">Element type of the array.</typeparam>
        /// <param name="array">Source array to pick from.</param>
        /// <param name="except">Element to exclude from selection.</param>
        /// <returns>A random element from the array that is not equal to <paramref name="except"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the array is empty.</exception>
        /// <remarks>Note: this method modifies the local reference of <paramref name="array"/>; the original caller's array reference is not modified.</remarks>
        public static T RandomItemExcept<T>(this T[] array, T except)
        {
            if (array.Length == 0)
                throw new IndexOutOfRangeException("Array is Empty");

            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                if (item.Equals(except))
                {
                    array[i] = array[array.Length - 1];
                    Array.Resize(ref array, array.Length - 1);
                    break;
                }
            }

            return array.RandomItem();
        }

        /// <summary>
        /// Returns a random element from <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">Element type of the array.</typeparam>
        /// <param name="array">Source array to pick from.</param>
        /// <returns>A randomly selected element from the array.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the array is empty.</exception>
        public static T RandomItem<T>(this T[] array)
        {
            if (array.Length == 0)
                throw new IndexOutOfRangeException("Array is Empty");

            var randomIndex = UnityEngine.Random.Range(0, array.Length);
            return array[randomIndex];
        }

        /// <summary>
        /// Compares two arrays for element-wise equality using <c>Equals</c>.
        /// </summary>
        /// <typeparam name="T">Element type of the arrays.</typeparam>
        /// <param name="array">First array to compare.</param>
        /// <param name="other">Second array to compare.</param>
        /// <returns>True if both arrays have the same length and each corresponding element is equal; otherwise false.</returns>
        public static bool AreEqual<T>(this T[] array, T[] other)
        {
            if (array.Length != other.Length) return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i].Equals(other[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether <paramref name="array"/> contains every element in <paramref name="others"/>.
        /// </summary>
        /// <typeparam name="T">Element type of the arrays.</typeparam>
        /// <param name="array">Array to check for the presence of elements.</param>
        /// <param name="others">Elements that must all be present in <paramref name="array"/>.</param>
        /// <returns>True if all elements in <paramref name="others"/> are found in <paramref name="array"/>; otherwise false.</returns>
        public static bool ContainsThese<T>(this T[] array, T[] others)
        {
            for (int i = 0; i < others.Length; i++)
            {
                if (!array.Contains(others[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to find the first element in <paramref name="list"/> that satisfies the provided <paramref name="match"/> predicate.
        /// </summary>
        /// <typeparam name="T">Element type of the list.</typeparam>
        /// <param name="list">Array to search.</param>
        /// <param name="match">Predicate used to test elements.</param>
        /// <param name="item">If found, the matched element is assigned to this out parameter.</param>
        /// <returns>True if a matching element was found; otherwise false.</returns>
        /// <remarks>Null entries in <paramref name="list"/> are skipped.</remarks>
        public static bool TryGetMatch<T>(this T[] list, Predicate<T> match, out T item)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var currentItem = list[i];
                if(currentItem == null) continue;
                if (match(currentItem))
                {
                    item = currentItem;
                    return true;
                }
            }
            
            item = default;
            return false;
        }

        /// <summary>
        /// Attempts to find the first element in <paramref name="list"/> that satisfies the provided <paramref name="match"/> predicate.
        /// </summary>
        /// <typeparam name="T">Element type of the list.</typeparam>
        /// <param name="list">Array to search.</param>
        /// <param name="match">Predicate used to test elements.</param>
        /// <returns>True if a matching element was found; otherwise false.</returns>
        /// <remarks>Null entries in <paramref name="list"/> are skipped.</remarks>
        public static bool Exists<T>(this T[] list, Predicate<T> match)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var currentItem = list[i];
                if (currentItem == null) continue;
                if (match(currentItem))
                {
                    return true;
                }
            }

            return false;
        }
    }
}