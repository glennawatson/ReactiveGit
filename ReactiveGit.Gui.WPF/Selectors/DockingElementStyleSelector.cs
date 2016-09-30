namespace ReactiveGit.Gui.WPF.Selectors
{
    using System.Windows;
    using System.Windows.Controls;

    using ReactiveGit.Gui.Core.ViewModel;
    using ReactiveGit.Gui.Core.ViewModel.Repository;

    /// <summary>
    /// Decides which style to use for the docking window system.
    /// </summary>
    public class DockingElementStyleSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets the style for documents.
        /// </summary>
        public Style DocumentStyle { get; set; }

        /// <summary>
        /// Gets or sets the style for support windows.
        /// </summary>
        public Style SupportPaneStyle { get; set; }

        /// <inheritdoc />
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is IRepositoryDocumentViewModel)
            {
                return this.DocumentStyle;
            }

            if (item is ISupportViewModel)
            {
                return this.SupportPaneStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}