using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllLines("input21.txt");
            Console.WriteLine(Day21.Part2(input).ToString());
            //new Day21().Part2(input.ToList());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
