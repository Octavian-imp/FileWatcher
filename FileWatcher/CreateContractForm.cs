using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class CreateContractForm : Form
    {
        DBConnect db = new DBConnect();
        private string fileName = string.Empty;
        public CreateContractForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.SafeFileName;
                FileInfo file = new FileInfo(ofd.FileName);
                file.CopyTo($"{User.rootPath}\\{User.Username}\\{ofd.SafeFileName}", true);
                MessageBox.Show($"Файл {fileName} загружен");
            }
        }

        private void CreateContractForm_Load(object sender, EventArgs e)
        {
            string fillActionsComboQuery = $"select Name from Actions order by id asc";
            SqlCommand command = new SqlCommand(fillActionsComboQuery, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            reader.Close();
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double amount;
            int idAction = comboBox1.SelectedIndex + 1;
            DateTime dateEnd = DateTime.Parse(dateTimePicker1.Value.ToString());
            if (fileName == string.Empty)
            {
                MessageBox.Show("Ошибка! Файл не указан или произошла ошибка при чтении, попробуйте еще раз", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Double.TryParse(textBox1.Text, out amount))
            {
                string filePath = $@"{User.rootPath}\{User.Username}\{fileName}";
                string addNewContract = $"insert into Contracts (path, idAction, amount, dateEnd, idOwner) values (@filePath, @idAction, @amount, @dateEnd, @idUser)";
                SqlCommand addNewContractCommand = new SqlCommand(addNewContract, db.getConnection());
                addNewContractCommand.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@filePath", filePath),
                    new SqlParameter("@idAction",idAction),
                    new SqlParameter("@amount",amount),
                    new SqlParameter("@dateEnd", dateEnd.ToString("yyyy-MM-dd")),
                    new SqlParameter("@idUser",User.IdUser),
                });
                db.openConnection();
                addNewContractCommand.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show($"Заявка создана, ожидайте ответа. \n Дата окончания договора: {dateTimePicker1.Value.ToString("yyyy-MM-dd")}", "Success", MessageBoxButtons.OK);
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка! Поле 'сумма' содержит недопустимые символы", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
