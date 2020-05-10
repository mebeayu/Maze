using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeUI
{
    public partial class MainUI : Form
    {
        char wall = (char)1;
        Maze.Maze maze = new Maze.Maze();
        char[][] maze_data = null;
        int width = 10;
        int top = 20;
        int left = 20;
        HatchBrush pen_wall = null;
        SolidBrush pen_road = null;
        SolidBrush pen_me = null;
        SolidBrush pen_empty = null;
        Pen pen = new Pen(Color.Gray);
        Bitmap bmp = null;
        int cur_x = 0;
        int cur_y = 0;
        Rectangle cur_rc;
        Graphics gp;
        public MainUI()
        {
            InitializeComponent();
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            maze.InitializeMaze(80);
            //maze.FindPath(maze.maze_size, 0, 0, maze.maze_size - 1, maze.maze_size - 1);
            maze_data = maze.MazeData;
            //Graphics gp = this.CreateGraphics();
            pen_wall = new HatchBrush(HatchStyle.HorizontalBrick,
                  Color.Gray, Color.LightGray);
            pen_road = new SolidBrush(Color.Green);
            pen_me = new SolidBrush(Color.Red);
            pen_empty = new SolidBrush(Color.White);
            ;
            bmp = new Bitmap(top+maze.maze_size*width+5,left+ maze.maze_size * width+5);
            layer.Width = bmp.Width+20;
            layer.Height = bmp.Height+20;
            gp = Graphics.FromImage(bmp);
            
            //gp.FillRectangle(pen_wall, new Rectangle(left, top, width, width));
            //for (int i = 0; i < maze_data.Length; i++)
            //{
            //     //DrawWall(i, 0, gp);
            //}
            for (int i = 0; i < maze_data.Length; i++)
            {
                for (int j = 0; j < maze_data.Length; j++)
                {
                    if (maze_data[i][j] == wall)
                    {

                        DrawWall(i, j, gp);
                    }
                    else
                    {
                        DrawEmpty(i, j, gp);
                    }
                    if (maze_data[i][j] == '1' || maze_data[i][j] == '2' || maze_data[i][j] == '3' || maze_data[i][j] == '4')
                    {
                        DrawRoad(i, j, gp);
                    }
                    //if(j==0) DrawWall(i, j, gp);
                }
            }
            DrawRoad(maze.maze_size - 1, maze.maze_size - 1, gp);
            //gp.DrawLine(pen, left, top, left + (maze_data.Length * width), top);
            //gp.DrawLine(pen, left, top, left, top + (maze_data.Length * width));
            //gp.DrawLine(pen, left, top + (maze_data.Length * width), left + (maze_data.Length * width), top + (maze_data.Length * width));
            //gp.DrawLine(pen, left + (maze_data.Length * width), top, left + (maze_data.Length * width), top + (maze_data.Length * width));
            //this.CreateGraphics().
            cur_rc = DrawMe(cur_x, cur_y, gp);

        }
        
        private void MainUI_Paint(object sender, PaintEventArgs e)
        {

            Graphics gp1 = e.Graphics;
            gp1.DrawImage(bmp, 0, 0);
        }
        private void DrawWall(int x,int y, Graphics gp)
        {
            Rectangle rc = new Rectangle(20+x*width,20+y*width, width, width);
            gp.FillRectangle(pen_wall, rc);
        }
        private void DrawEmpty(int x, int y, Graphics gp)
        {
            Rectangle rc = new Rectangle(20 + x * width, 20 + y * width, width, width);
            gp.FillRectangle(pen_empty, rc);
        }
        private void DrawRoad(int x, int y, Graphics gp)
        {
            Rectangle rc = new Rectangle(20 + x * width, 20 + y * width, width, width);
            gp.FillRectangle(pen_road, rc);
        }
        private Rectangle DrawMe(int x, int y, Graphics gp)
        {
            Rectangle rc = new Rectangle(20 + x * width, 20 + y * width, width, width);
            gp.FillRectangle(pen_me, rc);
            return rc;
        }
        private void MoveMeLeft()
        {
            if (cur_x - 1 < 0) return;
            if (maze_data[cur_x - 1][cur_y] == wall) return;
            DrawEmpty(cur_x, cur_y, gp);
            Invalidate(cur_rc);
            cur_x = cur_x - 1;
            cur_rc = DrawMe(cur_x, cur_y, gp);
            Invalidate(cur_rc);
        }
        private void MoveMeRight()
        {
            if (cur_x + 1 >maze.maze_size-1) return;
            if (maze_data[cur_x + 1][cur_y] == wall) return;
            DrawEmpty(cur_x, cur_y, gp);
            Invalidate(cur_rc);
            cur_x = cur_x + 1;
            cur_rc = DrawMe(cur_x, cur_y, gp);
            Invalidate(cur_rc);
        }
        private void MoveMeTop()
        {
            if (cur_y - 1 <0) return;
            if (maze_data[cur_x][cur_y-1] == wall) return;
            DrawEmpty(cur_x, cur_y, gp);
            Invalidate(cur_rc);
            cur_y = cur_y - 1;
            cur_rc = DrawMe(cur_x, cur_y, gp);
            Invalidate(cur_rc);
        }
        private void MoveMeDown()
        {
            if (cur_y + 1 > maze.maze_size - 1) return;
            if (maze_data[cur_x][cur_y + 1] == wall) return;
            DrawEmpty(cur_x, cur_y, gp);
            Invalidate(cur_rc);
            cur_y = cur_y + 1;
            cur_rc = DrawMe(cur_x, cur_y, gp);
            Invalidate(cur_rc);
        }
        private void MainUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.W|| e.KeyCode == Keys.Up)
            {
                MoveMeTop();
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                MoveMeDown();
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                MoveMeLeft();
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                MoveMeRight();
            }
            if (cur_x== maze.maze_size - 1&&cur_y== maze.maze_size - 1)
            {
                MessageBox.Show("win");
            }
        }

        private void layer_Paint(object sender, PaintEventArgs e)
        {
           
        }
    }
}
