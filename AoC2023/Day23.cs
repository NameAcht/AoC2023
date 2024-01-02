using Coord = (int row, int col);

namespace AoC2023
{
    internal class Day23
    {
        public class Node
        {
            public Coord pos;
            List<(Coord pos, int cost)> childVals;
            public List<(Node node, int cost)> children;
            public Node(Coord pos, string[] map, bool filterSlopes)
            {
                this.pos = pos;
                childVals = new List<(Coord pos, int cost)>();
                FindChildren(map, filterSlopes);
            }
            public override string ToString()
            {
                return pos.row + ";" + pos.col;
            }
            void FindChildren(string[] map, bool filterSlopes)
            {
                var queue = new Queue<(Coord pos, Coord prev, int steps)>();
                queue.Enqueue((pos, pos, 0));

                while (queue.Count > 0)
                {
                    var curr = queue.Dequeue();
                    if (IsBranch(curr.pos, map) && curr.pos != pos)
                    {
                        childVals.Add((curr.pos, curr.steps));
                        continue;
                    }
                    QueueCoords(queue, curr, map, filterSlopes);
                }
            }
            void QueueCoords(Queue<(Coord pos, Coord prev, int steps)> queue, (Coord pos, Coord prev, int steps) curr, string[] map, bool filterSlopes)
            {
                var p = curr.pos;
                // down
                if (Validate((p.row + 1, p.col), curr.prev, map) && (!filterSlopes || map[p.row + 1][p.col] != '^'))
                    queue.Enqueue(((p.row + 1, p.col), p, curr.steps + 1));
                // right
                if (Validate((p.row, p.col + 1), curr.prev, map) && (!filterSlopes || map[p.row][p.col + 1] != '<'))
                    queue.Enqueue(((p.row, p.col + 1), p, curr.steps + 1));
                // up
                if (Validate((p.row - 1, p.col), curr.prev, map) && (!filterSlopes || map[p.row - 1][p.col] != 'v'))
                    queue.Enqueue(((p.row - 1, p.col), p, curr.steps + 1));
                // left
                if (Validate((p.row, p.col - 1), curr.prev, map) && (!filterSlopes || map[p.row][p.col - 1] != '>'))
                    queue.Enqueue(((p.row, p.col - 1), p, curr.steps + 1));
            }
            bool Validate(Coord pos, Coord prev, string[] map)
            {
                if (pos.row < 0 || pos.row >= map.Length || pos.col < 0 || pos.col >= map[0].Length)
                    return false;
                if (pos == prev)
                    return false;
                if (map[pos.row][pos.col] == '#')
                    return false;
                return true;
            }
            public void LinkChildren(Dictionary<Coord, Node> nodes)
            {
                children = childVals.ConvertAll(val => (nodes[val.pos], val.cost));
            }
            public int FindLongestPath(int currCost, Coord end, HashSet<Coord> visited)
            {
                if (pos == end)
                    return currCost;

                int max = 0;
                var nextVisited = new HashSet<Coord>(visited) { pos };
                foreach (var child in children)
                    if (!nextVisited.Contains(child.node.pos))
                        max = Math.Max(max, child.node.FindLongestPath(currCost + child.cost, end, nextVisited));

                return max;
            }
        }
        public static bool IsBranch(Coord pos, string[] map)
        {
            if (map[pos.row][pos.col] == '#')
                return false;
            if (pos == (0, 1) || pos == (map.Length - 1, map[0].Length - 2))
                return true;

            int count = 0;
            if (map[pos.row + 1][pos.col] != '#')
                count++;
            if (map[pos.row - 1][pos.col] != '#')
                count++;
            if (map[pos.row][pos.col + 1] != '#')
                count++;
            if (map[pos.row][pos.col - 1] != '#')
                count++;
            return count >= 3;
        }
        public static Node ParseGraph(string[] map, bool filterSlopes = true)
        {
            var nodes = new Dictionary<Coord, Node>()
            {
                [(0, 1)] = new Node((0, 1), map, filterSlopes),
                [(map.Length - 1, map[0].Length - 2)] = new Node((map.Length - 1, map[0].Length - 2), map, filterSlopes)
            };
            for (int row = 1; row < map.Length - 1; row++)
                for (int col = 1; col < map[0].Length - 1; col++)
                    if (IsBranch((row, col), map))
                        nodes.Add((row, col), new Node((row, col), map, filterSlopes));

            foreach (var node in nodes)
                node.Value.LinkChildren(nodes);

            return nodes.First().Value;
        }
        public static int Part1(string[] map)
        {
            var node = ParseGraph(map);
            return node.FindLongestPath(0, (map.Length - 1, map.Last().Length - 2), new HashSet<Coord>() { (0, 0) });
        }
        public static int Part2(string[] map)
        {
            var node = ParseGraph(map, false);
            return node.FindLongestPath(0, (map.Length - 1, map.Last().Length - 2), new HashSet<Coord>() { (0, 0) });
        }
    }
}