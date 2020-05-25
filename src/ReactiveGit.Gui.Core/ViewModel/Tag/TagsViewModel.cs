// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveGit.Gui.Core.ViewModel.GitObject;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.Core.ViewModel.Tag
{
    /// <summary>
    ///  View model for handling git tags.
    /// </summary>
    public class TagsViewModel : GitObjectViewModelBase, ITagsViewModel
    {
        private readonly ReactiveCommand<Unit, GitTagViewModel> _refresh;

        private readonly ObservableAsPropertyHelper<GitTagViewModel> _selectedGitTag;

        private readonly ReadOnlyObservableCollection<GitTagViewModel> _tags;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsViewModel"/> class.
        /// </summary>
        public TagsViewModel()
        {
            IObservable<bool> isValidRepository = this.WhenAnyValue(x => x.RepositoryDetails).Select(details => details != null);

            _refresh = ReactiveCommand.CreateFromObservable(RefreshImpl, isValidRepository);

            _refresh.ToObservableChangeSet()
                .Sort(SortExpressionComparer<GitTagViewModel>.Descending(p => p.DateTime), SortOptions.UseBinarySearch)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _tags)
                .Subscribe();

            _selectedGitTag = this.WhenAnyValue(x => x.SelectedGitObject).ObserveOn(RxApp.MainThreadScheduler).Select(x => x as GitTagViewModel).ToProperty(this, x => x.SelectedTag, scheduler: RxApp.MainThreadScheduler);
        }

        /// <inheritdoc />
        public override string FriendlyName => "Tags";

        /// <inheritdoc />
        public IEnumerable<GitTagViewModel> Tags => _tags;

        /// <inheritdoc />
        public override ICommand Refresh => _refresh;

        /// <inheritdoc />
        public GitTagViewModel SelectedTag => _selectedGitTag.Value;

        private IObservable<GitTagViewModel> RefreshImpl()
        {
            return RepositoryDetails.TagManager.GetTags().Select(x => new GitTagViewModel(x));
        }
    }
}
