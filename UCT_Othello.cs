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
using System.Data.SqlClient;
using System.IO;

namespace 黑白棋
{
    public partial class UCT_Othello : Form
    {
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
        static Button[,] bu;
        string[,] bu1 = new string[8, 8];
        string[,] bu2 = new string[8, 8];
        string[,] bu3 = new string[8, 8];
        string[,] bu4 = new string[8, 8];
        string[,] bu5 = new string[8, 8];
        string[,] bu6 = new string[8, 8];
        string[,] bu7 = new string[8, 8];
        int chesscount = 0;
        int end = 0;
        int turn = 0;
        bool endgame = false;
        int tree_integral = 0;
        int gotimes = 0;
        List<UCT_candown>[] date = new List<UCT_candown>[65];
        bool Learn = false;
        private void ComputerTree4()
        {
            List<Chess_Tree> computer_integral = new List<Chess_Tree>();
            List<Point> p = new List<Point>();
            Random rd = new Random();
            int rdn = 0;
            double Max = -3000000;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu[i, j].BackColor = Color.LightSkyBlue;
                    if (bu[i, j].Text != "○" && bu[i, j].Text != "●")
                    {
                        bu[i, j].Text = "";
                    }
                }
            }
            foreach (var a in Chess_Now)
            {
                if (a.chess_change != 0)
                {
                    double anss = chess_Change_Computer_tree1(a.point.X, a.point.Y) * -1;
                    if (anss == 0) anss = 3000000;
                    double ans = anss + a.chess_change * 3 + a.integral_count * 2 + a.tree_integral;
                    computer_integral.Add(new Chess_Tree { integral_count = ans, point = new Point(a.point.X, a.point.Y) });
                    bu[a.point.X, a.point.Y].Text = ans.ToString();
                    bu[a.point.X, a.point.Y].Font = new Font("Consolas", 8);
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
            down_X = p[rdn].X;
            down_Y = p[rdn].Y;
            done = true;
            bu[p[rdn].X, p[rdn].Y].PerformClick();
        }
        private double chess_Change_Computer_tree1(int X, int Y)
        {
            Chess_Computer1_Update();
            bu1_Update();
            bu1[X, Y] = chess_color % 2 == 0 ? "●" : "○";
            Chess_Computer1[X * 8 + Y].chess_color = chess_color % 2+1;
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
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree1((chess_color + 1) % 2, i, j, -1, 0);
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
            bu2[X, Y] = (chess_color + 1) % 2 == 0 ? "●" : "○";
            Chess_Computer2[X * 8 + Y].chess_color = (chess_color + 1) % 2+1;
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
                        Change_count_Computer_tree2((chess_color) % 2, i, j, 1, 0);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, 1, 1);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, 0, 1);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, -1, -1);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, -1, 1);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, 1, -1);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, 0, -1);
                        Change_count_Computer_tree2((chess_color) % 2, i, j, -1, 0);
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
            bu3[X, Y] = (chess_color) % 2 == 0 ? "●" : "○";
            Chess_Computer3[X * 8 + Y].chess_color = (chess_color) % 2+1;
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
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree3((chess_color + 1) % 2, i, j, -1, 0);
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
            bu4[X, Y] = (chess_color + 1) % 2 == 0 ? "●" : "○";
            Chess_Computer4[X * 8 + Y].chess_color = (chess_color + 1) % 2+1;
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
                        Change_count_Computer_tree4((chess_color) % 2, i, j, 1, 0);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, 1, 1);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, 0, 1);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, -1, -1);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, -1, 1);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, 1, -1);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, 0, -1);
                        Change_count_Computer_tree4((chess_color) % 2, i, j, -1, 0);
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
            bu5[X, Y] = (chess_color) % 2 == 0 ? "●" : "○";
            Chess_Computer5[X * 8 + Y].chess_color = (chess_color) % 2+1;
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
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree5((chess_color + 1) % 2, i, j, -1, 0);
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
            bu6[X, Y] = (chess_color + 1) % 2 == 0 ? "●" : "○";
            Chess_Computer6[X * 8 + Y].chess_color = (chess_color + 1) % 2+1;
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
                        Change_count_Computer_tree6((chess_color) % 2, i, j, 1, 0);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, 1, 1);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, 0, 1);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, -1, -1);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, -1, 1);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, 1, -1);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, 0, -1);
                        Change_count_Computer_tree6((chess_color) % 2, i, j, -1, 0);
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
            bu7[X, Y] = (chess_color) % 2 == 0 ? "●" : "○";
            Chess_Computer7[X * 8 + Y].chess_color = (chess_color) % 2+1;
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
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, 1, 0);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, 1, 1);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, 0, 1);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, -1, -1);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, -1, 1);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, 1, -1);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, 0, -1);
                        Change_count_Computer_tree7((chess_color + 1) % 2, i, j, -1, 0);
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
            else if (chesscolor ==0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree1(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer1[X * 8 + Y].chess_color = color;
                    bu1[X, Y] = color == 1 ? "●" : "○";
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
            else if (chesscolor ==0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree2(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer2[X * 8 + Y].chess_color = color;
                    bu2[X, Y] = color == 1 ? "●" : "○";
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
            else if (chesscolor ==0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree3(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer3[X * 8 + Y].chess_color = color;
                    bu3[X, Y] = color == 1 ? "●" : "○";
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
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree4(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer4[X * 8 + Y].chess_color = color;
                    bu4[X, Y] = color == 1 ? "●" : "○";
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
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree5(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer5[X * 8 + Y].chess_color = color;
                    bu5[X, Y] = color == 1 ? "●" : "○";
 
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
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree6(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer6[X * 8 + Y].chess_color = color;
                    bu6[X, Y] = color == 1 ? "●" : "○";
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
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (chess_Change_Computer_tree7(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Computer7[X * 8 + Y].chess_color = color;
                    bu7[X, Y] = color == 1 ? "●" : "○";

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
            if (chesscolor == (color+1))
            {
                return true;
            }
            else if (chesscolor == 0)
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
            if (chesscolor == color+1)
            {
                return true;
            }
            else if (chesscolor == 0)
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
            if (chesscolor == color+1)
            {
                return true;
            }
            else if (chesscolor == 0)
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
            if (chesscolor == color+1)
            {
                return true;
            }
            else if (chesscolor == 0)
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
            if (chesscolor == color+1)
            {
                return true;
            }
            else if (chesscolor ==0)
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
            if (chesscolor == color+1)
            {
                return true;
            }
            else if (chesscolor == 0)
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
            if (chesscolor == color+1)
            {
                return true;
            }
            else if (chesscolor == 0)
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
            if (Chess_Now[0].chess_color != 0)
            {
                Chess_Now[1].tree_integral = 300;
                Chess_Now[8].tree_integral = 300;
                Chess_Now[9].tree_integral = -300;
            }
            if (Chess_Now[7].chess_color != 0)
            {
                Chess_Now[6].tree_integral = 300;
                Chess_Now[15].tree_integral = 300;
                Chess_Now[14].tree_integral = -300;
            }
            if (Chess_Now[56].chess_color != 0)
            {
                Chess_Now[48].tree_integral = 300;
                Chess_Now[57].tree_integral = 300;
                Chess_Now[49].tree_integral = -300;
            }
            if (Chess_Now[63].chess_color != 0)
            {
                Chess_Now[55].tree_integral = 300;
                Chess_Now[62].tree_integral = 300;
                Chess_Now[54].tree_integral = -300;
            }
        }

        private void Chess_Computer1_integral_Update()
        {
            if (Chess_Computer1[0].chess_color != 0)
            {
                Chess_Computer1[1].tree_integral = 300;
                Chess_Computer1[8].tree_integral = 300;
                Chess_Computer1[9].tree_integral = -30;
            }
            if (Chess_Computer1[7].chess_color != 0)
            {
                Chess_Computer1[6].tree_integral = 300;
                Chess_Computer1[15].tree_integral = 300;
                Chess_Computer1[14].tree_integral = -30;
            }
            if (Chess_Computer1[56].chess_color != 0)
            {
                Chess_Computer1[48].tree_integral = 300;
                Chess_Computer1[57].tree_integral = 300;
                Chess_Computer1[49].tree_integral = -30;
            }
            if (Chess_Computer1[63].chess_color != 0)
            {
                Chess_Computer1[55].tree_integral = 300;
                Chess_Computer1[62].tree_integral = 300;
                Chess_Computer1[54].tree_integral = -30;
            }
        }

        private void Chess_Computer2_integral_Update()
        {
            if (Chess_Computer2[0].chess_color != 0)
            {
                Chess_Computer2[1].tree_integral = 300;
                Chess_Computer2[8].tree_integral = 300;
                Chess_Computer2[9].tree_integral = -30;
            }
            if (Chess_Computer2[7].chess_color != 0)
            {
                Chess_Computer2[6].tree_integral = 300;
                Chess_Computer2[15].tree_integral = 300;
                Chess_Computer2[14].tree_integral = -30;
            }
            if (Chess_Computer2[56].chess_color != 0)
            {
                Chess_Computer2[48].tree_integral = 300;
                Chess_Computer2[57].tree_integral = 300;
                Chess_Computer2[49].tree_integral = -30;
            }
            if (Chess_Computer2[63].chess_color != 0)
            {
                Chess_Computer2[55].tree_integral = 300;
                Chess_Computer2[62].tree_integral = 300;
                Chess_Computer2[54].tree_integral = -30;
            }
        }

        private void Chess_Computer3_integral_Update()
        {
            if (Chess_Computer3[0].chess_color != 0)
            {
                Chess_Computer3[1].tree_integral = 300;
                Chess_Computer3[8].tree_integral = 300;
                Chess_Computer3[9].tree_integral = -30;
            }
            if (Chess_Computer3[7].chess_color != 0)
            {
                Chess_Computer3[6].tree_integral = 300;
                Chess_Computer3[15].tree_integral = 300;
                Chess_Computer3[14].tree_integral = -30;
            }
            if (Chess_Computer3[56].chess_color != 0)
            {
                Chess_Computer3[48].tree_integral = 300;
                Chess_Computer3[57].tree_integral = 300;
                Chess_Computer3[49].tree_integral = -30;
            }
            if (Chess_Computer3[63].chess_color != 0)
            {
                Chess_Computer3[55].tree_integral = 300;
                Chess_Computer3[62].tree_integral = 300;
                Chess_Computer3[54].tree_integral = -30;
            }
        }

        private void Chess_Computer4_integral_Update()
        {
            if (Chess_Computer4[0].chess_color != 0)
            {
                Chess_Computer4[1].tree_integral = 300;
                Chess_Computer4[8].tree_integral = 300;
                Chess_Computer4[9].tree_integral = -30;
            }
            if (Chess_Computer4[7].chess_color != 0)
            {
                Chess_Computer4[6].tree_integral = 300;
                Chess_Computer4[15].tree_integral = 300;
                Chess_Computer4[14].tree_integral = -30;
            }
            if (Chess_Computer4[56].chess_color != 0)
            {
                Chess_Computer4[48].tree_integral = 300;
                Chess_Computer4[57].tree_integral = 300;
                Chess_Computer4[49].tree_integral = -30;
            }
            if (Chess_Computer4[63].chess_color != 0)
            {
                Chess_Computer4[55].tree_integral = 300;
                Chess_Computer4[62].tree_integral = 300;
                Chess_Computer4[54].tree_integral = -30;
            }
        }

        private void Chess_Computer5_integral_Update()
        {
            if (Chess_Computer5[0].chess_color != 0)
            {
                Chess_Computer5[1].tree_integral = 300;
                Chess_Computer5[8].tree_integral = 300;
                Chess_Computer5[9].tree_integral = -30;
            }
            if (Chess_Computer5[7].chess_color != 0)
            {
                Chess_Computer5[6].tree_integral = 300;
                Chess_Computer5[15].tree_integral = 300;
                Chess_Computer5[14].tree_integral = -30;
            }
            if (Chess_Computer5[56].chess_color != 0)
            {
                Chess_Computer5[48].tree_integral = 300;
                Chess_Computer5[57].tree_integral = 300;
                Chess_Computer5[49].tree_integral = -30;
            }
            if (Chess_Computer5[63].chess_color != 0)
            {
                Chess_Computer5[55].tree_integral = 300;
                Chess_Computer5[62].tree_integral = 300;
                Chess_Computer5[54].tree_integral = -30;
            }
        }

        private void Chess_Computer6_integral_Update()
        {
            if (Chess_Computer6[0].chess_color !=0)
            {
                Chess_Computer6[1].tree_integral = 300;
                Chess_Computer6[8].tree_integral = 300;
                Chess_Computer6[9].tree_integral = -30;
            }
            if (Chess_Computer6[7].chess_color != 0)
            {
                Chess_Computer6[6].tree_integral = 300;
                Chess_Computer6[15].tree_integral = 300;
                Chess_Computer6[14].tree_integral = -30;
            }
            if (Chess_Computer6[56].chess_color !=0)
            {
                Chess_Computer6[48].tree_integral = 300;
                Chess_Computer6[57].tree_integral = 300;
                Chess_Computer6[49].tree_integral = -30;
            }
            if (Chess_Computer6[63].chess_color !=0)
            {
                Chess_Computer6[55].tree_integral = 300;
                Chess_Computer6[62].tree_integral = 300;
                Chess_Computer6[54].tree_integral = -30;
            }
        }

        private void Chess_Computer7_integral_Update()
        {
            if (Chess_Computer7[0].chess_color !=0)
            {
                Chess_Computer7[1].tree_integral = 300;
                Chess_Computer7[8].tree_integral = 300;
                Chess_Computer7[9].tree_integral = -30;
            }
            if (Chess_Computer7[7].chess_color !=0)
            {
                Chess_Computer7[6].tree_integral = 300;
                Chess_Computer7[15].tree_integral = 300;
                Chess_Computer7[14].tree_integral = -30;
            }
            if (Chess_Computer7[56].chess_color != 0)
            {
                Chess_Computer7[48].tree_integral = 300;
                Chess_Computer7[57].tree_integral = 300;
                Chess_Computer7[49].tree_integral = -30;
            }
            if (Chess_Computer7[63].chess_color != 0)
            {
                Chess_Computer7[55].tree_integral = 300;
                Chess_Computer7[62].tree_integral = 300;
                Chess_Computer7[54].tree_integral = -30;
            }
        }



        //-------------------------------   TREE

        int chess_color =0;
        int playbookchesscount = 0;
        List<UCT> Chess_UCT = new List<UCT>();
        List<UCT_candown> candown = new List<UCT_candown>();
        List<Chess_CountChess> Chess_computer = new List<Chess_CountChess>();
        List<Chess_CountChess> Chess_simulation = new List<Chess_CountChess>();
        //public  bool done = false;
        //public  int down_X;
        //public  int down_Y;
        //public bool  wingame =false;
        //public void down(int X,int Y)
        //{
        //    bu[X, Y].PerformClick();
        //}
        //--------------------------------------------------------------------------------------------------
        public UCT_Othello(bool ll)
        {
            Learn = ll;
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

        }

        private void addbutton()
        {
            groupBox1.Controls.Clear();
            Chess_Now = new List<Chess_Gametree>();
            bu = new Button[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bu[i, j] = new Button();
                    bu[i, j].Size = new Size(45, 45);
                    bu[i, j].Location = new Point(4 + j * 45, 10 + i * 45);
                    bu[i, j].Tag = 0;
                    bu[i, j].Font = new Font("Consolas", 20);
                    bu[i, j].Click += new EventHandler(bu_Click);
                    bu[i, j].Name = i * 8 + j + "";
                    bu[i, j].Enabled = false;
                    bu[i, j].FlatStyle = FlatStyle.Flat;
                    bu[i, j].BackColor = Color.LightSkyBlue;
                    Chess_computer.Add(new Chess_CountChess() { chess_change = 0, chess_color = 0, point = new Point(i, j) });
                    Chess_simulation.Add(new Chess_CountChess() { chess_change = 0, chess_color = 0, point = new Point(i, j) });
                    Chess_computer_candown.Add(new Chess_CountChess() { chess_change = 0, chess_color = 0, point = new Point(i, j) });
                    Chess_Now.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0 });
                    Chess_Computer1.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer2.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer3.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer4.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer5.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer6.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    Chess_Computer7.Add(new Chess_Gametree { point = new Point(i, j), chess_color = 0, chess_change = 0, chess_tree_integral = 0 });
                    groupBox1.Controls.Add(bu[i, j]);
                }
            }

            bu[3, 4].Text = "●";
            Chess_Now[3 * 8 + 4].chess_color = 1;
            bu[3, 3].Text = "○";
            Chess_Now[3 * 8 + 3].chess_color = 2;
            bu[4, 3].Text = "●";
            Chess_Now[4 * 8 + 3].chess_color = 1;
            bu[4, 4].Text = "○";
            Chess_Now[4 * 8 + 4].chess_color = 2;
        }
        //--------------------------------------------------------------------------------NOW
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
                        Change_count(chess_color % 2, i, j, 1, 0);
                        Change_count(chess_color % 2, i, j, 1, 1);
                        Change_count(chess_color % 2, i, j, 0, 1);
                        Change_count(chess_color % 2, i, j, -1, -1);
                        Change_count(chess_color % 2, i, j, -1, 1);
                        Change_count(chess_color % 2, i, j, 1, -1);
                        Change_count(chess_color % 2, i, j, 0, -1);
                        Change_count(chess_color % 2, i, j, -1, 0);
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
            if (chesscolor == (color+1))
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
            else if (chesscolor == 0)
            {
                return false;
            }
            else
            {
                if (chess_Change(color, X, Y, x_c, y_c) == true)
                {
                    Chess_Now[X * 8 + Y].chess_color = (color);
                    bu[X, Y].Text = color == 1 ? "●" : "○";
                    white += color == 1 ? -1 : 1;
                    black += color == 1 ? 1 : -1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        bool change = false;
        private void Change_User()
        {
            foreach (var a in Chess_Now)
            {
                if (a.chess_change != 0)
                {
                    if (end == 1)
                    {
                        change = true;
                        Thread.Sleep(5000);
                        //MessageBox.Show("無子可落，換邊");
                    }
                    end = 0;
                    return;
                }
            }
            end++;
            if (end >= 2)
            {
                winer();
                return;
            }
            chess_color++;
            turn++;
            chess_Change_count();
        }
        public int win_white = 0, win_black = 0;
        private void NObutton()
        {
            foreach (var a in Chess_Now)
            {
                if (a.chess_color != 1 && a.chess_color != 2)
                {
                    return;
                }
            }
            winer();
        }

        private void winer()
        {
            //this.Close();
            //MessageBox.Show("遊戲結束");
            endgame = true;
            win_white = white;
            win_black = black;
            if (white > black)
            {
                //MessageBox.Show("白子勝");
                label2.ForeColor = Color.Green;
            }
            else if (white < black)
            {
                //MessageBox.Show("黑子勝");
                label1.ForeColor = Color.Green;
            }
            else
            {
                //MessageBox.Show("平手");
            }
            //wingame = true;
            donegame = true;
        }
        public bool donegame = false;

        //---------------------------------------------------------------------------------Computer
        int chesscount_computer = 0;
        int chess_color_computer = 0;
        int black_computer = 0;
        int white_computer = 0;
        int end_computer = 0;
        bool endgame_computer = false;
        int win_computer = -1;
        private void chess_Change_count_computer()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Chess_computer[i * 8 + j].chess_color != 1 && Chess_computer[i * 8 + j].chess_color != 2)
                    {
                        //MessageBox.Show("");
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, 1, 0);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, 1, 1);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, 0, 1);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, -1, -1);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, -1, 1);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, 1, -1);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, 0, -1);
                        Change_count_computer(chess_color_computer % 2 == 0 ? 1 : 2, i, j, -1, 0);
                        Chess_computer[i * 8 + j].chess_change = chesscount_computer;
                        chesscount_computer = 0;
                    }
                }
            }
            if (endgame_computer == true) return;
            NObutton_computer();
            if (endgame_computer == true) return;
            Change_User_computer();
        }

        private bool Change_count_computer(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_computer[X * 8 + Y].chess_color;
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
                if (Change_count_computer(color, X, Y, x_c, y_c) == true)
                {
                    chesscount_computer++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_computer(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_computer[X * 8 + Y].chess_color;
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
                if (chess_Change_computer(color, X, Y, x_c, y_c) == true)
                {
                    Chess_computer[X * 8 + Y].chess_color = color;
                    if (color == 1)
                    {
                        //bu[X, Y].Text = "●";
                        black_computer++;
                        white_computer--;
                    }
                    else
                    {

                        //bu[X, Y].Text = "○";
                        white_computer++;
                        black_computer--;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Change_User_computer()
        {
            foreach (var a in Chess_computer)
            {
                if (a.chess_change != 0)
                {
                    end_computer = 0;
                    return;
                }
            }
            end_computer++;
            if (end_computer >= 2)
            {
                winer_computer();
                return;
            }
            chess_color_computer++;
            chess_Change_count_computer();
        }

        private void NObutton_computer()
        {
            foreach (var a in Chess_computer)
            {
                if (a.chess_color != 1 && a.chess_color != 2)
                {
                    return;
                }
            }
            winer_computer();
        }

        private void winer_computer()
        {
            //MessageBox.Show("遊戲結束");
            endgame_computer = true;
            if (white_computer > black_computer)
            {
                //MessageBox.Show("白子勝");
                //label2.ForeColor = Color.Green;
                win_computer = 2;
            }
            else if (white_computer < black_computer)
            {
                //MessageBox.Show("黑子勝");
                //label1.ForeColor = Color.Green;
                win_computer = 1;
            }
            else
            {
                //MessageBox.Show("平手");
                win_computer = 0;
            }

        }

        private void Chess_computer_Update()
        {
            for (int i = 0; i < Chess_Now.Count; i++)
            {
                Chess_computer[i].chess_change = Chess_Now[i].chess_change;
                Chess_computer[i].chess_color= Chess_Now[i].chess_color;
                Chess_computer[i].point = Chess_Now[i].point;
                Chess_computer_candown[i].chess_change = Chess_Now[i].chess_change;
                Chess_computer_candown[i].chess_color = Chess_Now[i].chess_color;
                Chess_computer_candown[i].point = Chess_Now[i].point;
            }
        }
        List<Chess_CountChess> Chess_computer_candown = new List<Chess_CountChess>();
        private void Chess_computer_candown_Update()
        {
            for (int i = 0; i < Chess_Now.Count; i++)
            {
                Chess_computer[i].chess_change = Chess_computer_candown[i].chess_change;
                Chess_computer[i].chess_color = Chess_computer_candown[i].chess_color;
                Chess_computer[i].point = Chess_computer_candown[i].point;
            }
        }

        private void Chess_computer_candown_Update2()
        {
            for (int i = 0; i < Chess_Now.Count; i++)
            {
                Chess_computer_candown[i].chess_change = Chess_computer[i].chess_change;
                Chess_computer_candown[i].chess_color = Chess_computer[i].chess_color;
                Chess_computer_candown[i].point = Chess_computer[i].point;
            }
        }

        private void computer_Click(int X,int Y)
        {
            if (chess_color_computer % 2 == 0)
            {
                black_computer++;
            }
            else
            {
                white_computer++;
            }
            Chess_computer[X * 8 + Y].chess_color = chess_color_computer % 2 == 0 ? 1 : 2;
            Chess_computer[X * 8 + Y].chess_change = 0;
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_computer(Chess_computer[X * 8 + Y].chess_color, X, Y, -1, 0);
            chess_color_computer++;
            chess_Change_count_computer();//更新chesscount
        }

        private void computer_number_Update()
        {
            chesscount_computer = 0;
            chess_color_computer = chess_color;
            black_computer = black;
            white_computer = white;
            end_computer = 0;
            endgame_computer = false;
        }
        string[] playbooknumber = new string[128];
        private string computer_playbook()
        {
            StringBuilder sb = new StringBuilder();
            //string playbook = "";
            //List<int> buffer = new List<int>();
            //foreach (var a in Chess_computer)
            //{
            //    buffer.Add(a.chess_color);
            //}
            //for (int i = 0; i < 64; i += 4)
            //{
            //    playbook = "";
            //    for (int j = i; j < i + 3; j++)
            //    {
            //        playbook += Convert.ToString(buffer[j], 2);
            //    }
            //    sb.Append(playbooknumber[Convert.ToInt32(playbook, 2)]);
            //}
            //sb.Append(playbooknumber[buffer[63]]);
            playbookchesscount = 0;
            foreach (var a in Chess_computer)
            {
                if(a.chess_color != 0)
                {
                    playbookchesscount ++;
                }
                sb.Append(a.chess_color);
            }
            return sb.ToString();
        }

        //---------------------------------------------------------------------------------simulation
        int chesscount_simulation = 0;
        int chess_color_simulation = 0;
        int black_simulation = 0;
        int white_simulation = 0;
        int end_simulation = 0;
        bool endgame_simulation = false;
        int win_simulation = -1;
        List<UCT> UCT_Now = new List<UCT>();
        private void chess_Change_count_simulation()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Chess_simulation[i * 8 + j].chess_color != 1 && Chess_simulation[i * 8 + j].chess_color != 2)
                    {
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, 1, 0);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, 1, 1);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, 0, 1);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, -1, -1);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, -1, 1);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, 1, -1);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, 0, -1);
                        Change_count_simulation(chess_color_simulation % 2 == 0 ? 1 : 2, i, j, -1, 0);
                        Chess_simulation[i * 8 + j].chess_change = chesscount_simulation;
                        chesscount_simulation = 0;
                    }
                }
            }
            if (endgame_simulation == true) return;
            NObutton_simulation();
            if (endgame_simulation == true) return;
            Change_User_simulation();
        }

        private bool Change_count_simulation(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_simulation[X * 8 + Y].chess_color;
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
                if (Change_count_simulation(color, X, Y, x_c, y_c) == true)
                {
                    chesscount_simulation++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool chess_Change_simulation(int color, int x, int y, int x_c, int y_c)
        {
            int X = x + x_c;
            int Y = y + y_c;
            if (X > 7 || Y > 7 || X < 0 || Y < 0)
            {
                return false;
            }
            int chesscolor = Chess_simulation[X * 8 + Y].chess_color;
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
                if (chess_Change_simulation(color, X, Y, x_c, y_c) == true)
                {
                    Chess_simulation[X * 8 + Y].chess_color = color;
                    if (color == 1)
                    {
                        //bu[X, Y].Text = "●";
                        black_simulation++;
                        white_simulation--;
                    }
                    else
                    {

                        //bu[X, Y].Text = "○";
                        white_simulation++;
                        black_simulation--;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Change_User_simulation()
        {
            foreach (var a in Chess_simulation)
            {
                if (a.chess_change != 0)
                {
                    end_simulation = 0;
                    return;
                }
            }
            end_simulation++;
            if (end_simulation >= 2)
            {
                winer_simulation();
                return;
            }
            chess_color_simulation++;
            chess_Change_count_simulation();
        }

        private void NObutton_simulation()
        {
            foreach (var a in Chess_simulation)
            {
                if (a.chess_color != 1 && a.chess_color != 2)
                {
                    return;
                }
            }
            winer_simulation();
        }

        private void winer_simulation()
        {
            //MessageBox.Show("遊戲結束");
            endgame_simulation = true;
            if (white_simulation > black_simulation)
            {
                //MessageBox.Show("白子勝");
                //label2.ForeColor = Color.Green;
                win_simulation = 2;
            }
            else if (white_simulation < black_simulation)
            {
                //MessageBox.Show("黑子勝");
                //abel1.ForeColor = Color.Green;
                win_simulation = 1;
            }
            else
            {
                //MessageBox.Show("平手");
                win_simulation = 0;
            }

        }

        private void Chess_simulation_Update()
        {
            for (int i = 0; i < Chess_computer.Count; i++)
            {
                Chess_simulation[i].chess_change = Chess_computer[i].chess_change;
                Chess_simulation[i].chess_color = Chess_computer[i].chess_color;
                Chess_simulation[i].point = Chess_computer[i].point;
            }
        }

        private void simulation_Click(int X, int Y)
        {
            Chess_simulation[X * 8 + Y].chess_color = chess_color_simulation % 2 == 0 ? 1 : 2;
            if(chess_color_simulation % 2 == 0)
            {
                black_simulation++;
            }
            else
            {
                white_simulation++;
            }
            Chess_simulation[X * 8 + Y].chess_change = 0;
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, 1, 0);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, 0, 1);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, 1, 1);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, -1, -1);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, -1, 1);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, 1, -1);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, 0, -1);
            chess_Change_simulation(Chess_simulation[X * 8 + Y].chess_color, X, Y, -1, 0);
            chess_color_simulation++;
            chess_Change_count_simulation();//更新chesscount
        }

        private void simulation_number_Update()
        {
            chesscount_simulation = 0;
            chess_color_simulation = chess_color_computer;
            black_simulation = black_computer;
            white_simulation = white_computer;
            end_simulation = 0;
            endgame_simulation = false;
        }
        int tttt = 0;
        private string simulation_playbook()
        {
            StringBuilder sb = new StringBuilder();
            //string playbook = "";
            //List<int> buffer = new List<int>();
            //foreach (var a in Chess_simulation)
            //{
            //    buffer.Add(a.chess_color);
            //}
            //for (int i = 0; i < 64; i += 4)
            //{
            //    playbook = "";
            //    for (int j = i; j < i + 3; j++)
            //    {
            //        playbook += Convert.ToString(buffer[j], 2);
            //    }
            //    sb.Append(playbooknumber[Convert.ToInt32(playbook, 2)]);
            //}
            //sb.Append(playbooknumber[buffer[63]]);
            playbookchesscount = 0;
            foreach(var a in Chess_simulation)
            {
                if(a.chess_color != 0)
                {
                    playbookchesscount++;
                }
                sb.Append(a.chess_color);
            }
            return sb.ToString();
        }
        private string now_playbook()
        {
            StringBuilder sb = new StringBuilder();
            //string playbook = "";
            //List<int> buffer = new List<int>();
            //foreach (var a in Chess_Now)
            //{
            //    buffer.Add(a.chess_color);
            //}
            //for (int i = 0; i < 64; i += 4)
            //{
            //    playbook = "";
            //    for (int j = i; j < i + 3; j++)
            //    {
            //        playbook += Convert.ToString(buffer[j], 2);
            //    }
            //    sb.Append(playbooknumber[Convert.ToInt32(playbook, 2)]);
            //}
            //sb.Append(playbooknumber[buffer[63]]);
            playbookchesscount = 0;
            foreach (var a in Chess_Now)
            {
                if(a.chess_color != 0)
                {
                    playbookchesscount++;
                }
                sb.Append(a.chess_color);
            }
            return sb.ToString();
        }
        //----------------------------------------------------------------------------
        //chesscolor 1 2
        List<UCT> UCT_simulation;
        List<UCT> UCT_computer;
        private void bu_Click(object sender, EventArgs e)
        {
            if (endgame == true) return;
            Button bb = (Button)(sender);
            int X = Convert.ToInt16(bb.Name) / 8;
            int Y = Convert.ToInt16(bb.Name) - 8 * X;
            down_X = X;
            down_Y = Y;
            bu[X,Y].Font = new Font("Consolas", 20);
            if (Chess_Now[X * 8 + Y].chess_change == 0)
            {
                //MessageBox.Show("無效步數");
                return;
            }
            //if (((Button)(sender)).Text != "") return;
            if (chess_color % 2 == 0)
            {
                ((Button)(sender)).Text = "●";
                Chess_Now[X * 8 + Y].chess_color = 1;
                black++;
            }
            else
            {
                ((Button)(sender)).Text = "○";
                Chess_Now[X * 8 + Y].chess_color = 2;
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
            chess_color++;
            if (turn % 2 == 1)
                done = true;
            turn++;
            change = false;
            chess_Change_count();//
            tttt = 0;
            foreach (var a in Chess_Now)
            {
                if (a.chess_color == 0)
                {
                    tttt++;
                }
            }
            if (tttt < 60)
            {
                UCT_Now.Add(new UCT { Chess_Playbook = now_playbook(),playchess=playbookchesscount, Win = -1 , Chess_Point = X*8+Y});
            }
            if (endgame == true) return;
            if (turn % 2 == 1)
            {
                if(Learn == true)
                {
                    if (change == true)
                    {
                        Thread.Sleep(6000);
                        change = false;
                    }
                }
                if (tttt < 25)
                {
                    // MessageBox.Show(date.Count.ToString());
                    ComputerTree4();
                    //ComputerUCT();
                    done = true;
                }
                else
                {
                    ComputerTree4();
                    done = true;
                }
            }
        }
        List<UCT_candown> dt = new List<UCT_candown>();
        private void ComputerUCT()
        {
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        bu[i, j].BackColor = Color.LightSkyBlue;
            //        if (bu[i, j].Text != "○" && bu[i, j].Text != "●")
            //        {
            //            bu[i, j].Text = "";
            //        }
            //    }
            //}
            gotimes = 10000;
            int times = 0;
            Start:
            int tt = 0;
            times++;
            computer_number_Update();
            Chess_computer_Update();
            Chess_computer_candown_Update2();
            UCT_computer = new List<UCT>();
            foreach(var a in UCT_Now)
            {
                UCT_computer.Add(a);
            }
            while (!endgame_computer)
            {
                Chess_computer_candown_Update2();
                tt++;
                candown = new List<UCT_candown>();
                int t = 0;
                int All = 0;
                List<string> s = new List<string>();
                foreach (var a in Chess_computer)
                {
                    if (a.chess_change != 0)
                    {
                        candown.Add(new UCT_candown { point = a.point, playbook = "", UCT_Value = 0, playchess = 0 });
                    }
                }
                foreach (var a in candown)
                {
                    t++;
                    int c1 = chesscount_computer;
                    int c2 = chess_color_computer;
                    int c3 = black_computer;
                    int c4 = white_computer;
                    int c5 = end_computer;
                    bool c6 = endgame_computer;
                    computer_Click(a.point.X, a.point.Y);
                    a.playbook = computer_playbook();
                    a.playchess = playbookchesscount;
                    s.Add(a.playbook);
                    chesscount_computer = c1;
                    chess_color_computer = c2;
                    black_computer = c3;
                    white_computer = c4;
                    end_computer = c5;
                    endgame_computer = c6;
                    //computer_number_Update();
                    Chess_computer_candown_Update();
                }
                dt = new List<UCT_candown>();
                if (endgame == false)
                {
                    //foreach (var aa in s)
                    //{
                    //    var aaaaa = from aaa in date
                    //                where aaa.playbook == aa
                    //                select new { aaa.playbook, aaa.Win_Black, aaa.Win_White };
                    //    foreach (var bb in aaaaa)
                    //    {
                    //        dt.Add(new UCT_candown() { playbook = bb.playbook, Win_White = bb.Win_White, Win_Black = bb.Win_Black });
                    //    }
                    //}
                    foreach (var a in date[candown[0].playchess])
                    {
                        foreach (var aa in s)
                        {
                            if (aa == a.playbook)
                            {
                                dt.Add(a);
                            }
                        }
                    }
                }
                if (dt.Count == 0 )
                {
                    Simulatiom();
                    goto Start;
                }
                else
                {
                    All = 0;
                    foreach (var a in dt)
                    {
                        All += a.Win_Black+a.Win_White;
                    }
                    foreach (var a in candown)
                    {
                       double pointAll = 0;
                       double pointWin = 0;
                        foreach (var aa in dt)
                        {

                            if (a.playbook.ToString() == aa.playbook)
                            {
                                pointAll =aa.Win_White + aa.Win_Black;
                                pointWin = chess_color % 2 == 0 ? aa.Win_Black : aa.Win_White;
                                break;
                            }
                        }
                        if (pointAll == 0)
                        {
                            computer_Click(a.point.X, a.point.Y);
                            //computer_number_Update();
                            UCT_computer.Add(new UCT { Chess_Playbook = computer_playbook(),playchess = playbookchesscount, Chess_Point = a.point.X*8+ a.point.Y, Win = -1 });
                            //MessageBox.Show(date.Count.ToString());
                            Simulatiom();
                            goto Start;
                        }
                        else
                        {
                            a.UCT_Value = (pointWin / pointAll) +  0.4* Math.Sqrt(Math.Log10(All) / pointAll);
                        }
                    }
                }
                if (times >=  gotimes)
                {
                    s = new List<string>();
                    t = 0;
                    candown = new List<UCT_candown>();
                    foreach (var a in Chess_Now)
                    {
                        if (a.chess_change != 0)
                        {
                            candown.Add(new UCT_candown { point = a.point, playbook = "",playchess=0, UCT_Value = 0 });
                        }
                    }
                    foreach (var a in candown)
                    {
                        t++;
                        int c1 = chesscount_computer;
                        int c2 = chess_color_computer;
                        int c3 = black_computer;
                        int c4 = white_computer;
                        int c5 = end_computer;
                        bool c6 = endgame_computer;
                        computer_Click(a.point.X, a.point.Y);
                        a.playbook = computer_playbook();
                        a.playchess = playbookchesscount;
                        s.Add(a.playbook);
                        chesscount_computer = c1;
                        chess_color_computer = c2;
                        black_computer = c3;
                        white_computer = c4;
                        end_computer = c5;
                        endgame_computer = c6;
                        //computer_number_Update();
                        Chess_computer_candown_Update();
                    }
                    dt = new List<UCT_candown>();
                    //foreach (var aa in s)
                    //{
                    //    var aaaaa = from aaa in date
                    //                where aaa.playbook == aa
                    //                select new { aaa.playbook, aaa.Win_Black, aaa.Win_White };
                    //    foreach (var bb in aaaaa)
                    //    {
                    //        dt.Add(new UCT_candown() { playbook = bb.playbook, Win_White = bb.Win_White, Win_Black = bb.Win_Black });
                    //    }
                    foreach (var a in date[candown[0].playchess])
                    {
                        foreach (var aa in s)
                        {
                            if (aa == a.playbook)
                            {
                                dt.Add(a);
                            }
                        }
                    }
                    //}
                    All = 0;
                    foreach (var a in dt)
                    {
                        All += a.Win_Black + a.Win_White;
                    }
                    foreach (var a in candown)
                    {
                        double pointAll = 0;
                        double pointWin = 0;
                        foreach (var aa in dt)
                        {

                            if (aa.playbook ==a.playbook)
                            {
                                pointAll = aa.Win_White + aa.Win_Black;
                                pointWin = pointWin = chess_color % 2 == 0 ? aa.Win_Black : aa.Win_White;
                                break;
                            }
                        }
                        if (pointAll == 0)
                        {
                            computer_Click(a.point.X, a.point.Y);
                            //computer_number_Update();
                            UCT_computer.Add(new UCT { Chess_Playbook = computer_playbook(),playchess = playbookchesscount, Win = -1 });
                            Simulatiom();
                            goto Start;
                        }
                        else
                        {
                            a.UCT_Value = (pointWin / pointAll) +0.4 * Math.Sqrt(Math.Log10(All) / pointAll);
                        }
                    }
                    candown.Sort((x, y) => { return -x.UCT_Value.CompareTo(y.UCT_Value); });
                    down_X = candown[0].point.X;
                    down_Y = candown[0].point.Y;
                    done = true;
                    foreach (var a in bu)
                    {
                        if (a.Text != "●" && a.Text != "○") { a.Text = ""; a.Font = new Font("Consolas", 20); };
                    }
                    foreach (var a in candown)
                    {
                        bu[a.point.X, a.point.Y].Text = a.UCT_Value.ToString();
                        bu[a.point.X, a.point.Y].Font = new Font("Consolas", 8);
                    }
                    bu[candown[0].point.X, candown[0].point.Y].BackColor = Color.LightPink;
                    bu[candown[0].point.X, candown[0].point.Y].PerformClick();
                    return;
                }
                else
                {
                    if (tt % 2 == 1)
                    {
                        candown.Sort((x, y) => { return -x.UCT_Value.CompareTo(y.UCT_Value); });
                    }
                    else
                    {
                        candown.Sort((x, y) => { return x.UCT_Value.CompareTo(y.UCT_Value); });
                    }
                    UCT_computer.Add(new UCT { Chess_Playbook = candown[0].playbook,playchess = candown[0].playchess, Win = -1 });
                    computer_Click(candown[0].point.X, candown[0].point.Y);
                }
            }
            bool b = true;
            if (win_computer > 0)
            {
                if (win_computer == 1)
                {
                    foreach (var a in UCT_computer)
                    {
                        b = true;
                        a.Win = 0;
                        foreach(var aa in date[a.playchess])
                        {
                            if(aa.playbook == a.Chess_Playbook)
                            {
                                aa.Win_Black += 1;
                                b = false;
                                break;
                            }
                        }
                        if(b)
                        date[a.playchess].Add(new UCT_candown() {playbook =a.Chess_Playbook,Win_Black=1,Win_White=0,playchess = a.playchess });
                    }
                }
                else
                {
                    foreach (var a in UCT_computer)
                    {
                        a.Win = 0;
                        foreach (var aa in date[a.playchess])
                        {
                            b = true;
                            if (aa.playbook == a.Chess_Playbook)
                            {
                                aa.Win_White += 1;
                                b = false;
                                break;
                            }
                        }
                        if(b)
                        date[a.playchess].Add(new UCT_candown() { playbook = a.Chess_Playbook, Win_Black = 0, Win_White = 1,playchess = a.playchess });
                    }
                }
            }
            else
            {
                foreach (var a in UCT_computer)
                {
                    b = true;
                    a.Win = 0;
                    foreach (var aa in date[a.playchess])
                    {
                        if (aa.playbook == a.Chess_Playbook)
                        {
                            aa.Win_Black += 1;
                            aa.Win_White += 1;
                            b = false;
                            break;
                        }
                    }
                    if (b)
                        date[a.playchess].Add(new UCT_candown() { playbook = a.Chess_Playbook, Win_Black = 1, Win_White = 1 ,playchess = a.playchess});
                }
            }
            if (times < gotimes) goto Start;
            else
            {
                candown = new List<UCT_candown>();
                int All = 0;
                List<string> s = new List<string>();
                foreach (var a in Chess_Now)
                {
                    if (a.chess_change != 0)
                    {
                        candown.Add(new UCT_candown { point = a.point, playbook = "",playchess=0, UCT_Value = 0 });
                    }
                }
                int t = 0;
                foreach (var a in candown)
                {
                    t++;
                    int c1 = chesscount_computer;
                    int c2 = chess_color_computer;
                    int c3 = black_computer;
                    int c4 = white_computer;
                    int c5 = end_computer;
                    bool c6 = endgame_computer;
                    computer_Click(a.point.X, a.point.Y);
                    a.playbook = computer_playbook();
                    a.playchess = playbookchesscount;
                    s.Add(a.playbook);
                    chesscount_computer = c1;
                    chess_color_computer = c2;
                    black_computer = c3;
                    white_computer = c4;
                    end_computer = c5;
                    endgame_computer = c6;
                    //computer_number_Update();
                    Chess_computer_candown_Update();
                }
                dt = new List<UCT_candown>();
                foreach(var a in date[candown[0].playchess])
                {
                    foreach(var aa in s)
                    {
                        if(a.playbook == aa)
                        {
                            dt.Add(a);
                        }
                    }
                }
                All = 0;
                foreach (var a in dt)
                {
                    All += a.Win_Black + a.Win_White;
                }
                foreach (var a in candown)
                {
                    double pointAll = 0;
                    double pointWin = 0;
                    foreach ( var aa in dt)
                    {
                        if (a.playbook == aa.playbook)
                        {
                            pointAll = aa.Win_Black +aa.Win_White;
                            pointWin = chess_color% 2 == 0 ? aa.Win_Black : aa.Win_White;
                            break;
                        }
                    }
                    if (pointAll == 0)
                    {
                        computer_Click(a.point.X, a.point.Y);
                        //computer_number_Update();
                        UCT_computer.Add(new UCT { Chess_Playbook = computer_playbook(),playchess = playbookchesscount, Win = -1 });
                        Simulatiom();
                        goto Start;
                    }
                    else
                    {
                        a.UCT_Value = (pointWin / pointAll) +0.4 * Math.Sqrt(Math.Log10(All) / pointAll);
                    }
                }
                candown.Sort((x, y) => { return -x.UCT_Value.CompareTo(y.UCT_Value); });
                down_X = candown[0].point.X;
                down_Y = candown[0].point.Y;
                done = true;
                foreach (var a in bu)
                {
                    if (a.Text != "●" && a.Text != "○") { a.Text = ""; a.Font = new Font("Consolas", 20); };
                }
                foreach (var a in candown)
                {
                    bu[a.point.X, a.point.Y].Text = a.UCT_Value.ToString();
                    bu[a.point.X, a.point.Y].Font = new Font("Consolas", 8);
                }
                bu[candown[0].point.X, candown[0].point.Y].BackColor = Color.LightPink;
                bu[candown[0].point.X, candown[0].point.Y].PerformClick();
                return;
            }
        }

        private void Simulatiom()
        {
            for(int i = 0; i < 1; i++)
            {
                simulation_number_Update();
                Chess_simulation_Update();
                UCT_simulation = new List<UCT>();
                foreach (var a in UCT_computer)
                {
                    UCT_simulation.Add(a);
                }
                List<UCT_candown> candown_simputer = new List<UCT_candown>();
                while (!endgame_simulation)
                {
                    candown_simputer = new List<UCT_candown>();
                    foreach (var a in Chess_simulation)
                    {
                        if (a.chess_change != 0)
                        {
                            candown_simputer.Add(new UCT_candown { point = a.point, playbook = "",playchess=0, UCT_Value = 0 });
                        }
                    }
                    if(candown_simputer.Count == 0)
                    {
                        win_simulation = white_simulation > black_simulation ? 2 : 1;
                        break;
                    }
                    else
                    {
                        Random rd = new Random(Guid.NewGuid().GetHashCode());
                        int rdn = rd.Next(0, candown_simputer.Count);
                        simulation_Click(candown_simputer[rdn].point.X, candown_simputer[rdn].point.Y);
                        UCT_simulation.Add(new UCT { Chess_Playbook = simulation_playbook(),playchess = playbookchesscount, Win = -1 });
                    }
                }
                //     MessageBox.Show(white_simulation+"    /    "+black_simulation.ToString());
                bool b = true;
                if (win_simulation > 0)
                {
                    if (win_simulation == 1)
                    {
                        foreach (var a in UCT_simulation)
                        {
                            b = true;
                                a.Win = 0;
                                foreach (var aa in date[a.playchess])
                                {
                                    if (aa.playbook == a.Chess_Playbook)
                                    {
                                        aa.Win_Black += 1;
                                        b = false;
                                        break;
                                    }
                                }
                                if(b)
                                date[a.playchess].Add(new UCT_candown() { playbook = a.Chess_Playbook,playchess = a.playchess, Win_Black = 1, Win_White = 0 });
                        }
                    }
                    else
                    {
                        foreach (var a in UCT_simulation)
                        {
                            b = true;
                            a.Win = 0;
                            foreach (var aa in date[a.playchess])
                            {
                                if (aa.playbook == a.Chess_Playbook)
                                {
                                    b = false;
                                    aa.Win_White += 1;
                                    break;
                                }
                            }
                            if(b)
                            date[a.playchess].Add(new UCT_candown() { playbook = a.Chess_Playbook,playchess =a.playchess, Win_Black = 0, Win_White = 1 });
                        }
                    }
                }
                else
                {
                    foreach (var a in UCT_simulation)
                    {
                        b = true;
                        a.Win = 0;
                        foreach (var aa in date[a.playchess])
                        {
                            if (aa.playbook == a.Chess_Playbook)
                            {
                                aa.Win_Black += 1;
                                aa.Win_White += 1;
                                b = false;
                                break;
                            }
                        }
                        if (b)
                            date[a.playchess].Add(new UCT_candown() { playbook = a.Chess_Playbook,playchess =a.playchess, Win_Black = 1, Win_White = 1 });
                    }
                }
            }
        }
        //---------------------------------------------------------------------------
        private void User_black(object sender, EventArgs e)
        {
            foreach (var a in bu)
            {
                a.Enabled = true;
            }
            label3.Text = "玩家：黑棋";
            button1.Enabled = false;
            button2.Enabled = false;
            //bu[2, 3].PerformClick();
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
            ComputerTree4();
            done = true;
        }
        string clint_string = "Data Source=JOEY-PC;Initial Catalog=Othello;Integrated Security=True";
        private void UCT_Othello_Load(object sender, EventArgs e)
        {
            // button2.PerformClick();
            //Computer();
                try
                {
                SqlConnection sqlcon = new SqlConnection(clint_string);
                    DataTable ddd = new DataTable();
                    SqlDataAdapter sqla = new SqlDataAdapter("select * from Othello_Record Group by Chess_Playbook,Win_Black,Win_White,Chess_Count", sqlcon);
                    sqlcon.Open();
                    sqla.Fill(ddd);
                    sqlcon.Close();
                    int counttt = 0;
                    for (int i = 5; i < 65; i++)
                    {
                        date[i] = new List<UCT_candown>();
                    }
                    foreach (DataRow dr in ddd.Rows)
                    {
                        counttt = int.Parse((dr[3].ToString()));
                        date[counttt].Add(new UCT_candown() { playbook = dr[0].ToString(), Win_Black = (int)(dr[1]), Win_White = (int)dr[2], playchess = (int)dr[3] });
                    }
                }
                catch
                {
                    Application.Restart();
                }
            addbutton();
            chess_Change_count();
            UCT_computer = new List<UCT>();
            foreach (var a in Chess_Now)
            {
                if (a.chess_change != 0)
                {
                    candown.Add(new UCT_candown { UCT_Value = 0 });
                }
            }
            white = 2;
            black = 2;
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
            for(int i = 0; i < 26; i++)
            {
                playbooknumber[i] = Convert.ToChar(97 + i).ToString();
            }
            for (int i = 26; i < 52; i++)
            {
                playbooknumber[i] = Convert.ToChar(39 + i).ToString();
            }
            for (int i = 52; i < 62; i++)
            {
                playbooknumber[i] = Convert.ToChar(-4 + i).ToString();
            }
            for (int i = 62; i < 88; i++)
            {
                playbooknumber[i] = Convert.ToChar(3 + i).ToString()+ Convert.ToChar(3 + i).ToString();
            }
            for (int i = 88; i < 114; i++)
            {
                playbooknumber[i] = Convert.ToChar(9 + i).ToString() + Convert.ToChar(9 + i).ToString();
            }
            for (int i = 114; i < 124; i++)
            {
                playbooknumber[i] = Convert.ToChar(-68 + i).ToString()+ Convert.ToChar(-68 + i).ToString();
            }
            playbooknumber[124] = "1A";
            playbooknumber[125] = "2A";
            playbooknumber[126] = "3A";
            playbooknumber[127] = "4A";
            if(Learn == true)
            button2.PerformClick();
        }

        private void GameTree_FormClosed(object sender, FormClosedEventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(clint_string);
            DataTable ddd = new DataTable();
            StringBuilder sb = new StringBuilder();
            int timess = 0;
            int countt = 0;
            for(int i = 5; i < 65; i++)
            {
                foreach (var a in date[i])
                {
                    countt++;
                }
            }
            bool safe = false;
            SqlCommand sqlcom = new SqlCommand();
            sqlcom.CommandTimeout = 0;
            sqlcon.Open();
            try
                {
                    sqlcom.Connection = sqlcon;
                    sqlcom.CommandText = "Delete from Safe";
                    sqlcom.ExecuteNonQuery();
                    for (int i = 5; i < 65; i++)
                    {
                        foreach (var aa in date[i])
                        {
                            timess++;
                            sb.AppendLine("Insert Into Safe (Chess_Playbook,Win_Black,Win_White,Chess_Count) Values('" + aa.playbook + "','" + aa.Win_Black + "','" + aa.Win_White + "','" + aa.playchess + "')");
                            if (timess % 100000 == 0)
                            {
                                 sqlcom.CommandText = sb.ToString();
                                sqlcom.ExecuteNonQuery();
                                sb = new StringBuilder();
                            }
                        }
                    }
                sqlcom.CommandText = sb.ToString();
                sqlcom.ExecuteNonQuery();
                safe = true;
                }
                catch
                {
                safe = false;

            }
            if(safe == true)
            {
                sqlcom.CommandText = "Drop table Othello_Record";
                sqlcom.ExecuteNonQuery();
                sqlcom.CommandText = "select * into [Othello_Record] from [Safe]";
                sqlcom.ExecuteNonQuery();
            }
            sqlcon.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool done = false;
        public int down_X=0, down_Y=0;
        public void downchess(int XX , int YY)
        {
            bu[XX, YY].PerformClick();
        }
    }
}

