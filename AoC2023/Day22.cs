using Coord = (int x, int y);

namespace AoC2023
{
	public class Brick
	{
		public int x1, x2, y1, y2, z1, z2;
		public bool poof = false;
		public override string ToString() => x1 + "," + y1 + "," + z1 + " ; " + x2 + "," + y2 + "," + z2 + (poof ? "poof" : "");
		public bool Equal(Brick b)
		{
			return x1 == b.x1 && x2 == b.x2 && y1 == b.y1 && y2 == b.y2 && z1 == b.z1 && z2 == b.z2;
		}
		public List<Coord> XYGrid()
		{
			var grid = new List<Coord>();
			for (int x = x1; x <= x2; x++)
				for (int y = y1; y <= y2; y++)
					grid.Add((x, y));
			return grid;
		}
		public bool XYGridMatch(Brick b2) => XYGrid().Any(pos1 => b2.XYGrid().Any(pos2 => pos1 == pos2));
		public List<Brick> Supports(List<Brick> bricks)
		{
			return bricks.Where(b2 => b2.z1 == z2 + 1 && b2.z2 > z2 && XYGridMatch(b2)).ToList();
		}
		public List<Brick> SupportedBy(List<Brick> bricks)
		{
			return bricks.Where(b2 => b2.z2 == z1 - 1 && XYGridMatch(b2)).ToList();
		}
	}
	internal class Day22
	{
		public static Brick Clone(Brick brick)
		{
			return new Brick()
			{
				x1 = brick.x1,
				y1 = brick.y1,
				z1 = brick.z1,
				x2 = brick.x2,
				y2 = brick.y2,
				z2 = brick.z2
			};
		}
		public static Dictionary<Coord, int> CreateHeightTable(List<Brick> bricks)
		{
			var heightTable = new Dictionary<Coord, int>();
			int minX = bricks.Min(brick => brick.x1);
			int maxX = bricks.Max(brick => brick.x2);
			int minY = bricks.Min(brick => brick.y1);
			int maxY = bricks.Max(brick => brick.y2);
			for (int x = minX; x <= maxX; x++)
				for (int y = minY; y <= maxY; y++)
					heightTable.Add(new Coord(x, y), 0);
			return heightTable;
		}
		public static void UpdateHeightTable(Dictionary<Coord, int> heightTable, Brick brick)
		{
			for (int x = brick.x1; x <= brick.x2; x++)
				for (int y = brick.y1; y <= brick.y2; y++)
					heightTable[(x, y)] = Math.Max(heightTable[(x, y)], brick.z2);
		}
		public static List<Brick> ParseBricks(string[] input)
		{
			var bricks = new List<Brick>();
			foreach (var line in input)
			{
				var split = line.Split([',', '~']);
				bricks.Add(new Brick()
				{
					x1 = int.Parse(split[0]),
					y1 = int.Parse(split[1]),
					z1 = int.Parse(split[2]),
					x2 = int.Parse(split[3]),
					y2 = int.Parse(split[4]),
					z2 = int.Parse(split[5]),
				});
			}
			return bricks;
		}
		public static void Drop(List<Brick> bricks, Dictionary<Coord, int> heightTable, out int falls)
		{
			falls = 0;
			foreach (var brick in bricks)
			{
				int newZ = brick.XYGrid().Max(coord => heightTable[coord]) + 1;

				if (newZ != brick.z1)
					falls++;

				brick.z2 -= brick.z1 - newZ;
				brick.z1 = newZ;
				UpdateHeightTable(heightTable, brick);
			}
		}
		public static void DetermineDisintegratability(List<Brick> bricks)
		{
			foreach (var brick in bricks)
			{
				var supports = brick.Supports(bricks);
				if (brick.Supports(bricks).Count == 0 || supports.All(b2 => b2.SupportedBy(bricks).Count > 1))
					brick.poof = true;
			}
		}
		public static int GetBrickFalls(List<Brick> bricks, Brick brick)
		{
			var cloned = bricks.ConvertAll(Clone);
			cloned.Remove(cloned.Find(b => b.Equal(brick)));
			Drop(cloned, CreateHeightTable(cloned), out int falls);
			return falls;
		}
		public static int Part1(string[] input)
		{
			var bricks = ParseBricks(input);
			bricks.Sort((a, b) => a.z1.CompareTo(b.z1));
			var heightTable = CreateHeightTable(bricks);
			Drop(bricks, heightTable, out int falls);
			DetermineDisintegratability(bricks);

			return bricks.Count(brick => brick.poof);
		}
		public static int Part2(string[] input)
		{
			var bricks = ParseBricks(input);
			bricks.Sort((a, b) => a.z1.CompareTo(b.z1));
			var heightTable = CreateHeightTable(bricks);
			Drop(bricks, heightTable, out int falls);
			DetermineDisintegratability(bricks);

			return bricks.Sum(b => GetBrickFalls(bricks, b));
		}
	}
}