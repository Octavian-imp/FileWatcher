using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class EditAccess : Form
    {
        DBConnect db = new DBConnect();
        public EditAccess()
        {
            InitializeComponent();
        }

        private void EditAccess_Load(object sender, EventArgs e)
        {
            string fillRolesQuery = $"select name from roles order by id asc";
            db.openConnection();
            SqlCommand cmd = new SqlCommand(fillRolesQuery, db.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            reader.Close();
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(textBox1.Text, out id))
            {
                db.openConnection();
                string changeRoleUserQuery = $"update users set idRole=@idRole where id=@id";
                SqlCommand cmd = new SqlCommand(changeRoleUserQuery, db.getConnection());
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@idRole", comboBox1.SelectedIndex + 1),
                    new SqlParameter("@id", id),
                });
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Права пользователя с id {id} изменены на {comboBox1.SelectedItem}", "Success", MessageBoxButtons.OK);
                db.closeConnection();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка! неверный тип данных. Допустимы только числа в поле 'id пользователя'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
