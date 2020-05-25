// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;

using ReactiveGit.Gui.Core.Model;
using ReactiveGit.Gui.Core.ViewModel.Content;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveGit.Gui.Core.ViewModel.Output
{
    /// <summary>
    /// View model for the output.
    /// </summary>
    public class OutputViewModel : ContentViewModelBase, IOutputViewModel
    {
        private readonly ReactiveCommand<Unit, Unit> _clear;

        private readonly StringBuilder _outputText = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputViewModel" /> class.
        /// </summary>
        public OutputViewModel()
        {
            _clear = ReactiveCommand.Create(ClearOutputImpl);

            var canOutput = this.WhenAnyValue(x => x.RepositoryDetails).Select(x => x != null);

            ReactiveCommand<string, Unit> addOutput = ReactiveCommand.Create<string>(
                x =>
                    {
                        _outputText.AppendLine(x);
                        this.RaisePropertyChanged(nameof(Output));
                    }, canOutput);

            this.WhenAnyObservable(x => x.RepositoryDetails.RepositoryManager.GitOutput).InvokeCommand(addOutput);
        }

        /// <inheritdoc />
        public ICommand Clear => _clear;

        /// <inheritdoc />
        public override string FriendlyName => "Output";

        /// <inheritdoc />
        public string Output => _outputText?.ToString();

        /// <inheritdoc />
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        private void ClearOutputImpl()
        {
            _outputText.Clear();
        }
    }
}