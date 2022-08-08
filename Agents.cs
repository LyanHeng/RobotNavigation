using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace search
{
    // acts as a class that gathers all agents (algorithms)
    // encapsulate all classes and allowing for a more abstract use of the algorithms
    public class Agents
    {
        private DFS _dfs;
        private BFS _bfs;
        private GBFS _gbfs;
        private AStar _aStar;
        private IDDFS _id;
        private IDAStar _idAStar;

        public Agents(int x, int y, Grid grid, Window window)
        {
            _dfs = new DFS(x, y, grid, window);
            _bfs = new BFS(x, y, grid, window);
            _gbfs = new GBFS(x, y, grid, window);
            _aStar = new AStar(x, y, grid, window);
            _id = new IDDFS(x, y, grid, window);
            _idAStar = new IDAStar(x, y, grid, window);
        }

        // returns number of nodes in the given search space
        public int NumberOfNodes(string algo)
        {
            switch (algo)
            {
                case "DFS":
                    return _dfs.TotalNodes;
                case "BFS":
                    return _bfs.TotalNodes;
                case "GBFS":
                    return _gbfs.TotalNodes;
                case "AS":
                    return _aStar.TotalNodes;
                case "CUS1":
                    return _id.TotalNodes;
                case "CUS2":
                    return _idAStar.TotalNodes;
                default:
                    return 0;
            }
        }

        // returns the path found by the given search algorithm
        public List<string> Search(string algo)
        {
            switch (algo)
            {
                case "DFS":
                    return _dfs.Search();
                case "BFS":
                    return _bfs.Search();
                case "GBFS":
                    return _gbfs.Search();
                case "AS":
                    return _aStar.Search();
                case "CUS1":
                    return _id.Search();
                case "CUS2":
                    return _idAStar.Search();
                default:
                    return new List<string>() { "Method does not exist." };
            }
        }
    }
}
