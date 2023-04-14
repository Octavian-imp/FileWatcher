using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class IndexForm : Form
    {
        DBConnect db = new DBConnect();
        public IndexForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = textBox1.Text;
            var passwordUser = textBox2.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string queryString = $"select id, username, password, idRole from users where username=@username and password=@password";
            db.openConnection();
            SqlCommand sqlCommand = new SqlCommand(queryString, db.getConnection());
            sqlCommand.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("@username", loginUser),
                new SqlParameter("@password", passwordUser)
            });
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                User.IdUser = reader.GetInt32(0);
                User.Username = reader.GetString(1);
                User.IsAdmin = reader.GetInt32(3) == 2 ? true : false;
            }
            reader.Close();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                this.Hide();
                if (!Directory.Exists($@".\watchingFolder\{User.Username}"))
                {
                    Directory.CreateDirectory($@".\watchingFolder\{User.Username}");
                }
                new Menu().Show();
            }
            else
            {
                MessageBox.Show("Ошибка", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            db.closeConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@".\watchingFolder"))
            {
                Directory.CreateDirectory(@".\watchingFolder");
            }
            textBox2.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new RegistrationForm().Show();
        }
    }
}
