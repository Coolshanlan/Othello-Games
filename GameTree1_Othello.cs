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
    public partial class GameTree1_Othello : Form
    {
        public GameTree1_Othello()
        {
            InitializeComponent();
            addbutton();
            Chess_integral_count();
            chess_Change_count();
            bu2_Update();
            Chess_Computer_Update();
        }

        static int chess = 0;
        int white = 2;
        int black = 2;
        static List<Chess_Gametree> Chess = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer = new List<Chess_Gametree>();
        static Button[,] bu,bbu;
        string[,] bu2 = new string[8,8];
        int chesscount = 0;
        double end = 0;
        double turn = 0;
        bool endgame = false;
        double tree_integral = 0;

        private void addbutton()
        {
            groupBox1.Controls.Clear();
            bu = new Button[8, 8];
            bbu = new Button[8, 8];
            Chess = new List<Chess_Gametree>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu[i, j] = new Button();
                    bu[i, j].Size = new Size(35, 35);
                    bu[i, j].Location = new Point(4 + j * 35, 10 + i * 35);
                    bu[i, j].Tag = 0;
                    bu[i, j].Font = new Font("Consolas", 18);
                    bu[i, j].Click += new EventHandler(bu_Click);
                    bu[i, j].Name = i * 8 + j + "";
                    bu[i, j].Enabled = false;
                    bu[i, j].FlatStyle = FlatStyle.Flat;
                    bu[i, j].BackColor = Color.LightSkyBlue;
                    Chess.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0 ,});
                    Chess_Computer.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0 });
                    groupBox1.Controls.Add(bu[i, j]);
                    bbu[i, j] = new Button();
                    bbu[i, j].Size = new Size(35, 35);
                    bbu[i, j].Location = new Point(4 + j * 35, 10 + i * 35);
                    bbu[i, j].Tag = 0;
                    bbu[i, j].Font = new Font("Consolas", 8);
                    bbu[i, j].Click += new EventHandler(bu_Click);
                    bbu[i, j].Name = i * 8 + j + "";
                    bbu[i, j].Enabled = false;
                    bbu[i, j].FlatStyle = FlatStyle.Flat;
                    bbu[i, j].BackColor = Color.LightSkyBlue;
                    groupBox2.Controls.Add(bbu[i, j]);
                }
            }
            bu[3, 4].Text = "●";
            Chess[3 * 8 + 3].chess_color = 1;
            bu[3, 3].Text = "○";
            Chess[3 * 8 + 4].chess_color = 0;
            bu[4, 3].Text = "●";
            Chess[4 * 8 + 4].chess_color = 1;
            bu[4, 4].Text = "○";
            Chess[4 * 8 + 3].chess_color = 0;
        }

        private void Chess_Computer_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer[i * 8 + j].chess_change = Chess[i * 8 + j].chess_change;
                    Chess_Computer[i * 8 + j].chess_color = Chess[i * 8 + j].chess_color;
                    Chess_Computer[i * 8 + j].integral_count = Chess[i * 8 + j].integral_count;
                    Chess_Computer[i * 8 + j].tree_integral = Chess[i * 8 + j].tree_integral;
                }
            }
        }

        private void bu2_Update()
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu2[i, j] = bu[i, j].Text;
                }
            }
        }

        private void integral_Update()
        {
            if (Chess[0].chess_color !=-1)
            {
                Chess[1].tree_integral = 100;
                Chess[8].tree_integral = 100;
                Chess[9].tree_integral = -50;
            }
            if (Chess[7].chess_color !=-1)
            {
                Chess[6].tree_integral = 100;
                Chess[15].tree_integral = 100;
                Chess[14].tree_integral = -50;
            }
            if (Chess[56].chess_color !=-1)
            {
                Chess[48].tree_integral = 100;
                Chess[57].tree_integral = 100;
                Chess[49].tree_integral = -50;
            }
            if (Chess[63].chess_color !=-1)
            {
                Chess[55].tree_integral = 100;
                Chess[62].tree_integral = 100;
                Chess[54].tree_integral = -50;
            }
        }

        private void bu_Click(object sender, EventArgs e)
        {
            if (endgame == true) return;
            Button bb = (Button)(sender);
            int X = Convert.ToInt16(bb.Name) / 8;
            int Y = Convert.ToInt16(bb.Name) - 8 * X;
            // if (bb.Text != "") return;
            if (Chess[X * 8 + Y].chess_change == 0)
            {
                MessageBox.Show("無效步數");
                return;
            }
            if (chess % 2 == 0)
            {
                ((Button)(sender)).Text = "●";
                Chess[X * 8 + Y].chess_color = chess % 2;
                black++;
            }
            else
            {
                ((Button)(sender)).Text = "○";
                Chess[X * 8 + Y].chess_color = chess % 2;
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
            integral_Update();
            Chess_Computer_Update();
            bu2_Update();
            chess_Change_count();
            turn++;
            if (endgame == true) return;
            if (turn % 2 == 1) Computer();
        }

        private void Computer()
        {
            textBox2.Text = "";
            List <Chess_Tree> computer_integral = new List<Chess_Tree>();
            List<Point> p = new List<Point>();
            Random rd = new Random();

            int rdn=0;
            double Max = -10000;
            textBox1.Text = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bbu[i, j].Text = bu[i, j].Text;
                    bbu[i, j].BackColor = Color.LightSkyBlue;
                    bu[i, j].BackColor = Color.LightSkyBlue;
                }
            }
            foreach(var a in Chess)
            {
                if(a.chess_change != 0)
                {
                    double ans =chess_Change_count_Computer(a.point.X, a.point.Y) * -1 + a.chess_change * 3 + a.integral_count*2 + a.tree_integral;
                    textBox1.Text += "加權： " + (chess_Change_count_Computer(a.point.X, a.point.Y) * -1).ToString() + "      翻棋*3： " + a.chess_change * 3 + "      位置加權： " + a.tree_integral + "       翻棋加權： " + a.integral_count * 2 + "       總加權： " + ans + "\r\n" + "\r\n";
                    computer_integral.Add(new Chess_Tree { integral_count = ans ,point = new Point(a.point.X,a.point.Y)});
                    bbu[a.point.X, a.point.Y].Text = ans.ToString();
                    bbu[a.point.X, a.point.Y].BackColor = Color.LightGreen;
                    //bu[a.point.X, a.point.Y].BackColor = Color.LightGreen;
                    if (ans > Max)
                    {
                        Max = ans;
                    }
                }
            }
            foreach (var a in computer_integral)
            {
                if(a.integral_count  == Max)
                {
                    p.Add(new Point(a.point.X,a.point.Y));
                }
            }
            rdn = rd.Next(0,p.Count());
            bu[p[rdn].X, p[rdn].Y].BackColor = Color.LightPink;
            bbu[p[rdn].X, p[rdn].Y].BackColor = Color.LightPink;
            bu[p[rdn].X, p[rdn].Y].PerformClick();
        }

        private double chess_Change_count_Computer(int X,int Y)
        {
            Chess_Computer_Update();
            bu2_Update();
            bu2[X, Y] = chess % 2 == 0 ? "●" : "○";
            Chess_Computer[X * 8 + Y].chess_color = chess % 2;
            Chess_Computer[X * 8 + Y].chess_change = 0;
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer(Chess_Computer[X * 8 + Y].chess_color, X, Y, -1, 0);
            integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu2[i, j] != "●" && bu2[i, j] != "○")
                    {
                        Change_count_Computer((chess+1) % 2, i, j, 1, 0);
                        Change_count_Computer((chess+1) % 2, i, j, 1, 1);
                        Change_count_Computer((chess+1) % 2, i, j, 0, 1);
                        Change_count_Computer((chess+1) % 2, i, j, -1, -1);
                        Change_count_Computer((chess+1) % 2, i, j, -1, 1);
                        Change_count_Computer((chess+1) % 2, i, j, 1, -1);
                        Change_count_Computer((chess+1) % 2, i, j, 0, -1);
                        Change_count_Computer((chess+1) % 2, i, j, -1, 0);
                        Chess_Computer[i * 8 + j].chess_change = chesscount;
                        Chess_Computer[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                    }
                }
            }
            List<double> tree_integral_totle = new List<double>();
            foreach(var a in Chess_Computer)
            {
                if(a.chess_change != 0)
                {
                    tree_integral_totle.Add(a.chess_change*3+a.integral_count*3+a.tree_integral);
                }
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            try
            {
                foreach (var a in Chess_Computer)
                {
                    if ((a.chess_change * 3 + a.integral_count * 3 + a.tree_integral) == tree_integral_totle[0])
                    {
                        textBox2.Text += "  翻棋數*3：" + a.chess_change * 3 + "  翻棋加權*3：" + a.integral_count * 3 + "  位置加權：" + a.tree_integral + "(" + a.point.X.ToString() + "," + a.point.Y + ")" + "\r\n" + "\r\n";
                        break;
                    }
                }
            }
            catch
            {
                tree_integral_totle.Add(0);
            }
            return tree_integral_totle[0];
        }

        private bool Change_count_Computer(int color,int x,int y,int x_c,int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer[X * 8 + Y].chess_color;
            if (chesscolor == color)
            {
                return true;
            }
            else if (chesscolor == -1)
            {
                return false;
            }
            else
            {
                if (Change_count_Computer(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    if (Chess_Computer[X * 8 + Y].tree_integral == -300)
                    {
                        tree_integral += Chess_Computer[X * 8 + Y].tree_integral;
                    }
                    else
                    {
                        tree_integral += Math.Abs(Chess_Computer[X * 8 + Y].tree_integral);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Chess_integral_count()
        {
            List<int> integral = new List<int>()
            {
                2000,-100,100,100,100,100,-100,2000,
                -100,-300,-10,-10,-10,-10,-300,-100,
                100,-10,3,0,0,3,-10,100,
                100,-10,0,0,0,0,-10,100,
                100,-10,0,0,0,0,-10,100,
                100,-10,3,0,0,3,-10,100,
                -100,-300,-10,-10,-10,-10,-300,-100,
                2000,-100,100,100,100,100,-100,2000,
            };
            for (int i = 0; i < Chess.Count; i++)
            {
                Chess[i].tree_integral = integral[i];
            }
        }

        private bool chess_Change_Computer(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer[X * 8 + Y].chess_color;
            if (chesscolor == color)
            {
                return true;
            }
            else if (chesscolor == -1)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer[X * 8 + Y].chess_color = color;
                    bu2[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu2[X, Y] = "●";
                    }
                    else
                    {
                        bu2[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
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
            else if (chesscolor == -1)
            {
                return false;
            }
            else
            {
                if (chess_Change(color, X, Y, x_c, y_c) == true)
                {
                    Chess[X * 8 + Y].chess_color = color;
                    bu[X, Y].Text = color == 0 ? "●" : "○";
                    if (color == 0)
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

        private void chess_Change_count()
        {
            chesscount = 0;
            lbblack.Text = black.ToString();
            lbwhite.Text = white.ToString();
            tree_integral = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu[i, j].Text != "●" && bu[i, j].Text != "○")
                    {
                        Change_count(chess % 2, i, j, 1, 0);
                        Change_count(chess % 2, i, j, 1, 1);
                        Change_count(chess % 2, i, j, 0, 1);
                        Change_count(chess % 2, i, j, -1, -1);
                        Change_count(chess % 2, i, j, -1, 1);
                        Change_count(chess % 2, i, j, 1, -1);
                        Change_count(chess % 2, i, j, 0, -1);
                        Change_count(chess % 2, i, j, -1, 0);
                        Chess[i * 8 + j].chess_change = chesscount;
                        Chess[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess[i * 8 + j].chess_change != 0)
                        //{
                        //    double ans = Chess[i * 8 + j].chess_change * 3 + Chess[i * 8 + j].integral_count * 2 + Chess[i * 8 + j].tree_integral;
                        //    textBox2.Text += "加權總分：" + ans.ToString() + "   翻棋數：" + (Chess[i * 8 + j].chess_change * 3).ToString() + "   翻棋加權：" + (Chess[i * 8 + j].integral_count * 2).ToString() + "   位置加權：" + (Chess[i * 8 + j].tree_integral).ToString() + "\r\n" + "\r\n";
                        //}
                    }
                }
            }
            if (endgame == true) return;
            NObutton();
            if (endgame == true) return;
            Change_User();
        }

        private bool Change_count(int color, int x, int y, int x_c,int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            double chesscolor = Chess[X * 8 + Y].chess_color;
            if (chesscolor == color)
            {
                return true;
            }
            else if (chesscolor == -1)
            {
                return false;
            }
            else
            {
               if(Change_count(color,X,Y,x_c,y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess[X*8+Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
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

        private void User_black(object sender, EventArgs e)
        {
            foreach (var a in bu)
            {
                a.Enabled = true;
            }
            label3.Text = "玩家：黑棋";
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void User_white(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            foreach(var a in bu)
            {
                a.Dispose();
            }
            white = 2;
          black = 2;
          List<Chess_Gametree> Chess = new List<Chess_Gametree>();
          List<Chess_Gametree> Chess_Computer = new List<Chess_Gametree>();
          bu2 = new string[8, 8];
          chesscount = 0;
          end = 0;
          turn = 0;
          endgame = false;
          tree_integral = 0;
          addbutton();
          Chess_integral_count();
          chess_Change_count();
          bu2_Update();
          Chess_Computer_Update();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void GameTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

