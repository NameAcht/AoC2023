using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;

//  Let:
//    <p_rock>(t) = <X,Y,Z> + t*<DX,DY,DZ> 
//    <p_hail>(t) = <x,y,z> + t*<dx,dy,dz>

//  A rock-hail collision requires the following to be true:
//    X+t*DX = x+t*dx
//    Y+t*DY = y+t*dy
//    Z+t*DZ = z+t*dz
/*
 *  ->
 *  t = (X-x)/(dx-DX)
 *  t = (Y-y)/(dy-DY)
 *  t = (Z-z)/(dz-DZ)
 *  
 *  Equating the first two equalities from above yields:
 *    (X-x)/(dx-DX) = (Y-y)/(dy-DY)
 *    (X-x) (dy-DY) = (Y-y) (dx-DX)
 *    X*dy - X*DY - x*dy + x*DY = Y*dx - Y*DX - y*dx + y*DX
 *    Y*DX - X*DY = Y*dx - y*dx + y*DX - X*dy + x*dy - x*DY
 *      
 *  Note that the LHS of the above equation is true for any hail stone. Evaluating
 *  the RHS again for a different hailstone, and setting the two RHS equal, yields 
 *  the first of the below equations:
 *      
 *  (dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY =  x' dy' - y' dx' - x dy + y dx
 *  (dz'-dz) X + (dx-dx') Z + (z-z') DX + (x'-x) DZ =  x' dz' - z' dx' - x dz + z dx
 *  (dz-dz') Y + (dy'-dy) Z + (z'-z) DY + (y-y') DZ = -y' dz' + z' dy' + y dz - z dy
 *      
 *  The second and third are yielded by repeating the above process with X & Z, and 
 *  then Y & Z. This is a system of equations with 6 unknowns. Using two different
 *  pairs of hailstones (e.g. three total hailstones) yields 6 equations with 6
 *  unknowns, which we can now solve relatively trivially using linear algebra.
 *  
 *  
 *  Ex 1:
 *  19x, 13y, 30z @ -2dx,  1dy, -2dz
 *  18x', 19y', 22z' @ -1dx', -1dy', -2dz'
 *  
 *  (dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY =  x' dy' - y' dx' - x dy + y dx
 *  (dz'-dz) X + (dx-dx') Z + (z-z') DX + (x'-x) DZ =  x' dz' - z' dx' - x dz + z dx
 *  (dz-dz') Y + (dy'-dy) Z + (z'-z) DY + (y-y') DZ = -y' dz' + z' dy' + y dz - z dy
 *  
 *  (-1-1) X + (-2-(+1)) Y + (13-(+19)) DX + (18-19) DY = 18*(-1) - 19*(-1) - 19*1 + 13*(-2)
 *  (-1-1) X + (-2-(+1)) Z + (30-22) DX + (18-19) DZ = 18*(-2) - 22*(-1) - 19*(-2) + 30*(-2)
 *  (-2-(-2)) Y + (-1-(-1)) Z + (22-30) DY + (13-19) DZ = -19*(-2) + 22*(-1) + 13*(-2) - 30*1
 *  
 *  20x, 25y, 34z @ -2dx, -2dy, -4dz
 *  12x', 31y', 28z' @ -1dx', -2dy', -1dz'
 *  
 *  
 *  
 *  
 */

namespace AoC2023
{
    struct Vec3
    {
        decimal X, Y, Z;
    }
	struct Hailstone
	{
		public float x, y, z, dx, dy, dz;
        public Hailstone EndOfLine((float min, float max) area)
        {
            float xMov = Math.Abs(dx < 0 ? (x - area.min) / dx : (area.max - x) / dx);
            float yMov = Math.Abs(dy < 0 ? (y - area.min) / dy : (area.max - y) / dy);
            float mov = Math.Min(xMov, yMov);
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
        public override string ToString()
        {
            return x + ", " + y + ", " + z + " @ " + dx + ", " + dy + ", " + dz;
        }
        public void PrintEquation(Hailstone p)
        {
            Console.WriteLine($"({p.dy}-{dy}) X + ({dx}-{p.dx}) Y + ({y} - {p.y}) DX + ({p.x} - {x}) DY = {p.x}*{p.dy} - {p.y}*{p.dx} - {x}*{dy} + {y}*{dx}");
            Console.WriteLine($"({p.dz}-{dz}) X + ({dx}-{p.dx}) Z + ({z} - {p.z}) DX + ({p.x} - {x}) DZ = {p.x}*{p.dz} - {p.z}*{p.dx} - {x}*{dz} + {z}*{dx}");
            Console.WriteLine($"({dz} - {p.dz}) Y + ({p.dy} - {dy}) Z + ({p.z}-{z}) DY + ({y} - {p.y}) DZ = -{p.y}*{p.dz} + {p.z}*{p.dy} + {y}*{dz} - {z}*{dy}");
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
                var split = line.Split(new char[] { '@', ',' });
                hailstones.Add(new Hailstone()
                {
                    x = float.Parse(split[0].Trim()),
                    y = float.Parse(split[1].Trim()),
                    z = float.Parse(split[2].Trim()),
                    dx = float.Parse(split[3].Trim()),
                    dy = float.Parse(split[4].Trim()),
                    dz = float.Parse(split[5].Trim())
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

                    var p1 = new XY(hs1.x, hs1.y);
                    var p2 = new XY(hs1End.x, hs1End.y);
                    var p3 = new XY(hs2.x, hs2.y);
                    var p4 = new XY(hs2End.x, hs2End.y);

                    if (IntersectIn2DArea(p1, p2, p3, p4, area))
                        sum++;
                }
            }

            return sum;
		}
        public static long Part2(string[] input)
        {
            var hailstones = ParseHailstones(input);


            return 0;
        }
    }
}