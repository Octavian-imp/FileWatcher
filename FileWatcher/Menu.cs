using System;
using System.IO;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class Menu : Form
    {
        FileSystemWatcher watcher = new FileSystemWatcher($@".\watchingFolder\{User.Username}");

        public Menu()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new ExtensionAccess().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            watcher.NotifyFilter = NotifyFilters.Attributes
                | NotifyFilters.CreationTime
                | NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.Security
                | NotifyFilters.Size;
            watcher.Changed += onChanged;
            watcher.Created += onCreated;
            watcher.Deleted += onDeleted;
            watcher.Renamed += onRenamed;

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            if (watcher.IncludeSubdirectories)
            {
                MessageBox.Show("Отслеживание запущено", "", MessageBoxButtons.OK);
            }
            button3.Visible = true;
        }
        private static void onChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            MessageBox.Show($"Changed: {e.FullPath}");
        }
        //TODO: сделать отправку данных о изменении в бд
        private static void onCreated(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show($"Created: {e.FullPath}");
        }
        private static void onDeleted(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show($"Deleted: {e.FullPath}");
        }
        private static void onRenamed(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show($"Renamed:\n New: {e.FullPath}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AllContracts().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new EditAccess().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            watcher.EnableRaisingEvents = false;
            button3.Visible = false;
            MessageBox.Show("Отслеживание выключено", "Success", MessageBoxButtons.OK);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            idUserLabel.Text = User.IdUser.ToString();
            if (User.IsAdmin)
            {
                label4.Visible = true;
                extAccessBtn.Visible = true;
                rolesBtn.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new CreateContractForm().Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new TransferContractForm().Show();
        }
    }
}
