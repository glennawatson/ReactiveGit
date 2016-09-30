namespace ReactiveGit.Gui.Core.ViewModel.Gravatar
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using ReactiveGit.Core.Model;
    using ReactiveGit.Gui.Core.ExtensionMethods;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    using Splat;

    /// <summary>
    /// A view model for loading a gravatar image.
    /// </summary>
    public class GravatarViewModel : ReactiveObject
    {
        private static readonly AsyncMemoizingMRUCache<string, IBitmap> Cache;

        static GravatarViewModel()
        {
            Cache = new AsyncMemoizingMRUCache<string, IBitmap>(GetBitmap, 100);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarViewModel" /> class.
        /// </summary>
        public GravatarViewModel()
        {
            ReactiveCommand<string, IBitmap> loadGravatar =
                ReactiveCommand.CreateFromTask<string, IBitmap>(x => Cache.Get(x));
            loadGravatar.Subscribe(x => this.GravatarBitmap = x);

            this.WhenAnyValue(x => x.EmailAddress).Where(x => x != null).InvokeCommand(loadGravatar);
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Reactive]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Gravatar bitmap.
        /// </summary>
        [Reactive]
        public IBitmap GravatarBitmap { get; set; }

        private static async Task<IBitmap> GetBitmap(string emailAddress, object ignore)
        {
            using (var wc = new HttpClient())
            {
                byte[] imageByteArray =
                    await wc.GetByteArrayAsync($"http://gravatar.com/avatar/{emailAddress.Md5Encode()}?d=retro&s=60");

                using (var ms = new MemoryStream(imageByteArray))
                {
                    // IBitmap is a type that provides basic image information such as dimensions
                    IBitmap profileImage =
                        await
                            BitmapLoader.Current.Load(ms, null /* Use original width */, null /* Use original height */);
                    return profileImage;
                }
            }
        }
    }
}