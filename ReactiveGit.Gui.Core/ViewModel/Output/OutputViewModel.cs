namespace ReactiveGit.Gui.Core.ViewModel.Output
{
    using System;
    using System.Reactive;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputViewModel"/> class.
        /// </summary>
        public OutputViewModel()
        {
            this.clear = ReactiveCommand.Create(this.ClearOutputImpl);

            this.WhenAnyObservable(x => x.RepositoryDetails.RepositoryManager.GitOutput).Subscribe(
                x =>
                {
                    this.Output = this.Output + "\r\n" + x;
                });
        }

        /// <inheritdoc />
        public override string FriendlyName => "Output";

        /// <inheritdoc />
        [Reactive]
        public IRepositoryDetails RepositoryDetails { get; set; }

        /// <inheritdoc />
        [Reactive]
        public string Output { get; set; }

        /// <inheritdoc />
        public ICommand Clear => this.clear;

        private void ClearOutputImpl()
        {
            this.Output = string.Empty;
        }
    }
}
