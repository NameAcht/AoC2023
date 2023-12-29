using InputMemory = System.Collections.Generic.Dictionary<string, bool>;

namespace AoC2023
{
	internal class Day20
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
        public class Module
		{
			public string name;
			public char type;
			public List<string> output;
            public object state;
            public Module(string name, char type, List<string> output)
			{
				this.name = name;
				this.type = type;
				this.output = output;
				if (type == '%')
					state = false;
				if(type == '&' || type == 'N')
					state = new InputMemory();
			}
			public void AddInputModules(string[] input)
			{
				foreach(var line in input)
					if (line.Split("->")[1].Contains(name))
						(state as InputMemory).Add(line.Split(' ')[0].Trim(['%', '&']), false);
			}
            public override string ToString()
			{
				string str = type + name + ": ";
				foreach(var module in output) 
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

				var module = new Module(name, line[0], modules);
				if (module.type == '&' || module.type == 'N')
					module.AddInputModules(input);

				dict.Add(name, module);
			}
			return dict;
		}
		public static void QueueNewPulses(Dictionary<string, Module> modules, Queue<(Module module, string inputName, bool isHighPulse)> queue, Module curr, bool output)
		{
            foreach (var name in curr.output)
			{
				if (!modules.ContainsKey(name))
					modules.Add(name, new Module(name, 'N', new List<string>()));
                queue.Enqueue((modules[name], curr.name, output));
            }
        }
        public static void PrintModuleState(Dictionary<string, Module> modules)
		{
			foreach (var module in modules.Values)
			{
				Console.WriteLine(module.name);
				if (module.state is bool)
					Console.WriteLine((bool)module.state ? "on" : "off");
				else if (module.state is InputMemory)
					foreach (var mem in module.state as InputMemory)
						Console.WriteLine(mem.Key + "\t" + (mem.Value ? "high" : "low"));
				Console.WriteLine();
			}
		}
		public static (int low, int high) PressButton(Dictionary<string, Module> modules, InputMemory lastConMemory = null, int press = 0, long[] cycles = null)
		{
            // button sends low pulse to broadcaster
            int lowPulses = 1;
			int highPulses = 0;
            var queue = new Queue<(Module module, string inputName, bool isHighPulse)>();

            foreach (var name in modules["broadcaster"].output)
                queue.Enqueue((modules[name], "broadcaster", false));

            while (queue.Count > 0)
            {
                var deq = queue.Dequeue();
                var curr = deq.module;
                var inputName = deq.inputName;
                bool isHighPulse = deq.isHighPulse;

                if (curr.type == '%' && !isHighPulse)
                {
                    curr.state = !(bool)curr.state;
                    QueueNewPulses(modules, queue, curr, (bool)curr.state);
                }
                if (curr.type == '&')
                {
                    (curr.state as InputMemory)[inputName] = isHighPulse;
                    bool output = (curr.state as InputMemory).Any(kvp => kvp.Value == false);
                    QueueNewPulses(modules, queue, curr, output);
                }

                if (isHighPulse)
                    highPulses++;
                else
                    lowPulses++;

                // part 2
                // pulse sent to rx -> check predefined memory for logic match (all high pulse) to get low pulse to rx 
                if (lastConMemory != null && curr.name == "rx")
				{
					var memoryList = lastConMemory.ToList();
					for (int i = 0; i < lastConMemory.Count; i++)
					{
						var con = memoryList[i];
						// only assign cycle length the first time it is seen
						if (con.Value)
							cycles[i] = cycles[i] == 0 ? press : cycles[i];
					}
				}
			}
			return (lowPulses, highPulses);
        }
		public static int Part1(string[] input)
		{
			var modules = ParseModules(input);
			int lowPulses = 0;
			int highPulses = 0;
			
			for (int i = 0; i < 1000; i++)
			{
				var pulses = PressButton(modules);
				lowPulses += pulses.low;
				highPulses += pulses.high;
            }
			
			return lowPulses * highPulses;
		}
		public static long Part2(string[] input)
		{
			var modules = ParseModules(input);
			
			// add rx module
			var rx = new Module("rx", 'N', new List<string>());
			rx.AddInputModules(input);
			modules.Add("rx", rx);

			// get con modules 2 steps backwards and create cycle array
			// this only works because these modules are all con modules
			var rxMem = rx.state as InputMemory;
            var lastCon = modules[rxMem.ToList()[0].Key].state as InputMemory;
			var cycles = new long[lastCon.Count];
			
			// press button as long as there is an undefined cycle
			for (int i = 1; cycles.Any(num => num == 0); i++)
				PressButton(modules, lastCon, i, cycles);

            return LeastCommonMultiple(cycles);
		}
	}
}