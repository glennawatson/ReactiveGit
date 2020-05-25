// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ReactiveGit.Gui.WPF.Behaviours
{
    /// <summary>
    /// Adds a behaviour so we can get the selected item from the tree view.
    /// </summary>
    public class TreeViewSelectedItemBehaviour : Behavior<TreeView>
    {
        /// <summary>
        /// Dependency property for the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(TreeViewSelectedItemBehaviour),
            new UIPropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        /// <summary>The on selected item changed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as TreeViewItem;
            item?.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        /// <summary>The on tree view selected item changed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}