namespace ReactiveGit.Gui.WPF
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    using Microsoft.WindowsAPICodePack.Dialogs;

    using ReactiveGit.Gui.Base.View;

    /// <summary>
    /// Shows a folder selector dialog.
    /// </summary>
    public class WpfFolderSelector : IFolderSelector
    {
        /// <inheritdoc />
        public IObservable<string> Prompt()
        {
            return Observable.Create<string>(observer =>
                {
                    var dialog = new CommonOpenFileDialog { IsFolderPicker = true };

                    observer.OnNext(dialog.ShowDialog() != CommonFileDialogResult.Ok ? null : dialog.FileName);

                    observer.OnCompleted();

                    return Disposable.Empty;
                });
        }
    }
}
