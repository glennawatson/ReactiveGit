// <copyright file="TreeViewSelectedItemBehaviour.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.Behaviours
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

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
                return this.GetValue(SelectedItemProperty);
            }

            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectedItemChanged += this.OnTreeViewSelectedItemChanged;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.SelectedItemChanged -= this.OnTreeViewSelectedItemChanged;
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
            this.SelectedItem = e.NewValue;
        }
    }
}