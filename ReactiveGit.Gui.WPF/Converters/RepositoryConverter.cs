namespace ReactiveGit.Gui.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    using ReactiveGit.Gui.Base.ViewModel.Repository;

    /// <summary>
    /// Makes only the repository selectable.
    /// </summary>
    public class RepositoryConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IRepositoryDocumentViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IRepositoryDocumentViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}