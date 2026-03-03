using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jan.Core
{
    /// <summary>
    /// Provides extension methods for the List class to extend its functionality with operations such as random retrieval, modification, and manipulation.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns a random item from inside the specified list and removes it.
        /// </summary>
        /// <typeparam name="T">The type of items contained in the list.</typeparam>
        /// <param name="list">The list to retrieve and remove the item from.</param>
        /// <returns>The randomly selected and removed item.</returns>
        public static T RandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException("List is Empty");

            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static T RandomItemExcept<T>(this List<T> list, T except)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException("List is Empty");

            var filteredList = list.FindAll(item => !item.Equals(except));
            if (filteredList.Count == 0)
                throw new IndexOutOfRangeException("No items available except the specified one");

            return filteredList.RandomItem();
        }

        public static bool IsIndexValid<T>(this List<T> list, int index, out T item)
        {
            item = default;
            if (index < 0 || index >= list.Count) return false;

            item = list[index];
            return item != null;
        }

        public static bool IsIndexValid<T>(this IReadOnlyList<T> list, int index, out T item)
        {
            item = default;
            if (index < 0 || index >= list.Count) return false;

            item = list[index];
            return item != null;
        }

        /// <summary>
        /// Removes and returns a random item from inside the
        /// <typeparam name="T">List</typeparam>
        /// >
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list to remove a random item from.</param>
        /// <returns>The removed random item from the list.</returns>
        public static T RandomItemRemove<T>(this List<T> list)
        {
            var item = list.RandomItem();
            list.Remove(item);
            return item;
        }

        /// <summary>
        /// Shuffles the elements in the specified list in a random order using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            for (var i = 0; i <= n - 2; i++)
            {
                //random index
                var rdn = Random.Range(0, n - i);

                //swap positions
                (list[i], list[i + rdn]) = (list[i + rdn], list[i]);
            }
        }

        /// <summary>
        /// Adds an item at the beginning of the List.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the list.</typeparam>
        /// <param name="list">The list to which the item will be added.</param>
        /// <param name="item">The item to be added at the beginning of the list.</param>
        public static void AddToFront<T>(this List<T> list, T item) => list.Insert(0, item);

        /// <summary>
        /// Adds an item before a specified item in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list in which to add the new item.</param>
        /// <param name="item">The item before which the new item will be added.</param>
        /// <param name="newItem">The new item to be added to the list.</param>
        public static void AddBeforeOf<T>(this List<T> list, T item, T newItem)
        {
            var targetPosition = list.IndexOf(item);
            list.Insert(targetPosition, newItem);
        }

        /// <summary>
        /// Adds an item to the list immediately after the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list where the item will be added.</param>
        /// <param name="item">The existing item after which the new item will be added.</param>
        /// <param name="newItem">The item to add to the list.</param>
        public static void AddAfterOf<T>(this List<T> list, T item, T newItem)
        {
            var targetPosition = list.IndexOf(item) + 1;
            list.Insert(targetPosition, newItem);
        }

        /// <summary>
        /// Prints the list in the following format: [item[0], item[1], ... , item[Count-1]].
        /// </summary>
        /// <typeparam name="T">Type of elements in the list.</typeparam>
        /// <param name="list">The list to be printed.</param>
        /// <param name="log">Optional log message prefix.</param>
        public static void Print<T>(this List<T> list, string log = "")
        {
            log += "[";
            for (var i = 0; i < list.Count; i++)
            {
                log += list[i].ToString();
                log += i != list.Count - 1 ? ", " : "]";
            }

            Debug.Log(log);
        }

        /// <summary>
        /// Returns the first item in the list that matches the specified predicate.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="match"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetMatch<T>(this List<T> list, Predicate<T> match, out T item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var currentItem = list[i];
                if (currentItem == null) continue;

                if (match(currentItem))
                {
                    item = currentItem;
                    return true;
                }
            }

            item = default;
            return false;
        }

        public static bool TryGetIndex<T>(this List<T> list, Predicate<T> match, out int index)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}