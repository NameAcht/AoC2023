﻿using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllLines("input25.txt");
            Console.WriteLine(Day25.Part1(input).ToString());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
