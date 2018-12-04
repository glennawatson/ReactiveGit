// <copyright file="AsyncMemoizingMRUCache.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Core.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// This data structure is a representation of a memoizing cache - i.e. a
    /// class that will evaluate a function, but keep a cache of recently
    /// evaluated parameters.
    /// Since this is a memoizing cache, it is important that this function be a
    /// "pure" function in the mathematical sense - that a key *always* maps to
    /// a corresponding return value.
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter to the calculation function.</typeparam>
    /// <typeparam name="TVal">
    /// The type of the value returned by the calculation
    /// function.
    /// </typeparam>
    public class AsyncMemoizingMRUCache<TParam, TVal>
    {
        private readonly Func<TParam, object, Task<TVal>> calculationFunction;

        private readonly int maxCacheSize;

        private readonly Action<TVal> releaseFunction;

        private Dictionary<TParam, Tuple<LinkedListNode<TParam>, TVal>> cacheEntries;

        private LinkedList<TParam> cacheMRUList;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncMemoizingMRUCache{TParam, TVal}" /> class.
        /// </summary>
        /// <param name="calculationFunc">
        /// The function whose results you want to cache,
        /// which is provided the key value, and an Tag object that is
        /// user-defined.
        /// </param>
        /// <param name="maxSize">
        /// The size of the cache to maintain, after which old
        /// items will start to be thrown out.
        /// </param>
        /// <param name="onRelease">
        /// A function to call when a result gets
        /// evicted from the cache (i.e. because Invalidate was called or the
        /// cache is full).
        /// </param>
        public AsyncMemoizingMRUCache(
            Func<TParam, object, Task<TVal>> calculationFunc,
            int maxSize,
            Action<TVal> onRelease = null)
        {
            Contract.Requires(calculationFunc != null);
            Contract.Requires(maxSize > 0);

            this.calculationFunction = calculationFunc;
            this.releaseFunction = onRelease;
            this.maxCacheSize = maxSize;
            this.InvalidateAll();
        }

        /// <summary>
        /// Returns all values currently in the cache.
        /// </summary>
        /// <returns>The cached values.</returns>
        public IEnumerable<TVal> CachedValues()
        {
            return this.cacheEntries.Select(x => x.Value.Item2);
        }

        /// <summary>
        /// Evaluates the function provided, returning the cached value if possible.
        /// </summary>
        /// <param name="key">The value to pass to the calculation function.</param>
        /// <param name="context">An additional optional user-specific parameter.</param>
        /// <returns>The value.</returns>
        public async Task<TVal> Get(TParam key, object context = null)
        {
            Contract.Requires(key != null);

            Tuple<LinkedListNode<TParam>, TVal> found;

            if (this.cacheEntries.TryGetValue(key, out found))
            {
                this.cacheMRUList.Remove(found.Item1);
                this.cacheMRUList.AddFirst(found.Item1);
                return found.Item2;
            }

            TVal result = await this.calculationFunction(key, context).ConfigureAwait(true);

            var node = new LinkedListNode<TParam>(key);
            this.cacheMRUList.AddFirst(node);
            this.cacheEntries[key] = new Tuple<LinkedListNode<TParam>, TVal>(node, result);
            this.MaintainCache();

            return result;
        }

        /// <summary>
        /// Ensure that the next time this key is queried, the calculation
        /// function will be called.
        /// </summary>
        /// <param name="key">The key to invalidate.</param>
        public void Invalidate(TParam key)
        {
            Contract.Requires(key != null);

            Tuple<LinkedListNode<TParam>, TVal> to_remove;

            if (!this.cacheEntries.TryGetValue(key, out to_remove))
            {
                return;
            }

            this.releaseFunction?.Invoke(to_remove.Item2);

            this.cacheMRUList.Remove(to_remove.Item1);
            this.cacheEntries.Remove(key);
        }

        /// <summary>
        /// Invalidate all items in the cache.
        /// </summary>
        public void InvalidateAll()
        {
            if ((this.releaseFunction == null) || (this.cacheEntries == null))
            {
                this.cacheMRUList = new LinkedList<TParam>();
                this.cacheEntries = new Dictionary<TParam, Tuple<LinkedListNode<TParam>, TVal>>();
                return;
            }

            if (this.cacheEntries.Count == 0)
            {
                return;
            }

            /*             We have to remove them one-by-one to call the release function
             * We ToArray() this so we don't get a "modifying collection while
             * enumerating" exception. */
            foreach (TParam v in this.cacheEntries.Keys.ToArray())
            {
                this.Invalidate(v);
            }
        }

        /// <summary>
        /// Try to get a value.
        /// </summary>
        /// <param name="key">The key to get.</param>
        /// <param name="result">The result value.</param>
        /// <returns>If the value can be found for the value.</returns>
        public bool TryGet(TParam key, out TVal result)
        {
            Contract.Requires(key != null);

            Tuple<LinkedListNode<TParam>, TVal> output;
            bool ret = this.cacheEntries.TryGetValue(key, out output);
            if (ret && (output != null))
            {
                this.cacheMRUList.Remove(output.Item1);
                this.cacheMRUList.AddFirst(output.Item1);
                result = output.Item2;
            }
            else
            {
                result = default(TVal);
            }

            return ret;
        }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(this.cacheEntries.Count == this.cacheMRUList.Count);
            Contract.Invariant(this.cacheEntries.Count <= this.maxCacheSize);
        }

        private void MaintainCache()
        {
            while (this.cacheMRUList.Count > this.maxCacheSize)
            {
                TParam to_remove = this.cacheMRUList.Last.Value;
                this.releaseFunction?.Invoke(this.cacheEntries[to_remove].Item2);

                this.cacheEntries.Remove(this.cacheMRUList.Last.Value);
                this.cacheMRUList.RemoveLast();
            }
        }
    }
}