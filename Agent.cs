using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    // acts as a superclass for all the algorithm
    // contains all the operation and fields needed for most agents
    public class Agent
    {
        // knows its own location
        private int _x;
        private int _y;
        private Grid _grid;
        private int _totalNodes;
        protected Window _window;
        private List<string> _path = new List<string>();

        // REF: for final result:
        // 0 - start / deadend
        // 1 - UP
        // 2 - LEFT
        // 3 - DOWN
        // 4 - RIGHT

        public Agent(int x, int y, Grid grid, Window window)
        {
            _x = x;
            _y = y;
            _grid = grid;
            _window = window;
        }

        public int X { get => _x; }
        public int Y { get => _y; }
        public Grid Grid { get => _grid; }
        public int TotalNodes { get => _totalNodes; set => _totalNodes = value; }
        public List<string> Path { get => _path; set => _path = value; }

        // h(n) - Find the step/cost it takes from the cell to the goal
        protected int Distance(int[] cell, int[] goal)
        {
            return Math.Abs(goal[0] - cell[0]) + Math.Abs(goal[1] - cell[1]);
        }

        // Check if the path is already visited - USED IN DFS
        protected bool Visited(List<int[]> stack, int[] element)
        {
            bool result = false;
            foreach (int[] s in stack)
            {
                if (s[0] == element[0] && s[1] == element[1])
                    result = true;
            }
            return result;
        }

        // check if its looping back to the same node
        // secondary purpose: delete all those nodes - USED IN BFS
        protected bool ContainsValue(int[] current, Dictionary<int[], int[]> _res)
        {
            foreach (int[] val in _res.Values)
            {
                if (val[0] == current[0] && val[1] == current[1])
                    return true;
            }
            return false;
        }

        // Convert path arrays to readable strings
        protected List<string> GetDirectionFromStack(Stack<int[]> res)
        {
            List<string> path = new List<string>();

            for (int i = res.Count - 2; i >= 0; i--)
            {
                switch (res.ElementAt(i)[2])
                {
                    case 1:
                        path.Add("up");
                        break;
                    case 2:
                        path.Add("left");
                        break;
                    case 3:
                        path.Add("down");
                        break;
                    case 4:
                        path.Add("right");
                        break;
                    default:
                        break;
                }
            }

            return path;
        }

        // add string list to path based on the hashmap given
        protected void GetDirectionFromMap(int[] key, int[] start, Dictionary<int[], int[]> _res)
        {
            int[] conNodes = key;
            Stack<string> pathNodes = new Stack<string>();

            // create the linking stack and convert to string directions
            do
            {
                key = conNodes;
                conNodes = GetValueFromMap(key, _res);
                int diffX = conNodes[0] - key[0];
                int diffY = conNodes[1] - key[1];
                if (diffX > 0)
                    pathNodes.Push("up");
                else if (diffX < 0)
                    pathNodes.Push("down");
                else if (diffY > 0)
                    pathNodes.Push("left");
                else if (diffY < 0)
                    pathNodes.Push("right");
            } while (conNodes != start);

            foreach (string p in pathNodes)
            {
                Path.Add(p);
            }
        }

        // return value given a key from a hashmap
        protected int[] GetValueFromMap(int[] key, Dictionary<int[], int[]> _res)
        {
            foreach (KeyValuePair<int[], int[]> kvp in _res)
            {
                if (key[0] == kvp.Key[0] && key[1] == kvp.Key[1])
                    return kvp.Value;
            }
            return new int[] { };
        }

        // draw the searching path from the stack given
        protected void DrawSearchStack(Stack<int[]> stack, Window window)
        {
            // change search cells to certain color
            foreach (int[] s in stack)
            {
                Grid.AddSearchCell(s);
            }

            // load to window
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.Silver);
            Grid.DrawGrid(window);
            SplashKit.RefreshScreen(10);

            // change search cells back to white blank cells
            if (!Grid.IsGoal(stack.Peek()))
            {
                foreach (int[] s in stack)
                {
                    Grid.RemoveSearchCell(s);
                }
            }
            else
            {
                Grid.RemoveSearchCell(stack.Peek());
            }
        }

        // draw the search path from hashmap given
        protected void DrawSearchMap(Dictionary<int[], int[]> map, int[] key, int[] start, Window window)
        {
            int[] val = key;
            int[] tempKey = key;
            
            // change search cells to a certain color
            while (val != start)
            {
                Grid.AddSearchCell(val);
                val = GetValueFromMap(key, map);
                key = val;
            }

            key = tempKey;
            val = key;

            // load to window
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.Silver);
            Grid.DrawGrid(window);
            SplashKit.RefreshScreen(10);

            // change back to white blank cells
            while (val != start)
            {
                Grid.RemoveSearchCell(val);
                val = GetValueFromMap(key, map);
                key = val;
            }
        }
    }
}
