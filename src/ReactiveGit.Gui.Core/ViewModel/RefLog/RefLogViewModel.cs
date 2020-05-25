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
using ReactiveGit.Gui.Core.ExtensionMethods;
using ReactiveGit.Gui.Core.ViewModel.GitObject;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.Core.ViewModel.RefLog
{
    /// <summary>
    /// Implementation of the ref log view model.
    /// </summary>
    public class RefLogViewModel : GitObjectViewModelBase, IRefLogViewModel
    {
        private readonly ReactiveCommand<Unit, GitRefLog> _refresh;

        private readonly ReadOnlyObservableCollection<GitRefLog> _refLogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogViewModel" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Must have valid repository details.</exception>
        public RefLogViewModel()
        {
            IObservable<bool> isCurrentBranchObservable =
                this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Select(details => details != null);

            _refresh = ReactiveCommand.CreateFromObservable(RefreshImpl, isCurrentBranchObservable);

            _refresh.ToObservableChangeSet()
                .Sort(SortExpressionComparer<GitRefLog>.Descending(p => p.DateTime), SortOptions.UseBinarySearch)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _refLogs)
                .Subscribe();

            this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Where(x => x != null).Subscribe(
                _ => Refresh.InvokeCommand());
        }

        /// <inheritdoc />
        public override string FriendlyName => "Undo Stack";

        /// <inheritdoc />
        public IEnumerable<GitRefLog> RefLog => _refLogs;

        /// <inheritdoc />
        public override ICommand Refresh => _refresh;

        private IObservable<GitRefLog> RefreshImpl()
        {
            return RepositoryDetails.RefLogManager.GetRefLog(RepositoryDetails.SelectedBranch);
        }
    }
}