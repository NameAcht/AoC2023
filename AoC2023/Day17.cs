//namespace AoC2023
//{
//    internal class Day17
//    {
//        public enum Direction
//        {
//            Up, Left, Down, Right
//        }
//        public struct State
//        {
//            public int row, col, dirStepsLeft, heatLoss;
//            public Direction dir;
//            public State(int row, int col, int dirStepsLeft, int heatLoss, Direction dir)
//            {
//                this.row = row;
//                this.col = col;
//                this.dirStepsLeft = dirStepsLeft;
//                this.dir = dir;
//                this.heatLoss = heatLoss;
//            }
//            public State GenerateState(Direction newDir, string[] map)
//            {
//                (int row, int col) newPos = newDir switch
//                {
//                    Direction.Up => (row - 1, col),
//                    Direction.Left => (row, col - 1),
//                    Direction.Down => (row + 1, col),
//                    Direction.Right => (row, col + 1)
//                };

//                // same direction -> subtract one from steps left, else 2
//                int newStepsLeft = newDir == dir ? dirStepsLeft - 1 : 2;
//                int newHeatLoss = heatLoss + int.Parse(map[newPos.row][newPos.col].ToString());

//                return new State(newPos.row, newPos.col, newStepsLeft, newHeatLoss, newDir);
//            }
//            public int EvaluatePriority(string[] map) => (int)(4.5 * (map.Length - row + map[0].Length - col - 2) + heatLoss);
//            //public int EvaluatePriority(string[] map) => heatLoss;
//        }
//        public static int Mod(int x, int m)
//        {
//            return (x % m + m) % m;
//        }
//        public static int MinimizeHeatLoss(string[] map, State curr, (int row, int col) end)
//        {
//            var queue = new PriorityQueue<State, int>();
//            queue.Enqueue(curr, 0);
//            int min = int.MaxValue;

//            var table = new Dictionary<(int row, int col, int stepsLeft, Direction dir), int>();

//            while (queue.Count > 0)
//            {
//                curr = queue.Dequeue();

//                // opposite direction is current direction + 2 mod 4
//                var opposite = (Direction)Mod((int)curr.dir + 2, 4);

//                for (Direction dir = 0; (int)dir < 4; dir++)
//                {
//                    if (dir == opposite)
//                        continue;
//                    if (dir == curr.dir && curr.dirStepsLeft == 0)
//                        continue;

//                    // dont enqueue out of bound states
//                    if (dir == Direction.Up && curr.row == 0)
//                        continue;
//                    if (dir == Direction.Left && curr.col == 0)
//                        continue;
//                    if (dir == Direction.Down && curr.row == map.Length - 1)
//                        continue;
//                    if (dir == Direction.Right && curr.col == map.First().Length - 1)
//                        continue;

//                    var newState = curr.GenerateState(dir, map);

//                    var seen = table.TryGetValue((newState.row, newState.col, newState.dirStepsLeft, newState.dir), out int seenVal);
//                    if (!seen || seenVal > newState.heatLoss)
//                    {
//                        queue.Enqueue(newState, newState.EvaluatePriority(map));
//                        table[(newState.row, newState.col, newState.dirStepsLeft, newState.dir)] = newState.heatLoss;
//                    }
//                }
//                if ((curr.row, curr.col) == end)
//                    min = Math.Min(min, curr.heatLoss);
//            } 

//            return min;
//        }
//        public static int Part1(string[] map) => MinimizeHeatLoss(map, new State() { dirStepsLeft = 3, dir = Direction.Right }, (map.Length - 1, map.Last().Length - 1));
//        public static int Part2(string[] input)
//        {


//            return 0;
//        }
//    }
//}
using System.Runtime.CompilerServices;

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
            public State(int row, int col, int dirSteps, int heatLoss, Direction dir)
            {
                this.row = row;
                this.col = col;
                this.dirSteps = dirSteps;
                this.dir = dir;
                this.heatLoss = heatLoss;
            }
            public State()
            {
                dir = Direction.Right;
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
            public int EvaluatePriority(string[] map) => (int)(4.5 * (map.Length - row + map[0].Length - col - 2) + heatLoss);
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
            if (dir == Direction.Right && state.col >= map.First().Length - 1)
                return true;
            return false;
        }
        public static bool CantStopInBounds(string[] map, State state, int minSteps, Direction dir)
        {
            if (dir != state.dir)
                state.dirSteps = 0;


            if (dir == Direction.Up && state.row + state.dirSteps <= minSteps - state.dirSteps)
                return true;
            if (dir == Direction.Left && state.col + state.dirSteps <= minSteps - state.dirSteps)
                return true;
            if (dir == Direction.Down && state.row - state.dirSteps >= map.Length - 1 - minSteps)
                return true;
            if (dir == Direction.Right && state.col - state.dirSteps >= map.First().Length - 1 - minSteps )
                return true;

            return false;
        }
        // Solves part 2 test input but not real input
        // might be clearing out states that should not be cleared out
        // check bound verification
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

                // opposite direction is current direction + 2 mod 4
                var opposite = (Direction)Mod((int)curr.dir + 2, 4);

                for (Direction dir = 0; (int)dir < 4; dir++)
                {
                    if (dir == opposite)
                        continue;
                    if (dir == curr.dir && curr.dirSteps == maxSteps)
                        continue;
                    if (dir != opposite && dir != curr.dir && curr.dirSteps <= minSteps)
                        continue;

                    if (OutOfBounds(map, curr, dir))
                        continue;
                    if (minSteps > 0 && CantStopInBounds(map, curr, minSteps, dir))
                        continue;

                    var newState = curr.GenerateState(dir, map);

                    var seen = table.TryGetValue((newState.row, newState.col, newState.dirSteps, newState.dir), out int seenVal);
                    if (!seen || seenVal > newState.heatLoss)
                    {
                        queue.Enqueue(newState, newState.EvaluatePriority(map));
                        table[(newState.row, newState.col, newState.dirSteps, newState.dir)] = newState.heatLoss;
                    }
                }
                if ((curr.row, curr.col) == end)
                    min = Math.Min(min, curr.heatLoss);
            }

            return min;
        }
        public static int Part1(string[] map) => MinimizeHeatLoss(map, -1, 3);
        public static int Part2(string[] map) => MinimizeHeatLoss(map, 3, 10);
    }
}