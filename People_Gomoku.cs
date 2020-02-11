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
    public partial class People_Gomoku : Form
    {
        Button[,] bu;
        int chess = 0;
        int chesscount = 0;
        List<gomoku> Chess;
        bool win = false;

        public People_Gomoku()
        {
            InitializeComponent();
            addbutton();
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
            if (clickbu.Text.Length != 0 | win == true) return;
            chess++;
            Chess[Convert.ToInt16(clickbu.Name)].chesscolor = chess % 2;
            clickbu.Text = chess % 2 == 1 ? "●" : "○";
            check_win(Convert.ToInt16(clickbu.Name) / 15, Convert.ToInt16(clickbu.Name) - (Convert.ToInt16(clickbu.Name) / 15) * 15);
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
            else{chesscount = 0;}
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

        private void People_Gomoku_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
    class gomoku
    {
        public int chesscolor { get; set; }
        public Point point { get; set; }
        public int integral_count { get; set; }
        public int integral_tree { get; set; }
    }

