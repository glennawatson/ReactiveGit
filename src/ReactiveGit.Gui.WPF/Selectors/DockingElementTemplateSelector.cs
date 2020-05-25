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
    /// Selects the appropriate data template.
    /// </summary>
    public class DockingElementTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the data template for branches.
        /// </summary>
        public DataTemplate Template { get; set; }

        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ISupportViewModel)
            {
                return Template;
            }

            if (item is IRepositoryDocumentViewModel)
            {
                return Template;
            }

            return base.SelectTemplate(item, container);
        }
    }
}