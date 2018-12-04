// <copyright file="DockingElementTemplateSelector.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.Selectors
{
    using System.Windows;
    using System.Windows.Controls;

    using ReactiveGit.Gui.Core.ViewModel;
    using ReactiveGit.Gui.Core.ViewModel.Repository;

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
                return this.Template;
            }

            if (item is IRepositoryDocumentViewModel)
            {
                return this.Template;
            }

            return base.SelectTemplate(item, container);
        }
    }
}