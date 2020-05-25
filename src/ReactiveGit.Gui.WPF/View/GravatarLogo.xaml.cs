// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;

using ReactiveUI;

using Splat;

namespace ReactiveGit.Gui.WPF.View
{
    /// <summary>
    /// Interaction logic for GravatarLogo.xaml.
    /// </summary>
    public partial class GravatarLogo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarLogo" /> class.
        /// </summary>
        public GravatarLogo()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                    {
                        d(
                            this.WhenAnyValue(view => view.ViewModel.GravatarBitmap).Where(x => x != null).SubscribeOn(
                                RxApp.MainThreadScheduler).Select(x => x.ToNative()).BindTo(
                                this,
                                view => view.LogoImage.Source));
                    });
        }
    }
}