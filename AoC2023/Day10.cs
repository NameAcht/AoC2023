using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Xml;

namespace AoC2023
{
    internal class Day10
    {
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

            var currDir = GetStartDirection(map, currRow, currCol, legend);

            var isLoop = new HashSet<(int row, int col)>();

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

            Print(map, isLoop);

            return steps / 2;
        }
    }
}
