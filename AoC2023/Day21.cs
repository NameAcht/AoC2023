using System.Text;
using Coord = (int row, int col);

namespace AoC2023
{
	internal class Day21
	{
        public static long SquareSeries(long n)
        {
            return n == 0 ? 1 : n * 4;
        }
        public static Coord GetStartCoord(string[] input)
        {
            for (int row = 0; row < input.Length; row++)
                for (int col = 0; col < input[row].Length; col++)
                    if (input[row][col] == 'S')
                        return (row, col);
            return (-1, -1);
        }
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

                QueueCoordinate(queue, (pos.row + 1, pos.col), mutMap, seen, curr.steps + 1);
                QueueCoordinate(queue, (pos.row - 1, pos.col), mutMap, seen, curr.steps + 1);
                QueueCoordinate(queue, (pos.row, pos.col + 1), mutMap, seen, curr.steps + 1);
                QueueCoordinate(queue, (pos.row, pos.col - 1), mutMap, seen, curr.steps + 1);
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
        public static void QueueCoordinate(Queue<(Coord, long)> queue, Coord pos, StringBuilder[] mutMap, HashSet<Coord> seen, long stepsTaken)
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
        public static long[] StartToCorners(StringBuilder[] mutMap, Coord start, Coord end)
        {
            var cornerPaths = new long[4];
            Coord[] corners = [(0, 0), (0, mutMap[0].Length - 1), (mutMap.Length - 1, 0), (mutMap.Length - 1, mutMap[0].Length - 1)];

            var seen = new HashSet<Coord>();

            var queue = new Queue<(Coord pos, long steps)>();
            queue.Enqueue((start, 0));
            
            while(queue.Count > 0) 
            {
                var curr = queue.Dequeue();
                var pos = curr.pos;
                var stepsTaken = curr.steps;

                for (int i = 0; i < corners.Length; i++)
                    if (pos == corners[i])
                        cornerPaths[i] = stepsTaken;

                QueueCoordinate(queue, (pos.row + 1, pos.col), mutMap, seen, stepsTaken + 1);
                QueueCoordinate(queue, (pos.row - 1, pos.col), mutMap, seen, stepsTaken + 1);
                QueueCoordinate(queue, (pos.row, pos.col + 1), mutMap, seen, stepsTaken + 1);
                QueueCoordinate(queue, (pos.row, pos.col - 1), mutMap, seen, stepsTaken + 1);
            }

            return cornerPaths;
        }
        public static long StepsForMaxPotential(StringBuilder[] mutMap, Coord start, HashSet<Coord> potential)
        {
            long steps = 0;
            var queue = new Queue<(Coord pos, long stepsLeft)>();
            queue.Enqueue((start, 0));

            var reached = new HashSet<Coord>();
            var seen = new HashSet<Coord>();
            (Coord pos, long stepsLeft) curr;

            while (reached.Count < potential.Count)
            {
                curr = queue.Dequeue();
                var pos = curr.pos;
                steps = curr.stepsLeft;

                if (potential.Contains(pos))
                    reached.Add(pos);

                QueueCoordinate(queue, (pos.row + 1, pos.col), mutMap, seen, steps + 1);
                QueueCoordinate(queue, (pos.row - 1, pos.col), mutMap, seen, steps + 1);
                QueueCoordinate(queue, (pos.row, pos.col + 1), mutMap, seen, steps + 1);
                QueueCoordinate(queue, (pos.row, pos.col - 1), mutMap, seen, steps + 1);
            }

            return steps;
        }
        public static long[] RequiredStepsPerEntrance(StringBuilder[] mutMap, HashSet<Coord> potential)
        {
            // #0#
            // 1#2 -> map edge indexing
            // #3#
            var maxRow = mutMap.Length;
            var maxCol = mutMap[0].Length;
            long[] arr = [
                StepsForMaxPotential(mutMap, (0, maxCol / 2), potential),
                StepsForMaxPotential(mutMap, (maxRow / 2, maxCol), potential),
                StepsForMaxPotential(mutMap, (maxRow, maxCol / 2), potential),
                StepsForMaxPotential(mutMap, (maxRow / 2, 0), potential)
            ];
            return arr;
        }
        public static long GetReachableTiles(string[] map, long maxSteps, Coord start)
        {
            var mutMap = map.ToList().ConvertAll(line => new StringBuilder(line)).ToArray();
            var potential = PotentialPositions(mutMap, start, maxSteps);

            var queue = new Queue<(Coord pos, long stepsLeft)>();
            queue.Enqueue((start, 0));

            var reached = new HashSet<Coord>();
            var seen = new HashSet<Coord>();
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                var pos = curr.pos;
                var stepsTaken = curr.stepsLeft;

                if (potential.Contains(pos))
                    reached.Add(pos);

                if (stepsTaken >= maxSteps)
                    continue;

                QueueCoordinate(queue, (pos.row + 1, pos.col), mutMap, seen, stepsTaken + 1);
                QueueCoordinate(queue, (pos.row - 1, pos.col), mutMap, seen, stepsTaken + 1);
                QueueCoordinate(queue, (pos.row, pos.col + 1), mutMap, seen, stepsTaken + 1);
                QueueCoordinate(queue, (pos.row, pos.col - 1), mutMap, seen, stepsTaken + 1);
            }
            return reached.Count;
        }
        public static long Part1(string[] map) => GetReachableTiles(map, 64, (map.Length / 2, map[0].Length / 2));
        public static long Part2(string[] map)
        {
            long steps = 26501365;
            var start = GetStartCoord(map);
            var mutMap = map.ToList().ConvertAll(line => new StringBuilder(line)).ToArray();
            
            var potential = PotentialPositions(mutMap, start, steps);
            var potentialOdd = PotentialPositions(mutMap, start, steps + 1);

            long mapMoveCost = mutMap.LongLength;
            long initialMoveCost = (mapMoveCost / 2) + 1;

            var reqSteps = RequiredStepsPerEntrance(mutMap, potential);
            var reqStepsOdd = RequiredStepsPerEntrance(mutMap, potentialOdd);
            long maxReq = Math.Max(reqSteps.Max(), reqStepsOdd.Max());


            long plots = 0;
            long currCost = 0;

            int i = 0;
            for (i = 0; currCost < steps; i++)
            {
                plots += SquareSeries(i) * (i % 2 == 0 ? potential.Count : potentialOdd.Count);
                currCost += i == 0 ? initialMoveCost : mapMoveCost;
            }

            var mul = SquareSeries(i);
            var finalSteps = steps - currCost + mapMoveCost;

            // all amounts of tiles, that can be reached per map with the remaining steps
            long[] reachablePerSide = [
                GetReachableTiles(map, finalSteps, (0, map[0].Length / 2)),
                GetReachableTiles(map, finalSteps, (map.Length / 2, map[0].Length)),
                GetReachableTiles(map, finalSteps, (map.Length, map[0].Length / 2)),
                GetReachableTiles(map, finalSteps, (map.Length / 2, 0))
            ];

            // enter from bottom
            plots += reachablePerSide.Max() * (mul / 2 - 1);
            // enter from right
            plots += reachablePerSide[1] * (mul / 4);
            // enter from left
            plots += reachablePerSide[3] * (mul / 4);
            // enter from top
            plots += reachablePerSide[0];

            return plots;
        }
	}
}