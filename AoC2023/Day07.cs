namespace AoC2023
{
    internal class Day07
    {
        static List<string> Debug(List<(List<int> cardVals, int bid, Type type)> list)
        {
            var ret = new List<string>();
            string buffer = "";
            foreach(var item in list)
            {
                foreach(var num in item.cardVals)
                {
                    string c = num switch
                    {
                        10 => "T",
                        1 => "J",
                        12 => "Q",
                        13 => "K",
                        14 => "A",
                        _ => num.ToString()
                    };

                    buffer += c;
                }
                buffer += "\t";
                buffer += item.bid.ToString();
                buffer += "\t";
                buffer += item.type.ToString();
                
                if(item.cardVals.Contains(1))
                    ret.Add(buffer);
                buffer = "";
            }
            return ret;
        }
        public enum Type
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfKind,
            FullHouse,
            FourOfKind,
            FiveOfKind
        }
        public static int Compare((List<int> cardVals, int bid, Type type) a, (List<int> cardVals, int bid, Type type) b)
        {
            if (a == b)
                return 0;

            if (a.type > b.type)
                return 1;
            if (a.type < b.type)
                return -1;

            for (int i = 0; i < a.cardVals.Count; i++)
            {
                if (a.cardVals[i] > b.cardVals[i])
                    return 1;
                if (a.cardVals[i] < b.cardVals[i])
                    return -1;
            }
            throw new NotImplementedException();
        }
        public static List<int> ParseCardVals(string card, int part)
        {
            int val = 0;
            var cardVals = new List<int>(5);
            foreach (var c in card)
            {
                if (char.IsDigit(c))
                    val = int.Parse(c.ToString());
                else
                    val = c.ToString() switch
                    {
                        "T" => 10,
                        "J" => part == 1 ? 11 : 1,
                        "Q" => 12,
                        "K" => 13,
                        "A" => 14,
                        _ => throw new NotImplementedException("huh")
                    };
                cardVals.Add(val);
            }
            return cardVals;
        }
        public static int Part1(string[] cards)
        {
            var parsedCards = new List<(List<int> cardVals, int bid, Type type)>();

            foreach (var card in cards)
            {
                var split = card.Split(' ');
                var cardStr = split[0];
                var bid = int.Parse(split[1]);

                var cardVals = ParseCardVals(cardStr, 1);

                var distinct = cardStr.Distinct().ToList();

                var type = distinct.Count() switch
                {
                    1 => Type.FiveOfKind,
                    2 => cardStr.Count(c => c == cardStr[0]) == 4 || cardStr.Count(c => c == cardStr[0]) == 1 ? Type.FourOfKind : Type.FullHouse,
                    3 => cardStr.Any(c => cardStr.Count(card => card == c) == 3) ? Type.ThreeOfKind : Type.TwoPair,
                    4 => Type.OnePair,
                    5 => Type.HighCard,
                    _ => throw new NotImplementedException("ಠ_ಠ")
                };

                parsedCards.Add((cardVals, bid, type));
            }
            parsedCards.Sort(Compare);

            int sum = 0;
            for (int i = 0; i < parsedCards.Count; i++)
                sum += parsedCards[i].bid * (i + 1);
            return sum;
        }
        public static int Part2(string[] cards)
        {
            var parsedCards = new List<(List<int> cardVals, int bid, Type type)>();

            foreach (var card in cards)
            {
                var split = card.Split(' ');
                var cardStr = split[0];
                var bid = int.Parse(split[1]);

                var cardVals = ParseCardVals(cardStr, 2);

                int yokerCount = cardStr.Count(c => c == 'J');

                // if joker in card subtract 1 from distinct amount of cards
                var distinctCount = cardStr.Distinct().ToList().Count - (yokerCount == 0 ? 0 : 1);

                // ignore many joker cases because joker directly upgrades card type in those
                var type = (distinctCount, yokerCount) switch
                {
                    (0, _) => Type.FiveOfKind,
                    (1, _) => Type.FiveOfKind,
                    (2, 0) => cardStr.Count(c => c == cardStr[0]) == 4 || cardStr.Count(c => c == cardStr[0]) == 1 ? Type.FourOfKind : Type.FullHouse,
                    (3, 0) => cardStr.Any(c => cardStr.Count(card => card == c) == 3) ? Type.ThreeOfKind : Type.TwoPair,
                    (4, _) => Type.OnePair,
                    (5, _) => Type.HighCard,

                    // joker special cases
                    (2, 1) => cardVals.Any(val => cardVals.Count(find => val == find) == 3) ? Type.FourOfKind : Type.FullHouse,
                    (2, >= 2) => Type.FourOfKind,
                    (3, >= 1) => Type.ThreeOfKind,
                    _ => throw new NotImplementedException("ಠ_ಠ")
                };

                parsedCards.Add((cardVals, bid, type));
            }
            parsedCards.Sort(Compare);

            int sum = 0;
            for (int i = 0; i < parsedCards.Count; i++)
                sum += parsedCards[i].bid * (i + 1);
            return sum;
        }
    }
}
