// <copyright file="RefLogViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.ObjectModel;

using DynamicData;
using DynamicData.Binding;

namespace ReactiveGit.Gui.Core.ViewModel.RefLog
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ExtensionMethods;
    using ReactiveGit.Gui.Core.ViewModel.GitObject;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Implementation of the ref log view model.
    /// </summary>
    public class RefLogViewModel : GitObjectViewModelBase, IRefLogViewModel
    {
        private readonly ReactiveCommand<Unit, GitRefLog> refresh;

        private readonly ReadOnlyObservableCollection<GitRefLog> refLogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefLogViewModel" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Must have valid repository details.</exception>
        public RefLogViewModel()
        {
            IObservable<bool> isCurrentBranchObservable =
                this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Select(details => details != null);

            this.refresh = ReactiveCommand.CreateFromObservable(this.RefreshImpl, isCurrentBranchObservable);

            this.refresh.ToObservableChangeSet()
                .Sort(SortExpressionComparer<GitRefLog>.Descending(p => p.DateTime), SortOptions.UseBinarySearch)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out this.refLogs)
                .Subscribe();

            this.WhenAnyValue(x => x.RepositoryDetails.SelectedBranch).Where(x => x != null).Subscribe(
                _ => this.Refresh.InvokeCommand());
        }

        /// <inheritdoc />
        public override string FriendlyName => "Undo Stack";

        /// <inheritdoc />
        public IEnumerable<GitRefLog> RefLog => this.refLogs;

        /// <inheritdoc />
        public override ICommand Refresh => this.refresh;

        private IObservable<GitRefLog> RefreshImpl()
        {
            return this.RepositoryDetails.RefLogManager.GetRefLog(this.RepositoryDetails.SelectedBranch);
        }
    }
}