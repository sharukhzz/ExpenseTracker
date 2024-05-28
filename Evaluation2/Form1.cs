using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace Evaluation2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }       

        private int rowindex = 0,i, k, lastRowId=1, prevamount;
        private string prevcategory,ss;
        private bool isClick,isEdit, isNull, isDone,isConnect;
        private Expense expense1 = new Expense();

        private void AddExpenseClick(object sender, EventArgs e)
        {
                panel1.Visible = true;
        }

        private void SubmitClick(object sender, EventArgs e)
        {
            if(!isConnect)
            {
                Expense.CreateTable();
                lastRowId = Expense.GetLastRowid(lastRowId);
                isConnect = true;
            }
            if (!isEdit)
            {
                Expense expense1 = new Expense
                {
                    id = lastRowId,
                    category = comboBox1.Text,
                    amount = textBox1.Text,
                    date = dateTimePicker1.Text,
                    expensename = textBox2.Text
                };
                DateTime d = Convert.ToDateTime(expense1.date);
                string s = expense1.category + d.Month + d.Year;
                if (!Expense.dict.ContainsKey(s))
                {
                    Expense.dict.Add(s, int.Parse(expense1.amount));
                }
                else
                {
                    Expense.dict[s] += int.Parse(expense1.amount);
                }
                    if(Expense.dict.ContainsKey(s) && Expense.budg.Count != 0 && Expense.budg.ContainsKey(s))
                    {
                        if(Expense.dict[s] >= Expense.budg[s]  )
                        {
                            MessageBox.Show("Custom category Amount Limit exceeded");
                        }
                    }
                dataGridView1.Rows.Add(lastRowId, expense1.category, expense1.amount, expense1.date, expense1.expensename);
                Expense.Values1.Add(expense1);
                Expense.Add(lastRowId++, expense1.category, int.Parse(expense1.amount), expense1.date, expense1.expensename);         
                lastRowId++;
            }
            else
            {
                Expense expense1 = new Expense
                {
                    id= Expense.Values1[rowindex].id,
                    category = comboBox1.Text,
                    amount = textBox1.Text,
                    date = dateTimePicker1.Text,
                    expensename = textBox2.Text
                };
                DateTime d = Convert.ToDateTime(expense1.date);
                string s = expense1.category + d.Month + d.Year;
                Expense.dict[ss] -= prevamount;
                if (Expense.dict.ContainsKey(ss))
                {
                    int value = Expense.dict[ss]+int.Parse(expense1.amount);
                    Expense.dict.Remove(ss);
                    Expense.dict.Add(s, value);
                }
                if (Expense.dict.ContainsKey(s) && Expense.budg.Count != 0)
                {
                    if (Expense.dict[s] >= Expense.budg[s] )
                    {
                        MessageBox.Show("Custom category Amount Limit exceeded");
                    }
                }
                dataGridView1.Rows.RemoveAt(rowindex);
                dataGridView1.Rows.Insert( rowindex, expense1.id,expense1.category, expense1.amount, expense1.date, expense1.expensename);
                Expense.Values1.RemoveAt(rowindex);
                Expense.Values1.Insert(rowindex, expense1);
                Expense.UpdateRowById(expense1.id, expense1.category, int.Parse(expense1.amount), expense1.date, expense1.expensename);
                isEdit = false;
            }
            comboBox1.Text = "";
            textBox1.Text = "";
            dateTimePicker1.Text = "";
            textBox2.Text = "";
            panel1.Visible = false;
        }

        private void Form1Load(object sender, EventArgs e)
        {
            dataGridView1 = Expense.AddDatas(dataGridView1);
            dataGridView2 = Expense.AddBudgDatas(dataGridView2);
        }


        private void ComboBox1MouseClick(object sender, MouseEventArgs e)
        {
            if (!isClick)
            {
                Expense.CreateCat();             
                isClick = true;
                Expense.Adddatasfromcategory(comboBox1);
            }
        }
        private void DataGridView1CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            rowindex = e.RowIndex;
        }

        private void RemoveClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                DateTime d = Convert.ToDateTime(Expense.Values1[rowindex].date);
                string s = Expense.Values1[rowindex].category + d.Month + d.Year;
                if (Expense.dict.ContainsKey(s)) Expense.dict[s] -=int.Parse(Expense.Values1[rowindex].amount);
                Expense.Delete(Expense.Values1[rowindex].id);
                dataGridView1.Rows.RemoveAt(rowindex);
                Expense.Values1.RemoveAt(rowindex);
            }
        }

        private void TextButtonClick(object sender, EventArgs e)
        {
            if (!comboBox1.Items.Contains(textBox3.Text))
            {
                comboBox1.Items.Add(textBox3.Text);
                Expense.Addcategory(textBox3.Text);
            }
            else
            {
                MessageBox.Show("Add different category");
            }
            panel2.Visible = false;
        }

        private void FilterClick(object sender, EventArgs e)
        {
            panel3.Visible = true;
            if (!isDone)
            {
                Expense.Adddatasfromcategory(comboBox2);
                isDone = true;
           }
        }

        private void Panel3Click(object sender, EventArgs e)
        {
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                dataGridView1.Rows[i].Visible = false;
            }
            for(int i=0; i < Expense.Values1.Count; i++)
            {
                Expense ee = Expense.Values1[i] as Expense;
                if(ee.category == comboBox2.Text)
                {
                    dataGridView1.Rows[i].Visible = true;
                }
            }
            panel3.Visible = false;
        }

        private void AddClickText(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void DateClick(object sender, EventArgs e)
        {
            panel4.Visible = true;

        }

        private void MDClick(object sender, EventArgs e)
        {
            panel5.Visible = true;
            comboBox5.Text = "";
            textBox4.Text = "";
            if (!isNull)
            {
                for (int k = 1; k <= 12; k++)
                {
                    comboBox5.Items.Add(k);
                }
                isNull = true;
            }
        }

        private void DMclick(object sender, EventArgs e)
        {
            panel4.Visible = false;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Visible = false;
            }
            int k = 0;
            foreach (var items in Expense.Values1)
            {
                DateTime d = Convert.ToDateTime(items.date);
                if (d >= dateTimePicker2.Value && d <= dateTimePicker3.Value)
                {
                    dataGridView1.Rows[k].Visible = true;
                }
                k++;
            }
        }

        private void ResetClick(object sender, EventArgs e)
        {
            if (Expense.Values1.Count != 0)
            {
                for (int i = 0; i < Expense.Values1.Count; i++)
                {
                    dataGridView1.Rows[i].Visible = true;
                }
            }
        }
        private void DelCatClick(object sender, EventArgs e)
        {
            panel6.Visible = true;
            comboBox3.Text = "";
            comboBox3.Items.Clear();
            Expense.Adddatasfromcategory(comboBox3);
        }

        private void DellsCatClick(object sender, EventArgs e)
        {
            Expense.Removecategory(comboBox3.Text);
            for (int i=0;i<comboBox1.Items.Count;i++)
            {
                if (comboBox1.Items[i]+""==comboBox3.Text)
                {
                    comboBox1.Items.RemoveAt(i);                 
                }
            }
            int y = Expense.Values1.Count;
            for(int i=0;i<y;i++)
            {
                if(Expense.Values1[i].category== comboBox3.Text)
                {
                    Expense.Delete(Expense.Values1[i].id);
                    dataGridView1.Rows.RemoveAt(i);
                    Expense.Values1.RemoveAt(i);                  

                    i=-1;
                    y--;
                }
            }
            Expense.ModifyBudgetcat(dataGridView2, comboBox3.Text);
            panel6.Visible = false;
        }
        private void button20_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }
        private void Cllick(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void BClick(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void BBClick(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void VClick(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void MmClick(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void KClick(object sender, EventArgs e)
        {
            panel7.Visible = false;
        }

        private void ModClick(object sender, EventArgs e)
        {
            panel7.Visible = true;
            comboBox4.Items.Clear();
            comboBox4.Text = "";
            Expense.Adddatasfromcategory(comboBox4);
            k = 0;
        }

        private void CatClick(object sender, EventArgs e)
        {
            panel12.Visible = true;
        }

        private void KKClick(object sender, EventArgs e)
        {
            panel8.Visible = false;
        }

        private void CpClick(object sender, EventArgs e)
        {
            panel12.Visible = false;
        }

        private void CustomCatClick(object sender, EventArgs e)
        {
            comboBox6.Items.Clear();
            comboBox6.Text = "";
            comboBox8.Text = "";
            textBox8.Text = "";
            textBox5.Text = "";
            panel8.Visible = true;
            Expense.CreateBudg();
            Expense.Adddatasfromcategory(comboBox6);
        }

        private void K3Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
        }

        private void CCclick(object sender, EventArgs e)
        {
            dataGridView2=Expense.Insertbudg(comboBox6.Text,int.Parse(comboBox8.Text), int.Parse(textBox8.Text), int.Parse(textBox5.Text),dataGridView2);
            panel8.Visible = false;
        }

        private void MClick(object sender, EventArgs e)
        {
            if (!comboBox4.Items.Contains(textBox6.Text))
            {
                Expense.Modcat(textBox6.Text, comboBox4.Text);
                Expense.CategoryModify(textBox6.Text, comboBox4.Text);
                if (i != -1)
                {
                    comboBox1.Items.RemoveAt(i);
                    comboBox1.Items.Insert(i, textBox6.Text);
                    panel7.Visible = false;
                }
                for (int k = 0; k < Expense.Values1.Count; k++)
                {
                    if (Expense.Values1[k].category == comboBox4.Text)
                    {
                        DateTime d = Convert.ToDateTime(Expense.Values1[k].date);
                        string s = Expense.Values1[k].category + d.Month + d.Year;
                        int amount;
                        if (Expense.dict.ContainsKey(s))
                        {
                            amount = Expense.dict[s];
                            Expense.dict.Remove(s);
                            s = textBox6.Text + d.Month + d.Year;
                            Expense.dict.Add(s, amount);
                            comboBox6.Text = textBox6.Text;
                        }
                        Expense.Values1[k].category = textBox6.Text;
                        dataGridView1.Rows.RemoveAt(k);
                        dataGridView1.Rows.Insert(k, Expense.Values1[k].id, textBox6.Text, Expense.Values1[k].amount, Expense.Values1[k].date, Expense.Values1[k].expensename);

                    }
                }
                Expense.AlterBudg(dataGridView2, textBox6.Text, comboBox4.Text);
                for(int i=0;i<Expense.budg.Count;i++)
                {
                    string s = Expense.budg.ElementAt(i).Key;
                    int value = Expense.budg.ElementAt(i).Value;
                    Expense.budg.Remove(s);
                    if(s.Contains(comboBox4.Text))
                    {
                        string ss = s.Remove(0, comboBox4.Text.Length);
                        ss = textBox6.Text + ss;
                        Expense.budg.Add(ss, value);
                    }
                }
            }
            else
            {
                MessageBox.Show("Add different category");
            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            if (k == 0)
            {
                i = comboBox4.SelectedIndex;
                k++;
            }
        }

        private void MothdateClick(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Visible = false;
            }
            int k = 0;
            if (comboBox5.Text != "" && textBox4.Text != "")
            {
                foreach (var items in Expense.Values1)
                {
                    DateTime d = Convert.ToDateTime(items.date);
                    if (d.Month == int.Parse(comboBox5.Text) && d.Day == int.Parse(textBox4.Text))
                    {
                        dataGridView1.Rows[k].Visible = true;
                    }
                    k++;
                }
            }
            else if (textBox4.Text == "")
            {
                k = 0;
                foreach (var items in Expense.Values1)
                {
                    DateTime d = Convert.ToDateTime(items.date);
                    if (d.Month == int.Parse(comboBox5.Text))
                    {
                        dataGridView1.Rows[k].Visible = true;
                    }
                    k++;
                }

            }
            else if(comboBox5.Text=="")
            {
               
                k = 0;
                foreach (var items in Expense.Values1)
                {
                    DateTime d = Convert.ToDateTime(items.date);
                    if (d.Day == int.Parse(textBox4.Text))
                    {
                        dataGridView1.Rows[k].Visible = true;
                    }
                    k++;
                }
            }
            panel5.Visible = false;
        }
        private void EditClick(object sender, EventArgs e)
        {
            if (Expense.Values1.Count != 0 && rowindex < Expense.Values1.Count)
            {
                panel1.Visible = true;
                expense1 = Expense.Values1[rowindex] as Expense;
                comboBox1.Text = expense1.category;
                textBox1.Text = expense1.amount;
                prevamount = int.Parse(textBox1.Text);
                prevcategory = comboBox1.Text;
                dateTimePicker1.Text = expense1.date;
                DateTime prevm = Convert.ToDateTime(dateTimePicker1.Text);
                textBox2.Text = expense1.expensename;
                ss = comboBox1.Text + prevm.Month + prevm.Year;
                isEdit = true;
            }
        }
    }
}
