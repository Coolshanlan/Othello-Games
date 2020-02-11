using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace MCTS_Othello
{
    public partial class MCTS : Form
    {
        public MCTS()
        {
            InitializeComponent();
        }
        Button[,] bu;
        int endgame = -1;//0黑1白2平
        int white_now = 2;
        int black_now = 2;
        double value_C = 0.3;
        int computer_turn = 0;
        int Alltimes = 10000;
        int endgame_turm = 18;
        bool computer_win = false;
        MCTS_Chess[,] chess_playbook_now = new MCTS_Chess[8,8];
        int chess_color = 0;//0黑色，1白色
        List<MCTS_Tree> DataTree = new List<MCTS_Tree>();
        void Racetxt_Change()
        {
            label3.Text = "黑棋："+black_now+"            白棋："+white_now+"";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            comboBox1.Items.Add("黑子");
            comboBox1.Items.Add("白子");
            comboBox1.SelectedIndex = 0;
            addbutton();
        }
        void ChessPlaybookNowToBu()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    bu[i, j].Text = chess_playbook_now[i, j].color == 0 ? "●" : chess_playbook_now[i, j].color == 1 ? "○" : "";
        }
        void Chess_Change_Update(MCTS_Chess[,] chess_playbook , int X, int Y , int color, ref int black, ref int white)
        {
            Chess_Change(chess_playbook, color, 1, 0,X,Y,ref black , ref white);
            Chess_Change(chess_playbook, color, 0, 1, X, Y,ref black , ref white);
            Chess_Change(chess_playbook, color, 1, 1, X, Y,ref black , ref white);
            Chess_Change(chess_playbook, color, -1, 0, X, Y,ref black , ref white);
            Chess_Change(chess_playbook, color, 0, -1, X, Y,ref black , ref white);
            Chess_Change(chess_playbook, color, -1, -1, X, Y,ref black , ref white);
            Chess_Change(chess_playbook, color, 1, -1,  X, Y,ref black , ref white);
            Chess_Change(chess_playbook, color, -1, 1, X, Y,ref black , ref white);
        }
        bool Chess_Change(MCTS_Chess[,] chess_playbook, int color, int valueX, int valueY, int X, int Y, ref int black, ref int white)
        {
            int x = X + valueX;
            int y = Y + valueY;
            if (x < 0 || y < 0 || x > 7 || y > 7) return false;
            if (chess_playbook[x, y].color == -1) return false;
            if (chess_playbook[x, y].color == color%2) return true;
            else
            {
                if (Chess_Change(chess_playbook, color, valueX, valueY, x, y,ref black,ref white))
                {
                    chess_playbook[x, y].color = color % 2;
                    black = color % 2 == 0 ? black + 1 : black - 1;
                    white = color % 2 == 1 ? white + 1 : white - 1;
                    return true;
                }
            }
            return false;
        }
        List<Point> Chess_Count_Update(MCTS_Chess[,] chess_playbook , int color)//當前顏色
        {
            List<Point> candown = new List<Point>();
            int count = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    count = 0;
                    if (chess_playbook[i,j].color == -1)
                    {
                        Chess_Count(chess_playbook, color, 1, 0, ref count, i, j);
                        Chess_Count(chess_playbook, color, 0, 1,ref count, i, j);
                        Chess_Count(chess_playbook, color, 1, 1,ref count, i, j);
                        Chess_Count(chess_playbook, color, -1, 0,ref count, i, j);
                        Chess_Count(chess_playbook, color, 0, -1,ref count, i, j);
                        Chess_Count(chess_playbook, color, -1, -1,ref count, i, j);
                        Chess_Count(chess_playbook, color, 1, -1, ref count, i, j);
                        Chess_Count(chess_playbook, color, -1, 1, ref count, i, j);
                    }
                    if (count > 0) candown.Add(new Point() { X = i, Y = j });
                    chess_playbook[i, j].count = count;
                }
            return candown;
        }
        bool Chess_Count(MCTS_Chess[,] chess_playbook , int color , int valueX , int valueY , ref int count , int X , int Y)
        {
            int x = X + valueX;
            int y = Y + valueY;
            if (x < 0 || y < 0 || x > 7 || y > 7) return false;
            if (chess_playbook[x, y].color == -1) return false;
            if (chess_playbook[x, y].color == color%2) return true;
            else
            {
                if (Chess_Count(chess_playbook, color, valueX, valueY, ref count, x, y))
                {
                    count++;
                    return true;
                }
            }
            return false;
        }
        int Change_Player(MCTS_Chess[,] chess_playbook, bool Message)
        {
            foreach (var a in chess_playbook)
            {
                if (a.count != 0) return 0;
            }
            if (Message) MessageBox.Show("無子可落，換手");
            return 1;
        }
        int check_win(MCTS_Chess[,] chess_playbook ,bool Message,int black , int white)
        {
            bool done = true;
            foreach(var a in chess_playbook)
            {
                if (a.color == -1 && a.count != 0) done = false;
            }
            if (!done) return -1;
            if(Message)
            {
                if(black > white)
                {
                    MessageBox.Show("黑子勝");
                    return 0;
                }
                else if(black < white)
                {
                    MessageBox.Show("白子勝");
                    return 1;
                }
                else
                {
                    MessageBox.Show("平手");
                    return 2;
                }
            }
            else
            {
                if (black > white)return 0;
                else if (black < white)return 1;
                else return 2;
            }
        }//
        void ArraytoArray(MCTS_Chess[,] a1, MCTS_Chess[,] a2)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int ii = 0; ii < 8; ii++)
                {
                    a1[i, ii] = new MCTS_Chess();
                    a1[i, ii].color = a2[i, ii].color;
                    a1[i, ii].count = a2[i, ii].count;
                }
            }
        }
        void addbutton()
        {
            bu = new Button[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    chess_playbook_now[i, j] = new MCTS_Chess();
                    chess_playbook_now[i, j].color = -1;
                    bu[i, j] = new Button();
                    bu[i, j].Name = i.ToString();
                    bu[i, j].Size = new Size(45, 45);
                    bu[i, j].Location = new Point(4 + j * 45, 10 + i * 45);
                    bu[i, j].Tag = j.ToString();
                    bu[i, j].Font = new Font("微軟正黑體", 23);
                    bu[i, j].Click += new EventHandler(bu_Click);
                    bu[i, j].Enabled = false;
                    bu[i, j].FlatStyle = FlatStyle.Flat;
                    bu[i, j].BackColor = Color.FromArgb(255, 173, 216, 230);
                    groupBox1.Controls.Add(bu[i, j]);
                }
            chess_playbook_now[3, 4].color = 0;
            chess_playbook_now[3, 3].color = 1;
            chess_playbook_now[4, 3].color = 0;
            chess_playbook_now[4, 4].color = 1;
            chess_color = 0;
            Chess_Count_Update(chess_playbook_now,chess_color);
            ChessPlaybookNowToBu();
        }
        void bu_Click(object sender , EventArgs e)
        {
            Button c_bu = ((Button)(sender));
            int x = int.Parse(c_bu.Name);
            int y = int.Parse(c_bu.Tag.ToString());

            if (endgame != -1) return;
            if (chess_playbook_now[x,y].color != -1) return;
            if (chess_playbook_now[x, y].count == 0) return;

            chess_playbook_now[x, y].color = chess_color % 2;

            black_now = chess_color % 2 == 0 ? black_now + 1 : black_now ;
            white_now = chess_color % 2 == 1 ? white_now + 1 : white_now ;
            Chess_Change_Update(chess_playbook_now, x, y, chess_color, ref black_now, ref white_now);

            chess_color++;
            Chess_Count_Update(chess_playbook_now, chess_color);
            chess_color += Change_Player(chess_playbook_now, true);
            List<Point> candown = Chess_Count_Update(chess_playbook_now, chess_color);

            Racetxt_Change();
            ChessPlaybookNowToBu();

            endgame = check_win(chess_playbook_now, true, black_now, white_now);

            if (endgame < 0)
            {
                if (!computer_win)
                {
                    bool have = false;
                    foreach (var a in DataTree)
                    {
                        if (a.point == new Point() { X = x, Y = y })
                        {
                            have = true;
                            MCTS_Tree m = new MCTS_Tree();
                            m = a;
                            DataTree = new List<MCTS_Tree>();
                            DataTree = m.Next;
                            break;
                        }
                    }
                    if (!have)
                        DataTree = new List<MCTS_Tree>();
                }
                if(chess_color % 2 == computer_turn)
                {
                    if (64 - (black_now + white_now) <= endgame_turm)
                    {
                        foreach (var a in candown)
                        {
                            if (endgame_search(chess_playbook_now, a.X, a.Y, chess_color, black_now, white_now) == computer_turn)
                            {
                                computer_win = true;
                                MessageBox.Show("你為你還有機會贏??????");
                                Computer_Click(a.X, a.Y);
                                return;
                            }
                        }
                    }
                    Task.Run(() => Computer(candown));
                }
            }
            button3.PerformClick();
        }
        void Computer(List<Point>candown)
        {

            //foreach (var a in candown)
            //{
            //    bool down = false;
            //    if(a.X == 0 && a.Y == 0) down = true;
            //    if (a.X == 7 && a.Y == 0) down = true;
            //    if (a.X == 0 && a.Y == 7) down = true;
            //    if (a.X == 7 && a.Y == 7) down = true;
            //    if (down) Computer_Click(a.X,a.Y);
            //    if (down) return;
            //}
            for(int i = 0; i < (int)((double)Alltimes*(double)(1+(black_now+white_now)/100.0))/**(candown.Count+((chess_color/10)+1))*/;i++)
            {
                MCTS_Chess[,] chess_playbook_computer = new MCTS_Chess[8, 8];
                List<MCTS_Tree> mt_computer = DataTree;
                ArraytoArray(chess_playbook_computer,chess_playbook_now);
                int black_computer = black_now;
                int white_computer = white_now;
                int color_computer = chess_color;
                List<Point> candown_computer = new List<Point>();
                foreach (var a in candown) candown_computer.Add(a);
                List<MCTS_Tree> down_computer = new List<MCTS_Tree>();
                bool start = false;
                while (!start )
                {
                    int end = check_win(chess_playbook_computer, false, black_computer, white_computer);
                    if(end == -1)
                    {
                        if (mt_computer.Count != candown_computer.Count)
                        {
                            foreach (var a in candown_computer)
                            {
                                if (mt_computer.FindAll(z => z.point == new Point() { X = a.X, Y = a.Y }).ToList().Count == 0)
                                {
                                    MCTS_Tree mt = new MCTS_Tree();
                                    MCTS_Chess[,] mc = new MCTS_Chess[8, 8];
                                    ArraytoArray(mc, chess_playbook_computer);
                                    int check = simulation(mt, a.X, a.Y, mc, black_computer, white_computer, color_computer);
                                    foreach (var aa in down_computer)
                                    {
                                        if (check != -1)
                                        {
                                            if (check == 0)
                                            {
                                                aa.black_win++;
                                            }
                                            else if (check == 1)
                                            {
                                                aa.white_win++;
                                            }
                                            else
                                            {
                                                aa.white_win++;
                                                aa.black_win++;
                                            }
                                        }
                                    }
                                    mt_computer.Add(mt);
                                    start = true;
                                    break;
                                }
                            }
                            if (start)
                                continue;
                        }
                        Data_uct_Update(mt_computer, color_computer);
                        mt_computer.Sort((z, s) => { return -z.uct_value.CompareTo(s.uct_value); });
                        down_computer.Add(mt_computer[0]);
                        chess_playbook_computer[mt_computer[0].point.X, mt_computer[0].point.Y].color = color_computer % 2;
                        black_computer = color_computer % 2 == 0 ? black_computer + 1 : black_computer;
                        white_computer = color_computer % 2 == 1 ? white_computer + 1 : white_computer;
                        Chess_Change_Update(chess_playbook_computer, mt_computer[0].point.X, mt_computer[0].point.Y, color_computer, ref black_computer, ref white_computer);
                        color_computer++;
                        Chess_Count_Update(chess_playbook_computer, color_computer);
                        color_computer += Change_Player(chess_playbook_computer, false);
                        candown_computer = Chess_Count_Update(chess_playbook_computer, color_computer);
                        //if(mt_computer[0].Next == null && check_win(chess_playbook_computer, false, black_computer, white_computer) == -1)
                        //{
                        //    check_win(chess_playbook_computer, false, black_computer, white_computer);
                        //}
                        mt_computer = mt_computer[0].Next;
                    }
                    else
                    {
                        foreach (var aa in down_computer)
                        {
                            if (end != -1)
                            {
                                if (end == 0)
                                {
                                    aa.black_win++;
                                }
                                else if (end == 1)
                                {
                                    aa.white_win++;
                                }
                                else
                                {
                                    aa.white_win++;
                                    aa.black_win++;
                                }
                            }
                        }
                        start = true;
                        break;
                    }
                  
                }
           }
            DataTree.Sort((z, s) => { return -z.uct_value.CompareTo(s.uct_value); });
            MessageBox.Show("black_win:"+DataTree[0].black_win.ToString()+"\r\n"+ "white_win:" + DataTree[0].white_win.ToString()+"\r\n"+ "uct_value:" + DataTree[0].uct_value.ToString());
            int x, y;
            x = DataTree[0].point.X;
            y = DataTree[0].point.Y;
            DataTree = DataTree[0].Next;
            bu[x, y].PerformClick();
       }
        int endgame_search( MCTS_Chess[,] chess_playbook, int x, int y, int color_simulation, int black_e, int white_e)
        {
            MCTS_Chess[,] chess_playbook_simulation= new MCTS_Chess[8,8];
            List<Point> candown = new List<Point>();
            int black = black_e;
            int white = white_e;
            ArraytoArray(chess_playbook_simulation,chess_playbook);
            chess_playbook_simulation[x, y].color = color_simulation % 2;
            black = color_simulation % 2 == 0 ? black + 1 : black;
            white = color_simulation % 2 == 1 ? white + 1 : white;
            Chess_Change_Update(chess_playbook_simulation, x, y, color_simulation, ref black, ref white);
            color_simulation++;
            Chess_Count_Update(chess_playbook_simulation, color_simulation);
            color_simulation += Change_Player(chess_playbook_simulation, false);
            candown = Chess_Count_Update(chess_playbook_simulation, color_simulation);
            int check = check_win(chess_playbook_simulation, false, black, white);
            if (check != -1)
            {
                return check;
            }
            else
            {               
                int now_color = color_simulation - 1;
                int value = -2;
                foreach (var a in candown)
                {
                    value = endgame_search(chess_playbook_simulation, a.X, a.Y, color_simulation, black, white);
                    if(now_color%2 == computer_turn)
                    {
                        if (value == computer_turn) continue;
                        else return value;
                    }
                    else
                    {
                        if (value == computer_turn) return value;
                        else continue;
                    }
                }
                return value;
            }
        }
        void Data_uct_Update(List<MCTS_Tree> mc,int color)
        {
            int All = 0;
            foreach(var a in mc)
            {
                All += a.black_win;
                All += a.white_win;
            }
            foreach(var a in mc)
            {
                a.uct_value = color % 2 == 0 ? ((double)a.black_win / (double)(a.black_win + a.white_win) + value_C * Math.Sqrt(Math.Log10((All) / (double)(a.black_win + a.white_win)))) : ((double)a.white_win / (double)(a.black_win + a.white_win) + value_C * Math.Sqrt(Math.Log10((All) / (double)(a.black_win + a.white_win))));
            }
        }
        void Data_Update(List<MCTS_Tree> mc,int win)
        {
            foreach(var a in mc)
            {
                if (win == 0)
                {
                    a.black_win++;
                }
                else if (win == 1)
                {
                    a.white_win++;
                }
                else
                {
                    a.white_win+=0;
                    a.black_win+=0;
                }
            }
        }
        void Computer_Click(int x,int y)
        {
            List<Point> candown = new List<Point>();
            chess_playbook_now[x, y].color = chess_color % 2;

            black_now = chess_color % 2 == 0 ? black_now + 1 : black_now;
            white_now = chess_color % 2 == 1 ? white_now + 1 : white_now;

            Chess_Change_Update(chess_playbook_now, x, y, chess_color,ref black_now,ref white_now);

            chess_color++;
            Chess_Count_Update(chess_playbook_now, chess_color);
            chess_color += Change_Player(chess_playbook_now, true);
            candown = Chess_Count_Update(chess_playbook_now, chess_color);

            Racetxt_Change();
            ChessPlaybookNowToBu();

            endgame = check_win(chess_playbook_now, true, black_now, white_now);
            if(!computer_win)
            DataTree = DataTree[0].Next;
            GC.Collect();

            if (endgame < 0)
            {
                if (!computer_win)
                {
                    bool have = false;
                    foreach (var a in DataTree)
                    {
                        if (a.point == new Point() { X = x, Y = y })
                        {
                            have = true;
                            MCTS_Tree m = new MCTS_Tree();
                            m = a;
                            DataTree = new List<MCTS_Tree>();
                            DataTree = m.Next;
                            break;
                        }
                    }
                    if (!have)
                        DataTree = new List<MCTS_Tree>();
                }
                if (chess_color % 2 == computer_turn)
                {
                    if (64 - (black_now + white_now) <= 17)
                    {
                        foreach (var a in candown)
                        {
                            if (endgame_search(chess_playbook_now, a.X, a.Y, chess_color, black_now, white_now) == computer_turn)
                            {
                                computer_win = true;
                                MessageBox.Show("你為你還有機會贏??????");
                                Computer_Click(a.X, a.Y);
                                return;
                            }
                        }
                    }
                    Task.Run(() => Computer(candown));
                }
            }
        }
        int simulation(MCTS_Tree mt,int x,int y,MCTS_Chess[,] chess_playbook_simulation, int black, int white,int color_simulation)
        {
            Random rd = new Random();
            mt.point = new Point() { X=x,Y=y};
            chess_playbook_simulation[x,y].color = color_simulation % 2;
            black = color_simulation % 2 == 0 ? black + 1 : black;
            white = color_simulation % 2 == 1 ? white + 1 : white;
            Chess_Change_Update(chess_playbook_simulation, x, y, color_simulation, ref black, ref white);
            color_simulation++;
            Chess_Count_Update(chess_playbook_simulation, color_simulation);
            color_simulation += Change_Player(chess_playbook_simulation, false);
            Chess_Count_Update(chess_playbook_simulation, color_simulation);
            int check = check_win(chess_playbook_simulation, false, black, white);
                if (check != -1)
                {
                    if (check == 0)
                    {
                        mt.black_win++;
                    }
                    else if (check == 1)
                    {
                        mt.white_win++;
                    }
                    else
                    {
                    mt.white_win++;
                    mt.black_win++;
                }
                        return check;
                    }
                else
                {
                    List<Point> candown = Chess_Count_Update(chess_playbook_simulation, color_simulation);
                    int rdn = rd.Next(0, candown.Count);
                    mt.Next = new List<MCTS_Tree>();
                    mt.Next.Add(new MCTS_Tree());
                    int win = simulation(mt.Next[0], candown[rdn].X, candown[rdn].Y, chess_playbook_simulation, black, white, color_simulation);
                    if (win == 0)
                    {
                        mt.black_win++;
                    }
                    else if (win == 1)
                    {
                        mt.white_win++;
                    }
                else
                {
                    mt.white_win++;
                    mt.black_win++;
                }
                return win;
                }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Button b in bu) b.Enabled = true;
            label2.ForeColor = Color.Red;
            label2.Text = "玩家："+ comboBox1.Text;
            comboBox1.Enabled = false;
            button1.Enabled = false;
            if(comboBox1.Text == "黑子")
            {
                chess_color = 0;
                computer_turn = 1;
            }
            else
            {
                chess_color = 0;
                computer_turn = 0;
                Computer(Chess_Count_Update(chess_playbook_now,0));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataTree = new List<MCTS_Tree>();
            comboBox1.Enabled = true;
            button1.Enabled = true;
            endgame = -1;
            foreach (Button b in bu)
            {
                b.Text = "";
                b.Enabled = false;
            }
            for (int i = 0; i < 8; i++)
                for (int ii = 0; ii < 8; ii++)
                {
                    chess_playbook_now[i, ii] = new MCTS_Chess();
                    chess_playbook_now[i, ii].color = -1;
                }
            chess_playbook_now[3, 4].color = 0;
            chess_playbook_now[3, 3].color = 1;
            chess_playbook_now[4, 3].color = 0;
            chess_playbook_now[4, 4].color = 1;
            black_now = 2;
            white_now = 2;
            Racetxt_Change();
            Chess_Count_Update(chess_playbook_now,0);
            ChessPlaybookNowToBu();
            chess_color = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (MCTS_Chess a in chess_playbook_now) sb.Append(a.color+1);
            textBox1.Text = sb.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            if (comboBox2.Text == "黑子")
            {
                computer_turn = 0;
                chess_color = 1;
            }
            else
            {
                chess_color = 0;
                computer_turn = 1;
            }
            string[] ss = Array.ConvertAll(textBox1.Text.ToArray(), Convert.ToString);
            chess_playbook_now = new MCTS_Chess[8, 8];
            DataTree = new List<MCTS_Tree>();
            endgame = -1;
            foreach (Button b in bu)
            {
                b.Text = "";
            }
            for (int i = 0; i < 8; i++)
                for (int ii = 0; ii < 8; ii++)
                {
                    chess_playbook_now[i, ii] = new MCTS_Chess();
                    chess_playbook_now[i, ii].color = int.Parse(ss[i * 8 + ii].ToString())-1;
                }
            black_now = 0;
            white_now = 0;
            foreach(var a in chess_playbook_now)
            {
                if (a.color == 0) black_now++;
                else if (a.color == 1) white_now ++;
            }

            chess_color++;
            Chess_Count_Update(chess_playbook_now, chess_color);
            chess_color += Change_Player(chess_playbook_now, true);
            List<Point> candown = Chess_Count_Update(chess_playbook_now, chess_color);

            Racetxt_Change();
            ChessPlaybookNowToBu();

            endgame = check_win(chess_playbook_now, true, black_now, white_now);

            if (endgame < 0)
            {
               
                    if (64 - (black_now + white_now) <= endgame_turm)
                    {
                        foreach (var a in candown)
                        {
                            if (endgame_search(chess_playbook_now, a.X, a.Y, chess_color, black_now, white_now) == computer_turn)
                            {
                                computer_win = true;
                                MessageBox.Show("你為你還有機會贏??????");
                                Computer_Click(a.X, a.Y);
                                return;
                            }
                        }
                    }
                    Task.Run(() => Computer(candown));
            }
            button3.PerformClick();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "黑子") computer_turn = 0;
            //else computer_turn = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
    class MCTS_Chess
    {
        public int color { get; set; }
        public int count { get; set; }
    }
    class MCTS_Tree
    {
        //public MCTS_Chess chess_playbook { get; set; }
        public List<MCTS_Tree> Next { get; set; }
        public Point point { get; set; }
        public int black_win { get; set; }
        public int white_win { get; set; }
        public double uct_value { get; set; }
    }
}
