using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace AoC2023
{
    internal class Day10
    {
        const char CONNECTOR = '+';
        public enum Direction
        {
            Up, Right, Down, Left
        }
        public static void Print(string[] map, HashSet<(int row, int col)> isLoop)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if(isLoop.Contains((i, j)))
                        Console.ForegroundColor = ConsoleColor.Blue;

                    Console.Write(map[i][j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }
        public static Direction GetStartDirection(string[] map, int startRow, int startCol, Dictionary<(char pipe, Direction entry), Direction> legend)
        {
            char up = startRow > 0 ? map[startRow - 1][startCol] : '.';
            char down = startRow < map.Length - 1 ? map[startRow + 1][startCol] : '.';
            char left = startCol > 0 ? map[startRow][startCol - 1] : '.';
            char right = startCol < map[startRow].Length - 1 ? map[startRow][startCol + 1] : '.';


            if (legend.TryGetValue((up, Direction.Up), out var dir))
                return Direction.Up;
            if (legend.TryGetValue((down, Direction.Down), out dir))
                return Direction.Down;
            if (legend.TryGetValue((right, Direction.Right), out dir))
                return Direction.Right;
            if (legend.TryGetValue((left, Direction.Left), out dir))
                return Direction.Left;
            throw new NotImplementedException("bruh");
        }
        public static char GetStartChar(string[] map, int startRow, int startCol)
        {
            char up = startRow > 0 ? map[startRow - 1][startCol] : '.';
            char down = startRow < map.Length - 1 ? map[startRow + 1][startCol] : '.';
            char left = startCol > 0 ? map[startRow][startCol - 1] : '.';
            char right = startCol < map[startRow].Length - 1 ? map[startRow][startCol + 1] : '.';

            bool isUp = false;
            bool isDown = false;
            bool isLeft = false;
            bool isRight = false;

            if (up == 'F' || up == '|' || up == '7')
                isUp = true;
            if (down == 'J' || down == '|' || down == 'L')
                isDown = true;
            if (left == 'F' || left == '-' || left == 'L')
                isLeft = true;
            if (right == 'J' || right == '-' || right == '7')
                isRight = true;

            char ret = (isUp, isDown, isLeft, isRight) switch
            {
                (true, true, false, false) => '|',
                (true, false, true, false) => 'J',
                (true, false, false, true) => 'L',
                (false, true, true, false) => '7',
                (false, true, false, true) => 'F',
                (false, false, true, true) => '-',
                _ => throw new NotImplementedException()
            };
            return ret;
        }
        public static void EditExtensionChar(StringBuilder[] expandedMap, int rowIter, int colIter)
        {
            char up = expandedMap[rowIter - 1][colIter];
            char down = expandedMap[rowIter + 1][colIter];
            char left = expandedMap[rowIter][colIter - 1];
            char right = expandedMap[rowIter][colIter + 1];

            bool connectsUp = up == '|' || up == 'F' || up == '7';
            bool connectsDown = down == '|' || down == 'J' || down == 'L';
            bool connectsLeft = left == '-' || left == 'F' || left == 'L';
            bool connectsRight = right == '-' || right == 'J' || right == '7';

            expandedMap[rowIter][colIter] = (connectsUp, connectsDown, connectsLeft, connectsRight) switch
            {
                (true, true, false, false) => '|',
                (false, false, true, true) => '-',
                _ => '+'
            };
        }
        public static int Part1(string[] map)
        {
            var currRow = map.ToList().FindIndex(line => line.Contains('S'));
            var currCol = map[currRow].IndexOf('S');

            var legend = new Dictionary<(char pipe, Direction entry), Direction>()
            {
                [('|', Direction.Up)] = Direction.Up,
                [('|', Direction.Down)] = Direction.Down,
                [('-', Direction.Left)] = Direction.Left,
                [('-', Direction.Right)] = Direction.Right,
                [('L', Direction.Down)] = Direction.Right,
                [('L', Direction.Left)] = Direction.Up,
                [('J', Direction.Right)] = Direction.Up,
                [('J', Direction.Down)] = Direction.Left,
                [('7', Direction.Right)] = Direction.Down,
                [('7', Direction.Up)] = Direction.Left,
                [('F', Direction.Left)] = Direction.Down,
                [('F', Direction.Up)] = Direction.Right,
            };

            var currDir = GetStartDirection(map, currRow, currCol, legend);
        
            int steps = 0;
            do
            {
                switch (currDir)
                {
                    case Direction.Up: currRow--; break;
                    case Direction.Down: currRow++; break;
                    case Direction.Left: currCol--; break;
                    case Direction.Right: currCol++; break;
                    default: throw new NotImplementedException();
                }
                var newPipe = map[currRow][currCol];
                legend.TryGetValue((newPipe, currDir), out currDir);
                steps++;
            } while (map[currRow][currCol] != 'S');

            return steps / 2;
        }
        public static int Part2(string[] map)
        {
            var currRow = map.ToList().FindIndex(line => line.Contains('S'));
            var currCol = map[currRow].IndexOf('S');

            var legend = new Dictionary<(char pipe, Direction entry), Direction>()
            {
                [('|', Direction.Up)] = Direction.Up,
                [('|', Direction.Down)] = Direction.Down,
                [('-', Direction.Left)] = Direction.Left,
                [('-', Direction.Right)] = Direction.Right,
                [('L', Direction.Down)] = Direction.Right,
                [('L', Direction.Left)] = Direction.Up,
                [('J', Direction.Right)] = Direction.Up,
                [('J', Direction.Down)] = Direction.Left,
                [('7', Direction.Right)] = Direction.Down,
                [('7', Direction.Up)] = Direction.Left,
                [('F', Direction.Left)] = Direction.Down,
                [('F', Direction.Up)] = Direction.Right,
            };



            var expandedMap = new StringBuilder[map.Length * 2 + 1];
            expandedMap[0] = new StringBuilder(new string(CONNECTOR, map[0].Length));

            for (int i = 0; i < expandedMap.Length - 1; i++)
            {
                if (i % 2 != 0)
                    expandedMap[i + 1] = new StringBuilder(new string(CONNECTOR, map[0].Length));
                else
                    expandedMap[i + 1] = new StringBuilder(map[i / 2]);
            }

            foreach(var line in expandedMap)
            {
                for (int i = 0; i < line.Length; i += 2)
                    line.Insert(i, CONNECTOR);
                line.Append(CONNECTOR);
            }

            // set currRow and currCol to start index on new map, edit out S for extension pipe interpreting
            currRow = expandedMap.ToList().FindIndex(line => line.ToString().Contains('S'));
            currCol = expandedMap[currRow].ToString().IndexOf('S');
            expandedMap[currRow][currCol] = GetStartChar(map, currRow / 2, currCol / 2);

            // add in extension pipes
            for (int rowIter = 1; rowIter < expandedMap.Length - 1; rowIter++)
                for (int colIter = 1; colIter < expandedMap[rowIter].Length - 1; colIter++)
                    if (expandedMap[rowIter][colIter] == '+')
                        EditExtensionChar(expandedMap, rowIter, colIter);

            // edit S back in for traversing
            expandedMap[currRow][currCol] = 'S';

            var currDir = GetStartDirection(expandedMap.ToList().ConvertAll(line => line.ToString()).ToArray(), currRow, currCol, legend);
            var isLoop = new HashSet<(int row, int col)>();

            // traverse new map
            do
            {
                switch (currDir)
                {
                    case Direction.Up: currRow--; break;
                    case Direction.Down: currRow++; break;
                    case Direction.Left: currCol--; break;
                    case Direction.Right: currCol++; break;
                    default: throw new NotImplementedException();
                }
                var newPipe = expandedMap[currRow][currCol];
                legend.TryGetValue((newPipe, currDir), out currDir);

                isLoop.Add((currRow, currCol));

            } while (expandedMap[currRow][currCol] != 'S');


            foreach(var line in expandedMap)
            {
                foreach(var c in line.ToString())
                {
                    if(c == '.')
                    {

                    }
                }
            }

            Print(expandedMap.ToList().ConvertAll(line => line.ToString()).ToArray(), isLoop);

            return 0;
        }
    }
}
