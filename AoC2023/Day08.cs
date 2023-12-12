namespace AoC2023
{
    internal class Day08
    {
        public static long LeastCommonMultiple(long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }
        static long GreatestCommonDivisor(long a, long b)
        {
            return b == 0 ? a : GreatestCommonDivisor(b, a % b);
        }
        public static Dictionary<string, (string left, string right)> ParseNodes(string[] input)
        {
            var nodes = new Dictionary<string, (string left, string right)>();
            for (int i = 2; i < input.Length; i++)
            {
                var split = input[i].Split(new char[] { ' ', '(', ')', ',' });
                nodes.Add(split[0], (split[3], split[5]));
            }
            return nodes;
        }
        public static List<(string left, string right)> GetNodesByEndChar(Dictionary<string, (string left, string right)> nodes, char endChar)
        {
            var startingNodes = new List<(string left, string right)>();
            foreach (var node in nodes)
                if (node.Key.EndsWith(endChar))
                    startingNodes.Add(node.Value);
            return startingNodes;
        }
        public static int Part1(string[] input)
        {
            string instructions = input[0];
            int iter = 0;
            var nodes = ParseNodes(input);
            var currNode = nodes["AAA"];

            for (iter = 0; currNode != nodes["ZZZ"]; iter++)
                currNode = instructions[iter % instructions.Length] switch
                {
                    'L' => nodes[currNode.left],
                    'R' => nodes[currNode.right],
                    _ => throw new NotImplementedException("What")
                };

            return iter;
        }
        public static long Part2(string[] input)
        {
            string instructions = input[0];
            int iter = 0;
            var nodes = ParseNodes(input);

            var currNodes = GetNodesByEndChar(nodes, 'A');
            var endNodes = GetNodesByEndChar(nodes, 'Z');

            var nodeMods = new Dictionary<int, long>(currNodes.Count);

            while (nodeMods.Count != currNodes.Count)
            {
                currNodes = instructions[iter % instructions.Length] switch
                {
                    'L' => currNodes.ConvertAll(currNode => nodes[currNode.left]),
                    'R' => currNodes.ConvertAll(currNode => nodes[currNode.right]),
                    _ => throw new NotImplementedException("What")
                };
                iter++;

                for (int i = 0; i < currNodes.Count; i++)
                    if (endNodes.Contains(currNodes[i]))
                        nodeMods.TryAdd(i, iter);
            }

            return LeastCommonMultiple(nodeMods.Values.ToArray());
        }
    }
}
