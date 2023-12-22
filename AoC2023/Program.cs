using System.Diagnostics;

namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var sw = Stopwatch.StartNew();
            var input = File.ReadAllText("input15.txt");
            Console.WriteLine(Day15.Part2(input).ToString());
            Console.WriteLine(sw.Elapsed.ToString());
        }
    }
}
