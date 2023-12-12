namespace AoC2023
{
    internal class Day09
    {
        public static long Part1(string[] input)
        {
            long sum = 0;
            foreach (var line in input)
            {
                var startingSeries = line.Split(' ').ToList().ConvertAll(int.Parse);
                var subSeries = new List<List<int>>();

                var currSeries = startingSeries;

                // construct sub series
                while (!subSeries.Last().All(num => num == 0))
                {
                    var nextSeries = new List<int>();
                    for (int i = 0; i < currSeries.Count - 2; i++)
                        nextSeries.Add(currSeries[i + 1] - currSeries[i]);

                    currSeries = nextSeries;
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
