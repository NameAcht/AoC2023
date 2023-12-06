namespace AoC2023
{
    public static class Day01
    {
        enum Numbers
        {
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9
        }
        public static int Part1(string[] input)
        {
            int sum = 0;
            var ascii = new char[26];
            for (int i = 0; i < ascii.Length; i++)
                ascii[i] = (char)(i + 97);

            foreach (var line in input)
            {
                var numbers = line.ToLowerInvariant().Trim(ascii);
                sum += int.Parse(numbers.First().ToString() + numbers.Last().ToString());
            }
            return sum;
        }
        public static int Part2(string[] input)
        {
            int sum = 0;

            foreach (var line in input)
            {
                int rightIndex = int.MinValue;
                int leftIndex = int.MaxValue;

                int left = 0;
                int right = 0;

                foreach (var number in Enum.GetValues(typeof(Numbers)))
                {
                    int leftIndexNumber = line.IndexOf(((int)number).ToString());
                    int leftIndexString = line.IndexOf(number.ToString());

                    int rightIndexNumber = line.LastIndexOf(((int)number).ToString());
                    int rightIndexString = line.LastIndexOf(number.ToString());

                    if (leftIndexNumber != -1)
                    {
                        if (leftIndexNumber < leftIndex)
                        {
                            leftIndex = leftIndexNumber;
                            left = (int)number;
                        }

                        if (rightIndexNumber > rightIndex)
                        {
                            rightIndex = rightIndexNumber;
                            right = (int)number;
                        }
                    }

                    if (leftIndexString == -1)
                        continue;

                    if (leftIndexString < leftIndex)
                    {
                        leftIndex = leftIndexString;
                        left = (int)number;
                    }

                    if (rightIndexString > rightIndex)
                    {
                        rightIndex = rightIndexString;
                        right = (int)number;
                    }
                }
                sum += int.Parse(left.ToString() + right.ToString());
            }
            return sum;
        }
    }
}