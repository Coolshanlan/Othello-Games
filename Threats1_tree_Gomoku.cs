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
    public partial class Threat1_tree_Gomoku: Form
    {
        Button[,] bu;
        int chess = 0;
        int chesscount = 0;
        List<gomoku> Chess;
        bool win = false;
        bool dead = false;
        bool Start = false;
        int turn = 0;

        public Threat1_tree_Gomoku()
        {
            InitializeComponent();
            addbutton();
            Chessintegral();
        }

        private void Chessintegral()
        {
            List<int> integralchess =new List<int> { 
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,
            0,1,2,2,2,2,2,2,2,2,2,2,2,1,0,
            0,1,2,3,3,3,3,3,3,3,3,3,2,1,0,
            0,1,2,3,4,4,4,4,4,4,4,3,2,1,0,
            0,1,2,3,4,5,5,5,5,5,4,3,2,1,0,
            0,1,2,3,4,5,6,6,6,5,4,3,2,1,0,
            0,1,2,3,4,5,6,7,6,5,4,3,2,1,0,
            0,1,2,3,4,5,6,6,6,5,4,3,2,1,0,
            0,1,2,3,4,5,5,5,5,5,4,3,2,1,0,
            0,1,2,3,4,4,4,4,4,4,4,3,2,1,0,
            0,1,2,3,3,3,3,3,3,3,3,3,2,1,0,
            0,1,2,2,2,2,2,2,2,2,2,2,2,1,0,
            0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
            for(int i = 0; i < 225; i++)
            {
                Chess[i].integral_tree = integralchess[i];
            }
        }

        private void addbutton()
        {
            bu = new Button[15, 15];
            Chess = new List<gomoku>();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    bu[i, j] = new Button();
                    bu[i, j].FlatStyle = FlatStyle.Flat;
                    bu[i, j].Font = new Font("新細明體", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    bu[i, j].Location = new Point(10 + 40 * i, 15 + 40 * j);
                    bu[i, j].Size = new Size(40, 40);
                    bu[i, j].BackColor = Color.White;
                    bu[i, j].Text = "";
                    bu[i, j].Name = (i * 15 + j).ToString();
                    bu[i, j].Click += new EventHandler(bu_Click);
                    Chess.Add(new gomoku { point = new Point(i, j), chesscolor = -1 });
                    groupBox1.Controls.Add(bu[i, j]);
                }
            }
        }

        private void bu_Click(object sender, EventArgs e)
        {
            Button clickbu = (Button)sender;
            if (Start == false)
            {
                MessageBox.Show("請先選擇黑或白棋");
                return;
            }
            if (clickbu.Text.Length != 0 | win == true) return;
            chess++;
            turn++;
            Chess[Convert.ToInt16(clickbu.Name)].chesscolor = chess % 2;
            clickbu.Text = chess % 2 == 1 ? "●" : "○";
            check_win(Convert.ToInt16(clickbu.Name) / 15, Convert.ToInt16(clickbu.Name) - (Convert.ToInt16(clickbu.Name) / 15) * 15);
            if (turn % 2 == 1) Computer();
        }

        private void chess_count(int chess_color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 14 || Y > 14 || X < 0 || Y < 0)
            {
                return;
            }
            int chesscolor = Chess[X * 15 + Y].chesscolor;
            if (chesscolor == chess_color)
            {
                chesscount++;
                chess_count(chess_color, X, Y, x_c, y_c);
            }
            else return;
        }

        private void check_win(int X, int Y)
        {
            chess_count(chess % 2, X, Y, 1, 1);
            chess_count(chess % 2, X, Y, -1, -1);
            if (chesscount >= 4) { winner(); return; }
            else { chesscount = 0; }
            chess_count(chess % 2, X, Y, -1, 1);
            chess_count(chess % 2, X, Y, 1, -1);
            if (chesscount >= 4) { winner(); return; }
            else { chesscount = 0; }
            chess_count(chess % 2, X, Y, 1, 0);
            chess_count(chess % 2, X, Y, -1, 0);
            if (chesscount >= 4) { winner(); return; }
            else { chesscount = 0; }
            chess_count(chess % 2, X, Y, 0, 1);
            chess_count(chess % 2, X, Y, 0, -1);
            if (chesscount >= 4) { winner(); return; }
            else { chesscount = 0; }
        }

        private void winner()
        {
            switch (chess % 2)
            {
                case 0:
                    MessageBox.Show("白子勝");
                    break;
                case 1:
                    MessageBox.Show("黑子勝");
                    break;
            }
            win = true;
        }

        private void Computer()
        {
            int count = 0;
            List<int> integral_count_totle = new List<int>();
            List<Point> p = new List<Point>();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    count = 0;
                    if (Chess[i * 15 + j].chesscolor != -1) continue;
                    chesscount = 0;
                    dead = false;
                    if ((i + 1) < 15 && (j + 1) < 15 )
                    {
                        if((i-1)>0 && (j - 1) > 0)
                        {
                            if(Chess[(i + 1) * 15 + (j + 1)].chesscolor == Chess[(i - 1) * 15 + (j - 1)].chesscolor)
                            {
                                integral_count(Chess[(i + 1) * 15 + (j + 1)].chesscolor, i, j, 1, 1);
                                integral_count(Chess[(i - 1) * 15 + (j - 1)].chesscolor, i, j, -1, -1);
                                count += check_dead(Chess[(i - 1) * 15 + (j - 1)].chesscolor);
                            }
                            else
                            {
                                integral_count(Chess[(i + 1) * 15 + (j + 1)].chesscolor, i, j, 1, 1);
                                count += check_dead(Chess[(i + 1) * 15 + (j + 1)].chesscolor);
                                chesscount = 0;
                                dead = false;
                                integral_count(Chess[(i - 1) * 15 + (j - 1)].chesscolor, i, j, -1, -1);
                                count += check_dead(Chess[(i - 1) * 15 + (j - 1)].chesscolor);
                            }
                        }
                        else
                        {
                            integral_count(Chess[(i + 1) * 15 + (j + 1)].chesscolor, i, j, 1, 1);
                            count += check_dead(Chess[(i + 1) * 15 + (j + 1)].chesscolor);
                        }
                    }
                    else if((i - 1) > 0 && (j - 1) > 0)
                    {
                        chesscount = 0;
                        dead = false;
                        integral_count(Chess[(i - 1) * 15 + (j - 1)].chesscolor, i, j, -1, -1);
                        count += check_dead(Chess[(i - 1) * 15 + (j - 1)].chesscolor);
                    }

                    chesscount = 0;
                    dead = false;

                    if ((i + 1) < 15 && (j - 1) > 0)
                    {
                        if ((i - 1) > 0 && (j + 1) < 15)
                        {
                            if (Chess[(i - 1) * 15 + (j + 1)].chesscolor == Chess[(i + 1) * 15 + (j - 1)].chesscolor)
                            {
                                integral_count(Chess[(i + 1) * 15 + (j - 1)].chesscolor, i, j, 1, -1);
                                integral_count(Chess[(i - 1) * 15 + (j + 1)].chesscolor, i, j, -1, 1);
                                count += check_dead(Chess[(i - 1) * 15 + (j + 1)].chesscolor);
                            }
                            else
                            {
                                integral_count(Chess[(i + 1) * 15 + (j - 1)].chesscolor, i, j, 1, -1);
                                count += check_dead(Chess[(i + 1) * 15 + (j - 1)].chesscolor);
                                chesscount = 0;
                                dead = false;
                                integral_count(Chess[(i - 1) * 15 + (j + 1)].chesscolor, i, j, -1, +1);
                                count += check_dead(Chess[(i - 1) * 15 + (j + 1)].chesscolor);
                            }
                        }
                        else
                        {
                            integral_count(Chess[(i + 1) * 15 + (j - 1)].chesscolor, i, j, 1, -1);
                            count += check_dead(Chess[(i + 1) * 15 + (j - 1)].chesscolor);
                        }
                    }
                    else if ((i - 1) > 0 && (j +1) < 15)
                    {
                        chesscount = 0;
                        dead = false;
                        integral_count(Chess[(i - 1) * 15 + (j + 1)].chesscolor, i, j, -1, 1);
                        count += check_dead(Chess[(i - 1) * 15 + (j + 1)].chesscolor);
                    }

                    chesscount = 0;
                    dead = false;

                    if ((i + 1) < 15)
                    {
                        if ((i - 1) > 0)
                        {
                            if (Chess[(i + 1) * 15 + (j )].chesscolor == Chess[(i - 1) * 15 + (j)].chesscolor)
                            {
                                integral_count(Chess[(i + 1) * 15 + (j )].chesscolor, i, j, 1, 0);
                                integral_count(Chess[(i - 1) * 15 + (j )].chesscolor, i, j, -1, 0);
                                count += check_dead(Chess[(i - 1) * 15 + (j)].chesscolor);
                            }
                            else
                            {
                                integral_count(Chess[(i + 1) * 15 + (j)].chesscolor, i, j, 1, 0);
                                count += check_dead(Chess[(i + 1) * 15 + (j )].chesscolor);
                                chesscount = 0;
                                dead = false;
                                integral_count(Chess[(i - 1) * 15 + (j )].chesscolor, i, j, -1, 0);
                                count += check_dead(Chess[(i - 1) * 15 + (j)].chesscolor);
                            }
                        }
                        else
                        {
                            integral_count(Chess[(i + 1) * 15 + (j)].chesscolor, i, j, 1, 0);
                            count += check_dead(Chess[(i + 1) * 15 + (j )].chesscolor);
                        }
                    }
                    else if ((i - 1) > 0)
                    {
                        chesscount = 0;
                        dead = false;
                        integral_count(Chess[(i - 1) * 15 + (j)].chesscolor, i, j, -1, 0);
                        count += check_dead(Chess[(i - 1) * 15 + (j )].chesscolor);
                    }


                    chesscount = 0;
                    dead = false;

                    if ((j + 1) < 15)
                    {
                        if ((j - 1) > 0)
                        {
                            if (Chess[(i ) * 15 + (j+1)].chesscolor == Chess[(i ) * 15 + (j-1)].chesscolor)
                            {
                                integral_count(Chess[(i) * 15 + (j+1)].chesscolor, i, j, 0, 1);
                                integral_count(Chess[(i) * 15 + (j-1)].chesscolor, i, j, 0, -1);
                                count += check_dead(Chess[(i ) * 15 + (j-1)].chesscolor);
                            }
                            else
                            {
                                integral_count(Chess[(i ) * 15 + (j+1)].chesscolor, i, j, 0, 1);
                                count += check_dead(Chess[(i ) * 15 + (j+1)].chesscolor);
                                chesscount = 0;
                                dead = false;
                                integral_count(Chess[(i) * 15 + (j-1)].chesscolor, i, j, 0, -1);
                                count += check_dead(Chess[(i) * 15 + (j-1)].chesscolor);
                            }
                        }
                        else
                        {
                            integral_count(Chess[(i) * 15 + (j+1)].chesscolor, i, j, 0, 1);
                            count += check_dead(Chess[(i) * 15 + (j+1)].chesscolor);
                        }
                    }
                    else if ((j - 1) > 0)
                    {
                        chesscount = 0;
                        dead = false;
                        integral_count(Chess[(i ) * 15 + (j-1)].chesscolor, i, j, 0, -1);
                        count += check_dead(Chess[(i ) * 15 + (j-1)].chesscolor);
                    }
                    Chess[i * 15 + j].integral_count = count;
                    //bu[i, j].Text = count.ToString();
                    integral_count_totle.Add(count);
                }
            }
            chesscount = 0;
            integral_count_totle.Sort();
            integral_count_totle.Reverse();
            if(integral_count_totle[0] == 0)
            {
                int hi = -1;
                foreach(var a in Chess)
                {
                    if(a.chesscolor==-1)
                    if (a.integral_tree > hi) hi = a.integral_tree;
                }
                foreach (var a in Chess)
                {
                    if (a.integral_tree ==hi&&a.chesscolor==-1)
                    {
                        p.Add(a.point);
                    }
                }
            }
            else
            {
                foreach(var a in Chess)
                {
                    if(a.integral_count == integral_count_totle[0]&&a.chesscolor==-1)
                    {
                        p.Add(a.point);
                    }
                }
            }
            Random rd = new Random();
            int rdn = rd.Next(0, p.Count());
            bu[p[rdn].X, p[rdn].Y].PerformClick();  
        }

        private int check_dead(int color)
        {
            string s = "";
            int a=0;
            if (dead == true) s = "d";
            s += chesscount.ToString();
            if (color == (chess+1) % 2) a = integral_Computer(s);
            else a = integral_people(s);
            return a;
        }

        private void integral_count(int chess_color,int x,int y, int x_c,int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 14 || Y > 14 || X < 0 || Y < 0)
            {
                return;
            }
            int chesscolor = Chess[X * 15 + Y].chesscolor;
            if (chesscolor == -1 && chess_color == -1) return;
            if (chesscolor == chess_color)
            {
                chesscount++;
                integral_count(chess_color, X, Y, x_c, y_c);
            }
            else if(chesscolor != -1)
            {
                dead = true;
                return;
            }
            else return;
        }

        private void People_Gomoku_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private int integral_people(string s)
        {
            int integralcount = 0;
            switch (s)
            {
                case "0":
                    integralcount = 0;
                    break;
                case "1":
                    integralcount = 1;
                    break;
                case "2":
                    integralcount = 7;
                    break;
                case "3":
                    integralcount = 50;
                    break;
                case "4":
                    integralcount = 100;
                    break;
                case "d1":
                    integralcount = 0;
                    break;
                case "d2":
                    integralcount =0;
                    break;
                case "d3":
                    integralcount = 7;
                    break;
                case "d4":
                    integralcount = 150;
                    break;
            }
            return integralcount;
        }

        private int integral_Computer(string s)
        {
            int integralcount = 0;
            switch (s)
            {
                case "0":
                    integralcount = 0;
                    break;
                case "1":
                    integralcount = 7;
                    break;
                case "2":
                    integralcount = 11;
                    break;
                case "3":
                    integralcount = 70;
                    break;
                case "4":
                    integralcount = 150;
                    break;
                case "d1":
                    integralcount = 0;
                    break;
                case "d2":
                    integralcount = 7;
                    break;
                case "d3":
                    integralcount = 9;
                    break;
                case "d4":
                    integralcount = 150;
                    break;
            }
            return integralcount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start = true;
            button2.Enabled = false;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            Start = true;
            turn++;
            Computer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            turn = 0;
            Chess = new List<gomoku>();
            chess = 0;
            win = false;
            Start = false;
            foreach(var a in bu)
            {
                a.Dispose();
            }
            addbutton();
            Chessintegral();
        }
    }
}

