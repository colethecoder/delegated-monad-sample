using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatedMonadSample
{
    public delegate SuperState<A> SuperThing<A>(SuperState state);

    public static class SuperThing
    {
        public static SuperThing<Unit> SetLogger(Action<string> logger) => s =>
            new SuperState<Unit>(new Unit(), s.SetLogger(logger));

        public static SuperThing<Unit> TellLog(string toLog) => s =>
        {
            s.Logger(toLog);
            return new SuperState<Unit>(new Unit(), s);
        };

        public static SuperThing<Unit> IncreaseTally(int increment) => s =>
            new SuperState<Unit>(new Unit(), s.IncreaseTally(increment));

        public static SuperThing<Unit> Exit() => s =>
        {
            return new SuperState<Unit>(new Unit(), s.Exit());
        };

        public static SuperThing<SuperState> GetSuperState = s =>
            new SuperState<SuperState>(s, s);

        public static SuperThing<int> GetTally = GetSuperState.Select(x => x.Tally);

        public static SuperThing<B> Select<A, B>(this SuperThing<A> ma, Func<A, B> f) => sa =>
        {
            var a = ma(sa);
            if (a.State.HasExited) return new SuperState<B>(default(B), a.State);
            return new SuperState<B>(f(a.Value), a.State);
        };

        public static SuperThing<B> SelectMany<A, B>(this SuperThing<A> ma, Func<A, SuperThing<B>> f) => sa =>
        {
            var a = ma(sa);
            if (a.State.HasExited) return new SuperState<B>(default(B), a.State);

            var b = f(a.Value)(a.State);
            return b;
        };

        public static SuperThing<C> SelectMany<A, B, C>(this SuperThing<A> ma, Func<A, SuperThing<B>> bind, Func<A, B, C> project) => sa =>
        {
            var a = ma(sa);
            if (a.State.HasExited) return new SuperState<C>(default(C), a.State);

            var b = bind(a.Value)(a.State);
            if (b.State.HasExited) return new SuperState<C>(default(C), b.State);

            return new SuperState<C>(project(a.Value, b.Value), b.State);
        };
    }
}
