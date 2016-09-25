namespace ReactiveGit.Gui.WPF.View
{
    using ReactiveGit.Gui.Core.ViewModel;

    using ReactiveUI;

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
                        d(this.OneWayBind(this.ViewModel, vm => vm.GravatarBitmap, view => view.LogoImage.Source));
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
        public GravatarViewModel ViewModel { get; set; }
    }
}
