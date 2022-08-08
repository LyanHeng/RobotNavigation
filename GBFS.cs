using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    // Greedy Best First Search
    public class GBFS : Agent
    {
        private List<int[]> _goal = new List<int[]>();
        private int[] _goalFound = new int[4];
        private Dictionary<int[], int[]> _res = new Dictionary<int[], int[]>();
        private List<int[]> _frontier = new List<int[]>();
        private int[] _start;
        private bool _isFound = false;
        private List<int> _goalDistanceInd = new List<int>();

        public GBFS(int x, int y, Grid grid, Window window) : base(x, y, grid, window)
        {
            _start = new int[] { X, Y, 0, 0 };
        }

        public List<string> Search()
        {
            // get the closest goal
            _goal = Grid.GetGoals;
            _start = new int[_goal.Count + 3];
            _start[0] = X;
            _start[1] = Y;
            _start[_goal.Count + 2] = 0;
            // x, y, distance to goal, cost it takes to node (depth)
            for (int i = 0; i < _goal.Count; i++)
            {
                _goalDistanceInd.Add(i);
                _start[i + 2] = Distance(new int[] { X, Y }, _goal[i]);
            }

            // result and frontier
            _frontier.Add(_start);
            TotalNodes++;

            Recursive(_start);

            if (_isFound)
            {
                // outline the paths in the hashmap
                GetDirectionFromMap(_goalFound, _start, _res);
                Path.Add("Goal Reached!");
            }
            else
            {
                Path.Add("No paths were found");
            }

            return Path;
        }

        // recursive part of the algorithm
        private void Recursive(int[] current)
        {
            TotalNodes++;
            DrawSearchMap(_res, current, _start, _window);
            // check if goal is reached
            if (Grid.IsGoal(current))
            {
                _isFound = true;
                if (_goalFound != current)
                    _goalFound = current;
            }
            else
            {
                // expand current node into the frontier
                // sorting the frontier by cost
                List<int[]> temp = Grid.NextMoves(current);
                _frontier.Remove(current);
                foreach (int[] t in temp)
                {
                    int[] noDir = new int[_goal.Count + 3];
                    // add a new node 
                    noDir[0] = t[0];
                    noDir[1] = t[1];
                    noDir[_goal.Count + 2] = current[_goal.Count + 2] + 1;

                    // get its distance to the goal
                    for (int i = 0; i < _goal.Count; i++)
                    {
                        noDir[i + 2] = Distance(t, _goal[i]);
                    }

                    // check for visited states
                    if (!ContainsValue(noDir, _res))
                    {
                        _frontier.Add(noDir);
                        _res.Add(noDir, current);
                    }

                }

                // only expands if the goal state is not found 
                if (!_isFound)
                {
                    // choose the frontier that has 
                    current = GetNextBestFrontier(_frontier);
                    Recursive(current);
                }
            }
        }

        // using greedy algorithm, choose the next best frontier
        private int[] GetNextBestFrontier(List<int[]> _frontier)
        {
            int tempMin = 0;
            int[] tempNextNode = new int[_goal.Count + 3];
            foreach (int[] f in _frontier)
            {
                for (int i = 0; i < _goalDistanceInd.Count; i++)
                {
                    if (tempMin == 0 || f[i + 2] < tempMin)
                    {
                        tempMin = f[i+2];
                        tempNextNode = f;
                    }
                }
            }
            return tempNextNode;
        }

    }
}
