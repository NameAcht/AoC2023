using System.Collections.Immutable;
using Cache = System.Collections.Generic.Dictionary<(string, System.Collections.Immutable.ImmutableStack<int>), long>;

namespace AoC2023
{
    internal class Day12
    {
        // ???
        static long ProcessHash(string pattern, ImmutableStack<int> nums, Cache cache)
        {
            if (!nums.Any())
            {
                return 0; // no more numbers left, this is no good
            }

            var n = nums.Peek();
            nums = nums.Pop();

            var potentiallyDead = pattern.TakeWhile(s => s == '#' || s == '?').Count();
            
            if (potentiallyDead < n)
            {
                return 0; // not enough dead springs 
            }
            else if (pattern.Length == n)
            {
                return EvalPattern("", nums, cache);
            }
            else if (pattern[n] == '#')
            {
                return 0; // dead spring follows the range -> not good
            }
            else
            {
                return EvalPattern(pattern[(n + 1)..], nums, cache);
            }
        }
        static long EvalPattern(string pattern, ImmutableStack<int> nums, Cache cache)
        {
            if (!cache.ContainsKey((pattern, nums)))
            {
                cache[(pattern, nums)] = pattern.FirstOrDefault() switch
                {
                    '.' => EvalPattern(pattern[1..], nums, cache),
                    '?' => EvalPattern("." + pattern[1..], nums, cache) + EvalPattern("#" + pattern[1..], nums, cache),
                    '#' => ProcessHash(pattern, nums, cache),
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