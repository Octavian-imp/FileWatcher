using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class AllContracts : Form
    {
        DBConnect db = new DBConnect();
        public AllContracts()
        {
            InitializeComponent();
        }

        private void AllContracts_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string contractsQuery = $"select id, createDate, dateEnd, idAction from contracts where idOwner = {User.IdUser}";
            SqlCommand cmd = new SqlCommand(contractsQuery, db.getConnection());
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            db.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.GetName(0).Length > 0)
            {
                reader.Close();
                createColumns();
                refreshDataGrid(dataGridView1);
            }
            reader.Close();
        }
        private void createColumns()
        {
            string query = $"select Id, createDate, dateEnd, idAction from contracts where idOwner = {User.IdUser} order by createDate asc";
            db.openConnection();
            SqlCommand cmd = new SqlCommand(query, db.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                dataGridView1.Columns.Add("id", reader.GetName(0));
                dataGridView1.Columns.Add("createDate", reader.GetName(1));
                dataGridView1.Columns.Add("dateEnd", reader.GetName(2));
                dataGridView1.Columns.Add("idAction", reader.GetName(3));
            }
            reader.Close();
            db.closeConnection();
        }
        private void refreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string query = $"select Id, createDate, dateEnd, idAction from contracts where idOwner = {User.IdUser} order by createDate asc";
            SqlCommand cmd = new SqlCommand(query, db.getConnection());
            db.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dgw.Rows.Add(reader.GetInt32(0), reader.GetDateTime(1).ToString("dd/MM/yyyy"), reader.GetDateTime(2).ToString("dd/MM/yyyy"), reader.GetInt32(3));
            }
            reader.Close();
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ContractForm contractForm = new ContractForm();
            contractForm.idContract = Convert.ToInt32(dataGridView1[0, e.RowIndex].Value);
            contractForm.Show();
        }
    }
}
