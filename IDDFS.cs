using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    /********
     * CUSTOM ALGO 1: Iterative Deepening Depth First Search
     * ******/
    public class IDDFS : Agent
    {
        private Stack<int[]> _result = new Stack<int[]>();
        private const int DEPTHLIMIT = 100;
        public IDDFS(int x, int y, Grid grid, Window window) : base(x, y, grid, window) { }

        public List<string> Search()
        {
            int[] start = new int[] { X, Y, 0 };

            // iteratively increase the depth of search
            for (int i = 0; i < DEPTHLIMIT; i++)
            {
                _result.Push(start);
                TotalNodes++;
                Recursive(start, 0, i);
                if (_result.Count != 0)
                    break;
                else
                    TotalNodes = 0;
            }

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
        private Stack<int[]> Recursive(int[] current, int curDepth, int iterDepth)
        {
            // expand current node
            List<int[]> frontier = Grid.NextMoves(current);

            // check every node in the frontier
            foreach (int[] nextNode in frontier)
            {
                DrawSearchStack(_result, _window);
                TotalNodes++;

                // check if goal state is already found
                if (Grid.IsGoal(_result.Peek()))
                {
                    return _result;
                }
                // check if the current node is goal state
                else if (Grid.IsGoal(nextNode))
                {
                    _result.Push(nextNode);
                    break;
                }
                // check for visited nodes
                else if (Visited(_result.ToList(), nextNode))
                {
                    if (nextNode == frontier[frontier.Count - 1])
                    {
                        _result.Pop();
                    }
                }
                else
                {
                    // check if reached the maximum depth given in iteration
                    if (curDepth >= iterDepth && nextNode == frontier[frontier.Count - 1])
                    {
                        // clear everything
                        _result.Clear();
                        return _result;
                    }
                    // continue expanding if the depth is not the limit
                    else if (curDepth + 1 < iterDepth)
                    {
                        _result.Push(nextNode);
                        _result = Recursive(nextNode, curDepth + 1, iterDepth);
                    }
                }
            }

            // case search reaches a deadend
            if (_result.Peek()[0] == current[0] && _result.Peek()[1] == current[1])
            {
                _result.Pop();
            }

            return _result;
        }

    }
}
