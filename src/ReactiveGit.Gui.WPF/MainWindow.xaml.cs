// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Threading;

using MahApps.Metro.Controls.Dialogs;

using ReactiveGit.Gui.Core.ExtensionMethods;
using ReactiveGit.Gui.Core.Interactions;
using ReactiveGit.Gui.WPF.ViewModel;

using ReactiveUI;

using Splat;

namespace ReactiveGit.Gui.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : IActivatableView
    {
        /// <summary>Initializes a new instance of the <see cref="MainWindow" /> class.</summary>
        public MainWindow()
        {
            InitializeComponent();

            AppBootstrapper = new AppBootstrapper();
            DataContext = AppBootstrapper;
            WindowLayoutViewModel = Locator.Current.GetService<IWindowLayoutViewModel>();
            WindowLayoutViewModel.Window = this;

            this.WhenActivated(
                d =>
                {
                    d(CommonInteractions.CheckToProceed.RegisterHandler(
                        async interaction =>
                        {
                            MessageDialogResult shouldContinue =
                                            await
                                                this.ShowMessageAsync(
                                                    "Please confirm",
                                                    interaction.Input,
                                                    MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(false);

                            interaction.SetOutput(shouldContinue == MessageDialogResult.Affirmative);
                        }));

                    d(CommonInteractions.GetStringResponse.RegisterHandler(
                        async interaction =>
                        {
                            string input = await this.ShowInputAsync("Please confirm", interaction.Input).ConfigureAwait(false);
                            interaction.SetOutput(input);
                        }));

                    d(this.WhenAnyValue(x => x.AppBootstrapper).BindTo(this, x => x.DataContext));
                });
        }

        /// <summary>
        /// Gets or sets the app bootstrapper.
        /// </summary>
        public AppBootstrapper AppBootstrapper { get; protected set; }

        /// <summary>
        /// Gets or sets the window layout.
        /// </summary>
        public IWindowLayoutViewModel WindowLayoutViewModel { get; protected set; }

        /// <inheritdoc />
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            WindowLayoutViewModel.Load.InvokeCommand();
        }

        /// <summary>The window_ closed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            // TODO: Remove once avalonDock fixes the bug described here : http://stackoverflow.com/questions/37834945/unhandled-system-componentmodel-win32exception-when-using-avalondock-2-0
            Application.Current.Dispatcher.ShutdownFinished += (o, args) => Environment.Exit(0);
            Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            WindowLayoutViewModel.Save.InvokeCommand();
        }
    }
}