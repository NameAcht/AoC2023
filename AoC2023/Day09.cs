namespace AoC2023
{
    internal class Day09
    {
        public static void ExtrapolateForward(List<List<int>> subSeries)
        {
            subSeries.Last().Add(0);
            while (subSeries.Count > 1)
            {
                var secondLast = subSeries[subSeries.Count - 2];
                secondLast.Add(secondLast.Last() + subSeries.Last().Last());
                subSeries.Remove(subSeries.Last());
            }
        }
        public static List<List<int>> ConstructSubSeries(string line)
        {
            var startingSeries = line.Split(' ').ToList().ConvertAll(int.Parse);
            var subSeries = new List<List<int>>();

            var currSeries = startingSeries;
            subSeries.Add(currSeries);

            // construct sub series
            while (!subSeries.Last().All(num => num == 0))
            {
                var nextSeries = new List<int>();

                for (int i = 0; i < currSeries.Count - 1; i++)
                    nextSeries.Add(currSeries[i + 1] - currSeries[i]);
                subSeries.Add(nextSeries);

                currSeries = nextSeries;
            }
            return subSeries;
        }
        public static void ExtrapolateBackward(List<List<int>> subSeries)
        {
            subSeries.Last().Insert(0, 0);
            while(subSeries.Count > 1)
            {
                var secondLast = subSeries[subSeries.Count - 2];
                secondLast.Insert(0, secondLast.First() - subSeries.Last().First());
                subSeries.Remove(subSeries.Last());
            }
        }
        public static long Part1(string[] series)
        {
            long sum = 0;
            foreach (var line in series)
            {
                var subSeries = ConstructSubSeries(line);
                ExtrapolateForward(subSeries);
                sum += subSeries.First().Last();
            }
            return sum;
        }
        public static long Part2(string[] series)
        {
            long sum = 0;
            foreach (var line in series)
            {
                var subSeries = ConstructSubSeries(line);
                ExtrapolateBackward(subSeries);
                sum += subSeries.First().First();
            }
            return sum;
        }
    }
}
