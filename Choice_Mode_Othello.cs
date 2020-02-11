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
    public partial class Choice_Mode_Othello : Form
    {
        public Choice_Mode_Othello()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Choice_Type_Othell().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new People_Othello().Show();
        }

        private void Insert_Othello_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
