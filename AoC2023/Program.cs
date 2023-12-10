using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllLines("input05.txt");
            Console.WriteLine(Day05.Part2(input).ToString());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
