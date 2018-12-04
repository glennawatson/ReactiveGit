// <copyright file="OutputViewModel.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.Core.ViewModel.Output
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Text;
    using System.Windows.Input;

    using ReactiveGit.Gui.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.Content;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// View model for the output.
    /// </summary>
    public class OutputViewModel : ContentViewModelBase, IOutputViewModel
    {
        private readonly ReactiveCommand<Unit, Unit> clear;

        private readonly StringBuilder outputText = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputViewModel" /> class.
        /// </summary>
        public OutputViewModel()
        {
            this.clear = ReactiveCommand.Create(this.ClearOutputImpl);

            var canOutput = this.WhenAnyValue(x => x.RepositoryDetails).Select(x => x != null);

            ReactiveCommand<string, Unit> addOutput = ReactiveCommand.Create<string>(
                x =>
                    {
                        this.outputText.AppendLine(x);
                        this.RaisePropertyChanged(nameof(this.Output));
                    }, canOutput);

            this.WhenAnyObservable(x => x.RepositoryDetails.RepositoryManager.GitOutput).InvokeCommand(addOutput);
        }

        /// <inheritdoc />
        public ICommand Clear => this.clear;

        /// <inheritdoc />
        public override string FriendlyName => "Output";

        /// <inheritdoc />
        public string Output => this.outputText?.ToString();

        /// <inheritdoc />
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        private void ClearOutputImpl()
        {
            this.outputText.Clear();
        }
    }
}