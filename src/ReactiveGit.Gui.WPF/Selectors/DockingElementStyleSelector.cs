// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

using ReactiveGit.Gui.Core.ViewModel;
using ReactiveGit.Gui.Core.ViewModel.Repository;

namespace ReactiveGit.Gui.WPF.Selectors
{
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
                return DocumentStyle;
            }

            if (item is ISupportViewModel)
            {
                return SupportPaneStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}