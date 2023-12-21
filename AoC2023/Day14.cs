using System.Text;
using Map = System.Collections.Generic.List<System.Text.StringBuilder>;

namespace AoC2023
{
    internal class Day14
    {
        public enum Directions
        {
            North, West, South, East
        }
        public static (int row, int col) CalcNorthTiltPos(Map input, int row, int col)
        {
            for (int rowIter = row; rowIter >= 0 && input[rowIter][col] != '#'; rowIter--)
                if (input[rowIter][col] == '.')
                    row--;
            return (row, col);
        }
        public static (int row, int col) CalcWestTiltPos(Map input, int row, int col)
        {
            for (int colIter = col; colIter >= 0 && input[row][colIter] != '#'; colIter--)
                if (input[row][colIter] == '.')
                    col--;
            return (row, col);
        }
        public static (int row, int col) CalcSouthTiltPos(Map input, int row, int col)
        {
            for (int rowIter = row; rowIter < input.Count && input[rowIter][col] != '#'; rowIter++)
                if (input[rowIter][col] == '.')
                    row++;
            return (row, col);
        }
        public static (int row, int col) CalcEastTiltPos(Map input, int row, int col)
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
        public static int GetNorthLoad(Map input)
        {
            int sum = 0;
            for (int rowIter = 0; rowIter < input.Count; rowIter++)
                for (int colIter = 0; colIter < input[rowIter].Length; colIter++)
                    if (input[rowIter][colIter] == 'O')
                        sum += input.Count - rowIter;
            return sum;
        }
        public static void UpdateMap(Map mutMap, List<(int row, int col)> rockList)
        {
            foreach (var line in mutMap)
                line.Replace('O', '.');
            foreach (var rock in rockList)
                mutMap[rock.row][rock.col] = 'O';
        }
        public static void Tilt(Map mutMap, List<(int row, int col)> rockList, Directions dir)
        {
            for (int row = 0; row < mutMap.Count; row++)
                for (int col = 0; col < mutMap[row].Length; col++)
                    if (mutMap[row][col] == 'O')
                        switch (dir)
                        {
                            case Directions.North: rockList.Add(CalcNorthTiltPos(mutMap, row, col)); break;
                            case Directions.West: rockList.Add(CalcWestTiltPos(mutMap, row, col)); break;
                            case Directions.South: rockList.Add(CalcSouthTiltPos(mutMap, row, col)); break;
                            case Directions.East: rockList.Add(CalcEastTiltPos(mutMap, row, col)); break;
                        }
            UpdateMap(mutMap, rockList);
            rockList.Clear();
        }
        public static long Hash(Map mutMap)
        {   
            long hash = 0;
            for (int i = 0; i < mutMap.Count; i++)
                hash += mutMap[i].ToString().GetHashCode();
            return hash;
        }
        public static void Cycle(Map mutMap, List<(int, int)> rockList)
        {
            Tilt(mutMap, rockList, Directions.North);
            Tilt(mutMap, rockList, Directions.West);
            Tilt(mutMap, rockList, Directions.South);
            Tilt(mutMap, rockList, Directions.East);
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
            var mutMap = input.ToList().ConvertAll(line => new StringBuilder(line));
            
            var set = new Dictionary<long, int>();

            int iter;
            for (iter = 0; set.TryAdd(Hash(mutMap), iter); iter++)
                Cycle(mutMap, rockList);

            int cycleLength = iter - set[Hash(mutMap)];
            int cycleOffset = (1000000000 - iter) % cycleLength;

            for (int i = 0; i < cycleOffset; i++)
                Cycle(mutMap, rockList);

            return GetNorthLoad(mutMap);
        }
    }
}
