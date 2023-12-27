namespace AoC2023
{
	internal class Day19
	{
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
        }
        public struct Bound
        {
            public long lower, upper;
            public Bound(int lower, int upper)
            {
                this.lower = lower;
                this.upper = upper;
            }
            public long Amount { get => upper - lower + 1; }
            public Bound ProcessTrueBound(char condition, long right)
            {
                if (condition == '<')
                    upper = Math.Min(upper, right - 1);
                else
                    lower = Math.Max(lower, right + 1);
                return this;
            }
            public Bound ProcessFalseBound(char condition, long right)
            {
                if (condition == '<')
                    lower = Math.Max(lower, right);
                else
                    upper = Math.Min(upper, right);
                return this;
            }
            public override string ToString() => lower.ToString() + " - " + upper.ToString();
        }
        public struct State
        {
            public Bound x, m, a, s;
            public string workflow;
            public long Arrangements { get => x.Amount * m.Amount * a.Amount * s.Amount; }
            public State ModifyTrueBound(char xmas, char condition, long right)
            {
                switch (xmas)
                {
                    case 'x': x = x.ProcessTrueBound(condition, right); break;
                    case 'm': m = m.ProcessTrueBound(condition, right); break;
                    case 'a': a = a.ProcessTrueBound(condition, right); break;
                    case 's': s = s.ProcessTrueBound(condition, right); break;
                }
                return this;
            }
            public State ModifyFalseBound(char xmas, char condition, long right)
            {
                switch (xmas)
                {
                    case 'x': x = x.ProcessFalseBound(condition, right); break;
                    case 'm': m = m.ProcessFalseBound(condition, right); break;
                    case 'a': a = a.ProcessFalseBound(condition, right); break;
                    case 's': s = s.ProcessFalseBound(condition, right); break;
                }
                return this;
            }

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
        public static long SolveState(State curr, Dictionary<string, string> workflows)
        {
            if (curr.workflow == "R")
                return 0;
            if (curr.workflow == "A")
                return curr.Arrangements;

            // case new workflow
            if (!curr.workflow.Contains('<') && !curr.workflow.Contains('>'))
            {
                curr.workflow = workflows[curr.workflow];
                return SolveState(curr, workflows);
            }

            // parse condition right side val
            long rightSide = long.Parse(curr.workflow.Split(['<', '>', ':'])[1]);

            var trueState = curr;
            var falseState = curr;

            // assign true workflow
            var trueFlow = curr.workflow.Split([',', ':'])[1];
            trueState.workflow = workflows.TryGetValue(trueFlow, out string newFlow) ? newFlow : trueFlow;

            // assign false workflow
            var falseFlow = curr.workflow.Substring(curr.workflow.IndexOf(',') + 1);
            falseState.workflow = workflows.TryGetValue(falseFlow, out newFlow) ? newFlow : falseFlow;

            char xmas = curr.workflow[0];
            char condition = curr.workflow[1];

            trueState = trueState.ModifyTrueBound(xmas, condition, rightSide);
            falseState = falseState.ModifyFalseBound(xmas, condition, rightSide);

            return SolveState(trueState, workflows) + SolveState(falseState, workflows);
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
            var start = new State()
            {
                x = new Bound(1, 4000),
                m = new Bound(1, 4000),
                a = new Bound(1, 4000),
                s = new Bound(1, 4000),
                workflow = workflows["in"]
            };
            return SolveState(start, workflows);
        }
    }
}