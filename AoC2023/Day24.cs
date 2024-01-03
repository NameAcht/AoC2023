using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;

/*
 * 
 */

namespace AoC2023
{
	struct Hailstone
	{
		public float px, py, pz, vx, vy, vz;
        public Hailstone EndOfLine((float min, float max) area)
        {
            float xMov = Math.Abs(vx < 0 ? (px - area.min) / vx : (area.max - px) / vx);
            float yMov = Math.Abs(vy < 0 ? (py - area.min) / vy : (area.max - py) / vy);
            float mov = Math.Min(xMov, yMov);
            var hs = new Hailstone()
            {
                px = px + vx * mov,
                py = py + vy * mov,
                pz = pz + vz * mov,
                vx = vx,
                vy = vy,
                vz = vz
            };
            return hs;
        }
        public override string ToString()
        {
            return px + ", " + py + ", " + pz + " @ " + vx + ", " + vy + ", " + vz;
        }
    }
	internal class Day24
	{
        public static bool IntersectIn2DArea(XY p1, XY p2, XY p3, XY p4, (float min, float max) area)
        {
            float ua = (p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X);
            float div = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);
            
            // parallel
            if (div == 0)
                return false;

            ua /= div;

            float x = p1.X + ua * (p2.X - p1.X);
            float y = p1.Y + ua * (p2.Y - p1.Y);

            // filter past intersections
            if (x > Math.Max(p1.X, p2.X) || x > Math.Max(p3.X, p4.X)
                || x < Math.Min(p1.X, p2.X) || x < Math.Min(p3.X, p4.X)
                || y > Math.Max(p1.Y, p2.Y) || y > Math.Max(p3.Y, p4.Y)
                || y < Math.Min(p1.Y, p2.Y) || y < Math.Min(p3.Y, p4.Y))
                return false;

            // outside area
            if(x < area.min || x > area.max || y < area.min || y > area.max)
                return false;

            return true;
        }
		public static List<Hailstone> ParseHailstones(string[] input)
		{
            var hailstones = new List<Hailstone>();
            foreach (var line in input)
            {
                var split = line.Split(['@', ',']);
                hailstones.Add(new Hailstone()
                {
                    px = float.Parse(split[0].Trim()),
                    py = float.Parse(split[1].Trim()),
                    pz = float.Parse(split[2].Trim()),
                    vx = float.Parse(split[3].Trim()),
                    vy = float.Parse(split[4].Trim()),
                    vz = float.Parse(split[5].Trim())
                });
            }
            return hailstones;
        }
        public static int Part1(string[] input)
		{
			var hailstones = ParseHailstones(input);            

            (float, float) area = input.Length == 5 ? (7, 27) : (200000000000000, 400000000000000);
            int sum = 0;

            for (int i = 0; i < hailstones.Count - 1; i++)
            {
                var hs1 = hailstones[i];
                var hs1End = hs1.EndOfLine(area);
                for (int j = i + 1; j < hailstones.Count; j++)
                {
                    var hs2 = hailstones[j];
                    var hs2End = hs2.EndOfLine(area);

                    var p1 = new XY(hs1.px, hs1.py);
                    var p2 = new XY(hs1End.px, hs1End.py);
                    var p3 = new XY(hs2.px, hs2.py);
                    var p4 = new XY(hs2End.px, hs2End.py);

                    if (IntersectIn2DArea(p1, p2, p3, p4, area))
                        sum++;
                }
            }

            return sum;
		}
        public static long Part2(string[] input)
        {
            var hailstones = ParseHailstones(input);

            for (int i = 0; i < hailstones.Count; i++)
            {
                var hs1 = hailstones[i];
                var hs1End = hs1.EndOfLine((hs1.px * (-2), hs1.px * 2));
                //Console.WriteLine("[[" + hs1.px + "," + hs1.py + "," + hs1.pz + "],[" + hs1End.px + "," + hs1End.py + "," + hs1End.pz + "]]");

                for (int j = i + 1; j < hailstones.Count; j++)
                {
                    var hs2 = hailstones[j];
                    var hs2End = hs2.EndOfLine((hs2.px * (-2), hs2.px * 2));

                    var p1 = new XYZ(hs1.px, hs1.py, hs1.pz);
                    var p2 = new XYZ(hs1End.px, hs1End.py, hs1End.pz);
                    var p3 = new XYZ(hs2.px, hs2.py, hs2.pz);
                    var p4 = new XYZ(hs2End.px, hs2End.py, hs2End.pz);

                    var p13 = p1 - p3;
                    var p21 = p2 - p1;
                    var p43 = p4 - p3;

                    double d1343 = p13.X * (double)p43.X + (double)p13.Y * p43.Y + (double)p13.Z * p43.Z;
                    double d4321 = p43.X * (double)p21.X + (double)p43.Y * p21.Y + (double)p43.Z * p21.Z;
                    double d1321 = p13.X * (double)p21.X + (double)p13.Y * p21.Y + (double)p13.Z * p21.Z;
                    double d4343 = p43.X * (double)p43.X + (double)p43.Y * p43.Y + (double)p43.Z * p43.Z;
                    double d2121 = p21.X * (double)p21.X + (double)p21.Y * p21.Y + (double)p21.Z * p21.Z;

                    double denom = d2121 * d4343 - d4321 * d4321;
                    double numer = d1343 * d4321 - d1321 * d4343;

                    double mua = numer / denom;
                    double mub = (d1343 + d4321 * (mua)) / d4343;

                    // shortest line between any two Vec3 points
                    var resultA = new XYZ(
                        p1.X + (float)mua * p21.X,
                        p1.Y + (float)mua * p21.Y,
                        p1.Z + (float)mua * p21.Z);
                    var resultB = new XYZ(
                        p3.X + (float)mub * p43.X,
                        p3.Y + (float)mub * p43.Y,
                        p3.Z + (float)mub * p43.Z);
                }
            }

            return 0;
        }
    }
}