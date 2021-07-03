using System;
using BenchmarkDotNet.Running;

namespace RandomThinking.BenchMarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
