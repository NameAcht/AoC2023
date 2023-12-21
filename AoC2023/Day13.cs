namespace AoC2023
{
    internal class Day13
    {
        public static int ProcessRow(string[] map, int row)
        {
            int sum = 0;
            if (map[row] == map[row + 1])
            {
                if (RowReflectionErrors(map, row) == 1)
                    sum += row + 1;
            }
            else if (map[row].Where((c, k) => c != map[row + 1][k]).Count() == 1)
                if (RowReflectionErrors(map, row) == 0)
                    sum += row + 1;
            return sum;
        }
        public static int ProcessCol(string[] map, int col)
        {
            int result = 0;
            // adjacent col match
            if (map.All(line => line[col] == line[col + 1]))
            {
                if (ColReflectionErrors(map, col) == 1)
                    result += col + 1;
            }
            // adjacent col 1 error
            else if (map.Where(line => line[col] != line[col + 1]).Count() == 1)
                if (ColReflectionErrors(map, col) == 0)
                    result += col + 1;
            return result;
        }
        public static bool ColReflects(string[] map, int col)
        {
            bool mirrored = true;
            for (int i = 2; col + i < map[0].Length && col - i + 1 >= 0; i++)
                if (!map.All(line => line[col + i] == line[col - i + 1]))
                    mirrored = false;
            return mirrored;
        }
        public static int ColReflectionErrors(string[] map, int col)
        {
            int sum = 0;
            for (int i = 2; col + i < map[0].Length && col - i + 1 >= 0; i++)
            {
                var first = map.Select(line => line[col + i]).ToList();
                var second = map.Select(line => line[col - i + 1]).ToList();
                sum += first.Where((c, iter) => c != second[iter]).Count();
            }
            return sum;
        }
        public static bool RowReflects(string[] map, int row)
        {
            bool mirrored = true;
            for (int i = 2; row + i < map.Length && row - i + 1 >= 0; i++)
                if (map[row + i] != map[row - i + 1])
                    mirrored = false;
            return mirrored;
        }
        public static int RowReflectionErrors(string[] map, int row)
        {
            int sum = 0;
            for (int i = 2; row + i < map.Length && row - i + 1 >= 0; i++)
            {
                string first = map[row + i];
                string second = map[row - i + 1];
                sum += first.Where((c, iter) => c != second[iter]).Count();
            }
            return sum;
        }
        public static List<string[]> ParseMaps(string[] input)
        {
            var result = new List<string[]>();
            while (input.Length > 1)
            {
                result.Add(input.TakeWhile(line => line != string.Empty).ToArray());
                if (result.Last().Length != input.Length)
                    input = input[(result.Last().Length + 1)..];
                else
                    break;
            }
            return result;
        }
        public static int Part1(string[] input)
        {
            int sum = 0;
            var mapList = ParseMaps(input);

            foreach (var map in mapList)
            {
                // check rows
                for (int i = 0; i < map.Length - 1; i++)
                    if (map[i] == map[i + 1] && RowReflects(map, i))    
                        sum += (i + 1) * 100;

                // check cols
                for (int i = 0; i < map[0].Length - 1; i++)
                    if (map.All(line => line[i] == line[i + 1]) && ColReflects(map, i))    
                        sum += i + 1;
            }
            return sum;
        }
        public static int Part2(string[] input)
        {
            int sum = 0;
            var mapList = ParseMaps(input);

            foreach (var map in mapList)
            {
                for (int i = 0; i < map.Length - 1; i++)
                    sum += ProcessRow(map, i) * 100;

                for (int i = 0; i < map[0].Length - 1; i++)
                    sum += ProcessCol(map, i);
            }
            return sum;
        }
    }
}
