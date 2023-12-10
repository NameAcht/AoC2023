using System.ComponentModel;

namespace AoC2023
{
    internal class Day06
    {
        public static int Part1(string[] input)
        {
            var listTime = input[0].Split(' ').ToList().FindAll(num => int.TryParse(num, out int temp));
            var listDistances = input[1].Split(' ').ToList().FindAll(num => int.TryParse(num, out int temp));
            int wins = 0;
            int result = 1;

            for (int i = 0; i < listTime.Count; i++)
            {
                int timeVal = int.Parse(listTime[i]);
                int distVal = int.Parse(listDistances[i]);
                for (int hold = 1; hold < timeVal; hold++)
                    if(hold * (timeVal - hold) > distVal)
                        wins++;
                result *= wins;
                wins = 0;
            }

            return result;
        }
        public static int Part2(string[] input)
        {
            string time = "";
            input[0].Split(' ').ToList().FindAll(num => int.TryParse(num, out int temp)).ForEach(num => time += num);
            string distance = "";
            input[1].Split(' ').ToList().FindAll(num => int.TryParse(num, out int temp)).ForEach(num => distance += num);

            long timeVal = long.Parse(time);
            long distVal = long.Parse(distance);

            int wins = 0;

            for (int hold = 1; hold < timeVal; hold++)
                if (hold * (timeVal - hold) > distVal)
                    wins++;

            return wins;
        }
    }
}
