namespace Zinnia.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    /// <summary>
    /// Extended methods for sorting <see cref="Array"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public static class ArraySortExtensions<T>
    {
        /// <summary>
        /// Whether a heap allocation free sort is available.
        /// </summary>
        public static bool IsHeapAllocationFreeSortAvailable => sortAction != null;

        /// <summary>
        /// The cached delegate of the sort method to call.
        /// </summary>
        private static readonly Action<T[], int, int, Comparison<T>> sortAction;

        static ArraySortExtensions()
        {
            const string fullTypeName = "System.Collections.Generic.ArraySortHelper`1";
            const string methodName = "Sort";

            Type type = typeof(Array).Assembly.GetType(fullTypeName);
            Type genericType = type?.MakeGenericType(typeof(T));
            MethodInfo methodInfo = genericType?.GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                null,
                new[]
                {
                    typeof(T[]),
                    typeof(int),
                    typeof(int),
                    typeof(Comparison<T>)
                },
                null);

            if (methodInfo == null)
            {
                sortAction = null;
                Debug.LogWarning($"No heap allocation free sort is available: Type '{fullTypeName}' wasn't found"
                                 + $" or the method '{methodName}' doesn't have the expected signature."
                                 + " Sorting will fall back to the implementation that will create heap allocations.");
                return;
            }

            sortAction = (Action<T[], int, int, Comparison<T>>)Delegate.CreateDelegate(
                typeof(Action<T[], int, int, Comparison<T>>),
                methodInfo);
        }

        /// <summary>
        /// Sorts the elements in a one-dimensional array.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        /// <param name="comparer">The implementation to use when comparing elements. Will only be used if <see cref="IsHeapAllocationFreeSortAvailable"/> is <see langword="false"/>.</param>
        /// <param name="comparison">The implementation to use when comparing elements. Will only be used if <see cref="IsHeapAllocationFreeSortAvailable"/> is <see langword="true"/>.</param>
        public static void Sort(T[] array, int index, int length, IComparer<T> comparer, Comparison<T> comparison)
        {
            if (sortAction == null)
            {
                Array.Sort(array, index, length, comparer);
                return;
            }

            sortAction(array, index, length, comparison);
        }
    }
}