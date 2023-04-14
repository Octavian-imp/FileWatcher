using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class ExtensionAccess : Form
    {
        DBConnect db = new DBConnect();
        public ExtensionAccess()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(textBox1.Text, out id))
            {
                string getCurrentDate = $"select subscriptionEndDate from Users where Id = @id";
                SqlCommand cmd = new SqlCommand(getCurrentDate, db.getConnection());
                cmd.Parameters.AddWithValue("@id", id);
                db.openConnection();
                SqlDataReader reader = cmd.ExecuteReader();
                DateTime newEndDate;
                if (reader.Read())
                {
                    newEndDate = reader.GetDateTime(0);
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка");
                    return;
                }
                int type = comboBox1.SelectedIndex;
                if (type == 0)
                {
                    newEndDate = newEndDate.AddDays(1);
                }
                else if (type == 1)
                {
                    newEndDate = newEndDate.AddMonths(1);
                }
                else if (type == 2)
                {
                    newEndDate = newEndDate.AddYears(1);
                }
                string extensAccessQuery = $"update Users set subscriptionEndDate = @subrEndDate where id = @id";
                SqlCommand sqlCommand = new SqlCommand(extensAccessQuery, db.getConnection());
                sqlCommand.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@subrEndDate", newEndDate.ToString("yyyy-MM-dd")),
                    new SqlParameter("@id", id)
                });
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    db.closeConnection();
                    MessageBox.Show(ex.Message);
                    return;
                }
                db.closeConnection();
                MessageBox.Show("Доступ продлен", "Success", MessageBoxButtons.OK);
            }
        }
    }
}
