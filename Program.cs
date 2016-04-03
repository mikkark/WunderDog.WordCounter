using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace WunderDog.WordFinder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<Benchmarker>();

            //Console.ReadKey();

            //return;

            Stopwatch w = new Stopwatch();
            w.Start();

            var finder = Run();

            w.Stop();

            Console.WriteLine("finished in " + w.ElapsedMilliseconds + "ms.");
            Console.WriteLine(finder.FoundWords.Count);
            Console.WriteLine(finder.FoundWordsCount);

            Console.ReadKey();
        }

        internal static WordFinder Run()
        {
            string plane0 = "AJFEAPUWOGMRMNXK";
            string plane1 = "DNSIFODSJEGIWKPR";
            string plane2 = "EQMFRKIDDMIREOSD";
            string plane3 = "RTSLDKPISPOIJQDT";

            var words = File.ReadAllLines(@"words.txt");

            var finder = new WordFinder(words.ToList(), new List<List<char>>()
            {
                plane0.ToCharArray().ToList(),
                plane1.ToCharArray().ToList(),
                plane2.ToCharArray().ToList(),
                plane3.ToCharArray().ToList()
            });

            finder.FindWords();

            return finder;
        }
    }

    public class Benchmarker
    {
        [Benchmark]
        public void RunFullTestWithBenchmark()
        {
            Program.Run();
        }
    }
}