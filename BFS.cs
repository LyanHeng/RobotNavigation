using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    // Breadth-first Search
    public class BFS : Agent
    {
        private int[] _goal = new int[3];
        private Dictionary<int[], int[]> _res = new Dictionary<int[], int[]>();
        private Queue<int[]> _frontier = new Queue<int[]>();
        private int[] _start;

        public BFS(int x, int y, Grid grid, Window window) : base(x, y, grid, window)
        {
            _start = new int[] { X, Y };
        }

        // BFS - initialisations
        public List<string> Search()
        {
            // result and frontier
            _frontier.Enqueue(_start);
            TotalNodes++;

            // find the goal and map the paths
            Recursive(_start);

            if (Grid.IsGoal(_goal))
            {
                // outline the paths in the hashmap
                GetDirectionFromMap(_goal, _start, _res);
                Path.Add("Goal Reached!");
            }
            else
            {
                Path.Add("No paths were found");
            }

            return Path;
        }

        // doing the BFS - recursive part
        private void Recursive(int[] current)
        {
            TotalNodes++;
            DrawSearchMap(_res, current, _start, _window);
            // check if goal is reached
            if (Grid.IsGoal(current))
            {
                if (!Grid.IsGoal(_goal))
                    _goal = current;
            }
            else
            {
                // expand current node into the frontier
                List<int[]> temp = Grid.NextMoves(current);
                _frontier.Dequeue();
                foreach (int[] t in temp)
                {
                    int[] noDir = { t[0], t[1] };

                    // check for visited states
                    if (!ContainsValue(noDir, _res))
                    {
                        _frontier.Enqueue(noDir);
                        _res.Add(noDir, current);
                    }
                }
                current = _frontier.Peek();
                Recursive(current);
            }
        }
    }
}
