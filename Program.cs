using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WunderDog.WordFinder
{
    internal class Program
    {
        private static void Main(string[] args)
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

            Stopwatch w = new Stopwatch();
            w.Start();

            finder.FindWords();

            w.Stop();

            Console.WriteLine("finished in " + w.ElapsedMilliseconds + "ms.");

            Console.WriteLine(finder.FoundWordsCount);

            //foreach (var found in finder.FoundWords)
            //{
            //    Console.WriteLine(found);
            //}

            Console.ReadKey();
        }
    }
}