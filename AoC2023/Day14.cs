using System.Text;

namespace AoC2023
{
    internal class Day14
    {
        public static (int row, int col) CalcNorthTiltPos(StringBuilder[] input, int row, int col)
        {
            for (int rowIter = row; rowIter >= 0 && input[rowIter][col] != '#'; rowIter--)
                if (input[rowIter][col] == '.')
                    row--;
            return (row, col);
        }
        public static (int row, int col) CalcWestTiltPos(StringBuilder[] input, int row, int col)
        {
            for (int colIter = col; colIter >= 0 && input[row][colIter] != '#'; colIter--)
                if (input[row][colIter] == '.')
                    col--;
            return (row, col);
        }
        public static (int row, int col) CalcSouthTiltPos(StringBuilder[] input, int row, int col)
        {
            for (int rowIter = row; rowIter < input.Length && input[rowIter][col] != '#'; rowIter++)
                if (input[rowIter][col] == '.')
                    row++;
            return (row, col);
        }
        public static (int row, int col) CalcEastTiltPos(StringBuilder[] input, int row, int col)
        {
            for (int colIter = col; colIter < input[0].Length && input[row][colIter] != '#'; colIter++)
                if (input[row][colIter] == '.')
                    col++;
            return (row, col);
        }
        public static int LoadAfterNorthTilt(string[] input, int row, int col)
        {
            int result = input.Length - row;
            for (int rowIter = row; rowIter >= 0 && input[rowIter][col] != '#'; rowIter--)
                if (input[rowIter][col] == '.')
                    result++;
            return result;
        }
        public static int GetNorthLoad(string[] input, int row, int col)
        {
            return 0;
        }
        public static void UpdateMap(StringBuilder[] mutMap, List<(int row, int col)> rockList)
        {
            foreach (var line in mutMap)
                line.Replace('O', '.');
            foreach (var rock in rockList)
                mutMap[rock.row][rock.col] = 'O';
        }
        public static void Print(StringBuilder[] mutMap)
        {
            foreach (var line in mutMap)
                Console.WriteLine(line);
        }
        public static void NorthTilt(StringBuilder[] mutMap, List<(int row, int col)> rockList)
        {
            for (int row = 0; row < mutMap.Length; row++)
                for (int col = 0; col < mutMap[row].Length; col++)
                    if (mutMap[row][col] == 'O')
                        rockList.Add(CalcNorthTiltPos(mutMap, row, col));
            UpdateMap(mutMap, rockList);
            rockList.Clear();
        }
        public static void WestTilt(StringBuilder[] mutMap, List<(int row, int col)> rockList)
        {
            for (int row = 0; row < mutMap.Length; row++)
                for (int col = 0; col < mutMap[row].Length; col++)
                    if (mutMap[row][col] == 'O')
                        rockList.Add(CalcWestTiltPos(mutMap, row, col));
            UpdateMap(mutMap, rockList);
            rockList.Clear();
        }
        public static void SouthTilt(StringBuilder[] mutMap, List<(int row, int col)> rockList)
        {
            for (int row = 0; row < mutMap.Length; row++)
                for (int col = 0; col < mutMap[row].Length; col++)
                    if (mutMap[row][col] == 'O')
                        rockList.Add(CalcSouthTiltPos(mutMap, row, col));
            UpdateMap(mutMap, rockList);
            rockList.Clear();
        }
        public static void EastTilt(StringBuilder[] mutMap, List<(int row, int col)> rockList)
        {
            for (int row = 0; row < mutMap.Length; row++)
                for (int col = 0; col < mutMap[row].Length; col++)
                    if (mutMap[row][col] == 'O')
                        rockList.Add(CalcEastTiltPos(mutMap, row, col));
            UpdateMap(mutMap, rockList);
            rockList.Clear();
        }
        public static int Part1(string[] input)
        {
            int sum = 0;
            for (int row = 0; row < input.Length; row++)
                for (int col = 0; col < input[row].Length; col++)
                    if (input[row][col] == 'O')
                        sum += LoadAfterNorthTilt(input, row, col);
            return sum;
        }
        public static int Part2(string[] input)
        {
            var rockList = new List<(int row, int col)>();
            var mutMap = input.ToList().ConvertAll(line => new StringBuilder(line)).ToArray();
            
            for (int i = 0; i < 100000; i++)
            {
                

                NorthTilt(mutMap, rockList);
                WestTilt(mutMap, rockList);
                SouthTilt(mutMap, rockList);
                EastTilt(mutMap, rockList);
            }

            Print(mutMap);

            return 0;
        }
    }
}
