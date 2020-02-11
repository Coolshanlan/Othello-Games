using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 黑白棋
{
    public partial class integral_Othello : Form
    {
        public integral_Othello()
        {
            InitializeComponent();
            addbutton();
            chess_Change_count();
            Chess_integral_count();
        }

        static int chess = 0;
        int white = 2;
        int black = 2;
        List<Chess_Integral> Chess = new List<Chess_Integral>();
        Button[,] bu;
        int chesscount = 0;
        int end = 0;
        int turn = 0;
        bool endgame = false;

        private void addbutton()
        {
            groupBox1.Controls.Clear();
            bu = new Button[8, 8];
            Chess = new List<Chess_Integral>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu[i, j] = new Button();
                    bu[i, j].Size = new Size(45, 45);
                    bu[i, j].Location = new Point(4 + j * 45, 10 + i * 45);
                    bu[i, j].Tag = 0;
                    bu[i, j].Font = new Font("Consolas", 25);
                    bu[i, j].Click += new EventHandler(bu_Click);
                    bu[i, j].Name = i * 8 + j + "";
                    bu[i, j].Enabled = false;
                    bu[i, j].FlatStyle = FlatStyle.Flat;
                    bu[i, j].BackColor = Color.LightSkyBlue;
                    Chess.Add(new Chess_Integral { point = new Point(i, j), chess_color = 0, chess_change = 0 });
                    groupBox1.Controls.Add(bu[i, j]);
                }
            }
            bu[3, 4].Text = "●";
            Chess[3 * 8 + 4].chess_color = 1;
            bu[3, 3].Text = "○";
            Chess[3 * 8 + 3].chess_color = 2;
            bu[4, 3].Text = "●";
            Chess[4 * 8 + 3].chess_color = 1;
            bu[4, 4].Text = "○";
            Chess[4 * 8 + 4].chess_color = 2;
        }

        private void Chess_integral_count()
        {
            List<int> integral = new List<int>()
            {
                1000,-50,20,20,20,20,-50,1000,
                -50,-500,-10,-10,-10,-10,-500,-50,
                20,-10,3,0,0,3,-10,20,
                20,-10,0,0,0,0,-10,20,
                20,-10,0,0,0,0,-10,20,
                20,-10,3,0,0,3,-10,20,
                -50,-500,-10,-10,-10,-10,-500,-50,
                1000,-50,20,20,20,20,-50,1000,
            };
            for(int  i = 0; i < Chess.Count; i++)
            {
                Chess[i].integral_count = integral[i];
            }
        }
       
        private void Chess_integral_Change()
        {
            if(Chess[0].chess_color ==chess%2)
            {
                Chess[1].integral_count = 20;
                Chess[8].integral_count = 20;
                Chess[9].integral_count = -10;
            }
            if (Chess[7].chess_color == chess % 2)
            {
                Chess[6].integral_count = 20;
                Chess[15].integral_count = 20;
                Chess[14].integral_count = -10;
            }
            if (Chess[56].chess_color == chess % 2)
            {
                Chess[48].integral_count = 20;
                Chess[57].integral_count = 20;
                Chess[49].integral_count = -10;
            }
            if (Chess[63].chess_color == chess % 2)
            {
                Chess[55].integral_count = 20;
                Chess[62].integral_count = 20;
                Chess[54].integral_count = -10;
            }
        }
        private bool chess_Change(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess[X * 8 + Y].chess_color;
            if (chesscolor == color)
            {
                return true;
            }
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (chess_Change(color, X, Y, x_c, y_c) == true)
                {
                    Chess[X * 8 + Y].chess_color = color;
                    bu[X, Y].Text = color == 1 ? "●" : "○";
                    if (color == 1)
                    {
                        bu[X, Y].Text = "●";
                        black++;
                        white--;
                    }
                    else
                    {
                        bu[X, Y].Text = "○";
                        white++;
                        black--;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void bu_Click(object sender, EventArgs e)
        {
            if (endgame == true) return;
            Button bb = (Button)(sender);
            int X = Convert.ToInt16(bb.Name) / 8;
            int Y = Convert.ToInt16(bb.Name) - 8 * X;
            if (bb.Text != "") return;
            if (Chess[X * 8 + Y].chess_change == 0)
            {
                MessageBox.Show("無效步數");
                return;
            }
            if (((Button)(sender)).Text != "") return;
            if (chess % 2 == 0)
            {
                ((Button)(sender)).Text = "●";
                Chess[X * 8 + Y].chess_color = 1;
                black++;
            }
            else
            {
                ((Button)(sender)).Text = "○";
                Chess[X * 8 + Y].chess_color = 2;
                white++;
            }
            Chess[X * 8 + Y].chess_change = 0;
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change(Chess[X * 8 + Y].chess_color, X, Y, -1, 0);
            chess++;
            chess_Change_count();
            turn++;
            if(endgame == true)return;
            if (turn % 2 == 1)Computer(); ;
        }

        private void Computer()
        {
            int Max = -1000;
            Random rd = new Random();
            List<Point> p = new List<Point>();
            foreach (var a in Chess)
            {
                if(a.chess_change != 0)
                {
                    if(Max < a.integral_count)
                    {
                        Max = a.integral_count;
                    }
                }
            }
            foreach (var a in Chess)
            {
                if (a.integral_count == Max && a.chess_change != 0)
                {
                    p.Add(new Point(a.point.X, a.point.Y));
                }
            }
            int rdans = rd.Next(0, p.Count());
            bu[p[rdans].X,p[rdans].Y].PerformClick();
        }

        private void chess_Change_count()
        {
            lbblack.Text = black.ToString();
            lbwhite.Text = white.ToString();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu[i, j].Text != "●" && bu[i, j].Text != "○")
                    {
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, 1, 0);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, 1, 1);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, 0, 1);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, -1, -1);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, -1, 1);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, 1, -1);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, 0, -1);
                        Change_count(chess % 2 == 0 ? 1 : 2, i, j, -1, 0);
                        Chess[i * 8 + j].chess_change = chesscount;
                        chesscount = 0;
                    }
                }
            }
            if (endgame == true) return;
            NObutton();
            if (endgame == true) return;
            Change_User();
        }

        private void Change_User()
        {
            foreach (var a in Chess)
            {
                if (a.chess_change != 0)
                {
                    if (end == 1)
                        MessageBox.Show("無子可落，換邊");
                    end = 0;
                    return;
                }
            }
            end++;
            turn++;
            chess++;
            if (end >= 2)
            {
                winer();
                return;
            }
            else
            {
                chess_Change_count();
            }
        }

        private void NObutton()
        {
            foreach (var a in bu)
            {
                if (a.Text != "●" && a.Text != "○")
                {
                    return;
                }
            }
            winer();
        }

        private void winer()
        {
            MessageBox.Show("遊戲結束");
            endgame = true;
            if (white > black)
            {
                MessageBox.Show("白子勝");
                label2.ForeColor = Color.Green;
            }
            else if (white < black)
            {
                MessageBox.Show("黑子勝");
                label1.ForeColor = Color.Green;
            }
            else
            {
                MessageBox.Show("平手");
            }
        }

        private bool Change_count(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess[X * 8 + Y].chess_color;
            if (chesscolor == color)
            {
                return true;
            }
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (Change_count(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            turn++;
            foreach (var a in bu)
            {
                a.Enabled = true;
            }
            label3.Text = "玩家：白棋";
            button1.Enabled = false;
            button2.Enabled = false;
            Computer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var a in bu)
            {
                a.Enabled = true;
            }
            label3.Text = "玩家：黑棋";
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (var a in bu)
            {
                a.Enabled = true;
            }
            label3.Text = "玩家：黑棋";
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            turn++;
            foreach (var a in bu)
            {
                a.Enabled = true;
            }
            label3.Text = "玩家：白棋";
            button1.Enabled = false;
            button2.Enabled = false;
            Computer();
        }

        private void integral_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
    class Chess_Integral
    {
    public Point point { get; set; }
    public int chess_color { get; set; }
    public int integral_count { get; set; }
    public int chess_change { get; set; }
}
