namespace AoC2023
{
	internal class Day20
	{
		public struct Module
		{
			public string name;
			public char type;
			public List<string> modules;
            public object state;
            public Module(string name, char type, List<string> modules)
			{
				this.name = name;
				this.type = type;
				this.modules = modules;
				if (type == '%')
					state = false;
			}
            public override string ToString()
			{
				string str = type + name + ": ";
				foreach(var module in modules) 
					str += module.ToString() + ", ";
				return str.TrimEnd([',', ' ']);
			}
        }
		public static Dictionary<string, Module> ParseModules(string[] input)
		{
			var dict = new Dictionary<string, Module>();
			foreach(var line in input)
			{
				var split = line.Split(' ');
				var name = split[0].Trim(['%', '&']);
				
				var modules = new List<string>();
				for (int i = 2; i < split.Length; i++)
					modules.Add(split[i].Trim(','));

				dict.Add(name, new Module(name, line[0], modules));
			}
			return dict;
		}
		public static int Part1(string[] input)
		{
			var modules = ParseModules(input);

			return 0;
		}
	}
}