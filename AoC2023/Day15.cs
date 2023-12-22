namespace AoC2023
{
	internal class Day15
	{
        	public static int Hash(string toHash)
		{
			int currVal = 0;
			foreach(var c in toHash)
				currVal = (currVal + c) * 17 % 256;
			return currVal;
		}
		public static int Part1(string input) => input.Split(',').Sum(entry => Hash(entry.Trim()));
		public static int Part2(string input)
		{
			int sum = 0;
			var entrySplitter = new char[] { '-', '=' };
			// initialize
			var boxes = new List<(string label, int lens)>[256];
			for (int i = 0; i < boxes.Length; i++)
				boxes[i] = new List<(string, int)>();
			
			// do box operations
			foreach (var entry in input.Split(','))
			{
				var split = entry.Trim().Split(entrySplitter);
	        		var box = boxes[Hash(split[0])];
	
				var boxEntry = box.Find(l => l.label == split[0]);
	
				if (entry.Trim().Last() == '-')
	                    		box.Remove(boxEntry);
				else if (entry.Contains('=') && boxEntry != (null, 0))
					box[box.IndexOf(boxEntry)] = (split[0], int.Parse(split[1]));
				else
					box.Add((split[0], int.Parse(split[1])));
			}
	
			// sum lens powers
			for (int i = 0; i < boxes.Length; i++)
				for (int j = 0; j < boxes[i].Count; j++)
					sum += (j + 1) * (i + 1) * boxes[i][j].lens;
				
			return sum;
		}
	}
}
