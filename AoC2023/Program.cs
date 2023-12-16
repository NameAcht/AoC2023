using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllLines("input11Test.txt");
            Console.WriteLine(Day11.Part1(input).ToString());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
