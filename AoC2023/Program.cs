using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllLines("input14Test.txt");
            Console.WriteLine(Day14.Part2(input).ToString());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
