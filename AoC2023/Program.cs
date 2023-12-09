namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var input = File.ReadAllLines("input04.txt");
            Console.WriteLine(Day04.Part2(input).ToString());
        }
    }
}
