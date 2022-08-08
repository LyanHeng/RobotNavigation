using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    public class Grid
    {
        private int[,] _grid;
        private int _n;
        private int _m;
        private List<int[]> _goals = new List<int[]>();

        public Grid(int row, int col)
        {
            _grid = new int[row, col];
            _n = row;
            _m = col;
        }

        public List<int[]> GetGoals { get => _goals; }
        public int NumberOfTiles { get => _n * _m; }

        // return the closest goal from the agent starting cell
        public int[] GetClosestGoal(int[] start)
        {
            int tempMin = 0;
            int[] tempBestGoal = new int[2];
            foreach (int[] goal in _goals)
            {
                int d = Math.Abs(goal[0] - start[0]) + Math.Abs(goal[1] - start[1]);
                if (tempMin == 0 || d < tempMin)
                {
                    tempMin = d;
                    tempBestGoal = goal;
                }
            }
            return tempBestGoal;
        }

        // print the whole grid
        public void PrintGrid()
        {
            int row = _grid.GetUpperBound(0);
            int col = _grid.GetUpperBound(1);

            for (int i = 0; i <= row; i++)
            {
                for (int j = 0; j <= col; j++)
                {
                    Console.Write(_grid[i, j]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        // update a cell of grid at coordinate g with type of cell
        public void UpdateAtWith(int[] g, int type)
        {
            _grid[g[1], g[0]] = type;
            if (type == 2)
                _goals.Add(new int[] { g[1], g[0] });
        }

        // add walls in the grid
        public void UpdateWalls(int[] wallPos)
        {
            for (int i = 0; i < wallPos[3]; i++)
            {
                for (int j = 0; j < wallPos[2]; j++)
                {
                    UpdateAtWith(new int[] { wallPos[0] + j, wallPos[1] + i }, 3);
                }
            }
        }

        // return the possible route based on heuristics
        // UP, LEFT, DOWN, RIGHT
        // First constraint - check that its not out of bound
        // Second constraint - check that its not a wall
        public List<int[]> NextMoves(int[] current)
        {
            List<int[]> nextPossible = new List<int[]>();

            for (int i = 0; i < 4; i++)
            {
                int[] dir = new int[3];
                switch (i)
                {
                    case 0:
                        dir[0] = current[0] - 1;
                        dir[1] = current[1];
                        dir[2] = 1;
                        break;
                    case 1:
                        dir[0] = current[0];
                        dir[1] = current[1] - 1;
                        dir[2] = 2;
                        break;
                    case 2:
                        dir[0] = current[0] + 1;
                        dir[1] = current[1];
                        dir[2] = 3;
                        break;
                    case 3:
                        dir[0] = current[0];
                        dir[1] = current[1] + 1;
                        dir[2] = 4;
                        break;
                    default:
                        break;
                }

                // check each possibilities
                if (InBound(dir))
                {
                    if (_grid[dir[0], dir[1]] != 3)
                    {
                        nextPossible.Add(dir);
                    }
                }
            }
            return nextPossible;
        }

        // check that the next move would not be out of the board
        public bool InBound(int[] move)
        {
            return (move[1] >= 0) && (move[1] < _m) && (move[0] >= 0) && (move[0] < _n);
        }

        // returns true if agent resides in cell
        public bool IsAgent(int[] cell)
        {
            return _grid[cell[0], cell[1]] == 1;
        }

        // returns true if cell is goal cell
        public bool IsGoal(int[] cell)
        {
            return _grid[cell[0], cell[1]] == 2;
        }

        // returns true if cell is wall cell
        public bool IsWall(int[] cell)
        {
            return _grid[cell[0], cell[1]] == 3;
        }

        // changes cells to earch cells
        public void AddSearchCell(int[] cell)
        {
            _grid[cell[0], cell[1]] += 5;
        }

        // changes search cells back to normal cells
        public void RemoveSearchCell(int[] cell)
        {
            _grid[cell[0], cell[1]] -= 5;
        }

        // draw grid on window
        public void DrawGrid(Window window)
        {
            int row = _grid.GetUpperBound(0);
            int col = _grid.GetUpperBound(1);

            for (int i = 0; i <= row; i++)
            {
                for (int j = 0; j <= col; j++)
                {
                    switch (_grid[i, j])
                    {
                        case 1:
                            SplashKit.FillRectangleOnWindow(window, Color.Red, (j * 75) + 10, (i * 75) + 10, 75, 75);
                            break;
                        case 2:
                            SplashKit.FillRectangleOnWindow(window, Color.Green, (j * 75) + 10, (i * 75) + 10, 75, 75);
                            break;
                        case 3:
                            SplashKit.FillRectangleOnWindow(window, Color.Gray, (j * 75) + 10, (i * 75) + 10, 75, 75);
                            break;
                        case 0:
                            SplashKit.FillRectangleOnWindow(window, Color.White, (j * 75) + 10, (i * 75) + 10, 75, 75);
                            break;
                        default:
                            SplashKit.FillRectangleOnWindow(window, Color.IndianRed, (j * 75) + 10, (i * 75) + 10, 75, 75);
                            break;
                    }
                    SplashKit.DrawRectangleOnWindow(window, Color.Black, (j * 75) + 10, (i * 75) + 10, 75, 75);
                }
            }
        }
    }
}
