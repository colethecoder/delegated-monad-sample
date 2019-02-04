using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatedMonadSample
{
    public class SuperState<A>
    {
        public readonly A Value;
        public readonly SuperState State;

        public SuperState(A value, SuperState state)
        {
            Value = value;
            State = state;
        }
    }

    public class SuperState
    {
        public readonly bool HasExited;
        public readonly int Tally;
        public readonly Action<string> Logger;

        public SuperState(bool hasExited, int tally, Action<string> logger)
        {
            HasExited = hasExited;
            Tally = tally;
            Logger = logger;
        }

        public SuperState With(bool? HasExited = null, int? Tally = null, Action<string> Logger = null) =>
            new SuperState(
                HasExited ?? this.HasExited,
                Tally ?? this.Tally,
                Logger ?? this.Logger);

        public static SuperState Empty = new SuperState(false, 0, null);

        public SuperState IncreaseTally(int increment) =>
            With(Tally: this.Tally + increment);

        public SuperState Exit() =>
            With(HasExited: true);

        public SuperState SetLogger(Action<string> logger) =>
            With(Logger: logger);
    }
}
