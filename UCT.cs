using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace 黑白棋
{
    class UCT_candown
    {
        public Point point { get; set; }
        public double UCT_Value { get; set; }
        public string playbook { get; set; }
        public int Win_White { get; set; }
        public int Win_Black { get; set; }

        public int playchess { get; set; }
    }
    class UCT
    {
        public int Win { get; set; }
        public string Chess_Playbook { get; set; }
        public int Chess_Point { get; set; }
        public int Chess_Color { get; set; }
        public int playchess { get; set; }
    }
    class Chess_CountChess
    {
        public Point point { get; set; }
        public int chess_color { get; set; }
        public int chess_change { get; set; }
    }
    class Chess_Gametree
    {
        public Point point { get; set; }
        public int chess_color { get; set; }
        public double integral_count { get; set; }
        public int tree_integral { get; set; }
        public int chess_change { get; set; }
        public double chess_tree_integral { get; set; }
    }
    class Chess_Tree
    {
        public Point point { get; set; }
        public double integral_count { get; set; }
    }
    class gomoku_Threat
    {
        public int chesscolor { get; set; }
        public Point point { get; set; }
        public int integral_count { get; set; }
        public int integral_tree { get; set; }
    }
    class Chess_Towplayer
    {
        public int chess_color { get; set; }
        public int chess_change { get; set; }
    }
}
