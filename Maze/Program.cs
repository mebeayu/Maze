using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze mazeObj = new Maze();
            int size = 40;
            char[][] maze = mazeObj.InitializeMaze(size);
            
            mazeObj.Outmaze();
            mazeObj.FindPath(mazeObj.maze_size, 0, 0, mazeObj.maze_size - 1, mazeObj.maze_size - 1);
            mazeObj.Outmaze();
            mazeObj.OutPath();

        }
    }
}
