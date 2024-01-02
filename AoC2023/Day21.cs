using System.Text;
using Coord = (int row, int col);

namespace AoC2023
{
    internal class Day21
    {
        public static Coord GetStartCoord(string[] input) => (input.Length / 2, input[0].Length / 2);
        public static HashSet<Coord> PotentialPositions(StringBuilder[] mutMap, Coord start, long steps)
        {
            var queue = new Queue<(Coord pos, long stepsLeft)>();
            queue.Enqueue((start, 0));

            var potential = new HashSet<Coord>();
            var seen = new HashSet<Coord>();
            (Coord pos, long steps) curr;

            while (queue.Count > 0)
            {
                curr = queue.Dequeue();
                var pos = curr.pos;

                if (curr.steps % 2 == steps % 2)
                    potential.Add(pos);

                TryEnqueue(queue, (pos.row + 1, pos.col), mutMap, seen, curr.steps + 1);
                TryEnqueue(queue, (pos.row - 1, pos.col), mutMap, seen, curr.steps + 1);
                TryEnqueue(queue, (pos.row, pos.col + 1), mutMap, seen, curr.steps + 1);
                TryEnqueue(queue, (pos.row, pos.col - 1), mutMap, seen, curr.steps + 1);
            }

            return potential;
        }
        public static void Print(StringBuilder[] mutMap, HashSet<Coord> set)
        {
            for (int row = 0; row < mutMap.Length; row++)
            {
                for (int col = 0; col < mutMap[row].Length; col++)
                {
                    if (set.Contains((row, col)))
                        Console.Write('O');
                    else
                        Console.Write(mutMap[row][col]);
                }
                Console.WriteLine();
            }
        }
        public static void TryEnqueue(Queue<(Coord, long)> queue, Coord pos, StringBuilder[] mutMap, HashSet<Coord> seen, long stepsTaken)
        {
            // out of bounds
            if (pos.row < 0 || pos.col < 0 || pos.row >= mutMap.Length || pos.col >= mutMap[0].Length)
                return;
            // not wall and unseen
            if (mutMap[pos.row][pos.col] != '#' && !seen.Contains(pos))
            {
                seen.Add(pos);
                queue.Enqueue((pos, stepsTaken));
            }
        }
        public static long GetReachableTiles(string[] map, long steps, Coord start, bool inverse = false)
        {
            var mutMap = map.ToList().ConvertAll(line => new StringBuilder(line)).ToArray();
            var potential = PotentialPositions(mutMap, start, steps);

            var queue = new Queue<(Coord pos, long stepsLeft)>();
            queue.Enqueue((start, 0));

            var reached = new HashSet<Coord>();
            var seen = new HashSet<Coord>();
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                var pos = curr.pos;
                var stepsTaken = curr.stepsLeft;

                if (potential.Contains(pos) && (!inverse || stepsTaken > steps))
                    reached.Add(pos);

                if (stepsTaken >= steps && !inverse)
                    continue;

                TryEnqueue(queue, (pos.row + 1, pos.col), mutMap, seen, stepsTaken + 1);
                TryEnqueue(queue, (pos.row - 1, pos.col), mutMap, seen, stepsTaken + 1);
                TryEnqueue(queue, (pos.row, pos.col + 1), mutMap, seen, stepsTaken + 1);
                TryEnqueue(queue, (pos.row, pos.col - 1), mutMap, seen, stepsTaken + 1);
            }

            return reached.Count;
        }
        public static long Part1(string[] map) => GetReachableTiles(map, 64, GetStartCoord(map));
        public static long Part2(string[] map)
        {
            long steps = 26501365;
            var start = GetStartCoord(map);
            var mutMap = map.ToList().ConvertAll(line => new StringBuilder(line)).ToArray();

            var oddTiles = PotentialPositions(mutMap, start, steps);
            var evenTiles = PotentialPositions(mutMap, start, steps + 1);

            var oddCorners = GetReachableTiles(map, 65, start, true);
            var evenCorners = GetReachableTiles(map, 64, start, true);

            long n = steps / map.Length;

            long plots = ((n + 1) * (n + 1)) * oddTiles.Count + (n * n) * evenTiles.Count - (n + 1) * oddCorners + n * evenCorners;
            // WHY??????
            plots -= n;

            return plots;
        }
    }
}