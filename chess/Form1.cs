using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool isFirst;
        private List<Button> buttons = new List<Button>();

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.ColumnCount = 8;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Label label = new Label();
                    label.Dock = DockStyle.Fill;
                    if ((row + col) % 2 == 0)
                        label.BackColor = Color.White;
                    else
                        label.BackColor = Color.Gray;
                    tableLayoutPanel1.Controls.Add(label, col, row);
                }
            }
        }

        private void BLACKPAWNCLICK(object sender, EventArgs e)
        {

        }

        private void TableLayoutPanel1Click(object sender, EventArgs e)
        {

        }
    }
}
