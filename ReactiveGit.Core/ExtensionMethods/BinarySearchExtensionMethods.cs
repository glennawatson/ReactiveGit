namespace ReactiveGit.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods relating to performing a binary search.
    /// </summary>
    public static class BinarySearchExtensionMethods
    {
        /// <summary>
        /// Performs a binary search using the defined comparer.
        /// Searches a section of the list for a given element using a binary search
        /// algorithm. This method assumes that the given
        /// section of the list is already sorted; if this is not the case, the
        /// result will be incorrect.
        /// </summary>
        /// <typeparam name="TItem">The list item type.</typeparam>
        /// <param name="list">The list to do the search in.</param>
        /// <param name="targetValue">The target value.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>
        /// The method returns the index of the given value in the list. If the
        /// list does not contain the given value, the method returns a negative
        /// integer. The bitwise complement operator (~) can be applied to a
        /// negative result to produce the index of the first element (if any) that
        /// is larger than the given search value. This is also the index at which
        /// the search value should be inserted into the list in order for the list
        /// to remain sorted.
        /// </returns>
        public static int BinarySearchIndexOf<TItem>(
            this IList<TItem> list,
            TItem targetValue,
            IComparer<TItem> comparer = null)
        {
            Func<TItem, TItem, int> compareFunc = comparer != null
                                                      ? comparer.Compare
                                                      : (Func<TItem, TItem, int>)Comparer<TItem>.Default.Compare;
            int index = BinarySearchIndexOfBy(list, compareFunc, targetValue);
            return index;
        }

        /// <summary>
        /// Performs a binary search using the defined comparer.
        /// Searches a section of the list for a given element using a binary search
        /// algorithm. This method assumes that the given
        /// section of the list is already sorted; if this is not the case, the
        /// result will be incorrect.
        /// </summary>
        /// <typeparam name="TItem">The item type to search.</typeparam>
        /// <typeparam name="TValue">The value type to search.</typeparam>
        /// <param name="list">The list to search.</param>
        /// <param name="comparer">The coparer to compare against.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// The method returns the index of the given value in the list. If the
        /// list does not contain the given value, the method returns a negative
        /// integer. The bitwise complement operator (~) can be applied to a
        /// negative result to produce the index of the first element (if any) that
        /// is larger than the given search value. This is also the index at which
        /// the search value should be inserted into the list in order for the list
        /// to remain sorted.
        /// </returns>
        public static int BinarySearchIndexOfBy<TItem, TValue>(
            this IList<TItem> list,
            Func<TItem, TValue, int> comparer,
            TValue value)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (list.Count == 0)
            {
                return -1;
            }

            // Implementation below copied largely from .NET4 ArraySortHelper.InternalBinarySearch()
            var lo = 0;
            int hi = list.Count - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = comparer(list[i], value);

                if (order == 0)
                {
                    return i;
                }

                if (order < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return ~lo;
        }
    }
}