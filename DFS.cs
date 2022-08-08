using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    // Depth-first Search
    public class DFS : Agent
    {
        private Stack<int[]> _result = new Stack<int[]>();

        public DFS(int x, int y, Grid grid, Window window) : base(x, y, grid, window)
        { }

        // Depth-first Search - initialise
        public List<string> Search()
        {
            int[] start = new int[] { X, Y, 0 };
            _result.Push(start);
            TotalNodes++;

            // get path
            _result = Recursive(start);

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

        // Recursive DFS algorithm
        private Stack<int[]> Recursive(int[] current)
        {
            // expand nodes
            List<int[]> frontier = Grid.NextMoves(current);

            // iterating over all nodes in the frontier
            foreach (int[] nextNode in frontier)
            {
                DrawSearchStack(_result, _window);
                TotalNodes++;
                
                // check that goal is already found
                if (Grid.IsGoal(_result.Peek()))
                {
                    DrawSearchStack(_result, _window);
                    return _result;
                }

                // check that the current node is the goal
                else if (Grid.IsGoal(nextNode))
                {
                    _result.Push(nextNode);
                    break;
                }

                // check for visited states
                else if (Visited(_result.ToList(), nextNode))
                {
                    if (nextNode == frontier[frontier.Count - 1])
                    {
                        _result.Pop();
                    }
                }

                // expand
                else
                {
                    _result.Push(nextNode);
                    _result = Recursive(nextNode);
                }
            }

            // case search hits a dead-end branch
            if (_result.Peek()[0] == current[0] && _result.Peek()[1] == current[1])
            {
                _result.Pop();
            }

            return _result;
        }
    }
}
