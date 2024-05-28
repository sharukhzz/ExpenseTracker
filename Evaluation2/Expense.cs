using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evaluation2
{
    public class Expense
    {
        public int id;
        public string category;
        public string amount;
        public string date;
        public string expensename;
        public static List<Expense> Values1=new List<Expense>();
        public static Dictionary<string, int> dict = new Dictionary<string, int>();
        public static Dictionary<string, int> budg = new Dictionary<string, int>();
        private const string connectionstring = "Server=localhost;Database=winformdb;Uid=root;Pwd=Sharuk@123.;";
        public static void CreateTable()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "CREATE TABLE IF NOT EXISTS expensetracker(id INT  PRIMARY KEY,category VARCHAR(25),amount INT,date VARCHAR(25),expensename VARCHAR(25))";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void CreateCat()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "CREATE table if not exists category(cid int auto_increment primary key,category varchar(50) ) ";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "select count(*) from category";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result + "" == "0")
                    {
                        Addcategory("Food");
                        Addcategory("Travel");
                        Addcategory("Sports");
                    }
                }
            }
        }
        public static void CreateBudg()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "create table if not exists Budget(category varchar(255),month int,year int,budget int)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public static DataGridView Insertbudg(string category, int month, int year, int budget,DataGridView dataGridView)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM Budget WHERE category = @c AND month = @m AND year = @y";
                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@c", category);
                    selectCommand.Parameters.AddWithValue("@m", month);
                    selectCommand.Parameters.AddWithValue("@y", year);
                    int count = Convert.ToInt32(selectCommand.ExecuteScalar());
                    if (count == 0)
                    {

                        string query = "insert into Budget(category,month,year,budget) values(@c,@m,@y,@b)";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@c", category);
                            command.Parameters.AddWithValue("@m", month);
                            command.Parameters.AddWithValue("@y", year);
                            command.Parameters.AddWithValue("@b", budget);
                            command.ExecuteNonQuery();
                        }
                        dataGridView.Rows.Add(category, month, year, budget);
                        string s = category + month + year;
                        budg.Add(s, budget);
                    }
                    else
                    {
                        using (MySqlConnection connection1 = new MySqlConnection(connectionstring))
                        {
                            connection1.Open();
                            string query = "update Budget set budget=@b where category=@c AND month=@m AND year=@y";
                            using (MySqlCommand command = new MySqlCommand(query, connection1))
                            {
                                command.Parameters.AddWithValue("@c", category);
                                command.Parameters.AddWithValue("@m", month);
                                command.Parameters.AddWithValue("@y", year);
                                command.Parameters.AddWithValue("@b", budget);
                                command.ExecuteNonQuery();
                            }
                            string s = category + month + year;
                            budg[s] = budget;
                        }
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if (row.Cells["Category"].Value.ToString() == category &&
                                Convert.ToInt32(row.Cells["month"].Value) == month &&
                                Convert.ToInt32(row.Cells["year"].Value) == year)
                            {
                                row.Cells["budget"].Value = budget;
                                break;
                            }
                        }
                       
                    }

                }
            }
            return dataGridView;
        }
        public static void Adddatasfromcategory(ComboBox comboBox)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "select category from category";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader["category"]);
                        }
                    }
                }
            }
        }

        public static void Addcategory(string v)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "insert into category (category)values(@category)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category", v);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void Removecategory(string v)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "delete from category where category=@id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", v);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void Modcat(string text, string oldtext)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "update expensetracker set category=@category where category=@oldtext";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category", text);
                    command.Parameters.AddWithValue("@oldtext", oldtext);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void CategoryModify(string text, string oldtext)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "update category set category=@category where category=@oldtext";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                        command.Parameters.AddWithValue("@category", text);
                        command.Parameters.AddWithValue("@oldtext", oldtext);
                        command.ExecuteNonQuery();  
                }
            }
        }
        public static void Add(int id,string category, int amount, string date, string expensename)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "INSERT INTO expensetracker (id,category,amount,date,expensename) VALUES (@id,@category,@amount,@date,@expensename)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@expensename", expensename);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateRowById(int id, string newCategory, int newAmount, string newDate, string newExpenseName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "UPDATE expensetracker SET category = @newCategory, amount = @newAmount, date = @newDate, expensename = @newExpenseName WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newCategory", newCategory);
                    command.Parameters.AddWithValue("@newAmount", newAmount);
                    command.Parameters.AddWithValue("@newDate", newDate);
                    command.Parameters.AddWithValue("@newExpenseName", newExpenseName);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "DELETE FROm expensetracker WHERE id=@id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static int GetLastRowid(int lastRowId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "SELECT id FROM expensetracker ORDER BY id DESC LIMIT 1";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        lastRowId = Convert.ToInt32(result) + 1;
                    }
                }
            }
            return lastRowId;
        }
        public static DataGridView AddDatas(DataGridView dataGridView1)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM expensetracker";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowCount;
                    try
                    {
                        rowCount = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch
                    {
                        return null;
                    }
                    if (rowCount > 0)
                    {

                        query = "SELECT * FROM expensetracker";
                        using (MySqlCommand innerCommand = new MySqlCommand(query, connection))
                        {
                            using (MySqlDataReader reader = innerCommand.ExecuteReader())
                            {
                                dataGridView1.Columns.Clear();
                                dataGridView1.Rows.Clear();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataGridView1.Columns.Add(reader.GetName(i), reader.GetName(i));
                                }
                                while (reader.Read())
                                {
                                    object[] values = new object[reader.FieldCount];
                                    reader.GetValues(values);
                                    dataGridView1.Rows.Add(values);
                                    Expense expense1 = new Expense
                                    {
                                        id = int.Parse(values[0] + ""),
                                        category = values[1] + "",
                                        amount = values[2] + "",
                                        date = values[3] + "",
                                        expensename = values[4] + "",
                                    };
                                    Expense.Values1.Add(expense1);
                                    DateTime d = Convert.ToDateTime(expense1.date);
                                    string s = expense1.category + d.Month + d.Year;
                                    if (Expense.dict.ContainsKey(s)) Expense.dict[s] += int.Parse(expense1.amount);
                                    else Expense.dict.Add(s, int.Parse(expense1.amount));
                                }
                            }
                        }
                    }
                }
            }
            return dataGridView1;
        }
        public static DataGridView ModifyBudgetcat(DataGridView dataGridView2,string s)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "delete from Budget where category=@c";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@c", s);
                }
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["category"].Value.ToString() == s )
                {
                    dataGridView2.Rows.Remove(row);
                }
            }
            int x = dict.Count;
            for(int i=0;i<x;i++)
            {
                string sss = dict.ElementAt(i).Key;
                if(sss.Contains(s))
                {
                    budg.Remove(sss);
                    x--;
                }
            }
            return dataGridView2;
        }
        public static DataGridView AlterBudg(DataGridView dataGridView2,string category,string old)
        {
            using (MySqlConnection connection1 = new MySqlConnection(connectionstring))
            {
                connection1.Open();
                string query = "update Budget set category=@c where category=@o";
                using (MySqlCommand command = new MySqlCommand(query, connection1))
                {
                    command.Parameters.AddWithValue("@c", category);
                    command.Parameters.AddWithValue("@o", old);
                    command.ExecuteNonQuery();
                }
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["Category"].Value.ToString() == old)
                {
                    row.Cells["Category"].Value = category;
                    break;
                }
            }

            return dataGridView2;
        }
        public static DataGridView AddBudgDatas(DataGridView dataGridView2)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Budget";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowCount;
                    try
                    {
                        rowCount = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch
                    {
                        return null;
                    }
                    if (rowCount > 0)
                    {
                        query = "SELECT * FROM Budget";
                        using (MySqlCommand innerCommand = new MySqlCommand(query, connection))
                        {
                            using (MySqlDataReader reader = innerCommand.ExecuteReader())
                            {
                                dataGridView2.Columns.Clear();
                                dataGridView2.Rows.Clear();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataGridView2.Columns.Add(reader.GetName(i), reader.GetName(i));
                                }
                                while (reader.Read())
                                {
                                    object[] values = new object[reader.FieldCount];
                                    reader.GetValues(values);
                                    dataGridView2.Rows.Add(values);
                                    string s = values[0]+"" + values[1] + values[2] + "";
                                    if (budg.ContainsKey(s)) budg[s] += int.Parse(values[3]+"");
                                    else budg.Add(s, int.Parse(values[3] + ""));
                                }
                            }
                        }
                    }
                }
            }

        return dataGridView2;
        }

    }
}
