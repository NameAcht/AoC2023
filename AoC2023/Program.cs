namespace AoC2023
{
    internal class Program
    {
        public static void Main()
        {
            var input = File.ReadAllLines("input02.txt");
            Console.WriteLine(Day02.Part2(input).ToString());
        }
    }
}
