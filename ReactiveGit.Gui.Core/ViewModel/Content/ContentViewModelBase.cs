namespace ReactiveGit.Gui.Core.ViewModel.Content
{
    using System;

    using ReactiveUI;

    /// <summary>
    /// View model for view models that contain content.
    /// </summary>
    public abstract class ContentViewModelBase : ReactiveObject, IContentViewModelBase
    {
        private readonly Lazy<string> contentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModelBase"/> class.
        /// </summary>
        protected ContentViewModelBase()
        {
            this.contentId = new Lazy<string>(() => this.GetType().Name);
        }

        /// <inheritdoc />
        public string ContentId => this.contentId.Value;

        /// <inheritdoc />
        public abstract string FriendlyName { get; }
    }
}
