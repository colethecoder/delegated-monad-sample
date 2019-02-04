using System;
using static DelegatedMonadSample.SuperThing;

namespace DelegatedMonadSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = from _1 in SetLogger(x => Console.WriteLine(x))
                          from _2 in TellLog("Started")
                          from _3 in IncreaseTally(99)
                          from _4 in TellLog("I just added 99")
                          from _5 in IncreaseTally(88)
                          from _6 in TellLog("I just added 88")
                          from t  in GetTally
                          from _7 in TellLog($"Total: {t}")
                          from _8 in Exit()
                          from _9 in TellLog("Should never write this!!!")
                          select "END";

            var result = process(SuperState.Empty);

            Console.ReadLine();

        }
    }
}
