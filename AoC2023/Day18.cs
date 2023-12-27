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
        }
        public struct Range
        {
            public Coord start;
            public Coord end;
            public Direction dir;
            public Range(Coord start, Coord end, Direction dir)
            {
                this.start = start;
                this.end = end;
                this.dir = dir;
            }
            // works only on right range and left range
            public long RowOverlapAndReconstruct(Range left, List<Range> rRanges)
            {
                // left range contains right range
                if (start.col >= left.end.col && end.col <= left.start.col)
                    return end.col - start.col + 1;
                // right range contains left range
                else if (left.end.col >= start.col && left.start.col <= end.col)
                {
                    rRanges.Add(new Range(new Coord(start.row, start.col), new Coord(start.row, left.end.col - 1), Direction.Right));
                    rRanges.Add(new Range(new Coord(start.row, left.start.col + 1), new Coord(start.row, end.col), Direction.Right));
                    return left.start.col - left.end.col + 1;
                }
                // right bound partial overlap
                else if (left.end.col >= start.col && left.end.col <= end.col)
                {
                    rRanges.Add(new Range(new Coord(start.row, start.col), new Coord(start.row, left.end.col - 1), Direction.Right));
                    return end.col - left.end.col + 1;
                }
                // left bound partial overlap
                else if (left.start.col >= start.col && left.start.col <= end.col)
                {
                    rRanges.Add(new Range(new Coord(start.row, left.start.col + 1), new Coord(start.row, end.col), Direction.Right));
                    return left.start.col - start.col + 1;
                }
                // bug
                return -1;
            }
        }
        public static List<Range> DigPathRanges(string[] instr)
        {
            var ranges = new List<Range>();

            var curr = new Coord(0, 0);
            for (int i = 0; i < instr.Length; i++)
            {
                var line = instr[i];

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

                // if right range, and next range is down range, subtract 1 pixel from end of current range
                // however, keep old position for start of next range
                // same thing with right range, and previous range being up range, add 1 to start
                if (dir == Direction.Right)
                {
                    bool nextDown = instr[i + 1].Split(' ')[0] == "D";
                    bool prevUp = i == 0 ? true : instr[i - 1].Split(' ')[0] == "U";

                    if (nextDown && prevUp)
                        ranges.Add(new Range(new Coord(curr.row, curr.col + 1), new Coord(end.row, end.col - 1), dir));
                    else if (nextDown)
                        ranges.Add(new Range(curr, new Coord(end.row, end.col - 1), dir));
                    else if (prevUp)
                        ranges.Add(new Range(new Coord(curr.row, curr.col + 1), end, dir));
                    else
                        ranges.Add(new Range(curr, end, dir));
                }
                else
                    ranges.Add(new Range(curr, end, dir));

                curr = end;
            }
            return ranges;
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
        public static long SumEdges(string[] input)
        {
            long sum = 0;
            foreach (var line in input)
                sum += long.Parse(line.Split(' ')[1]);
            return sum;
        }
        public static Range GetFirstOverlapRange(Range rRange, List<Range> lRanges)
        {
            // start or end of left range inbetween right range, or left range contains right range and left range is below right range
            return lRanges.First(lRange => 
                ((lRange.start.col >= rRange.start.col && lRange.start.col <= rRange.end.col) 
                || (lRange.end.col >= rRange.start.col && lRange.end.col <= rRange.end.col)
                || (lRange.end.col <= rRange.start.col && lRange.start.col >= rRange.end.col))
                && lRange.start.row > rRange.start.row);
        }
        public static long Solve(string[] input)
        {
            var ranges = DigPathRanges(input);
            // sum up edges at beginning, add inside area later
            long sum = SumEdges(input);
            // sorting by row allows iterating over the overlap ranges later
            ranges.Sort((a, b) => a.start.row.CompareTo(b.start.row));

            var rRanges = ranges.Where(range => range.dir == Direction.Right).ToList();
            var lRanges = ranges.Where(range => range.dir == Direction.Left).ToList();
      
            while (rRanges.Count > 0)
            {
                var rRange = rRanges.First();
                rRanges.RemoveAt(0);

                // added range with nothing left
                if (rRange.start.col > rRange.end.col)
                    continue;

                var lRange = GetFirstOverlapRange(rRange, lRanges);

                long mul = lRange.start.row - rRange.start.row - 1;
                var overlap = rRange.RowOverlapAndReconstruct(lRange, rRanges);

                sum += overlap * mul;
            }

            return sum;
        }
        public static long Part1(string[] input) => Solve(input);
        public static long Part2(string[] input) => Solve(ParseInstructions(input));
    }
}