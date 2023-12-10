namespace AoC2023
{
    internal class Day05
    {
        public class Range
        {
            public long Start { get; set; }
            public long Destination { get; set; }
            public long Amount { get => Destination - Start + 1; }
            public long Length { get; set; }
        }
        public static List<Range>[] ParseMaps(string[] input)
        {
            var maps = new List<Range>[7];

            // Counts to 0 before first Array access
            int currMap = -1;

            // Parse Maps
            for (int i = 2; i < input.Length; i++)
            {
                var line = input[i];
                string[] splitRange;

                if (line.Contains("map"))
                {
                    currMap++;
                    maps[currMap] = new List<Range>();
                }
                else if ((splitRange = line.Split(' ')).Length == 3)
                {
                    var range = new Range()
                    {
                        Destination = long.Parse(splitRange[0]),
                        Start = long.Parse(splitRange[1]),
                        Length = long.Parse(splitRange[2])
                    };
                    maps[currMap].Add(range);
                }
            }
            return maps;
        }
        public static List<Range> ParseSeedRanges(string[] splitPut)
        {
            var seeds = new List<Range>();
            for (int i = 1; i < splitPut.Length; i += 2)
            {
                var range = new Range()
                {
                    Start = long.Parse(splitPut[i]),
                    Destination = long.Parse(splitPut[i]) + long.Parse(splitPut[i + 1]) - 1
                };
                seeds.Add(range);
            }
            return seeds;
        }
        public static long Part1(string[] input)
        {
            var seeds = new List<long>();
            var split = input[0].Split(' ');

            // Parse Seeds
            foreach (var seed in split)
                if (long.TryParse(seed, out long seedNumber))
                    seeds.Add(seedNumber);

            var maps = ParseMaps(input);

            foreach (var map in maps)
            {
                for (int seedIter = 0; seedIter < seeds.Count; seedIter++)
                {
                    foreach (var mapping in map)
                    {
                        if (seeds[seedIter] >= mapping.Start && seeds[seedIter] < mapping.Start + mapping.Length)
                        {
                            long toAdd = mapping.Destination - mapping.Start;
                            seeds[seedIter] = seeds[seedIter] + toAdd;
                            break;
                        }
                    }
                }
            }
            return seeds.Min();
        }
        public static long Part2(string[] input)
        {
            var split = input[0].Split(' ');
            var seeds = ParseSeedRanges(split);
            var maps = ParseMaps(input);

            var updateSeeds = new List<Range>();
            foreach (var map in maps)
            {
                for (int seedIter = 0; seedIter < seeds.Count; seedIter++)
                {
                    var currSeed = seeds[seedIter];
                    foreach (var mapping in map)
                    {
                        long mappingEnd = mapping.Start + mapping.Length - 1;
                        long offset = mapping.Destination - mapping.Start;

                        // Seed completely outside range
                        if (currSeed.Destination < mapping.Start || currSeed.Start > mappingEnd)
                            continue;
                        // Seed completely inside range
                        else if(currSeed.Destination <= mappingEnd && currSeed.Start >= mapping.Start)
                        {
                            long start = currSeed.Start + offset;
                            long dest = currSeed.Destination + offset;

                            var newSeed = new Range()
                            {
                                Start = start,
                                Destination = dest
                            };

                            updateSeeds.Add(newSeed);
                            
                            // Count down seed iterator because list entries moved back
                            if (seeds.Remove(currSeed))
                                seedIter--;
                            else
                                Console.WriteLine("seed Remove error");
                            break;
                        }
                        // Seed mapping partial overlap
                        else
                        {
                            long start = Math.Max(mapping.Start, currSeed.Start) + offset;
                            long dest = Math.Min(mappingEnd, currSeed.Destination) + offset;

                            var newSeed = new Range()
                            {
                                Start = start,
                                Destination = dest
                            };

                            updateSeeds.Add(newSeed);

                            if (currSeed.Start == start - offset)
                                currSeed.Start = mappingEnd + 1;
                            else if (currSeed.Destination == dest - offset)
                                currSeed.Destination = mapping.Start - 1;
                        }
                    }
                }

                // update seeds outside all ranges
                if (seeds.Count > 0)
                    updateSeeds.AddRange(seeds);

                seeds.Clear();
                seeds.AddRange(updateSeeds);
                updateSeeds = new List<Range>();
            }

            // find min seed
            long minSeed = long.MaxValue;
            foreach (var seed in seeds)
                if(seed.Start < minSeed)
                    minSeed = seed.Start;

            return minSeed;
        }
    }
}