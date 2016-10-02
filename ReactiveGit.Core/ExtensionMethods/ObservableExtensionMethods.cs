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
            return Observable.Create<Unit>(
                observer =>
                    {
                        return source.Subscribe(
                            _ => { },
                            observer.OnError,
                            () =>
                                {
                                    observer.OnNext(Unit.Default);
                                    observer.OnCompleted();
                                });
                    });
        }

        /// <summary>
        /// Completes a observable, only if the first observable
        /// returns a true value.
        /// </summary>
        /// <param name="canCompleteObservable">The observable to do the sanity check against.</param>
        /// <param name="toCompleteObservable">The observable to complete if the first observable returns true.</param>
        /// <returns>The observable to monitor the progress of the process.</returns>
        public static IObservable<Unit> CompleteIfTrue(
            this IObservable<bool> canCompleteObservable,
            IObservable<Unit> toCompleteObservable)
        {
            return Observable.Create<Unit>(
                async (observer, token) =>
                    {
                        bool proceed = await canCompleteObservable;
                        if (proceed)
                        {
                            toCompleteObservable.Subscribe(
                                        _ => { },
                                        observer.OnError,
                                        () =>
                                {
                                    observer.OnNext(Unit.Default);
                                    observer.OnCompleted();
                                },
                                        token);
                        }
                    });
        }
    }
}