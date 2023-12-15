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
        public static Dictionary<(char pipe, Direction entry), Direction> DirectionsFromState()
        {
            var hashMap = new Dictionary<(char pipe, Direction entry), Direction>()
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
            return hashMap;
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

            var stateMap = DirectionsFromState();

            var bigMap = new StringBuilder[map.Length * 2 + 1];
            bigMap[0] = new StringBuilder(new string(CONNECTOR, map[0].Length));

            // insert missing spaces into map
            for (int i = 0; i < bigMap.Length - 1; i++)
            {
                if (i % 2 != 0)
                    bigMap[i + 1] = new StringBuilder(new string(CONNECTOR, map[0].Length));
                else
                    bigMap[i + 1] = new StringBuilder(map[i / 2]);
            }

            foreach(var line in bigMap)
            {
                for (int i = 0; i < line.Length; i += 2)
                    line.Insert(i, CONNECTOR);
                line.Append(CONNECTOR);
            }

            // set currRow and currCol to start index on new map, edit out S for extension pipe interpreting
            currRow = bigMap.ToList().FindIndex(line => line.ToString().Contains('S'));
            currCol = bigMap[currRow].ToString().IndexOf('S');
            bigMap[currRow][currCol] = GetStartChar(map, currRow / 2, currCol / 2);

            // add in extension pipes
            for (int rowIter = 1; rowIter < bigMap.Length - 1; rowIter++)
                for (int colIter = 1; colIter < bigMap[rowIter].Length - 1; colIter++)
                    if (bigMap[rowIter][colIter] == '+')
                        EditExtensionChar(bigMap, rowIter, colIter);

            // edit S back in for traversing
            bigMap[currRow][currCol] = 'S';

            var currDir = GetStartDirection(bigMap.ToList().ConvertAll(line => line.ToString()).ToArray(), currRow, currCol, stateMap);
            var loopCoords = new HashSet<(int row, int col)>();

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
                var newPipe = bigMap[currRow][currCol];
                stateMap.TryGetValue((newPipe, currDir), out currDir);

                loopCoords.Add((currRow, currCol));
            } while (bigMap[currRow][currCol] != 'S');


            // get tiles outside loop
            var outsideCoords = new HashSet<(int row, int col)>();
            var queue = new Queue<(int row, int col)>();
            queue.Enqueue((0, 0));

            // Bfs to get all outside tiles
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();

                if (outsideCoords.Contains(curr))
                    continue;
                else
                    outsideCoords.Add(curr); 

                // enqueue tiles if inside bounds and not loop
                if (curr.row > 0 && !loopCoords.Contains((curr.row - 1, curr.col)))
                    queue.Enqueue((curr.row - 1, curr.col));
                if (curr.row < bigMap.Length - 1 && !loopCoords.Contains((curr.row + 1, curr.col)))
                    queue.Enqueue((curr.row + 1, curr.col));
                if (curr.col > 0 && !loopCoords.Contains((curr.row, curr.col - 1)))
                    queue.Enqueue((curr.row, curr.col - 1));
                if (curr.col < bigMap[0].Length && !loopCoords.Contains((curr.row, curr.col + 1)))
                    queue.Enqueue((curr.row, curr.col + 1));
            }

            int sum = 0;
            for (int rowIter = 1; rowIter < bigMap.Length - 1; rowIter += 2)
                for (int colIter = 1; colIter < bigMap[rowIter].Length - 1; colIter += 2)
                    if (!loopCoords.Contains((rowIter, colIter)) && !outsideCoords.Contains((rowIter, colIter)))
                        sum++;
            return sum;
        }
    }
}
