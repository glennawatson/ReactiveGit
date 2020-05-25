// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveGit.Gui.Core.Interactions;
using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.ViewModel.Content;
using ReactiveGit.Library.Core.ExtensionMethods;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.Core.ViewModel.GitObject
{
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
                new ObservableGitObjectAction("Hard - Delete Changes", ReactiveCommand.CreateFromObservable(() => ResetImpl(SelectedGitObject, ResetMode.Hard),  validGitObject)),
                new ObservableGitObjectAction("Soft - Keep Changes", ReactiveCommand.CreateFromObservable(() => ResetImpl(SelectedGitObject, ResetMode.Soft),  validGitObject)),
                new ObservableGitObjectAction("Mixed - Reset Index/Keep Working Directory", ReactiveCommand.CreateFromObservable(() => ResetImpl(SelectedGitObject, ResetMode.Mixed),  validGitObject)),
            };

            gitObjectActions.Add(new InfoGitObjectAction("Reset", resetActions));

            gitObjectActions.Add(new ObservableGitObjectAction("Checkout", ReactiveCommand.CreateFromObservable(() => CheckoutImpl(SelectedGitObject, false), validGitObject)));
            gitObjectActions.Add(new ObservableGitObjectAction("Checkout (force)", ReactiveCommand.CreateFromObservable(() => CheckoutImpl(SelectedGitObject, true), validGitObject)));

            Actions = gitObjectActions;
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
                    RepositoryDetails.GitObjectManager.Reset(gitIdObject, resetMode));
        }

        private IObservable<Unit> CheckoutImpl(IGitIdObject gitObject, bool force)
        {
            return RepositoryDetails.GitObjectManager.Checkout(gitObject, force);
        }
    }
}