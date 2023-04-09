using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class RegistrationForm : Form
    {
        DBConnect db = new DBConnect();
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string email = textBox2.Text;
            string password = textBox3.Text;
            if (username.Trim().Length == 0 || email.Trim().Length == 0 || password.Trim().Length == 0)
            {
                MessageBox.Show("Поля не должны быть пустыми", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime freeAccessEnd = DateTime.Today;
            string addNewUser = $"insert into Users (username, email, password, idRole, subscriptionEndDate) values ('{username}', '{email}', '{password}', 1, {freeAccessEnd.AddDays(7).ToString("yyyy-MM-dd")})";
            SqlCommand cmd = new SqlCommand(addNewUser, db.getConnection());
            db.openConnection();
            cmd.ExecuteNonQuery();
            db.closeConnection();
            MessageBox.Show("Вы зарегистрировались", "Success", MessageBoxButtons.OK);
            this.Close();
            new IndexForm().Show();
        }
    }
}
