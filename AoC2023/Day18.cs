using System.Text;
using System.Globalization;

namespace AoC2023
{
	internal class Day18
	{
        public enum Direction
        {
            Up, Left, Down, Right
        }
        public struct Coord
        {
            public long row, col;
            public Coord(long row, long col)
            {
                this.row = row;
                this.col = col;
            }
            public Coord MoveUp(long amount) => new Coord(row - amount, col);
            public Coord MoveLeft(long amount) => new Coord(row, col - amount);
            public Coord MoveDown(long amount) => new Coord(row + amount, col);
            public Coord MoveRight(long amount) => new Coord(row, col + amount);
            public override string ToString() => row.ToString() + "," + col .ToString();
        }
        public struct Range
        {
            public Coord start;
            public Coord end;
            public Direction dir;
            public long len;
            public Range(Coord start, Coord end, Direction dir, long len)
            {
                this.start = start;
                this.end = end;
                this.dir = dir;
                this.len = len;            
            }
            public override string ToString() => start.ToString() + " ; " + end.ToString() + " " + dir + " " + len;
        }
        public static HashSet<Coord> DigPath(string[] input, out long minRow, out long minCol, out long maxRow, out long maxCol)
		{
            var set = new HashSet<Coord>();
            minRow = long.MaxValue;
            minCol = long.MaxValue;
            maxRow = long.MinValue;
            maxCol = long.MinValue;

            var curr = new Coord(0, 0);
            set.Add(curr);
            foreach (var line in input)
            {
                long len = long.Parse(line.Split(' ')[1]);
                string dir = line.Split(' ')[0];
                for (long i = 1; i <= len; i++)
                {
                    curr = dir switch
                    {
                        "U" => curr.MoveUp(1),
                        "L" => curr.MoveLeft(1),
                        "D" => curr.MoveDown(1),
                        "R" => curr.MoveRight(1)
                    };
                    set.Add(curr);
                }
                minRow = Math.Min(minRow, curr.row);
                minCol = Math.Min(minCol, curr.col);
                maxRow = Math.Max(maxRow, curr.row);
                maxCol = Math.Max(maxCol, curr.col);
            }
            return set;
        }
        public static List<Range> DigPathRanges(string[] instr, out long minRow, out long minCol, out long maxRow, out long maxCol)
        {
            var ranges = new List<Range>();
            minRow = long.MaxValue;
            minCol = long.MaxValue;
            maxRow = long.MinValue;
            maxCol = long.MinValue;

            var curr = new Coord(0, 0);
            foreach(var line in instr)
            {
                long len = long.Parse(line.Split(' ')[1]);
                var end = line.Split(' ')[0] switch
                {
                    "U" => curr.MoveUp(len),
                    "L" => curr.MoveLeft(len),
                    "D" => curr.MoveDown(len),
                    "R" => curr.MoveRight(len),
                    _ => throw new NotImplementedException()
                };
                var dir = line.Split(' ')[0] switch
                {
                    "U" => Direction.Up,
                    "L" => Direction.Left,
                    "D" => Direction.Down,
                    "R" => Direction.Right,
                    _ => throw new NotImplementedException()
                };

                ranges.Add(new Range(curr, end, dir, len));
                curr = end;
            }
            return ranges;
        }
        public static Coord FindStart(StringBuilder[] map, long minRow, long minCol)
        {
            // start in second row, find polong past first #
            for (int i = 0; i < map[1].Length; i++)
                if (map[1][i] == '#')
                    return new Coord(1, i + 1 + minCol);
            return new Coord(-1, -1);
        }
        public static string[] ParseInstructions(string[] input)
        {
            var instr = new string[input.Length];
            for (long i = 0; i < input.Length; i++)
            {
                string curr = input[i][input[i].Length - 2] switch
                {
                    '0' => "R ",
                    '1' => "D ",
                    '2' => "L ",
                    '3' => "U ",
                    _ => throw new NotImplementedException()
                };
                curr += long.Parse(input[i].Split(' ')[2].Substring(2, 5), NumberStyles.HexNumber).ToString();
                instr[i] = curr;
            }
            return instr;
        }
		public static long Part1(string[] input)
		{
			long sum = 0;
            var set = DigPath(input, out long minRow, out long minCol, out long maxRow, out long maxCol);

            var map = new StringBuilder[maxRow - minRow + 1];
            for (long i = 0; i < map.Length; i++)
                map[i] = new StringBuilder(new string('.', (int)(maxCol - minCol) + 1));

            foreach(var entry in set)
                map[entry.row - minRow][(int)(entry.col - minCol)] = '#';

            var queue = new Queue<Coord>();
            var curr = FindStart(map, minRow, minCol);
            queue.Enqueue(new Coord(1, 1));
            while (queue.Count > 0)
            {
                curr = queue.Dequeue();

                if (!set.Contains(curr.MoveUp(1)))
                {
                    queue.Enqueue(curr.MoveUp(1));
                    set.Add(curr.MoveUp(1));
                }
                if (!set.Contains(curr.MoveLeft(1)))
                {
                    queue.Enqueue(curr.MoveLeft(1));
                    set.Add(curr.MoveLeft(1));
                }
                if (!set.Contains(curr.MoveDown(1)))
                {
                    queue.Enqueue(curr.MoveDown(1));
                    set.Add(curr.MoveDown(1));
                }
                if (!set.Contains(curr.MoveRight(1)))
                {
                    queue.Enqueue(curr.MoveRight(1));
                    set.Add(curr.MoveRight(1));
                }
            }

            foreach(var line in map)
            {
                foreach (var c in line.ToString())
                    Console.Write(c);
                Console.WriteLine();
            }

            return set.Count;
		}
        public static long Part2(string[] input)
        {
            //input = ParseInstructions(input);
            var ranges = DigPathRanges(input, out long minRow, out long minCol, out long maxRow, out long maxCol);
            
            // sort by start col to make search for relevant ranges easier
            ranges.Sort((a, b) => a.start.col.CompareTo(b.start.col));

            var relevant = ranges.Where(range => range.dir == Direction.Right || range.dir == Direction.Down).ToList();

            foreach(var range in relevant)
            {

            }

            return 0;
        }
	}
}