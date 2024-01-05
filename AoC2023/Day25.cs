namespace AoC2023
{
	internal class Day25
	{
		class Node
		{
			public string name;
			public List<(Node node, int weight)> children;
			public Node(string name)
			{
				this.name = name;
				children = new List<(Node, int)>();
			}
            public override string ToString()
            {
				string str = name + ": ";
				foreach (var child in children)
					str += child.node.name + " ";
				return str;
			}
			public Node FurthestNode(int cuts, HashSet<string> visited)
			{
				visited.Add(name);
				foreach (var child in children)
					if(!visited.Contains(child.node.name))
						return child.node.FurthestNode(cuts, visited);
				return this;
			}
        }
		static Node ParseNodes(string[] input)
		{
			var nodes = new Dictionary<string, Node>();
			foreach(var line in input)
			{
				var left = line.Split(' ').First().Trim(':');
				nodes.TryAdd(left, new Node(left));
				
				foreach (var right in line.Split(' ')[1..])
				{
					var name = right.Trim(':');
					nodes.TryAdd(name, new Node(name));
					nodes[name].children.Add((nodes[left], 1));
					nodes[left].children.Add((nodes[name], 1));
				}
			}
			return nodes.First().Value;
		}
		public static int Part1(string[] input)
		{
			var s = ParseNodes(input);
			var t = s.FurthestNode(0, new HashSet<string>());
			

			return 0;
		}
	}
}