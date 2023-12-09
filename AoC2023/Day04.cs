namespace AoC2023
{
    internal class Day04
    {
        public static int Part1(string[] cards)
        {
            int sum = 0;

            for (int i = 0; i < cards.Length; i++)
            {
                var card = cards[i];
                var splitCard = card.Split(' ');
                bool pastWinning = false;

                var winningNumbers = new List<int>();
                var myWinningNumbers = new List<int>();

                for (int j = 2; j < splitCard.Length; j++)
                {
                    var curr = splitCard[j];
                    if (curr == "|")
                        pastWinning = true;
                    else if (!pastWinning)
                    {
                        if (int.TryParse(curr, out int winningNumber))
                            winningNumbers.Add(winningNumber);
                    }
                    else
                    {
                        if (int.TryParse(curr, out int myNumber) && winningNumbers.Contains(myNumber))
                            myWinningNumbers.Add(myNumber);
                    }
                }
                sum += (int)Math.Pow(2, myWinningNumbers.Count - 1);
            }
            return sum;
        }
        public static int Part2(string[] cards)
        {
            var cardAmounts = new List<int>();

            for (int i = 0; i < cards.Length; i++)
                cardAmounts.Add(1);

            for (int curr = 0; curr < cards.Length; curr++)
            {
                var card = cards[curr];
                var splitCard = card.Split(' ');
                bool pastWinning = false;

                var winningNumbers = new List<int>();
                var myWinningNumbers = new List<int>();

                for (int j = 2; j < splitCard.Length; j++)
                {
                    var entry = splitCard[j];
                    if (entry == "|")
                        pastWinning = true;
                    else if (!pastWinning)
                    {
                        if (int.TryParse(entry, out int winningNumber))
                            winningNumbers.Add(winningNumber);
                    }
                    else
                    {
                        if (int.TryParse(entry, out int myNumber) && winningNumbers.Contains(myNumber))
                            myWinningNumbers.Add(myNumber);
                    }
                }

                int winnings = myWinningNumbers.Count;
                int currAmount = cardAmounts[curr];

                for (int i = curr + 1; i <= curr + winnings && i < cards.Length; i++)
                    cardAmounts[i] += currAmount;
            }
            return cardAmounts.Sum();
        }
    }
}
