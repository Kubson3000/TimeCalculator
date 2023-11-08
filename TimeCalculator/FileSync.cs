using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Microsoft.VisualBasic.ApplicationServices;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace TimeCalculator
{
    public partial class FileSync : Form
    {
        string fullDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Timer");
        List<FileInfo> files = new();
        MySqlConnection conn;
        public class ActionData
        {
            public string UserID { get; set; }
            public string LoginTime { get; set; }
            public string LogoutTime { get; set; }
        }
        public FileSync()
        {
            conn = new MySqlConnection("Server=10.144.0.1;User ID=root;Password=toor;Database=evidence");
            InitializeComponent();
            get_files();
            progressBar1.Minimum = 0;
        }

        private void get_files()
        {
            textBox1.Text = "Getting file dirs ready";
            DirectoryInfo dir = new DirectoryInfo(fullDirectoryPath);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.Equals("conf.json")) continue;
                files.Add(file);
            }
            textBox1.Text = "Found " + files.Count.ToString() + " files";
        }
        private ActionData get_from_file(string path)
        {
            string json = File.ReadAllText(path);
            ActionData data = JsonConvert.DeserializeObject<ActionData>(json);
            return data;
        }
        List<object[]> ExecuteCommandSync(MySqlConnection connection, string sqlCommand)
        {
            var results = new List<object[]>();
            try
            {
                connection.Open();
                using (var command = new MySqlCommand(sqlCommand, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rowValues = new object[reader.FieldCount];
                            reader.GetValues(rowValues);
                            results.Add(rowValues);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Woopsie. Nie moża połączyć się z serverem, skontatuj się z lokalnym informatykiem.");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return results;
        }
        private bool already_uploaded(ActionData ad)
        {
            string sql = "SELECT * FROM useractivity WHERE UserID = " + ad.UserID + " AND(LoginTime = \"" + ad.LoginTime + "\" OR LogoutTime = \"" + ad.LogoutTime + "\") AND(LoginTime IS NULL OR LogoutTime IS NULL)";
            List<object[]> sql_response = ExecuteCommandSync(conn, sql);
            if (sql_response.Count > 0) return true;
            return false;
        }
        public void auto_sync()
        {
            all_button.PerformClick();
        }
        private void today_button_Click(object sender, EventArgs e)
        {
            List<FileInfo> filesCreatedToday = files.Where(x => x.CreationTime.Date == DateTime.Today).ToList();
            progressBar1.Maximum = filesCreatedToday.Count;
            progressBar1.Value = progressBar1.Minimum;
            for (int i = 0; i < filesCreatedToday.Count; i++)
            {
                progressBar1.Value++;
                ActionData data = get_from_file(filesCreatedToday[i].FullName);
                textBox1.Text = "Checking: " + filesCreatedToday[i].Name;
                if (already_uploaded(data)) continue;
                textBox1.Text = "Uploading: " + filesCreatedToday[i].Name;
                if (data.LoginTime != null)
                {
                    Thread newThread = new Thread(() =>
                    {
                        ExecuteCommandSync(conn, "insert into useractivity(UserID, LoginTime) values (" + data.UserID + ",\"" + data.LoginTime + "\")");
                    });
                    newThread.Start();
                }
                else
                {
                    Thread newThread = new Thread(() =>
                    {
                        ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (" + data.UserID + ",\"" + data.LogoutTime + "\")");
                    });
                    newThread.Start();
                }
            }
            progressBar1.Value = progressBar1.Maximum;
            textBox1.Text = "Done";
        }

        private void all_button_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = files.Count;
            progressBar1.Value = progressBar1.Minimum;
            for (int i = 0; i < files.Count; i++)
            {
                progressBar1.Value++;
                ActionData data = get_from_file(files[i].FullName);
                textBox1.Text = "Checking: " + files[i].Name;
                if (already_uploaded(data)) continue;
                textBox1.Text = "Uploading: " + files[i].Name;
                if (data.LoginTime != null)
                {
                    Thread newThread = new Thread(() =>
                    {
                        ExecuteCommandSync(conn, "insert into useractivity(UserID, LoginTime) values (" + data.UserID + ",\"" + data.LoginTime + "\")");
                    });
                    newThread.Start();
                }
                else
                {
                    Thread newThread = new Thread(() =>
                    {
                        ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (" + data.UserID + ",\"" + data.LogoutTime + "\")");
                    });
                    newThread.Start();
                }
            }
            progressBar1.Value = progressBar1.Maximum;
            textBox1.Text = "Done";

        }
    }
}
