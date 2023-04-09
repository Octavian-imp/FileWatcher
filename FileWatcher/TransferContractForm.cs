using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class TransferContractForm : Form
    {
        DBConnect db = new DBConnect();
        public TransferContractForm()
        {
            InitializeComponent();
        }

        private void TransferContractForm_Load(object sender, EventArgs e)
        {
            string getDepartamentsQuery = $"SELECT name FROM Departaments ORDER BY id ASC";
            SqlCommand departamentsCmd = new SqlCommand(getDepartamentsQuery, db.getConnection());
            db.openConnection();
            SqlDataReader depReader = departamentsCmd.ExecuteReader();
            while (depReader.Read())
            {
                departamentField.Items.Add(depReader.GetString(0));
            }
            depReader.Close();
            string getStagesQuery = $"SELECT name FROM Stages ORDER BY id ASC";
            SqlCommand stagesCmd = new SqlCommand(getStagesQuery, db.getConnection());
            SqlDataReader stagesReader = stagesCmd.ExecuteReader();
            while (stagesReader.Read())
            {
                stageField.Items.Add(stagesReader.GetString(0));
            }
            stagesReader.Close();
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idContract;
            if (int.TryParse(textBox1.Text, out idContract))
            {
                int idDepartament = departamentField.SelectedIndex + 1;
                int idStage = stageField.SelectedIndex + 1;
                string transferContract = $"INSERT INTO HistoryStages (idContract, idStage, idDepartament) VALUES ({idContract}, {idStage}, {idDepartament})";
                SqlCommand transferContractCmd = new SqlCommand(transferContract, db.getConnection());
                db.openConnection();
                transferContractCmd.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show($"Контракт переведен в отдел {departamentField.Text} на этап {stageField.Text}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка! В поле 'id контракта' введено недопустимое значение. Поле должно содержать только числа", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = string.Empty;
            }
        }
    }
}
