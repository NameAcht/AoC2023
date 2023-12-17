namespace AoC2023
{
    internal class Day12
    {
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
            int sum = 0;
            foreach (var line in input)
            {
                var mutLine = line.ToCharArray();
                
                var cfg = "";
                foreach(var c in line.Split(' ')[1].Split(','))
                    cfg += c;
                int groups = int.Parse(cfg);
                string springs = line.Split(' ')[0];

                int amount = groups.ToString().Sum(c => c - '0');
                int toAdd = amount - springs.Where(c => c == '#').Count();

                
                
                int knownMask = 0;
                int unknownMask = 0;
                int brokeMask = 0;
                int funcMask = 0;

                var unknownIndices = new List<int>();

                for (int i = 0; i < springs.Length; i++)
                {
                    if (springs[i] != '?')
                        knownMask += 1 << i;

                    if (springs[i] == '#')
                        brokeMask += 1 << i;

                    if (springs[i] == '.')
                        funcMask += 1 << i;

                    if (springs[i] == '?')
                    {
                        unknownMask += 1 << i;
                        unknownIndices.Add(i);
                    }
                }


                int max = FindLargestBit(unknownMask);
                
                
            }

            return sum;
        }
        public static long Part2(string[] input)
        {


            return 0;
        }
    }
}
