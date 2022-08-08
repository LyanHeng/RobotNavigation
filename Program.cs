using System;
using SplashKitSDK;
using System.IO;
using search;
using System.Collections.Generic;

public class Program
{
    // cell types constants
    public const int GREEN = 2;
    public const int AGENT = 1;
    public const int BLANK = 0;
    public const int WALL = 3;

    public static void Main(string[] args)
    {
        string fileName = "";
        string method = "";
        string[] lines = new string[] { };

        // arguments handling
        if (args.Length == 0)
        {
            throw new ArgumentNullException("No arguments were provided.");
        }
        else if (args.Length != 2)
        {
            throw new ArgumentException("Use of arguments: search <filename> <method>");
        }
        else
        {
            fileName = args[0];
            method = args[1];
        }

        // file handling
        try
        {
            lines = File.ReadAllLines(fileName);
        }
        catch (Exception e)
        {
            throw new FileLoadException("Cannot Open File");
        }

        // get N & M
        int[] gridDim = TextParsingExtension.GetPairValue(lines[0], '[', ']');
        Grid grid = new Grid(gridDim[0], gridDim[1]);

        // get agent's initial position
        int[] agentDim = TextParsingExtension.GetPairValue(lines[1], '(', ')');
        grid.UpdateAtWith(agentDim, AGENT);

        // get green cells' positions
        List<int[]> greens = TextParsingExtension.GetGreenValues(lines[2]);
        foreach (int[] green in greens)
        {
            grid.UpdateAtWith(green, GREEN);
        }

        // get walls' positions
        for (int i = 3; i < lines.Length; i++)
        {
            int[] wallDim = TextParsingExtension.GetWallPos(lines[i]);
            grid.UpdateWalls(wallDim);
        }

        // create window (GUI)
        Window window = new Window("Robot Navigation - " + method, gridDim[1] * 75 + 20, (gridDim[0] * 75) + 220);

        // create agent
        Agents agent = new Agents(agentDim[1], agentDim[0], grid, window);

        // search based on methods given
        List<string> path = agent.Search(method);
        Console.WriteLine(fileName + " " + method + " " + agent.NumberOfNodes(method));
        foreach (string p in path)
        {
            Console.Write(p + "; ");
            Console.WriteLine();
        }
        Console.WriteLine();

        do
        {
            SplashKit.ProcessEvents();
            SplashKit.ClearScreen(Color.Silver);
            SplashKit.FillRectangleOnWindow(window, Color.White, 10, 10, gridDim[1] * 75, (gridDim[0] * 75));
            // draw grid
            grid.DrawGrid(window);

            SplashKit.RefreshScreen(10);
        } while (!SplashKit.WindowCloseRequested("Robot Navigation - " + method));


        
    }
}
