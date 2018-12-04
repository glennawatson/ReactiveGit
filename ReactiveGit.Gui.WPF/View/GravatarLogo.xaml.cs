// <copyright file="GravatarLogo.xaml.cs" company="Glenn Watson">
// Copyright (c) 2018 Glenn Watson. All rights reserved.
// See LICENSE file in the project root for full license information.
// </copyright>

namespace ReactiveGit.Gui.WPF.View
{
    using System.Reactive.Linq;

    using ReactiveUI;

    using Splat;

    /// <summary>
    /// Interaction logic for GravatarLogo.xaml
    /// </summary>
    public partial class GravatarLogo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravatarLogo" /> class.
        /// </summary>
        public GravatarLogo()
        {
            this.InitializeComponent();

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