// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using ReactiveGit.Gui.Core.ViewModel.Repository;

using ReactiveUI;

namespace ReactiveGit.Gui.WPF.Converters
{
    /// <summary>
    /// Converts from avalon dock to the view model selected document property.
    /// Will make repository documents set appropriately, and ignore child repository documents.
    /// </summary>
    public class DocumentTypeConverter : IBindingTypeConverter
    {
        /// <inheritdoc />
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (toType != typeof(IRepositoryDocumentViewModel))
            {
                return 0;
            }

            if (fromType == typeof(object))
            {
                return 100;
            }

            return 0;
        }

        /// <inheritdoc />
        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (from == null)
            {
                result = null;
                return true;
            }

            var repositoryDocument = from as IRepositoryDocumentViewModel;
            if (repositoryDocument == null)
            {
                result = null;
                return false;
            }

            result = repositoryDocument;

            return true;
        }
    }
}