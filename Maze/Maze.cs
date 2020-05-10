using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Maze
    {
        const char road = ' ';
        const char wall = 'w';
        const char connect = (char)1;
        const char began = (char)26;
        const char end = (char)26;
        const char path = '.';
        const char up = '1';
        const char down = '2';
        const char right = '3';
        const char left = '4';
        const char walked = wall;
        char[][] maze_data;
        public int maze_size = 0;
        public char[][] MazeData
        {
            get { return maze_data; }
        }
        public char[][] InitializeMaze(int size)
        {
            Random rd = new Random();
            size = size / 2 * 2 + 1;
            maze_size = size;
            char[][] block = new char[size][];
            for (int i = 0; i < size; i++)
            {
                block[i] = new char[size];
            }
            //initialize the 2D array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        block[i][j] = road;
                    }
                    if (i % 2 != 0 && j % 2 != 0)
                    {
                        block[i][j] = wall;
                    }
                    else
                    {
                        if (i % 2 != 0 || j % 2 != 0)
                        {
                            if (rd.Next() % 2 < 1)
                            {
                                block[i][j] = road;
                            }
                            else
                            {
                                block[i][j] = wall;
                            }
                        }
                    }
                }
            }
            //Outmaze(block, size);
            //but the 2D array is not a maze , we should modify it 
            Modify(size, block);
            //block[0][0] = ' ';
            //block[size - 1][size - 1] = ' ';
            maze_data = block;
            return block;
        }
        private bool TestConnect(int size, int x, int y, char[][] block)
        {//test wether exists the problem of close

            int n = 0;

            if ((x - 1 < 0) || (block[x - 1][y] == connect)) { n++; }

            if ((x + 1 > size - 1) || (block[x + 1][y] == connect)) { n++; }

            if ((y - 1 < 0) || (block[x][y - 1] == connect)) { n++; }

            if ((y + 1 > size - 1) || (block[x][y + 1] == connect)) { n++; }

            if (n >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private int TagConnect(int size, int x, int y, char[][] block)
        {//tag the walls that connected to the bound as "connect"

            //tag the walls and solve problem of close 
            if ((block[x][y] == wall) && (!TestConnect(size, x, y, block)))
            {//if the block is wall and is not close then tag it as "connect"
                block[x][y] = connect;
            }
            else
            {//if the block cause close then tag it as "road"
                block[x][y] = road;
                return 0;
            }

            //go along four directs to continue this work
            if ((x - 1 >= 0) && block[x - 1][y] == wall)
                TagConnect(size, x - 1, y, block);//go up
            if ((x + 1 <= size - 1) && block[x + 1][y] == wall)
                TagConnect(size, x + 1, y, block);//go down 
            if ((y - 1 >= 0) && block[x][y - 1] == wall)
                TagConnect(size, x, y - 1, block);//go left
            if ((y + 1 <= size - 1) && block[x][y + 1] == wall)
                TagConnect(size, x, y + 1, block);//go right
            return 0;
        }
        private void Modify(int size, char[][] block)
        {//modify the array to be a maze 

            //tag the walls that connected to the bound
            for (int i = 1; i < size - 1; i = i + 2)
            {
                TagConnect(size, i, size - 1, block);//from the right bound
                TagConnect(size, i, 0, block);//from the left bound
                TagConnect(size, size - 1, i, block);//from the bottom bound
                TagConnect(size, 0, i, block);//from the top bound
            }

            //there still some walls which are isolation (not connect to any bounds),we have to solve the problem
            Join(size, block);
        }
        private void Join(int size, char[][] block)
        {//connect the walls that are isolation to the bound

            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                {
                    if (block[i][j] == road && !(i % 2 == 0 && j % 2 == 0) && !(i % 2 != 0 && j % 2 != 0))
                    {

                        if (!TestConnect(size, i, j, block))
                        {
                            block[i][j] = wall;
                            TagConnect(size, i, j, block);
                        }
                    }
                }
            }
        }
        public bool FindPath(int size, int x, int y, int o, int p)
        {//find the path of the maze. x,y are the coordinate of the began point 
         //	and o,p are the coordinate of the end point 

            if ((x == o) && (y == p))
            {
                maze_data[x][y] = 'O';
                return true;
            }

            maze_data[x][y] = walked;

            if ((x - 1 >= 0) && maze_data[x - 1][y] == road)
            {
                if (FindPath(size, x - 1, y, o, p))
                {
                    maze_data[x][y] = up;
                    return true;
                }
            }

            if ((x + 1 <= size - 1) && maze_data[x + 1][y] == road)
            {
                if (FindPath(size, x + 1, y, o, p))
                {
                    maze_data[x][y] = down;
                    return true;
                }
            }

            if ((y - 1 >= 0) && maze_data[x][y - 1] == road)
            {
                if (FindPath(size, x, y - 1, o, p))
                {
                    maze_data[x][y] = left;
                    return true;
                }
            }

            if ((y + 1 <= size - 1) && maze_data[x][y + 1] == road)
            {
                if (FindPath(size, x, y + 1, o, p))
                {
                    maze_data[x][y] = right;
                    return true;
                }
            }

            maze_data[x][y] = road;

            return false;
        }
        public void OutPath()
        {
            int size = maze_size;
            char[][] block = maze_data;
            //output the top bound
            for (int m = 0; m < size * 2 + 3; m++)
                Console.Write(' ');
            Console.WriteLine();
            //Console.ReadKey();
            for (int i = 0; i < size; i++)
            {

                for (int j = 0; j < size; j++)
                {
                    if (j == 0)
                    {//output the left bound
                        Console.Write(' ');
                        Console.Write(' ');

                    }
                    if(block[i][j]==up|| block[i][j] == down||block[i][j] == left||block[i][j] == right) Console.Write(block[i][j]);
                    else Console.Write(' ');
                    Console.Write(' ');

                    if (j == size - 1)
                    {//output the right bound
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }

            //output the bottom bound
            for (int n = 0; n < size * 2 + 3; n++)
                Console.Write(' ');
            Console.WriteLine();
        }
        public void Outmaze(char[][] block,int size)
        {
            //output the top bound
            for (int m = 0; m < size * 2 + 3; m++)
                Console.Write(connect);
            Console.WriteLine();
            //Console.ReadKey();
            for (int i = 0; i < size; i++)
            {

                for (int j = 0; j < size; j++)
                {
                    if (j == 0)
                    {//output the left bound
                        Console.Write(connect);
                        Console.Write(' ');

                    }
                    Console.Write(block[i][j]);
                    Console.Write(' ');

                    if (j == size - 1)
                    {//output the right bound
                        Console.Write(connect);
                    }
                }
                Console.WriteLine();
            }

            //output the bottom bound
            for (int n = 0; n < size * 2 + 3; n++)
                Console.Write(connect);
            Console.WriteLine();
        }
        public void Outmaze()
        {//output the maze
            int size = maze_size;
            char[][] block = maze_data;
            //output the top bound
            for (int m = 0; m < size * 2 + 3; m++)
                Console.Write(connect);
            Console.WriteLine();
            //Console.ReadKey();
            for (int i = 0; i < size; i++)
            {

                for (int j = 0; j < size; j++)
                {
                    if (j == 0)
                    {//output the left bound
                        Console.Write(connect);
                        Console.Write(' ');
                        
                    }
                    Console.Write(block[i][j]);
                    Console.Write(' ');

                    if (j == size - 1)
                    {//output the right bound
                        Console.Write(connect);
                    }
                }
                Console.WriteLine();
            }

            //output the bottom bound
            for (int n = 0; n < size * 2 + 3; n++)
                Console.Write(connect);
            Console.WriteLine();
        }
    }
}
