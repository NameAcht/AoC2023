namespace AoC2023
{
	internal class Day25
	{
		class Node
		{
			public string name;
			public Dictionary<Node, int> children;
			public int vertexCount;
			public Node(string name)
			{
				this.name = name;
				vertexCount = 1;
				children = new Dictionary<Node, int>();
			}
			public override string ToString()
			{
				string str = name + ": ";
				foreach (var child in children)
					str += child.Key.name + " ";
				return str;
			}
			public Node FurthestNode(int cuts, HashSet<string> visited)
			{
				visited.Add(name);
				foreach (var child in children.Keys)
					if (!visited.Contains(child.name))
						return child.FurthestNode(cuts, visited);
				return this;
			}
			public void Consume(Node toEat)
			{
				foreach (var child in toEat.children)
				{
					if (child.Key == this)
						continue;

					children.TryGetValue(child.Key, out int val);
					children[child.Key] = val + toEat.children[child.Key];
					child.Key.children[this] = val + toEat.children[child.Key];

					// remove consumed node from its children
					child.Key.children.Remove(toEat);
				}
				vertexCount += toEat.vertexCount;
				toEat.children.Clear();
				children.Remove(toEat);
			}
		}
		static List<Node> ParseNodes(string[] input)
		{
			var nodes = new Dictionary<string, Node>();
			foreach (var line in input)
			{
				var left = line.Split(' ').First().Trim(':');
				nodes.TryAdd(left, new Node(left));

				foreach (var right in line.Split(' ')[1..])
				{
					var name = right.Trim(':');
					nodes.TryAdd(name, new Node(name));
					nodes[name].children.Add(nodes[left], 1);
					nodes[left].children.Add(nodes[name], 1);
				}
			}
			return nodes.Values.ToList();
		}
		public static int Part1(string[] input)
		{
			var nodes = ParseNodes(input);
			var rndm = new Random();

			while (nodes.First().children.Sum(n => n.Value) != 3)
			{
				nodes = ParseNodes(input);
				while (nodes.Count > 2)
				{
					var idx1 = rndm.Next(0, nodes.Count);
					var idx2 = rndm.Next(0, nodes[idx1].children.Count);

					var remove = nodes[idx1].children.Keys.ToList()[idx2];
					nodes[idx1].Consume(remove);
					nodes.Remove(remove);
				}
			}

			return nodes.First().vertexCount * nodes.Last().vertexCount;
		}
	}
}