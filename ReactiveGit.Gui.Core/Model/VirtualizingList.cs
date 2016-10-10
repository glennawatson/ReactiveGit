namespace ReactiveGit.Gui.Core.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reactive.Linq;

    using ReactiveGit.Core.Managers;
    using ReactiveGit.Core.Model;

    /// <summary>
    /// Specialized list implementation that provides data virtualization. The collection is divided up into pages,
    /// and pages are dynamically fetched from the IItemsProvider when required. Stale pages are removed after a
    /// configurable period of time.
    /// Intended for use with large collections on a network or disk resource that cannot be instantiated locally
    /// due to memory consumption or fetch latency.
    /// </summary>
    /// <remarks>
    /// The IList implmentation is not fully complete, but should be sufficient for use as read only collection
    /// data bound to a suitable ItemsControl.
    /// </remarks>
    public class VirtualizingList : IList<GitCommit>
    {
        private readonly IBranchManager branchManager;

        private readonly GitBranch branchName;

        private readonly Dictionary<int, IList<GitCommit>> pages = new Dictionary<int, IList<GitCommit>>();

        private readonly Dictionary<int, DateTime> pageTouchTimes = new Dictionary<int, DateTime>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingList" /> class.
        /// </summary>
        /// <param name="branchManager">The items provider.</param>
        /// <param name="branchName">The name of the branch.</param>
        public VirtualizingList(IBranchManager branchManager, GitBranch branchName)
        {
            if (branchManager == null)
            {
                throw new ArgumentNullException(nameof(branchManager));
            }

            if (branchName == null)
            {
                throw new ArgumentNullException(nameof(branchName));
            }

            this.branchManager = branchManager;
            this.branchName = branchName;
        }

        /// <inheritdoc />
        public int Count => this.branchManager.GetCommitCount(this.branchName);

        /// <inheritdoc />
        public bool IsReadOnly => true;

        /// <summary>
        /// Gets the sizes of the pages.
        /// </summary>
        public int PageSize { get; } = 100;

        /// <summary>
        /// Gets the time out of the pages.
        /// </summary>
        public TimeSpan PageTimeout { get; } = TimeSpan.FromMilliseconds(10000);

        /// <inheritdoc />
        public GitCommit this[int index]
        {
            get
            {
                // determine which page and offset within page
                int pageIndex = index / this.PageSize;
                int pageOffset = index % this.PageSize;

                // request primary page
                this.RequestPage(pageIndex);

                // if accessing upper 50% then request next page
                if ((pageOffset > this.PageSize / 2) && (pageIndex < this.Count / this.PageSize))
                {
                    this.RequestPage(pageIndex + 1);
                }

                // if accessing lower 50% then request prev page
                if ((pageOffset < this.PageSize / 2) && (pageIndex > 0))
                {
                    this.RequestPage(pageIndex - 1);
                }

                // remove stale pages
                this.CleanUpPages();

                // defensive check in case of async load
                return this.pages[pageIndex] == null ? null : this.pages[pageIndex][pageOffset];
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <inheritdoc />
        public void Add(GitCommit item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(GitCommit item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void CopyTo(GitCommit[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IEnumerator<GitCommit> GetEnumerator()
        {
            for (var i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc />
        public int IndexOf(GitCommit item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Insert(int index, GitCommit item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Remove(GitCommit item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void CleanUpPages()
        {
            IList<int> keys = new List<int>(this.pageTouchTimes.Keys);
            foreach (int key in keys)
            {
                // page 0 is a special case, since WPF ItemsControl access the first item frequently
                if ((key == 0) || (DateTime.Now - this.pageTouchTimes[key] <= this.PageTimeout))
                {
                    continue;
                }

                this.pages.Remove(key);
                this.pageTouchTimes.Remove(key);
            }
        }

        private void LoadPage(int pageIndex)
        {
            int index = pageIndex * this.PageSize;

            IList<GitCommit> pageContents =
                this.branchManager.GetCommitsForBranch(this.branchName, index, this.PageSize, GitLogOptions.None).ToList().Wait();

            this.PopulatePage(pageIndex, pageContents);
        }

        private void PopulatePage(int pageIndex, IList<GitCommit> page)
        {
            if (this.pages.ContainsKey(pageIndex))
            {
                this.pages[pageIndex] = page;
            }
        }

        private void RequestPage(int pageIndex)
        {
            if (!this.pages.ContainsKey(pageIndex))
            {
                this.pages.Add(pageIndex, null);
                this.pageTouchTimes.Add(pageIndex, DateTime.Now);
                this.LoadPage(pageIndex);
            }
            else
            {
                this.pageTouchTimes[pageIndex] = DateTime.Now;
            }
        }
    }
}