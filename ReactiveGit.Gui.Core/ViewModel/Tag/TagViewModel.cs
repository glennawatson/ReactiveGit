namespace ReactiveGit.Gui.Core.ViewModel.Tag
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ViewModel.GitObject;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    ///  View model for handling git tags.
    /// </summary>
    public class TagViewModel : GitObjectViewModelBase, ITagViewModel
    {
        private readonly ReactiveCommand<Unit, GitTag> refresh;

        private readonly ObservableAsPropertyHelper<GitTag> selectedGitTag;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagViewModel"/> class.
        /// </summary>
        public TagViewModel()
        {
            IObservable<bool> isValidRepository = this.WhenAnyValue(x => x.RepositoryDetails).Select(details => details != null);

            this.refresh = ReactiveCommand.CreateFromObservable(this.RefreshImpl, isValidRepository);

            this.selectedGitTag = this.WhenAnyValue(x => x.SelectedGitObject).Select(x => x as GitTag).ToProperty(this, x => x.SelectedTag, out this.selectedGitTag);
        }

        /// <inheritdoc />
        public override string FriendlyName => "Tags";

        /// <inheritdoc />
        [Reactive]
        public IEnumerable<GitTag> Tags { get; set; }

        /// <inheritdoc />
        public override ICommand Refresh => this.refresh;

        /// <inheritdoc />
        public GitTag SelectedTag => this.selectedGitTag.Value;

        private IObservable<GitTag> RefreshImpl()
        {
            this.Tags = this.refresh.CreateCollection();

            return this.RepositoryDetails.TagManager.GetTags();
        }
    }
}
