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
using System.IO;

namespace 黑白棋
{
    public partial class GameTree4_Othello : Form
    {
        bool learn = false;
        static int chess = 0;
        int white = 2;
        int black = 2;
        static List<Chess_Gametree> Chess_Now = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer1 = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer2 = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer3 = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer4 = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer5 = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer6 = new List<Chess_Gametree>();
        static List<Chess_Gametree> Chess_Computer7 = new List<Chess_Gametree>();
        static Button[,] bu, bbu;
        string[,] bu1 = new string[8, 8];
        string[,] bu2 = new string[8, 8];
        string[,] bu3 = new string[8, 8];
        string[,] bu4 = new string[8, 8];
        string[,] bu5 = new string[8, 8];
        string[,] bu6 = new string[8, 8];
        string[,] bu7 = new string[8, 8];
        int chesscount = 0;
        double end = 0;
        double turn = 0;
        bool endgame = false;
        double tree_integral = 0;

        //public bool done = false;
        //public int down_X;
        //public int down_Y;
        //public  void down(int X, int Y)
        //{
        //    bu[X, Y].PerformClick();
        //}

        public GameTree4_Othello(bool ll)
        {
            InitializeComponent();
            learn = ll;
            addbutton();
            Chess_integral_count();
            chess_Change_count();
            bu1_Update();
            bu2_Update();
            bu3_Update();
            bu4_Update();
            bu5_Update();
            bu6_Update();
            bu7_Update();
            Chess_Computer1_Update();
            Chess_Computer2_Update();
            Chess_Computer3_Update();
            Chess_Computer4_Update();
            Chess_Computer5_Update();
            Chess_Computer6_Update();
            Chess_Computer7_Update();
        }

        private void addbutton()
        {
            groupBox1.Controls.Clear();
            bu = new Button[8, 8];
            bbu = new Button[8, 8];
            Chess_Now = new List<Chess_Gametree>();
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
                    Chess_Now.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0 });
                    Chess_Computer1.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer2.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer3.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer4.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer5.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer6.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer7.Add(new Chess_Gametree { point = new Point(i, j), chess_color = -1, chess_change = 0, chess_tree_integral = 0 });
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
            Chess_Now[3 * 8 + 3].chess_color = 1;
            bu[3, 3].Text = "○";
            Chess_Now[3 * 8 + 4].chess_color = 0;
            bu[4, 3].Text = "●";
            Chess_Now[4 * 8 + 4].chess_color = 1;
            bu[4, 4].Text = "○";
            Chess_Now[4 * 8 + 3].chess_color = 0;
        }

        private void bu_Click(object sender, EventArgs e)
        {
            if (endgame == true) return;
            Button bb = (Button)(sender);
            if (bb.Text != "") return;
            //bb.Text = "1";
            int X = Convert.ToInt16(bb.Name) / 8;
            int Y = Convert.ToInt16(bb.Name) - 8 * X;
            // if (bb.Text != "") return;
            down_X = X;
            down_Y = Y;
            if (Chess_Now[X * 8 + Y].chess_change == 0)
            {
               // MessageBox.Show("無效步數");
                return;
            }
            if (chess % 2 == 0)
            {
                ((Button)(sender)).Text = "●";
                Chess_Now[X * 8 + Y].chess_color = chess % 2;
                black++;
            }
            else
            {
                ((Button)(sender)).Text = "○";
                Chess_Now[X * 8 + Y].chess_color = chess % 2;
                white++;
            }
            Chess_Now[X * 8 + Y].chess_change = 0;
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change(Chess_Now[X * 8 + Y].chess_color, X, Y, -1, 0);
            chess++;
            integral_Update();
            Chess_Computer1_Update();
            bu1_Update();
            chess_Change_count();
            if (turn % 2 == 1)
                done = true;
            turn++;
            if (endgame == true) return;
            if (turn % 2 == 1)
            {
                if(learn == true)
                {
                    if (change == true)
                    {
                        Thread.Sleep(6000);
                        change = false;
                    }
                }
                Computer();
                //Thread.Sleep(200);
            }
        }
        bool change = false;
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
                        Chess_Now[i * 8 + j].chess_change = chesscount;
                        Chess_Now[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess_Now[i * 8 + j].chess_change != 0)
                        //{
                        //    double ans = Chess_Now[i * 8 + j].chess_change * 3 + Chess_Now[i * 8 + j].integral_count * 2 + Chess_Now[i * 8 + j].tree_integral;
                        //    textBox2.Text += "加權總分：" + ans.ToString() + "   翻棋數：" + (Chess_Now[i * 8 + j].chess_change * 3).ToString() + "   翻棋加權：" + (Chess_Now[i * 8 + j].integral_count * 2).ToString() + "   位置加權：" + (Chess_Now[i * 8 + j].tree_integral).ToString() + "\r\n" + "\r\n";
                        //}
                    }
                }
            }
            if (endgame == true) return;
            NObutton();
            if (endgame == true) return;
            Change_User();
        }

        private bool Change_count(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Now[X * 8 + Y].chess_color;
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
                if (Change_count(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Now[X * 8 + Y].tree_integral;
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
            int chesscolor = Chess_Now[X * 8 + Y].chess_color;
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
                    Chess_Now[X * 8 + Y].chess_color = color;
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

        private void Computer()
        {
            textBox2.Text = "";
            List<Chess_Tree> computer_integral = new List<Chess_Tree>();
            List<Point> p = new List<Point>();
            Random rd = new Random();
            int rdn = 0;
            double Max = -3000000;
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
            foreach (var a in Chess_Now)
            {
                if (a.chess_change != 0)
                {
                    double anss = chess_Change_Computer_tree1(a.point.X, a.point.Y) * -1;
                    if (anss == 0) anss = 3000000;
                    double ans = anss + a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    textBox1.Text += "加權： " + (chess_Change_Computer_tree1(a.point.X, a.point.Y) * -1).ToString() + "      翻棋*3： " + a.chess_change * 3 + "      位置加權： " + a.tree_integral + "       翻棋加權*2： " + a.integral_count * 2 + "       總加權： " + ans + "\r\n" + "\r\n";
                    computer_integral.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
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
                if (a.integral_count == Max)
                {
                    p.Add(new Point(a.point.X, a.point.Y));
                }
            }
            rdn = rd.Next(0, p.Count());
            bu[p[rdn].X, p[rdn].Y].BackColor = Color.LightPink;
            bbu[p[rdn].X, p[rdn].Y].BackColor = Color.LightPink;
            down_X = p[rdn].X;
            down_Y = p[rdn].Y;
            done = true;
            bu[p[rdn].X, p[rdn].Y].PerformClick();
        }

        private double chess_Change_Computer_tree1(int X, int Y)
        {
            Chess_Computer1_Update();
            bu1_Update();
            bu1[X, Y] = chess % 2 == 0 ? "●" : "○";
            Chess_Computer1[X * 8 + Y].chess_color = chess % 2;
            Chess_Computer1[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree1(Chess_Computer1[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer1_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu1[i, j] != "●" && bu1[i, j] != "○")
                    {
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree1((chess + 1) % 2, i, j, -1, 0);
                        Chess_Computer1[i * 8 + j].chess_change = chesscount;
                        Chess_Computer1[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                    }
                }
            }
            List<double> tree_integral_totle = new List<double>();
            //foreach(var a in Chess_Computer1)
            //{
            //    if(a.chess_change!=0)
            //    tree_integral_totle.Add(a.chess_change * 3 + a.integral_count * 5 + a.tree_integral * 5+a.chess_tree_integral);
            //}
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            foreach (var a in Chess_Computer1)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                //    //tree_integral_totle.Add(ans);
                //    if (reduce.Count < 5 || ans == reduce[0].integral_count)
                //    {
                        reduce.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                //    }
                //    else
                //    {
                //        foreach (var aa in reduce)
                //        {
                //            if (aa.integral_count < ans)
                //            {
                //                aa.integral_count = ans;
                //            }
                //        }
                //    }
                }
            }
            foreach (var a in reduce)
            {
                tree_integral_totle.Add(a.integral_count + chess_Change_Computer_tree2(a.point.X, a.point.Y) * -0.5);
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private double chess_Change_Computer_tree2(int X, int Y)
        {
            Chess_Computer2_Update();
            bu2_Update();
            bu2[X, Y] = (chess + 1) % 2 == 0 ? "●" : "○";
            Chess_Computer2[X * 8 + Y].chess_color = (chess + 1) % 2;
            Chess_Computer2[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree2(Chess_Computer2[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer2_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu2[i, j] != "●" && bu2[i, j] != "○")
                    {
                        Change_count_Computer_tree2((chess) % 2, i, j, 1, 0);
                        Change_count_Computer_tree2((chess) % 2, i, j, 1, 1);
                        Change_count_Computer_tree2((chess) % 2, i, j, 0, 1);
                        Change_count_Computer_tree2((chess) % 2, i, j, -1, -1);
                        Change_count_Computer_tree2((chess) % 2, i, j, -1, 1);
                        Change_count_Computer_tree2((chess) % 2, i, j, 1, -1);
                        Change_count_Computer_tree2((chess) % 2, i, j, 0, -1);
                        Change_count_Computer_tree2((chess) % 2, i, j, -1, 0);
                        Chess_Computer2[i * 8 + j].chess_change = chesscount;
                        Chess_Computer2[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess_Computer2[i * 8 + j].chess_change != 0)
                        //{
                        //    Chess_Computer2[i * 8 + j].chess_tree_integral = chess_Change_Computer_tree3(i, j);
                        //}
                    }
                }
            }
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            List<double> tree_integral_totle = new List<double>();
            foreach (var a in Chess_Computer2)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    //tree_integral_totle.Add(ans);
                    if (reduce.Count < 5 || ans == reduce[0].integral_count)
                    {
                        reduce.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                    }
                    else
                    {
                        foreach (var aa in reduce)
                        {
                            if (aa.integral_count < ans)
                            {
                                aa.integral_count = ans;
                            }
                        }
                    }
                }
            }
            foreach (var a in reduce)
            {
                tree_integral_totle.Add(a.integral_count + chess_Change_Computer_tree3(a.point.X, a.point.Y) * -0.5);
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private double chess_Change_Computer_tree3(int X, int Y)
        {
            Chess_Computer3_Update();
            bu3_Update();
            bu3[X, Y] = (chess) % 2 == 0 ? "●" : "○";
            Chess_Computer3[X * 8 + Y].chess_color = (chess) % 2;
            Chess_Computer3[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree3(Chess_Computer3[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer3_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu3[i, j] != "●" && bu3[i, j] != "○")
                    {
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree3((chess + 1) % 2, i, j, -1, 0);
                        Chess_Computer3[i * 8 + j].chess_change = chesscount;
                        Chess_Computer3[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess_Computer4[i * 8 + j].chess_change != 0)
                        //{
                        //    Chess_Computer4[i * 8 + j].chess_tree_integral = chess_Change_Computer_tree4(i, j);
                        //}
                    }
                }
            }
            List<double> tree_integral_totle = new List<double>();
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            foreach (var a in Chess_Computer3)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    //tree_integral_totle.Add(ans);
                    if (reduce.Count < 4 || ans == reduce[0].integral_count)
                    {
                        reduce.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                    }
                    else
                    {
                        foreach (var aa in reduce)
                        {
                            if (aa.integral_count < ans)
                            {
                                aa.integral_count = ans;
                            }
                        }
                    }
                }
            }
            foreach (var a in reduce)
            {
                tree_integral_totle.Add(a.integral_count + chess_Change_Computer_tree4(a.point.X, a.point.Y) * -0.5);
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private double chess_Change_Computer_tree4(int X, int Y)
        {
            Chess_Computer4_Update();
            bu4_Update();
            bu4[X, Y] = (chess + 1) % 2 == 0 ? "●" : "○";
            Chess_Computer4[X * 8 + Y].chess_color = (chess + 1) % 2;
            Chess_Computer4[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree4(Chess_Computer4[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer4_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu4[i, j] != "●" && bu4[i, j] != "○")
                    {
                        Change_count_Computer_tree4((chess) % 2, i, j, 1, 0);
                        Change_count_Computer_tree4((chess) % 2, i, j, 1, 1);
                        Change_count_Computer_tree4((chess) % 2, i, j, 0, 1);
                        Change_count_Computer_tree4((chess) % 2, i, j, -1, -1);
                        Change_count_Computer_tree4((chess) % 2, i, j, -1, 1);
                        Change_count_Computer_tree4((chess) % 2, i, j, 1, -1);
                        Change_count_Computer_tree4((chess) % 2, i, j, 0, -1);
                        Change_count_Computer_tree4((chess) % 2, i, j, -1, 0);
                        Chess_Computer4[i * 8 + j].chess_change = chesscount;
                        Chess_Computer4[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess_Computer5[i * 8 + j].chess_change != 0)
                        //{
                        //    Chess_Computer5[i * 8 + j].chess_tree_integral = chess_Change_Computer_tree5(i, j);
                        //}
                    }
                }
            }
            List<double> tree_integral_totle = new List<double>();
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            foreach (var a in Chess_Computer4)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    //tree_integral_totle.Add(ans);
                    if (reduce.Count < 3 || ans == reduce[0].integral_count)
                    {
                        reduce.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                    }
                    else
                    {
                        foreach (var aa in reduce)
                        {
                            if (aa.integral_count < ans)
                            {
                                aa.integral_count = ans;
                            }
                        }
                    }
                }
            }
            foreach (var a in reduce)
            {
                tree_integral_totle.Add(a.integral_count + chess_Change_Computer_tree5(a.point.X, a.point.Y) * -0.5);
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private double chess_Change_Computer_tree5(int X, int Y)
        {
            Chess_Computer5_Update();
            bu5_Update();
            bu5[X, Y] = (chess) % 2 == 0 ? "●" : "○";
            Chess_Computer5[X * 8 + Y].chess_color = (chess) % 2;
            Chess_Computer5[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree5(Chess_Computer5[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer5_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu5[i, j] != "●" && bu5[i, j] != "○")
                    {
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree5((chess + 1) % 2, i, j, -1, 0);
                        Chess_Computer5[i * 8 + j].chess_change = chesscount;
                        Chess_Computer5[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                    }
                }
            }
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            List<double> tree_integral_totle = new List<double>();
            foreach (var a in Chess_Computer5)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    //tree_integral_totle.Add(ans);
                    if (reduce.Count < 2 || ans == reduce[0].integral_count)
                    {
                        reduce.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                    }
                    else
                    {
                        foreach (var aa in reduce)
                        {
                            if (aa.integral_count < ans)
                            {
                                aa.integral_count = ans;
                            }
                        }
                    }
                }
            }
            foreach (var a in reduce)
            {
                tree_integral_totle.Add(a.integral_count + chess_Change_Computer_tree6(a.point.X, a.point.Y) * -0.5);
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private double chess_Change_Computer_tree6(int X, int Y)
        {
            Chess_Computer6_Update();
            bu6_Update();
            bu6[X, Y] = (chess + 1) % 2 == 0 ? "●" : "○";
            Chess_Computer6[X * 8 + Y].chess_color = (chess + 1) % 2;
            Chess_Computer6[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree6(Chess_Computer6[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer6_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu6[i, j] != "●" && bu6[i, j] != "○")
                    {
                        Change_count_Computer_tree6((chess) % 2, i, j, 1, 0);
                        Change_count_Computer_tree6((chess) % 2, i, j, 1, 1);
                        Change_count_Computer_tree6((chess) % 2, i, j, 0, 1);
                        Change_count_Computer_tree6((chess) % 2, i, j, -1, -1);
                        Change_count_Computer_tree6((chess) % 2, i, j, -1, 1);
                        Change_count_Computer_tree6((chess) % 2, i, j, 1, -1);
                        Change_count_Computer_tree6((chess) % 2, i, j, 0, -1);
                        Change_count_Computer_tree6((chess) % 2, i, j, -1, 0);
                        Chess_Computer6[i * 8 + j].chess_change = chesscount;
                        Chess_Computer6[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess_Computer2[i * 8 + j].chess_change != 0)
                        //{
                        //    Chess_Computer2[i * 8 + j].chess_tree_integral = chess_Change_Computer_tree3(i, j);
                        //}
                    }
                }
            }
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            List<double> tree_integral_totle = new List<double>();
            foreach (var a in Chess_Computer6)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    //tree_integral_totle.Add(ans);
                    if (reduce.Count < 1 || ans == reduce[0].integral_count)
                    {
                        reduce.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                    }
                    else
                    {
                        foreach (var aa in reduce)
                        {
                            if (aa.integral_count < ans)
                            {
                                aa.integral_count = ans;
                            }
                        }
                    }
                }
            }
            foreach (var a in reduce)
            {
                tree_integral_totle.Add(a.integral_count + chess_Change_Computer_tree7(a.point.X, a.point.Y) * -0.5);
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private double chess_Change_Computer_tree7(int X, int Y)
        {
            Chess_Computer7_Update();
            bu7_Update();
            bu7[X, Y] = (chess) % 2 == 0 ? "●" : "○";
            Chess_Computer7[X * 8 + Y].chess_color = (chess) % 2;
            Chess_Computer7[X * 8 + Y].chess_change = 0;
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_Computer_tree7(Chess_Computer7[X * 8 + Y].chess_color, X, Y, -1, 0);
            Chess_Computer7_integral_Update();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bu7[i, j] != "●" && bu7[i, j] != "○")
                    {
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree7((chess + 1) % 2, i, j, -1, 0);
                        Chess_Computer7[i * 8 + j].chess_change = chesscount;
                        Chess_Computer7[i * 8 + j].integral_count = tree_integral;
                        chesscount = 0;
                        tree_integral = 0;
                        //if (Chess_Computer4[i * 8 + j].chess_change != 0)
                        //{
                        //    Chess_Computer4[i * 8 + j].chess_tree_integral = chess_Change_Computer_tree4(i, j);
                        //}
                    }
                }
            }
            List<double> tree_integral_totle = new List<double>();
            List<Chess_Tree> reduce = new List<Chess_Tree>();
            foreach (var a in Chess_Computer7)
            {
                if (a.chess_change != 0)
                {
                    double ans = a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    //tree_integral_totle.Add(ans);
                    tree_integral_totle.Add(ans);
                }
            }
            tree_integral_totle.Sort();
            tree_integral_totle.Reverse();
            if (tree_integral_totle.Count() == 0)
                tree_integral_totle.Add(-4000);
            return tree_integral_totle[0];
        }

        private bool chess_Change_Computer_tree1(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer1[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree1(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer1[X * 8 + Y].chess_color = color;
                    bu1[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu1[X, Y] = "●";
                    }
                    else
                    {
                        bu1[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_Computer_tree2(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer2[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree2(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer2[X * 8 + Y].chess_color = color;
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

        private bool chess_Change_Computer_tree3(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer3[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree3(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer3[X * 8 + Y].chess_color = color;
                    bu3[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu3[X, Y] = "●";
                    }
                    else
                    {
                        bu3[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_Computer_tree4(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer4[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree4(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer4[X * 8 + Y].chess_color = color;
                    bu4[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu4[X, Y] = "●";
                    }
                    else
                    {
                        bu4[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_Computer_tree5(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer5[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree5(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer5[X * 8 + Y].chess_color = color;
                    bu5[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu5[X, Y] = "●";
                    }
                    else
                    {
                        bu5[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_Computer_tree6(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer6[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree6(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer6[X * 8 + Y].chess_color = color;
                    bu6[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu6[X, Y] = "●";
                    }
                    else
                    {
                        bu6[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_Computer_tree7(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer7[X * 8 + Y].chess_color;
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
                if (chess_Change_Computer_tree7(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer7[X * 8 + Y].chess_color = color;
                    bu7[X, Y] = color == 0 ? "●" : "○";
                    if (color == 0)
                    {
                        bu7[X, Y] = "●";
                    }
                    else
                    {
                        bu7[X, Y] = "○";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree1(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer1[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree1(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer1[X * 8 + Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree2(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer2[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree2(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer2[X * 8 + Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree3(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer3[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree3(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer3[X * 8 + Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree4(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer4[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree4(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer4[X * 8 + Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree5(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer5[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree5(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer5[X * 8 + Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree6(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer6[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree6(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer6[X * 8 + Y].tree_integral;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool Change_count_Computer_tree7(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_Computer7[X * 8 + Y].chess_color;
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
                if (Change_count_Computer_tree7(color, X, Y, x_c, y_c) == true)
                {
                    chesscount++;
                    tree_integral += Chess_Computer7[X * 8 + Y].tree_integral;
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
                6000,-300,600,300,300,600,-300,6000,
                -300,-2000,-40,-30,-30,-40,-2000,-300,
                600,-40,3,1,0,3,-40,600,
                300,-30,0,0,0,0,-30,300,
                300,-30,0,0,0,0,-30,300,
                600,-40,3,0,0,3,-30,600,
                -300,-2000,-40,-30,-30,-40,-2000,-300,
                6000,-600,300,300,300,600,-300,6000,
            };
            for (int i = 0; i < Chess_Now.Count; i++)
            {
                Chess_Now[i].tree_integral = integral[i];
            }
        }

        private void Chess_Computer1_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer1[i * 8 + j].chess_change = Chess_Now[i * 8 + j].chess_change;
                    Chess_Computer1[i * 8 + j].chess_color = Chess_Now[i * 8 + j].chess_color;
                    Chess_Computer1[i * 8 + j].integral_count = Chess_Now[i * 8 + j].integral_count;
                    Chess_Computer1[i * 8 + j].tree_integral = Chess_Now[i * 8 + j].tree_integral;
                    Chess_Computer1[i * 8 + j].chess_tree_integral = Chess_Now[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void Chess_Computer2_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer2[i * 8 + j].chess_change = Chess_Computer1[i * 8 + j].chess_change;
                    Chess_Computer2[i * 8 + j].chess_color = Chess_Computer1[i * 8 + j].chess_color;
                    Chess_Computer2[i * 8 + j].integral_count = Chess_Computer1[i * 8 + j].integral_count;
                    Chess_Computer2[i * 8 + j].tree_integral = Chess_Computer1[i * 8 + j].tree_integral;
                    Chess_Computer2[i * 8 + j].chess_tree_integral = Chess_Computer1[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void Chess_Computer3_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer3[i * 8 + j].chess_change = Chess_Computer2[i * 8 + j].chess_change;
                    Chess_Computer3[i * 8 + j].chess_color = Chess_Computer2[i * 8 + j].chess_color;
                    Chess_Computer3[i * 8 + j].integral_count = Chess_Computer2[i * 8 + j].integral_count;
                    Chess_Computer3[i * 8 + j].tree_integral = Chess_Computer2[i * 8 + j].tree_integral;
                    Chess_Computer3[i * 8 + j].chess_tree_integral = Chess_Computer2[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void Chess_Computer4_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer4[i * 8 + j].chess_change = Chess_Computer3[i * 8 + j].chess_change;
                    Chess_Computer4[i * 8 + j].chess_color = Chess_Computer3[i * 8 + j].chess_color;
                    Chess_Computer4[i * 8 + j].integral_count = Chess_Computer3[i * 8 + j].integral_count;
                    Chess_Computer4[i * 8 + j].tree_integral = Chess_Computer3[i * 8 + j].tree_integral;
                    Chess_Computer4[i * 8 + j].chess_tree_integral = Chess_Computer3[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void Chess_Computer5_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer5[i * 8 + j].chess_change = Chess_Computer4[i * 8 + j].chess_change;
                    Chess_Computer5[i * 8 + j].chess_color = Chess_Computer4[i * 8 + j].chess_color;
                    Chess_Computer5[i * 8 + j].integral_count = Chess_Computer4[i * 8 + j].integral_count;
                    Chess_Computer5[i * 8 + j].tree_integral = Chess_Computer4[i * 8 + j].tree_integral;
                    Chess_Computer5[i * 8 + j].chess_tree_integral = Chess_Computer4[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void Chess_Computer6_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer6[i * 8 + j].chess_change = Chess_Computer5[i * 8 + j].chess_change;
                    Chess_Computer6[i * 8 + j].chess_color = Chess_Computer5[i * 8 + j].chess_color;
                    Chess_Computer6[i * 8 + j].integral_count = Chess_Computer5[i * 8 + j].integral_count;
                    Chess_Computer6[i * 8 + j].tree_integral = Chess_Computer5[i * 8 + j].tree_integral;
                    Chess_Computer6[i * 8 + j].chess_tree_integral = Chess_Computer5[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void Chess_Computer7_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Chess_Computer7[i * 8 + j].chess_change = Chess_Computer6[i * 8 + j].chess_change;
                    Chess_Computer7[i * 8 + j].chess_color = Chess_Computer6[i * 8 + j].chess_color;
                    Chess_Computer7[i * 8 + j].integral_count = Chess_Computer6[i * 8 + j].integral_count;
                    Chess_Computer7[i * 8 + j].tree_integral = Chess_Computer6[i * 8 + j].tree_integral;
                    Chess_Computer7[i * 8 + j].chess_tree_integral = Chess_Computer6[i * 8 + j].chess_tree_integral;
                }
            }
        }

        private void bu1_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu1[i, j] = bu[i, j].Text;
                }
            }
        }

        private void bu2_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu2[i, j] = bu1[i, j];
                }
            }
        }

        private void bu3_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu3[i, j] = bu2[i, j];
                }
            }
        }

        private void bu4_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu4[i, j] = bu3[i, j];
                }
            }
        }

        private void bu5_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu5[i, j] = bu4[i, j];
                }
            }
        }

        private void bu6_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu6[i, j] = bu5[i, j];
                }
            }
        }

        private void bu7_Update()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu7[i, j] = bu6[i, j];
                }
            }
        }

        private void integral_Update()
        {
            if (Chess_Now[0].chess_color != -1)
            {
                Chess_Now[1].tree_integral = 300;
                Chess_Now[8].tree_integral = 300;
                Chess_Now[9].tree_integral = -300;
            }
            if (Chess_Now[7].chess_color != -1)
            {
                Chess_Now[6].tree_integral = 300;
                Chess_Now[15].tree_integral = 300;
                Chess_Now[14].tree_integral = -300;
            }
            if (Chess_Now[56].chess_color != -1)
            {
                Chess_Now[48].tree_integral = 300;
                Chess_Now[57].tree_integral = 300;
                Chess_Now[49].tree_integral = -300;
            }
            if (Chess_Now[63].chess_color != -1)
            {
                Chess_Now[55].tree_integral = 300;
                Chess_Now[62].tree_integral = 300;
                Chess_Now[54].tree_integral = -300;
            }
        }

        private void Chess_Computer1_integral_Update()
        {
            if (Chess_Computer1[0].chess_color != -1)
            {
                Chess_Computer1[1].tree_integral = 300;
                Chess_Computer1[8].tree_integral = 300;
                Chess_Computer1[9].tree_integral = -30;
            }
            if (Chess_Computer1[7].chess_color != -1)
            {
                Chess_Computer1[6].tree_integral = 300;
                Chess_Computer1[15].tree_integral = 300;
                Chess_Computer1[14].tree_integral = -30;
            }
            if (Chess_Computer1[56].chess_color != -1)
            {
                Chess_Computer1[48].tree_integral = 300;
                Chess_Computer1[57].tree_integral = 300;
                Chess_Computer1[49].tree_integral = -30;
            }
            if (Chess_Computer1[63].chess_color != -1)
            {
                Chess_Computer1[55].tree_integral = 300;
                Chess_Computer1[62].tree_integral = 300;
                Chess_Computer1[54].tree_integral = -30;
            }
        }

        private void Chess_Computer2_integral_Update()
        {
            if (Chess_Computer2[0].chess_color != -1)
            {
                Chess_Computer2[1].tree_integral = 300;
                Chess_Computer2[8].tree_integral = 300;
                Chess_Computer2[9].tree_integral = -30;
            }
            if (Chess_Computer2[7].chess_color != -1)
            {
                Chess_Computer2[6].tree_integral = 300;
                Chess_Computer2[15].tree_integral = 300;
                Chess_Computer2[14].tree_integral = -30;
            }
            if (Chess_Computer2[56].chess_color != -1)
            {
                Chess_Computer2[48].tree_integral = 300;
                Chess_Computer2[57].tree_integral = 300;
                Chess_Computer2[49].tree_integral = -30;
            }
            if (Chess_Computer2[63].chess_color != -1)
            {
                Chess_Computer2[55].tree_integral = 300;
                Chess_Computer2[62].tree_integral = 300;
                Chess_Computer2[54].tree_integral = -30;
            }
        }

        private void Chess_Computer3_integral_Update()
        {
            if (Chess_Computer3[0].chess_color != -1)
            {
                Chess_Computer3[1].tree_integral = 300;
                Chess_Computer3[8].tree_integral = 300;
                Chess_Computer3[9].tree_integral = -30;
            }
            if (Chess_Computer3[7].chess_color != -1)
            {
                Chess_Computer3[6].tree_integral = 300;
                Chess_Computer3[15].tree_integral = 300;
                Chess_Computer3[14].tree_integral = -30;
            }
            if (Chess_Computer3[56].chess_color != -1)
            {
                Chess_Computer3[48].tree_integral = 300;
                Chess_Computer3[57].tree_integral = 300;
                Chess_Computer3[49].tree_integral = -30;
            }
            if (Chess_Computer3[63].chess_color != -1)
            {
                Chess_Computer3[55].tree_integral = 300;
                Chess_Computer3[62].tree_integral = 300;
                Chess_Computer3[54].tree_integral = -30;
            }
        }

        private void Chess_Computer4_integral_Update()
        {
            if (Chess_Computer4[0].chess_color != -1)
            {
                Chess_Computer4[1].tree_integral = 300;
                Chess_Computer4[8].tree_integral = 300;
                Chess_Computer4[9].tree_integral = -30;
            }
            if (Chess_Computer4[7].chess_color != -1)
            {
                Chess_Computer4[6].tree_integral = 300;
                Chess_Computer4[15].tree_integral = 300;
                Chess_Computer4[14].tree_integral = -30;
            }
            if (Chess_Computer4[56].chess_color != -1)
            {
                Chess_Computer4[48].tree_integral = 300;
                Chess_Computer4[57].tree_integral = 300;
                Chess_Computer4[49].tree_integral = -30;
            }
            if (Chess_Computer4[63].chess_color != -1)
            {
                Chess_Computer4[55].tree_integral = 300;
                Chess_Computer4[62].tree_integral = 300;
                Chess_Computer4[54].tree_integral = -30;
            }
        }

        private void Chess_Computer5_integral_Update()
        {
            if (Chess_Computer5[0].chess_color != -1)
            {
                Chess_Computer5[1].tree_integral = 300;
                Chess_Computer5[8].tree_integral = 300;
                Chess_Computer5[9].tree_integral = -30;
            }
            if (Chess_Computer5[7].chess_color != -1)
            {
                Chess_Computer5[6].tree_integral = 300;
                Chess_Computer5[15].tree_integral = 300;
                Chess_Computer5[14].tree_integral = -30;
            }
            if (Chess_Computer5[56].chess_color != -1)
            {
                Chess_Computer5[48].tree_integral = 300;
                Chess_Computer5[57].tree_integral = 300;
                Chess_Computer5[49].tree_integral = -30;
            }
            if (Chess_Computer5[63].chess_color != -1)
            {
                Chess_Computer5[55].tree_integral = 300;
                Chess_Computer5[62].tree_integral = 300;
                Chess_Computer5[54].tree_integral = -30;
            }
        }

        private void Chess_Computer6_integral_Update()
        {
            if (Chess_Computer6[0].chess_color != -1)
            {
                Chess_Computer6[1].tree_integral = 300;
                Chess_Computer6[8].tree_integral = 300;
                Chess_Computer6[9].tree_integral = -30;
            }
            if (Chess_Computer6[7].chess_color != -1)
            {
                Chess_Computer6[6].tree_integral = 300;
                Chess_Computer6[15].tree_integral = 300;
                Chess_Computer6[14].tree_integral = -30;
            }
            if (Chess_Computer6[56].chess_color != -1)
            {
                Chess_Computer6[48].tree_integral = 300;
                Chess_Computer6[57].tree_integral = 300;
                Chess_Computer6[49].tree_integral = -30;
            }
            if (Chess_Computer6[63].chess_color != -1)
            {
                Chess_Computer6[55].tree_integral = 300;
                Chess_Computer6[62].tree_integral = 300;
                Chess_Computer6[54].tree_integral = -30;
            }
        }

        private void Chess_Computer7_integral_Update()
        {
            if (Chess_Computer7[0].chess_color != -1)
            {
                Chess_Computer7[1].tree_integral = 300;
                Chess_Computer7[8].tree_integral = 300;
                Chess_Computer7[9].tree_integral = -30;
            }
            if (Chess_Computer7[7].chess_color != -1)
            {
                Chess_Computer7[6].tree_integral = 300;
                Chess_Computer7[15].tree_integral = 300;
                Chess_Computer7[14].tree_integral = -30;
            }
            if (Chess_Computer7[56].chess_color != -1)
            {
                Chess_Computer7[48].tree_integral = 300;
                Chess_Computer7[57].tree_integral = 300;
                Chess_Computer7[49].tree_integral = -30;
            }
            if (Chess_Computer7[63].chess_color != -1)
            {
                Chess_Computer7[55].tree_integral = 300;
                Chess_Computer7[62].tree_integral = 300;
                Chess_Computer7[54].tree_integral = -30;
            }
        }

        private void Change_User()
        {
            foreach (var a in Chess_Now)
            {
                if (a.chess_change != 0)
                {
                    if (end == 1)
                        // MessageBox.Show("無子可落，換邊");
                        change = true;
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
        public int min = 0, sec = 0;
        public int win_white = 0, win_black = 0;
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
           // MessageBox.Show("遊戲結束");
            endgame = true;
            win_white = white;
            win_black = black;
            //if (white > black)
            //{
            //    //MessageBox.Show("白子勝");
            //    StreamWriter writer = new StreamWriter(@"C:\Users\User\Desktop\勝負.txt", true);
            //    writer.WriteLine("White,"+black.ToString() + "," + white.ToString() + ","+min+"分"+sec+"秒 ");
            //    writer.Close();
            //    label2.ForeColor = Color.Green;
            //}
            //else if (white < black)
            //{
            //    StreamWriter writer = new StreamWriter(@"C:\Users\User\Desktop\勝負.txt", true);
            //    writer.WriteLine("Black,"+black.ToString() + "," + white.ToString() + ","+ min + "分" + sec + "秒 ");
            //    writer.Close();
            //    // MessageBox.Show("黑子勝");
            //    label1.ForeColor = Color.Green;
            //}
            //else
            //{
            //   // MessageBox.Show("平手");
            //}
            donegame = true;
            //Application.Restart();
        }
        public bool donegame =false;
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

        private void GameTree4_Othello_Load(object sender, EventArgs e)
        {
            if(learn == true)
            button1.PerformClick();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        
        }

        private void GameTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        public bool done = false;
        public int down_X = 0, down_Y = 0;
        int needtime = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            needtime++;
            min = needtime / 60;
            sec = needtime - (needtime / 60) * 60;
        }

        public void downchess(int XX, int YY)
        {
            bu[XX, YY].PerformClick();
        }
    }
}
