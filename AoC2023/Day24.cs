using XY = System.Numerics.Vector2;
using MathNet.Numerics;

namespace AoC2023
{
    struct Hailstone
    {
        public decimal x, y, z, dx, dy, dz;
        public Hailstone EndOfLine((float min, float max) area)
        {
            decimal xMov = Math.Abs(dx < 0 ? (x - (decimal)area.min) / dx : ((decimal)area.max - x) / dx);
            decimal yMov = Math.Abs(dy < 0 ? (y - (decimal)area.min) / dy : ((decimal)area.max - y) / dy);
            decimal mov = Math.Min(xMov, yMov);
            var hs = new Hailstone()
            {
                x = x + dx * mov,
                y = y + dy * mov,
                z = z + dz * mov,
                dx = dx,
                dy = dy,
                dz = dz
            };
            return hs;
        }
    }
    internal class Day24
    {
        public static decimal[,] EquationMatrix(Hailstone a, Hailstone b, Hailstone c, Hailstone d)
        {
            var vector = EquationVector(a, b, c, d);
            return new decimal[,] {
                { b.dy-a.dy, a.dx-b.dx, 0, a.y-b.y, b.x-a.x, 0, vector[0] },
                { b.dz-a.dz, 0, a.dx-b.dx, a.z-b.z, 0, b.x-a.x, vector[1] },
                { 0, a.dz-b.dz, b.dy-a.dy, 0, b.z-a.z, a.y-b.y, vector[2] },

                { d.dy-c.dy, c.dx-d.dx, 0, c.y-d.y, d.x-c.x, 0, vector[3] },
                { d.dz-c.dz, 0, c.dx-d.dx, c.z-d.z, 0, d.x-c.x, vector[4] },
                { 0, c.dz-d.dz, d.dy-c.dy, 0, d.z-c.z, c.y-d.y, vector[5] }
            };
        }
        public static decimal[] EquationVector(Hailstone a, Hailstone b, Hailstone c, Hailstone d)
        {
            decimal[] vector = [
                b.x * b.dy - b.y * b.dx - a.x * a.dy + a.y * a.dx,
                b.x * b.dz - b.z * b.dx - a.x * a.dz + a.z * a.dx,
                -b.y * b.dz + b.z * b.dy + a.y * a.dz - a.z * a.dy,
                d.x * d.dy - d.y * d.dx - c.x * c.dy + c.y * c.dx,
                d.x * d.dz - d.z * d.dx - c.x * c.dz + c.z * c.dx,
                -d.y * d.dz + d.z * d.dy + c.y * c.dz - c.z * c.dy
            ];
            return vector;
        }
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
            if (x < area.min || x > area.max || y < area.min || y > area.max)
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
                    x = decimal.Parse(split[0].Trim()),
                    y = decimal.Parse(split[1].Trim()),
                    z = decimal.Parse(split[2].Trim()),
                    dx = decimal.Parse(split[3].Trim()),
                    dy = decimal.Parse(split[4].Trim()),
                    dz = decimal.Parse(split[5].Trim())
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

                    var p1 = new XY((float)hs1.x, (float)hs1.y);
                    var p2 = new XY((float)hs1End.x, (float)hs1End.y);
                    var p3 = new XY((float)hs2.x, (float)hs2.y);
                    var p4 = new XY((float)hs2End.x, (float)hs2End.y);

                    if (IntersectIn2DArea(p1, p2, p3, p4, area))
                        sum++;
                }
            }

            return sum;
        }
        public static decimal Part2(string[] input)
        {
            var hs = ParseHailstones(input);
            var matrix = EquationMatrix(hs[0], hs[1], hs[2], hs[3]);
            return LinearSolver.Solve(matrix)[0..3].Sum().Round(0);
        }
    }
}