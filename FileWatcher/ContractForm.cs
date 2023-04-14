using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class ContractForm : Form
    {
        DBConnect db = new DBConnect();
        public int idContract;
        public ContractForm()
        {
            InitializeComponent();
            this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void ContractsForm_Load(object sender, EventArgs e)
        {
            //таблица истории этапов
            string stagesQuery = $"SELECT Stages.Name, createDate FROM HistoryStages INNER JOIN Stages ON HistoryStages.idStage = Stages.Id WHERE HistoryStages.idContract = @idContract";
            SqlCommand stagesCmd = new SqlCommand(stagesQuery, db.getConnection());
            stagesCmd.Parameters.AddWithValue("@idContract", idContract);
            db.openConnection();
            SqlDataReader stagesReader = stagesCmd.ExecuteReader();
            if (stagesReader.Read())
            {
                stagesReader.Close();
                createColumns(dataGridView1, stagesQuery);
                refreshDataGrid(dataGridView1, stagesQuery);
            }
            stagesReader.Close();
            //таблица истории действий
            string actionsHistoryQuery = $"SELECT idContract as contract, Actions.Name as action, Statuses.Name as status, emailResponsible as responsible, date FROM ActionsHistory INNER JOIN actions ON ActionsHistory.idAction = Actions.id INNER JOIN Statuses ON ActionsHistory.idStatus = Statuses.id where idContract = @idContract";
            SqlCommand actionsHistoryCmd = new SqlCommand(actionsHistoryQuery, db.getConnection());
            actionsHistoryCmd.Parameters.AddWithValue("@idContract", idContract);
            db.openConnection();
            SqlDataReader actionsHistoryReader = actionsHistoryCmd.ExecuteReader();
            if (actionsHistoryReader.Read())
            {
                actionsHistoryReader.Close();
                createColumns(dataGridView2, actionsHistoryQuery);
                refreshDataGrid(dataGridView2, actionsHistoryQuery);
            }
            actionsHistoryReader.Close();
            //информация о контракте
            string contractInfoQuery = $"SELECT contracts.id, createDate, dateEnd, amount, Actions.Name FROM contracts INNER JOIN actions ON contracts.idAction = actions.Id where contracts.id=@idContract ";
            db.openConnection();
            SqlCommand contractInfoCmd = new SqlCommand(contractInfoQuery, db.getConnection());
            contractInfoCmd.Parameters.AddWithValue("@idContract", idContract);
            SqlDataReader contractInfoReader = contractInfoCmd.ExecuteReader();
            if (contractInfoReader.Read())
            {
                contractNumberValue.Text = contractInfoReader.GetInt32(0).ToString();
                createDateValue.Text = contractInfoReader.GetDateTime(1).ToString("dd-MM-yyyy");
                endDateValue.Text = contractInfoReader.GetDateTime(2).ToString("dd-MM-yyyy");
                amountValue.Text = contractInfoReader.GetSqlMoney(3).ToString();
                actionsValue.Text = contractInfoReader.GetString(4);
            }
            contractInfoReader.Close();
            db.closeConnection();
        }
        private void createColumns(DataGridView dgw, string query)
        {
            db.openConnection();
            SqlCommand cmd = new SqlCommand(query, db.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dgw.Columns.Add(reader.GetName(i), reader.GetName(i));
                }
            }
            reader.Close();
            db.closeConnection();
        }
        private void refreshDataGrid(DataGridView dgw, string query)
        {
            dgw.Rows.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand queryCmd = new SqlCommand(query, db.getConnection());
            db.openConnection();
            adapter.SelectCommand = queryCmd;
            adapter.Fill(dt);
            int y = dt.Rows.Count;
            int x = dt.Columns.Count;
            for (int i = 0; i < y; i++)
            {
                dgw.Rows.Add();
                for (int j = 0; j < x; j++)
                {
                    string outValue = string.Empty;
                    DateTime dateValue;
                    if (DateTime.TryParse(dt.Rows[i][j].ToString(), out dateValue))
                    {
                        outValue = dateValue.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        outValue = dt.Rows[i][j].ToString();
                    }
                    dgw[j, i].Value = outValue;
                }
            }
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //refreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(e.RowIndex.ToString() + e.ColumnIndex.ToString());
        }
    }
}
