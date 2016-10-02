namespace ReactiveGit.Gui.Core.ViewModel.CommitHistory
{
    using System;

    using ReactiveUI;

    /// <summary>
    /// Details about a commit.
    /// </summary>
    public class CommitHistoryItemDetailsViewModel : ReactiveObject
    {
        public DateTime DateTime { get; set; }
    }
}