using System.Text;

namespace AoC2023
{
    internal class Day11
    {
        public static string[] ExpandMap(string[] input)
        {
            var mutMap = input.ToList().ConvertAll(line => new StringBuilder(line));

            // add lines
            for (int i = 0; i < mutMap.Count - 1; i++)
            {
                var line = mutMap[i];
                if (!line.ToString().Contains('#'))
                {
                    mutMap.Insert(i, new StringBuilder(new string('.', line.Length)));
                    i++;
                }
            }

            // add cols
            for (int i = 0; i < mutMap[0].Length - 1; i++)
            {
                if (!mutMap.Any(line => line[i] == '#'))
                {
                    foreach (var line in mutMap)
                        line.Insert(i, '.');
                    i++;
                }
            }

            return mutMap.ConvertAll(line => line.ToString()).ToArray();
        }
        public static int Part1(string[] input)
        {
            var map = ExpandMap(input);

            var galaxies = new List<(int row, int col)>();

            foreach (var line in map)
                Console.WriteLine(line);

            return 0;
        }
        public static int Part2(string[] input)
        {


            return 0;
        }
    }
}
