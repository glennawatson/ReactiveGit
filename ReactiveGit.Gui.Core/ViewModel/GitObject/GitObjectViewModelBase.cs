// <copyright file="GitObjectViewModelBase.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

using DynamicData.Binding;

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Core.ExtensionMethods;
    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.Interactions;
    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.Content;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Base class for classes handling git objects.
    /// </summary>
    public abstract class GitObjectViewModelBase : ContentViewModelBase, IGitObjectViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitObjectViewModelBase" /> class.
        /// </summary>
        /// <param name="actions">Additional actions provided by the derived class.</param>
        protected GitObjectViewModelBase(IReadOnlyList<IGitObjectAction> actions = null)
        {
            var validGitObject = this.WhenAnyValue(x => x.SelectedGitObject).Select(x => x != null);

            var gitObjectActions = new ObservableCollectionExtended<IGitObjectAction>();
            if (actions != null)
            {
                gitObjectActions.AddRange(actions);
            }

            var resetActions = new List<IGitObjectAction>()
            {
                new ObservableGitObjectAction("Hard - Delete Changes", ReactiveCommand.CreateFromObservable(() => this.ResetImpl(this.SelectedGitObject, ResetMode.Hard),  validGitObject)),
                new ObservableGitObjectAction("Soft - Keep Changes", ReactiveCommand.CreateFromObservable(() => this.ResetImpl(this.SelectedGitObject, ResetMode.Soft),  validGitObject)),
                new ObservableGitObjectAction("Mixed - Reset Index/Keep Working Directory", ReactiveCommand.CreateFromObservable(() => this.ResetImpl(this.SelectedGitObject, ResetMode.Mixed),  validGitObject)),
            };

            gitObjectActions.Add(new InfoGitObjectAction("Reset", resetActions));

            gitObjectActions.Add(new ObservableGitObjectAction("Checkout", ReactiveCommand.CreateFromObservable(() => this.CheckoutImpl(this.SelectedGitObject, false), validGitObject)));
            gitObjectActions.Add(new ObservableGitObjectAction("Checkout (force)", ReactiveCommand.CreateFromObservable(() => this.CheckoutImpl(this.SelectedGitObject, true), validGitObject)));

            this.Actions = gitObjectActions;
        }

        /// <inheritdoc />
        public abstract ICommand Refresh { get; }

        /// <summary>
        /// Gets or sets the repository details.
        /// </summary>
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        /// <inheritdoc />
        public IReadOnlyList<IGitObjectAction> Actions { get; }

        /// <inheritdoc />
        [Reactive]
        public IGitIdObject SelectedGitObject { get; set; }

        private IObservable<Unit> ResetImpl(IGitIdObject gitIdObject, ResetMode resetMode)
        {
            var canCompleteObservable =
                CommonInteractions.CheckToProceed.Handle(
                    $"Do you really want to reset {gitIdObject.ShaShort} using {resetMode} reset?");

            return canCompleteObservable.CompleteIfTrue(
                    this.RepositoryDetails.GitObjectManager.Reset(gitIdObject, resetMode));
        }

        private IObservable<Unit> CheckoutImpl(IGitIdObject gitObject, bool force)
        {
            return this.RepositoryDetails.GitObjectManager.Checkout(gitObject, force);
        }
    }
}