namespace ReactiveGit.Core.ExtensionMethods
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;

    /// <summary>
    /// A set of extension methods related to observables.
    /// </summary>
    public static class ObservableExtensionMethods
    {
        /// <summary>
        /// Changes a observable just to indicate when it's done rather than any intermediary values.
        /// </summary>
        /// <typeparam name="T">The type of source observable.</typeparam>
        /// <param name="source">The source observable.</param>
        /// <returns>A observable that triggers when done.</returns>
        public static IObservable<Unit> WhenDone<T>(this IObservable<T> source)
        {
            return Observable.Create<Unit>(observer =>
                {
                    return source.Subscribe(_ => { }, observer.OnError, () =>
                        {
                            observer.OnNext(Unit.Default);
                            observer.OnCompleted();
                        });
                });
        }
    }
}