namespace AoC2023
{
    internal class Day17
    {
        public enum Direction
        {
            Up, Left, Down, Right
        }
        public struct State
        {
            public int row, col, dirSteps, heatLoss;
            public Direction dir;
            public List<(int row, int col)> path;
            public State(int row, int col, int dirSteps, int heatLoss, Direction dir)
            {
                this.row = row;
                this.col = col;
                this.dirSteps = dirSteps;
                this.dir = dir;
                this.heatLoss = heatLoss;
                this.path = new List<(int row, int col)> ();
            }
            public State()
            {
                dir = Direction.Right;
                path = new List<(int row, int col)>();
            }
            public State GenerateState(Direction newDir, string[] map)
            {
                (int row, int col) newPos = newDir switch
                {
                    Direction.Up => (row - 1, col),
                    Direction.Left => (row, col - 1),
                    Direction.Down => (row + 1, col),
                    Direction.Right => (row, col + 1)
                };

                int newSteps = newDir == dir ? dirSteps + 1 : 1;
                int newHeatLoss = heatLoss + int.Parse(map[newPos.row][newPos.col].ToString());

                return new State(newPos.row, newPos.col, newSteps, newHeatLoss, newDir);
            }
            public override string ToString()
            {
                return row + "," + col + "," + dir + "," + dirSteps;
            }
        }
        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
        public static bool OutOfBounds(string[] map, State state, Direction dir)
        {
            if (dir == Direction.Up && state.row <= 0)
                return true;
            if (dir == Direction.Left && state.col <= 0)
                return true;
            if (dir == Direction.Down && state.row >= map.Length - 1)
                return true;
            if (dir == Direction.Right && state.col >= map[state.row].Length - 1)
                return true;
            return false;
        }
        public static int MinimizeHeatLoss(string[] map, int minSteps, int maxSteps)
        {
            var curr = new State();
            (int row, int col) end = (map.Length - 1, map.Last().Length - 1);

            var queue = new PriorityQueue<State, int>();
            queue.Enqueue(curr, 0);

            int min = int.MaxValue;

            var table = new Dictionary<(int row, int col, int stepsLeft, Direction dir), int>();

            while (queue.Count > 0)
            {
                curr = queue.Dequeue();

                if ((curr.row, curr.col) == end && curr.dirSteps >= minSteps)
                    min = Math.Min(min, curr.heatLoss);

                // opposite direction is current direction + 2 mod 4
                var opposite = (Direction)Mod((int)curr.dir + 2, 4);

                // iterate over possible directions, continue if direction is not valid
                for (Direction dir = 0; (int)dir < 4; dir++)
                {
                    // turn left or right -> invalid if minimum steps not fulfilled
                    // allow turning for starting square (0, 0)
                    if(dir != opposite && dir != curr.dir)
                        if (curr.dirSteps < minSteps && (curr.row, curr.col) != (0, 0))
                            continue;

                    // same direction -> invalid if max steps are already reached
                    if (dir == curr.dir && maxSteps == curr.dirSteps)
                        continue;

                    // opposite direction always invalid
                    if (dir == opposite)
                        continue;

                    if (OutOfBounds(map, curr, dir))
                        continue;

                    var nextState = curr.GenerateState(dir, map);

                    var seen = table.TryGetValue((nextState.row, nextState.col, nextState.dirSteps, nextState.dir), out int seenVal);
                    if (!seen || seenVal > nextState.heatLoss)
                    {
                        queue.Enqueue(nextState, nextState.heatLoss);
                        table[(nextState.row, nextState.col, nextState.dirSteps, nextState.dir)] = nextState.heatLoss;
                    }
                }
            }

            return min;
        }
        public static int Part1(string[] map) => MinimizeHeatLoss(map, 0, 3);
        public static int Part2(string[] map) => MinimizeHeatLoss(map, 4, 10);
    }
}