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
    public partial class Choice_Type_Othell : Form
    {
        public Choice_Type_Othell()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CountChess_Othello().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new integral_Othello().Show();
        }

        private void Computer_FormClosed(object sender, FormClosedEventArgs e)
        {
            //G3.Hide();
            //UCTO.Hide();
            //this.Hide();
            //new Choice_Type_Othell().Show();
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GameTree1_Othello().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GameTree2_Othello().Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GameTree3_Othello().Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GameTree4_Othello(false).Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GameTree5_Othello().Show();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            new UCT_Othello(false).Show();
        }
        public UCT_Othello UCTO;
        public GameTree4_Othello G4;
        private void button9_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            G4 = new GameTree4_Othello(true);
            G4.Show();
            UCTO = new UCT_Othello(true);
            this.WindowState = FormWindowState.Minimized;
            G4.WindowState = FormWindowState.Minimized;
            UCTO.WindowState = FormWindowState.Minimized;
            UCTO.Show();
            Thread t1 = new Thread(down_G4);
            Thread t2 = new Thread(down_UCTO);
            Thread t3 = new Thread(restart);
            t3.IsBackground = true;
            t1.IsBackground = true;
            t2.IsBackground = true;
            t2.Start();
            Thread.Sleep(1000);
            t1.Start();
            t3.Start();
        }

        void down_G4()
        {
            while (true)
            {
                if(UCTO.done == true)
                {
                    UCTO.done = false;
                    while(G4.down_X != UCTO.down_X || G4.down_Y != UCTO.down_Y)
                    {
                        G4.downchess(UCTO.down_X, UCTO.down_Y);
                        times = 0;
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(10);
            }
        }

        void down_UCTO()
        {
            while (true)
            {
                if (G4.done == true)
                {
                    G4.done = false;
                    while(G4.down_X != UCTO.down_X || G4.down_Y != UCTO.down_Y)
                    {
                        UCTO.downchess(G4.down_X, G4.down_Y);
                        times = 0;
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(10);
                }
            }
        }
        private void Choice_Type_Othell_Load(object sender, EventArgs e)
        {
            //button8.PerformClick();
        }
        void restart()
        {
            while (G4.donegame == false && UCTO.donegame == false)
            {
                Thread.Sleep(1000);
            }
            if(UCTO.donegame == true)
            {
                if (UCTO.win_white > UCTO.win_black)
                {
                    //MessageBox.Show("白子勝");
                    StreamWriter writer = new StreamWriter(@"C:\Users\User\Desktop\勝負2.txt", true);
                    writer.WriteLine("U," + "White," + UCTO.win_black.ToString() + "," + UCTO.win_white.ToString() + "," + G4.min + "分" + G4.sec + "秒," + DateTime.Now + "");
                    writer.Close();
                }
                else if (UCTO.win_white < UCTO.win_black)
                {
                    //MessageBox.Show("白子勝");
                    StreamWriter writer = new StreamWriter(@"C:\Users\User\Desktop\勝負2.txt", true);
                    writer.WriteLine("U," + "Black," + UCTO.win_black.ToString() + "," + UCTO.win_white.ToString() + "," + G4.min + "分" + G4.sec + "秒," + DateTime.Now + "");
                    writer.Close();
                }
            }
            else
            {

                if (G4.win_white > G4.win_black)
                {
                    //MessageBox.Show("白子勝");
                    StreamWriter writer = new StreamWriter(@"C:\Users\User\Desktop\勝負2.txt", true);
                    writer.WriteLine("4," + "White," + G4.win_black.ToString() + "," + G4.win_white.ToString() + "," + G4.min + "分" + G4.sec + "秒," + DateTime.Now + " ");
                    writer.Close();
                }
                else if (G4.win_white < G4.win_black)
                {
                    //MessageBox.Show("白子勝");
                    StreamWriter writer = new StreamWriter(@"C:\Users\User\Desktop\勝負2.txt", true);
                    writer.WriteLine("4," + "Black," + G4.win_black.ToString() + "," + G4.win_white.ToString() + "," + G4.min + "分" + G4.sec + "秒," + DateTime.Now + " ");
                    writer.Close();
                }
            }
            Application.Restart();
        }
        int times = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            times++;
            if(times >= 5)
            {
                Application.Restart();
            }
        }
    }
}
