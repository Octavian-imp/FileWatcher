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
                string getCurrentDate = $"select subscriptionEndDate from Users where id={id}";
                SqlCommand cmd = new SqlCommand(getCurrentDate, db.getConnection());
                db.openConnection();
                SqlDataReader reader = cmd.ExecuteReader();
                DateTime newEndDate = reader.GetDateTime(0);
                reader.Close();
                int type = comboBox1.SelectedIndex;
                if (type == 0)
                {
                    newEndDate.AddDays(1);
                }
                else if (type == 1)
                {
                    newEndDate.AddMonths(1);
                }
                else if (type == 2)
                {
                    newEndDate.AddYears(1);
                }
                string extensAccessQuery = $"update Users set subscriptionEndDate = {newEndDate.ToString("yyyy-MM-dd")} where id = {id}";
                SqlCommand sqlCommand = new SqlCommand(extensAccessQuery, db.getConnection());
                sqlCommand.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show("Доступ продлен", "Success", MessageBoxButtons.OK);
            }
        }
    }
}
