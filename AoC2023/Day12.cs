namespace AoC2023
{
    internal class Day12
    {
        public static List<int> GetBitGroups(long n)
        {
            var list = new List<int>();
            
            int groupCount = 0;
            while (n > 0)
            {
                if ((n & 1) == 1)
                    groupCount++;
                else if(groupCount != 0)
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
        public static long NextPerm(int x)
        {
            int t = (x | (x - 1)) + 1;
            return t | ((((t & -t) / (x & -x)) >> 1) - 1);
        }
        public static int FindLargestBit(int bitmask)
        {
            int largestBit = 1;
            while(bitmask >= largestBit)
                largestBit <<= 1;
            return largestBit;
        }
        public static long Part1(string[] input)
        {
            long sum = 0;
            var bitGroups = new List<int>();
            foreach (var line in input)
            {
                var cfg = new List<int>();
                foreach(var c in line.Split(' ')[1].Split(','))
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


            return 0;
        }
    }
}
