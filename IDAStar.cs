using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    /*********
     * CUS2: A Star Iterative Deepening Depth First Search
     * *******/
    public class IDAStar : Agent
    {
        private List<int[]> _goals = new List<int[]>();
        private Stack<int[]> _result = new Stack<int[]>();
        private int curDepth = 0;
        private int minF = 0;
        private const int SEARCHDEGREE = 10;
        public IDAStar(int x, int y, Grid grid, Window window) : base(x, y, grid, window) { }

        public List<string> Search()
        {
            int[] start = new int[] { X, Y, 0 };
            _goals = Grid.GetGoals;

            // set initial minF value 
            minF = Grid.NumberOfTiles - 1;

            // increase the degree of freedom if paths are not found
            for (int i = 1; i < SEARCHDEGREE; i++)
            {
                _result.Push(start);
                TotalNodes++;
                Recursive(start, i);
                if (_result.Count > 0)
                    break;
                else
                    TotalNodes = 0;
            }

            // get path
            if (_result.Count > 0 && Grid.IsGoal(_result.Peek()))
            {
                // convert path to readable strings
                Path = GetDirectionFromStack(_result);
                Path.Add("Goal Reached!");
            }
            else
            {
                Path.Add("No paths were found");
            }
            return Path;
        }

        // Recursive algorithm
        private Stack<int[]> Recursive(int[] current, int searchFreedom)
        {
            List<int[]> frontier = Grid.NextMoves(current);
            frontier.Reverse();

            do
            {
                DrawSearchStack(_result, _window);
                TotalNodes++;
                
                // provide full freedom to move in case it is allowed
                if (frontier.Count == 1 && curDepth <= searchFreedom)
                    minF = Grid.NumberOfTiles - 1;

                // get the next best node
                int[] nextNode = GetNextBestFrontier(frontier);

                // check that goal state is already found
                if (Grid.IsGoal(_result.Peek()))
                {
                    return _result;
                }
                // check if current state is the goal state
                else if (Grid.IsGoal(nextNode))
                {
                    _result.Push(nextNode);
                    break;
                }
                // check for visited states
                else if (Visited(_result.ToList(), nextNode))
                {
                    frontier.Remove(nextNode);
                }
                else
                {
                    // update threshold and expand
                    frontier.Remove(nextNode);
                    int tempF = Distance(nextNode, Grid.GetClosestGoal(nextNode)) + curDepth + 1;
                    if (tempF <= minF)
                    {
                        curDepth++;
                        minF = tempF;
                        _result.Push(nextNode);
                        Recursive(nextNode, searchFreedom);
                    }
                }
            } while (frontier.Count > 0);

            if (_result.Peek()[0] == current[0] && _result.Peek()[1] == current[1])
            {
                _result.Pop();
                // move back
                curDepth--;
                // check if there are cheaper nodes to expand after exhausting all frontiers
                // case the level is still allowed some search freedoms
                if (_result.Count == searchFreedom)
                    minF = Grid.NumberOfTiles - 1;
                else if (_result.Count > 1)
                    minF = Distance(_result.Peek(), Grid.GetClosestGoal(_result.Peek())) + curDepth;
            }

            return _result;
        }

        // choose the next best frontier by h(n) + g(n)
        private int[] GetNextBestFrontier(List<int[]> frontier)
        {
            int tempMin = 0;
            int[] tempNextNode = new int[3];
            foreach (int[] f in frontier)
            {
                if (tempMin == 0 || Distance(f, Grid.GetClosestGoal(f)) + curDepth < tempMin)
                {
                    tempMin = Distance(f, Grid.GetClosestGoal(f)) + curDepth + 1;
                    tempNextNode = f;
                }
            }
            return tempNextNode;
        }
    }
}
