namespace AoC2023
{
	internal class Day19
	{
        public enum Condition
        {
            Larger, Smaller
        }
        public struct Item
        {
            public long x, m, a, s;
            public Item(int x, int m, int a, int s)
            {
                this.x = x;
                this.m = m;
                this.a = a;
                this.s = s;
            }
            public long Sum { get => x + m + a + s; }
            public override string ToString() => "x=" + x + " m=" + m + " a=" + a + " s=" + s;
        }
        public static List<Item> ParseItems(string[] input)
        {
            var list = new List<Item>();
            foreach(var line in input)
            {
                if(line.StartsWith("{"))
                {
                    var split = line.Split(['=', ',', '}']);
                    list.Add(new Item(int.Parse(split[1]), int.Parse(split[3]), int.Parse(split[5]), int.Parse(split[7])));
                }
            }
            return list;
        }
        public static Dictionary<string, string> ParseWorkflows(string[] input)
        {
            var workflows = new Dictionary<string, string>();
            foreach (var line in input)
            {
                if (line == string.Empty)
                    break;
                workflows.Add(line.Split(['{', '}'])[0], line.Split(['{', '}'])[1]);
            }
            return workflows;
        }
        public static long Part1(string[] input)
        {
            long sum = 0;
            var items = ParseItems(input);
            var workflows = ParseWorkflows(input);
            
            foreach(var item in items)
            {
                string curr = "in";
                string currFlow = workflows[curr];
                var split = currFlow.Split(['<', '>', ':', ',']);
                while (curr != "A" && curr != "R")
                {
                    split = currFlow.Split(['<', '>', ':', ',']);

                    long rightSide = int.Parse(split[1]);
                    long leftSide = split[0] switch
                    {
                        "x" => item.x,
                        "m" => item.m,
                        "a" => item.a,
                        "s" => item.s,
                    };

                    if (currFlow[1] == '<' && leftSide < rightSide)
                    {
                        curr = split[2];
                        workflows.TryGetValue(curr, out currFlow);
                    }
                    else if (currFlow[1] == '>' && leftSide > rightSide)
                    {
                        curr = split[2];
                        workflows.TryGetValue(curr, out currFlow);
                    }
                    else if (split[3].Length == 1)
                    {
                        currFlow = currFlow.Substring(currFlow.IndexOf(',') + 1);
                        curr = split[3];
                    }
                    else
                    {
                        curr = split[3];
                        workflows.TryGetValue(curr, out currFlow);
                    }
                }
                if (curr == "A")
                    sum += item.Sum;
            }

            return sum;
        }
        public static long Part2(string[] input)
        {
            var workflows = ParseWorkflows(input);

            // Recursion?

            return 0;
        }
    }
}