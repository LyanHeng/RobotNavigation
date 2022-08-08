using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    // A* 
    public class AStar : Agent
    {
        private List<int[]> _goals = new List<int[]>();
        private int[] _goalReached = new int[4];
        private Dictionary<int[], int[]> _res = new Dictionary<int[], int[]>();
        private List<int[]> _frontier = new List<int[]>();
        private int[] _start;
        private List<int> _goalDistanceInd = new List<int>();

        public AStar(int x, int y, Grid grid, Window window) : base(x, y, grid, window)
        {
            _start = new int[] { X, Y, 0, 0, 0 };
        }

        public List<string> Search()
        {
            // get goals and initialise start cells
            _goals = Grid.GetGoals;
            _start = new int[_goals.Count + 3];
            _start[0] = X;
            _start[1] = Y;
            _start[_goals.Count + 2] = 0;
            // x, y, distance to goal, cost it takes to node (depth)
            for (int i = 0; i < _goals.Count; i++)
            {
                _goalDistanceInd.Add(i);
                _start[i+2] = Distance(new int[] { X, Y }, _goals[i]);
            }

            // result and frontier
            _frontier.Add(_start);
            TotalNodes++;

            Recursive(_start);
            if (Grid.IsGoal(_goalReached))
            {
                // convert path to readable strings
                GetDirectionFromMap(_goalReached, _start, _res);
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
            // check if goal is reached
            if (Grid.IsGoal(current))
            {
                if (!Grid.IsGoal(_goalReached))
                    _goalReached = current;
            }
            else
            {
                // expand current node into the frontier
                // sorting the frontier by cost
                List<int[]> temp = Grid.NextMoves(current);
                _frontier.Remove(current);
                foreach (int[] t in temp)
                {
                    int[] noDir = new int[_goals.Count + 3];
                    // add a new node 
                    noDir[0] = t[0];
                    noDir[1] = t[1];
                    noDir[_goals.Count + 2] = current[_goals.Count + 2] + 1;
                    for (int i = 0; i < _goals.Count; i++)
                    {
                        noDir[i + 2] = Distance(t, _goals[i]);
                    }
                    // g(n) = h(n) + f(n);
                    if (!ContainsValue(noDir, _res))
                    {
                        DrawSearchMap(_res, current, _start, _window);
                        _frontier.Add(noDir);
                        _res.Add(noDir, current);
                    }
                }

                // choose the frontier that has 
                current = GetNextBestFrontier(_frontier);
                Recursive(current);
            }
        }

        // using greedy algorithm, choose the next best frontier
        private int[] GetNextBestFrontier(List<int[]> _frontier)
        {
            int tempMin = 0;
            int[] tempNextNode = new int[_goals.Count + 3];
            foreach (int[] f in _frontier)
            {
                for (int i = 0; i < _goalDistanceInd.Count; i++)
                {
                    // compare the states based on the sum of their cost and heuristics
                    if (tempMin == 0 || (f[i + 2] + f[_goalDistanceInd.Count + 2]) < tempMin)
                    {
                        tempMin = f[i+2] + f[_goalDistanceInd.Count + 2];
                        tempNextNode = f;
                    }
                }
            }
            return tempNextNode;
        }
    }
}
