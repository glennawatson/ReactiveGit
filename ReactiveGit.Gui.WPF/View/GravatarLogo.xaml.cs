namespace ReactiveGit.Gui.WPF.View
{
    using System;
    using System.Reactive.Linq;

    using AutoDependencyPropertyMarker;

    using ReactiveGit.Gui.Core.ViewModel;

    using ReactiveUI;

    using Splat;

    /// <summary>
    /// Interaction logic for GravatarLogo.xaml
    /// </summary>
    public partial class GravatarLogo : IViewFor<GravatarViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarLogo"/> class.
        /// </summary>
        public GravatarLogo()
        {
            this.InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(this.WhenAnyValue(view => view.ViewModel.GravatarBitmap).Where(x => x != null).SubscribeOn(RxApp.MainThreadScheduler).Select(x => x.ToNative()).BindTo(this, view => view.LogoImage.Source));
                    });
        }

        /// <inheritdoc />
        object IViewFor.ViewModel
        {
            get
            {
                return this.ViewModel;
            }

            set
            {
                this.ViewModel = (GravatarViewModel)value;
            }
        }

        /// <inheritdoc />
        [AutoDependencyProperty]
        public GravatarViewModel ViewModel { get; set; }
    }
}
