namespace AoC2023
{
    internal class Day02
    {
        public static int Part1(string[] input)
        {
            int sum = 0;
            foreach(var game in input)
            {
                bool isPossible = true;
                var entries = game.Split(new char[]{ ':', ';', ',' });

                foreach(var entry in entries)
                {
                    if (!int.TryParse(entry.Trim().Split(' ')[0], out int cubeNumber))
                        continue;

                    if (entry.Contains("red") && cubeNumber > 12)
                        isPossible = false;
                    if (entry.Contains("green") && cubeNumber > 13)
                        isPossible = false;
                    if (entry.Contains("blue") && cubeNumber > 14)
                        isPossible = false;
                }
                if (isPossible)
                    sum += int.Parse(entries[0].Split(' ')[1]);
            }
            return sum;
        }
        public static long Part2(string[] input)
        {
            long sum = 0;
            foreach(var game in input)
            {
                int minRed = int.MinValue, minGreen = int.MinValue, minBlue = int.MinValue;
                var entries = game.Split(':', ';', ',');
                
                foreach(var entry in entries)
                {
                    if (!int.TryParse(entry.Trim().Split(' ')[0], out int cubeNumber))
                        continue;

                    if (entry.Contains("red"))
                        minRed = int.Max(cubeNumber, minRed);
                    else if (entry.Contains("green"))
                        minGreen = int.Max(cubeNumber, minGreen);
                    else if (entry.Contains("blue"))
                        minBlue = int.Max(cubeNumber, minBlue);
                }
                sum += minRed * minGreen * minBlue;
            }

            return sum;
        }
    }
}
