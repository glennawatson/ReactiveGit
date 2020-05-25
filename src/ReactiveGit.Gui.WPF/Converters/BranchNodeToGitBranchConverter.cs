// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using ReactiveGit.Gui.Core.Model.Branches;
using ReactiveGit.Library.Core.Model;

namespace ReactiveGit.Gui.WPF.Converters
{
    /// <summary>
    /// A converter for going to and from the branch node and git branch models.
    /// </summary>
    public class BranchNodeToGitBranchConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gitBranch = value as GitBranch;
            return gitBranch != null ? new BranchLeaf(gitBranch) : null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var branchLeaf = value as BranchLeaf;
            return branchLeaf?.Branch;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}