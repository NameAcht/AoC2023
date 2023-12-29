using System.Text;
using Coord = (int row, int col);

namespace AoC2023
{
	internal class Day21
	{
        public static Coord GetStartCoord(string[] input)
        {
            for (int row = 0; row < input.Length; row++)
                for (int col = 0; col < input[row].Length; col++)
                    if (input[row][col] == 'S')
                        return (row, col);
            return (-1, -1);
        }
        public static int MapPotentialPositions(StringBuilder[] mutMap, Coord start, int steps)
        {
            int sum = 0;
            for (int row = 0; row < mutMap.Length; row++)
            {
                for (int col = 0; col < mutMap[row].Length; col++)
                {
                    var dist = Math.Abs(start.row - row) + Math.Abs(start.col - col);
                    if (dist % 2 == 0 && dist <= steps && mutMap[row][col] != '#')
                    {
                        sum++;
                        mutMap[row][col] = 'O';
                    }
                }
            }
            return sum;
        }
        public static void Print(StringBuilder[] mutMap)
        {
            foreach(var line in mutMap)
                Console.WriteLine(line);
        }
        public static int Part1(string[] input)
		{
            int steps = 64;
            var start = GetStartCoord(input);
			var mutMap = input.ToList().ConvertAll(line => new StringBuilder(line)).ToArray();
            int ret = MapPotentialPositions(mutMap, start, steps);
            Print(mutMap);

            var queue = new Queue<(Coord coord, int stepsLeft)>();
            queue.Enqueue((start, steps));

            var set = new HashSet<Coord>();
            var seen = new HashSet<Coord>();
            while(queue.Count > 0)
            {
                var curr = queue.Dequeue();
                var pos = curr.coord;
                var stepsLeft = curr.stepsLeft;

                if (mutMap[pos.row][pos.col] == 'O')
                    set.Add(pos);

                if (stepsLeft == 0)
                    continue;

                if (mutMap[pos.row + 1][pos.col] != '#' && !seen.Contains((pos.row + 1, pos.col)))
                {
                    seen.Add((pos.row + 1, pos.col));
                    queue.Enqueue(((pos.row + 1, pos.col), stepsLeft - 1));
                }

                if (mutMap[pos.row - 1][pos.col] != '#' && !seen.Contains((pos.row - 1, pos.col)))
                {
                    seen.Add((pos.row - 1, pos.col));
                    queue.Enqueue(((pos.row - 1, pos.col), stepsLeft - 1));
                }

                if (mutMap[pos.row][pos.col + 1] != '#' && !seen.Contains((pos.row, pos.col + 1)))
                {
                    seen.Add((pos.row, pos.col + 1));
                    queue.Enqueue(((pos.row, pos.col + 1), stepsLeft - 1));
                }

                if (mutMap[pos.row][pos.col - 1] != '#' && !seen.Contains((pos.row, pos.col - 1)))
                {
                    seen.Add((pos.row, pos.col - 1));
                    queue.Enqueue(((pos.row, pos.col - 1), stepsLeft - 1));
                }
            }

            return set.Count;
		}
	}
}