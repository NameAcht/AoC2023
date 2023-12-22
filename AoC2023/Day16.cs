using System.Reflection.Metadata.Ecma335;
using CoordSet = System.Collections.Generic.HashSet<(int row, int col)>;
using StateSet = System.Collections.Generic.HashSet<(int, int, AoC2023.Day16.Direction)>;

namespace AoC2023
{
	internal class Day16
	{
        public enum Direction
        {
            Up, Left, Down, Right
        }
        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
        public static void Traverse(string[] map, StateSet set, CoordSet energized, int row, int col, Direction dir)
        {
            // move to new location
            switch (dir)
            {
                case Direction.Up: row--; break;
                case Direction.Down: row++; break;
                case Direction.Left: col--; break;
                case Direction.Right: col++; break;
                default: throw new NotImplementedException();
            }

            // exit if out of bounds
            if (row < 0 || row >= map.Length || col < 0 || col >= map[row].Length)
                return;

            energized.Add((row, col));

            // prevent infinite loop
            if (!set.Add((row, col, dir)))
                return;
            
            // dir % 2 == 1 -> left or right, else up or down
            switch(((int)dir % 2, map[row][col]))
            {
                case (1, '/'): Traverse(map, set, energized, row, col, (Direction)Mod((int)dir + 1, 4)); break;
                case (0, '/'): Traverse(map, set, energized, row, col, (Direction)Mod((int)dir - 1, 4)); break;

                case (1, '\\'): Traverse(map, set, energized, row, col, (Direction)Mod((int)dir - 1, 4)); break;
                case (0, '\\'): Traverse(map, set, energized, row, col, (Direction)Mod((int)dir + 1, 4)); break;

                case(0, '-'):
                case (1, '|'): 
                    Traverse(map, set, energized, row, col, (Direction)Mod((int)dir - 1, 4));
                    Traverse(map, set, energized, row, col, (Direction)Mod((int)dir + 1, 4)); 
                    break;
                
                // '.' or pointy splitter end
                default: Traverse(map, set, energized, row, col, dir); break;
            }
        }
        public static int TraverseBigStack(string[] map, StateSet set, CoordSet energized, int row, int col, Direction dir)
        {
            // fix pls
            var thread = new Thread(() => Traverse(map, set, energized, row, col, dir), 2000000);
            thread.Start();
            
            while (thread.IsAlive)
                Thread.Sleep(5);

            return energized.Count;
        }
        public static int Part1(string[] map)
        {
            var energized = new CoordSet();
            var set = new StateSet();

            Traverse(map, set, energized, 0, -1, Direction.Right);
            return energized.Count;
        }
        public static int Part2(string[] map)
        {
            int max = 0;

            for (int i = 0; i < map.Length; i++)
            {
                max = Math.Max(max, TraverseBigStack(map, new StateSet(), new CoordSet(), i, -1, Direction.Right));
                max = Math.Max(max, TraverseBigStack(map, new StateSet(), new CoordSet(), i, map[i].Length, Direction.Left));
            }

            for (int i = 0; i < map[0].Length; i++)
            {
                max = Math.Max(max, TraverseBigStack(map, new StateSet(), new CoordSet(), -1, i, Direction.Down));
                max = Math.Max(max, TraverseBigStack(map, new StateSet(), new CoordSet(), map.Length, i, Direction.Up));
            }

            return max;
        }
    }
}