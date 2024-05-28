using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evaluation2
{
    public partial class Add : Form
    {
        public Add()
        {
            InitializeComponent();
        }
        public event EventHandler<Expense> Datas;
        public static int r = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            Expense expense = new Expense()
            {
                category = comboBox1.Text,
                amount = textBox2.Text,
                date = dateTimePicker1.Text,
                expensename=textBox4.Text,
            };
            Expense.Values1.Add(expense);
            r++;
            Datas?.Invoke(null,expense);
        }
    }
}
