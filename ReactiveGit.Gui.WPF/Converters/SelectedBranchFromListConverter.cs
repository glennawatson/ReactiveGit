// <copyright file="SelectedBranchFromListConverter.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.Converters
{
    using System;
    using System.Collections;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.Model.Branches;

    using ReactiveUI;

    /// <summary>
    /// Converts from a selected list to a git branch.
    /// </summary>
    public class SelectedBranchFromListConverter : IBindingTypeConverter
    {
        /// <inheritdoc />
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (toType != typeof(GitBranch))
            {
                return 0;
            }

            if (fromType != typeof(IList))
            {
                return 0;
            }

            return 100;
        }

        /// <inheritdoc />
        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (from == null)
            {
                result = null;
                return false;
            }

            var list = from as IList;

            if (list == null)
            {
                result = null;
                return false;
            }

            if (list.Count == 0)
            {
                result = null;
                return true;
            }

            var leaf = list[0] as BranchLeaf;

            result = leaf?.Branch;
            return true;
        }
    }
}