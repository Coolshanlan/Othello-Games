using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 黑白棋
{
    public partial class CountChess_Othello : Form
    {
        public CountChess_Othello()
        {
            InitializeComponent();
            addbutton();
            chess_Change_count();
        }

        int chess = 0;
        int white = 2;
        int black = 2;
        List<黑白棋.Chess_CountChess> Chess;
        Button[,] bu;
        int chesscount = 0;
        int end = 0;
        int turn = 0;
        bool endgame = false;

        private void addbutton()
        {
            groupBox1.Controls.Clear();
            Chess = new List<黑白棋.Chess_CountChess>();
            bu = new Button[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu[i, j] = new Button();
                    bu[i, j].Size = new Size(35, 35);
                    bu[i, j].Location = new Point(4 + j * 40, 10 + i * 40);
                    bu[i, j].Tag = 0;
                    bu[i, j].Font = new Font("Consolas", 17);
                    bu[i, j].Click += new EventHandler(bu_Click);
                    bu[i, j].Name = i * 8 + j + "";
                    bu[i, j].Enabled = false;
                    Chess.Add(new 黑白棋.Chess_CountChess() { point = new Point(i,j),chess_color = 0, chess_change = 0 });
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
            if (turn %2 == 1) Computer();
        }

        private void Computer()
        {
            int Max = 0;
            Random rd = new Random();
            List<int> c = new List<int>();
            foreach(var a in Chess)
            {
                if(Max < a.chess_change)
                {
                    Max = a.chess_change;
                }
            }
            foreach(var a in Chess)
            {
                if(a.chess_change == Max)
                {
                    c.Add(a.point.X * 8 + a.point.Y);
                }
            }
            int rdans = rd.Next(0,c.Count());
            bu[c[rdans] / 8, (c[rdans]) - (c[rdans] / 8) * 8].PerformClick();
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
                    if(end == 1)
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

        private void CountChess_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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

        private void CountChess_Load(object sender, EventArgs e)
        {

        }
    }
}

