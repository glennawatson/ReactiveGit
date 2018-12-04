// <copyright file="DocumentTypeConverter.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.Converters
{
    using System;

    using ReactiveGit.Gui.Core.ViewModel.Repository;

    using ReactiveUI;

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