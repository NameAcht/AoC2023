using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllLines("input23.txt");
            Console.WriteLine(Day23.Part2(input).ToString());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
