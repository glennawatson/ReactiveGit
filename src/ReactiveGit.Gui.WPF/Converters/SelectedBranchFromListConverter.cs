// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using ReactiveGit.Gui.Core.Model.Branches;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;

namespace ReactiveGit.Gui.WPF.Converters
{
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