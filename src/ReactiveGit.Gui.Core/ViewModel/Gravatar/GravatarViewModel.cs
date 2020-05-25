// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveGit.Gui.Core.ExtensionMethods;
using ReactiveGit.Library.Core.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace ReactiveGit.Gui.Core.ViewModel.Gravatar
{
    /// <summary>
    /// A view model for loading a gravatar image.
    /// </summary>
    public class GravatarViewModel : ReactiveObject
    {
        private static readonly AsyncMemoizingMRUCache<string, IBitmap> Cache = new AsyncMemoizingMRUCache<string, IBitmap>(GetBitmapAsync, RxApp.BigCacheLimit);

        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarViewModel" /> class.
        /// </summary>
        public GravatarViewModel()
        {
            ReactiveCommand<string, IBitmap> loadGravatar =
                ReactiveCommand.CreateFromTask<string, IBitmap>(x => Cache.Get(x));
            loadGravatar.Subscribe(x => GravatarBitmap = x);

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

        private static async Task<IBitmap> GetBitmapAsync(string emailAddress, object ignore)
        {
            using (var wc = new HttpClient())
            {
                byte[] imageByteArray =
                    await wc.GetByteArrayAsync($"http://gravatar.com/avatar/{emailAddress.Md5Encode()}?d=retro&s=60").ConfigureAwait(true);

                using (var ms = new MemoryStream(imageByteArray))
                {
                    // IBitmap is a type that provides basic image information such as dimensions
                    return await BitmapLoader.Current.Load(ms, null /* Use original width */, null /* Use original height */).ConfigureAwait(false);
                }
            }
        }
    }
}