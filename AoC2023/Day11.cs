using System.Text;

namespace AoC2023
{
    internal class Day11
    {
        public static long GetSum(string[] input, int expand)
        {
            var galaxies = new List<(int row, int col)>();
            var emptyRows = new List<int>();
            var emptyCols = new List<int>();

            // get empty rows
            for (int i = 0; i < input.Length; i++)
                if (!input[i].Contains('#'))
                    emptyRows.Add(i);

            // get empty cols
            for (int i = 0; i < input[0].Length; i++)
                if (input.All(line => line[i] == '.'))
                    emptyCols.Add(i);

            // get galaxies
            for (int i = 0; i < input.Length; i++)
                for (int j = 0; j < input[i].Length; j++)
                    if (input[i][j] == '#')
                        galaxies.Add((i, j));

            long sum = 0;
            for (int i = 0; i < galaxies.Count - 1; i++)
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    sum += Math.Abs(galaxies[i].row - galaxies[j].row) + Math.Abs(galaxies[i].col - galaxies[j].col);
                    sum += emptyRows.Where(row => row > galaxies[i].row && row < galaxies[j].row).Count() * (expand - 1);
                    sum += emptyCols.Where(col => (col > galaxies[i].col && col < galaxies[j].col) || (col > galaxies[j].col && col < galaxies[i].col)).Count() * (expand - 1);
                }

            return sum;
        }
        public static long Part1(string[] input) => GetSum(input, 2);
        public static long Part2(string[] input) => GetSum(input, 1000000);
    }
}
