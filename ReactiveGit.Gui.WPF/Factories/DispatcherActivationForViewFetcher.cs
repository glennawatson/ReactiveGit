namespace ReactiveGit.Gui.WPF.Factories
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Reflection;
    using System.Windows.Threading;

    using ReactiveUI;

#if NETFX_CORE
using Windows.UI.Xaml;
#endif

    public class DispatcherActivationForViewFetcher : IActivationForViewFetcher
    {
        public int GetAffinityForView(Type view)
        {
            return (typeof(FrameworkElement).GetTypeInfo().IsAssignableFrom(view.GetTypeInfo())) ? 20 : 0;
        }

        public IObservable<bool> GetActivationForView(IActivatable view)
        {
            var fe = view as FrameworkElement;

            if (fe == null)
                return Observable.Empty<bool>();
#if WINDOWS_UWP
            var viewLoaded = WindowsObservable.FromEventPattern<FrameworkElement, object>(x => fe.Loading += x,
                x => fe.Loading -= x).Select(_ => true);
#else
            var viewLoaded = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(x => fe.Loaded += x,
                x => fe.Loaded -= x).Select(_ => true);
#endif

            var viewUnloaded = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(x => fe.Unloaded += x,
                x => fe.Unloaded -= x).Select(_ => false);

            var dispatcherShutdown =
                Observable.FromEventPattern(
                    x => Dispatcher.CurrentDispatcher.ShutdownStarted += x,
                    x => Dispatcher.CurrentDispatcher.ShutdownStarted -= x).Select(_ => false);

            return viewLoaded
                .Merge(viewUnloaded)
                .Merge(dispatcherShutdown)
                .Select(b => b ? fe.WhenAnyValue(x => x.IsHitTestVisible).SkipWhile(x => !x) : Observable.Return(false))
                .Switch()
                .DistinctUntilChanged();
        }
    }
}
