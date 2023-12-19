using System.Collections.Immutable;
using Cache = System.Collections.Generic.Dictionary<(string, System.Collections.Immutable.ImmutableStack<int>), long>;

namespace AoC2023
{
    internal class Day12
    {
        public static long ProcessDamagedSpring(string pattern, ImmutableStack<int> nums, Cache cache)
        {
            if (!nums.Any())
                return 0;

            int currGroupSize = nums.Peek();
            nums = nums.Pop();

            // highest potential amount of damaged springs for this group
            int maxDamagedSprings = pattern.TakeWhile(c => c == '#' || c == '?').Count();

            // current group larger than whats possible -> impossible pattern
            if (maxDamagedSprings < currGroupSize)
                return 0;
            // if currGroup == maxSprings == patternLength, theres only one possible group (all damaged)
            // if no groups are left return 0
            else if (pattern.Length == currGroupSize)
                return nums.Any() ? 0 : 1;
            // damaged spring follows current group,
            // for example -> groupSize is 5, pattern is #????#
            // a pattern of 5 is not possible here
            else if (pattern[currGroupSize] == '#')
                return 0;
            // continue to pattern after damaged spring group
            else
                return EvalPattern(pattern[(currGroupSize + 1)..], nums, cache);
        }
        public static long EvalPattern(string pattern, ImmutableStack<int> nums, Cache cache)
        {
            if(!cache.ContainsKey((pattern, nums)))
            {
                cache[(pattern, nums)] = pattern.FirstOrDefault() switch
                {
                    '.' => EvalPattern(pattern[1..], nums, cache),
                    '?' => EvalPattern('.' + pattern[1..], nums, cache) + EvalPattern('#' + pattern[1..], nums, cache),
                    '#' => ProcessDamagedSpring(pattern, nums, cache),
                    _ => nums.Any() ? 0 : 1
                };
            }
            return cache[(pattern, nums)];
        }
        static string Unfold(string pattern, char join, int reps) => string.Join(join, Enumerable.Repeat(pattern, reps));
        public static List<int> GetBitGroups(long n)
        {
            var list = new List<int>();

            int groupCount = 0;
            while (n > 0)
            {
                if ((n & 1) == 1)
                    groupCount++;
                else if (groupCount != 0)
                {
                    list.Add(groupCount);
                    groupCount = 0;
                }
                n >>= 1;
            }

            list.Add(groupCount);
            return list;
        }
        public static long GetBitCount(long n)
        {
            long bitCount = 0;
            while (n > 0)
            {
                bitCount++;
                n &= (n - 1);
            }
            return bitCount;
        }
        public static long Part1(string[] input)
        {
            long sum = 0;
            var bitGroups = new List<int>();
            foreach (var line in input)
            {
                var cfg = new List<int>();
                foreach (var c in line.Split(' ')[1].Split(','))
                    cfg.Add(int.Parse(c.ToString()));

                string springs = line.Split(' ')[0];

                int amount = cfg.Sum();
                int toAdd = amount - springs.Where(c => c == '#').Count();

                long knownMask = 0;
                long unknownMask = 0;
                long springMask = 0;
                long funcMask = 0;
                for (int i = 0; i < springs.Length; i++)
                {
                    if (springs[i] != '?')
                        knownMask += 1 << i;

                    if (springs[i] == '#')
                        springMask += 1 << i;

                    if (springs[i] == '.')
                        funcMask += 1 << i;

                    if (springs[i] == '?')
                        unknownMask += 1 << i;
                }

                long max = springMask | unknownMask;
                for (long i = springMask; i <= max; i++)
                {
                    if ((i & funcMask) > 0)
                        continue;

                    if ((i & springMask) < springMask)
                        continue;

                    if (GetBitCount(i ^ springMask) != toAdd)
                        continue;

                    bitGroups = GetBitGroups(i);
                    if (!bitGroups.SequenceEqual(cfg))
                        continue;

                    sum++;
                }
            }

            return sum;
        }
        public static long Part2(string[] input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                var pattern = Unfold(line.Split(' ')[0], '?', 5);
                var nums = ImmutableStack.CreateRange(Unfold(line.Split(' ')[1], ',', 5).Split(',').Select(int.Parse).Reverse());

                sum += EvalPattern(pattern, nums, new Cache());
            }

            return sum;
        }
    }
}