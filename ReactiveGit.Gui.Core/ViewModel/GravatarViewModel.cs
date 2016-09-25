namespace ReactiveGit.Gui.Core.ViewModel
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using ReactiveGit.Gui.Core.ExtensionMethods;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    using Splat;

    /// <summary>
    /// A view model for loading a gravatar image. 
    /// </summary>
    public class GravatarViewModel : ReactiveObject
    {
        private ObservableAsPropertyHelper<IBitmap> gravatarBitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarViewModel"/> class.
        /// </summary>
        public GravatarViewModel()
        {
            var loadGravatar = ReactiveCommand.CreateFromTask<string, IBitmap>(
                async x => await this.LoadGravatarLogo(x));

            this.gravatarBitmap = loadGravatar.ToProperty(this, x => x.GravatarBitmap, out this.gravatarBitmap);

            this.WhenAnyValue(x => x.EmailAddress).InvokeCommand(loadGravatar);
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Reactive]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets the Gravatar bitmap. 
        /// </summary>
        public IBitmap GravatarBitmap => this.gravatarBitmap.Value;

        private async Task<IBitmap> LoadGravatarLogo(string emailAddress)
        {
            using (var wc = new HttpClient())
            {
                var imageStream =
                    await wc.GetStreamAsync($"http://gravatar.com/avatar/{emailAddress.Md5Encode()}?d=retro&s=60");

                // IBitmap is a type that provides basic image information such as dimensions
                IBitmap profileImage =
                    await
                        BitmapLoader.Current.Load(
                            imageStream,
                            null /* Use original width */,
                            null /* Use original height */);

                return profileImage;
            }
        }
    }
}