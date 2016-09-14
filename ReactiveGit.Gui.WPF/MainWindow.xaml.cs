// <copyright file="MainWindow.xaml.cs" company="Glenn Watson">
// Copyright (c) Glenn Watson. All rights reserved.
// </copyright>

namespace ReactiveGit.Gui.WPF
{
    using System.Windows;

    using AutoDependencyPropertyMarker;

    using Base.ViewModel;

    using ReactiveGit.Gui.Base.Model.Factories;
    using ReactiveGit.Gui.Base.ViewModel.Factories;

    using ReactiveUI;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : IViewFor<IMainViewModel>
    {
        /// <summary>Initializes a new instance of the <see cref="MainWindow"/> class.</summary>
        public MainWindow()
        {
            this.InitializeComponent();
            Application.Current.Dispatcher.InvokeAsync(() => this.ViewModel = new MainViewModel(new WpfFolderSelector(), new DefaultRepositoryDetailsFactory(), new DefaultRepositoryViewModelFactory()));
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
                this.ViewModel = (IMainViewModel)value;
            }
        }

        /// <inheritdoc />
        [AutoDependencyProperty]
        public IMainViewModel ViewModel { get; set; }
    }
}