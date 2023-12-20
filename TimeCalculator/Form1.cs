// # Copyright (C) 2023 Jakub Mońka
using MySqlConnector;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using Octokit;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;

namespace TimeCalculator
{
    public partial class Form1 : Form
    {
        public class ActionData
        {
            public string UserID { get; set; }
            public string LoginTime { get; set; }
            public string LogoutTime { get; set; }
        }
        public class Configuration
        {
            public string UserID { get; set; }
            public bool memesEnabled { get; set; }
        }
        private string input = string.Empty;
        private string secret = "SECRET";
        string fullDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Timer");
        MySqlConnection conn;
        DateTime startTime;
        private TimeSpan elapsed = TimeSpan.Zero;
        TimeSpan min_time = new TimeSpan(7, 0, 0);
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer sync_timer = new();
        System.Windows.Forms.Timer ping_timer = new();
        int user_id = -1;
        Configuration conf = new();
        public Form1()
        {
            ping(null, null);
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }
            InitializeComponent();
            this.FormClosed += update_logout_time;
            this.KeyPreview = true;
            string fullFilePath = Path.Combine(fullDirectoryPath, "conf.json");

            if (File.Exists(fullFilePath))
            {
                string json = File.ReadAllText(fullFilePath);
                Configuration data = JsonConvert.DeserializeObject<Configuration>(json);
                user_id = int.Parse(data.UserID);
                conf.UserID = data.UserID;
                if (data.memesEnabled == null)
                {
                    conf.memesEnabled = false;
                    memes_checkbox.Enabled = false;
                }
                else if (data.memesEnabled == true) memes_checkbox.Checked = true;
            }
            if (user_id == -1)
            {
                start_button.Enabled = false;
                stop_button.Enabled = false;
            }
            else
            {
                sync_timer.Tick += new EventHandler(auto_sync_files);
                sync_timer.Interval = 30 * 60 * 1000;
                sync_timer.Start();
                ping_timer.Tick += new EventHandler(ping);
                ping_timer.Interval = 30 * 1000;
                ping_timer.Start();
                if (min_time <= DateTime.Now.TimeOfDay)
                {
                    start_button.Enabled = true;
                }
            }
            progressBar1.Maximum = 8 * 60 * 60;
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer1_Tick);
            update_progressbar();
            conn = new MySqlConnection("Server=10.144.0.1;User ID=root;Password=toor;Database=evidence");
            upload_version();
        }
        void update_logout_time(object sender, FormClosedEventArgs e)
        {
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
            ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (" + user_id + ",\"" + formattedTime + "\")", 0);
            ActionData data = new ActionData
            {
                UserID = user_id.ToString(),
                LogoutTime = formattedTime
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string fullFilePath = Path.Combine(fullDirectoryPath, formattedTime.Replace(":", ".") + ".json");
            File.WriteAllText(fullFilePath, json);
            //Process.Start("cscript", "\"run_hidden.vbs\"");
        }
        List<object[]> ExecuteCommandSync(MySqlConnection connection, string sqlCommand, int error)
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
                if (error > 0)
                {
                    MessageBox.Show("Woopsie. Nie mo¿a po³¹czyæ siê z serverem, skontatuj siê z lokalnym informatykiem.");
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return results;
        }
        private void PrintResults(List<object[]> results)
        {
            foreach (var row in results)
            {
                foreach (var item in row)
                {
                    textBox1.Text += (item.ToString() + " ");
                }
                textBox1.Text += "\n";
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            void resync()
            {
                if (progressBar1.Value % 300 == 0) stop_button.PerformClick();
                if (progressBar1.Value > 25000) return;
                if (progressBar1.Value % 1000 != 0) return;
                var directory = new DirectoryInfo(fullDirectoryPath);
                var earliest_file = directory.GetFiles("*.json").OrderBy(f => f.CreationTime).FirstOrDefault();
                if (earliest_file == null) return;
                var time_diff = DateTime.Now - earliest_file.CreationTime;
                if (time_diff.TotalSeconds > progressBar1.Maximum) return;
                progressBar1.Value = ((int)time_diff.TotalSeconds);

            }
            var totalElapsedSeconds = elapsed.TotalSeconds + (DateTime.Now - startTime).TotalSeconds;
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum;
                DateTime now = DateTime.Now;
                string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (" + user_id + ",\"" + formattedTime + "\")", 0);
                ActionData data = new ActionData
                {
                    UserID = user_id.ToString(),
                    LogoutTime = formattedTime
                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string fullFilePath = Path.Combine(fullDirectoryPath, formattedTime.Replace(":", ".") + ".json");
                File.WriteAllText(fullFilePath, json);
                textBox1.Text = "Work finnished :D";
                MessageBox.Show("Work time has been saved and you can leave :)");
            }
            else
            {
                progressBar1.Value++;
                resync();
                var timeLeft = TimeSpan.FromSeconds(progressBar1.Maximum) - TimeSpan.FromSeconds(progressBar1.Value);
                textBox1.Text = "Time left: " + timeLeft.ToString(@"hh\:mm\:ss");
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            input += e.KeyCode.ToString();
            if (input.Length > secret.Length)
            {
                input = input.Substring(input.Length - secret.Length);
            }
            if (input == secret)
            {
                textBox1.Text = "hewwo ^^";
                panel1.Visible = true;
            }
        }
        private void update_progressbar()
        {
            string[] files = Directory.GetFiles(fullDirectoryPath, "*.json");
            DateTime earliestDate = DateTime.Now;
            DateTime earliestDateCopy = earliestDate;
            DateTime today = DateTime.Today;

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime.Date != today)
                    continue;

                string content = File.ReadAllText(file);
                ActionData session = JsonConvert.DeserializeObject<ActionData>(content);
                if (session.LoginTime != null)
                {
                    if (DateTime.Parse(session.LoginTime) < earliestDate)
                    {
                        earliestDate = DateTime.Parse(session.LoginTime);
                    }
                }
            }
            TimeSpan deltaTime = DateTime.Now - earliestDate;
            double progressInSeconds = deltaTime.TotalSeconds;
            if (earliestDate != earliestDateCopy && user_id != -1)
            {
                timer.Start();
                start_button.Enabled = false;
                stop_button.Enabled = true;
                if ((int)progressInSeconds < progressBar1.Maximum)
                {
                    progressBar1.Value = (int)progressInSeconds;
                }
                else
                {
                    progressBar1.Value = progressBar1.Maximum - 1;
                }
            }
        }
        public void ping(object sender, EventArgs e)
        {
            Ping pingSender = new Ping();
            string server = "10.144.0.1";
            int numberOfPings = 10;

            for (int i = 0; i < numberOfPings; i++)
            {
                pingSender.Send(server);
            }
        }
        private void upload_version()
        {
            if (user_id == -1) return;
            string sql = "update versionlist set AppVersion = \"" + version_label.Text + "\" where UserID = " + user_id;
            ExecuteCommandSync(conn, sql, 0);
        }
        private void start_button_Click(object sender, EventArgs e)
        {
            void memez()
            {
                if (!conf.memesEnabled) return;
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        Monday monday = new();
                        monday.Show();
                        break;
                    case DayOfWeek.Tuesday:
                        Jacek tuesday = new();
                        tuesday.Show();
                        break;
                    case DayOfWeek.Wednesday:
                        Wednesday wednesday = new();
                        wednesday.Show();
                        break;
                    case DayOfWeek.Thursday:
                        Thursday thursday = new();
                        thursday.Show();
                        break;
                    case DayOfWeek.Friday:
                        Friday friday = new();
                        friday.Show();
                        break;
                    case DayOfWeek.Saturday:
                        break;
                    case DayOfWeek.Sunday:
                        break;
                    default:
                        break;
                }
            }
            if (user_id == -1)
            {
                MessageBox.Show("Nie wiem jak to zrobi³e/a ale OK, zawo³aj KUBA1 i nie zamykaj tego okienka!");
                return;
            }
            if (DateTime.Now.TimeOfDay > min_time)
            {
                textBox1.Text = ("Zaczêto odliczaæ czas :)");
                DateTime now = DateTime.Now;
                startTime = DateTime.Now;
                string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                timer.Start();
                start_button.Enabled = false;
                stop_button.Enabled = true;
                Thread newThread = new Thread(() =>
                {
                    ExecuteCommandSync(conn, "insert into useractivity(UserID, LoginTime) values (" + user_id + ",\"" + formattedTime + "\")", 0);
                });
                newThread.Start();
                ActionData data = new ActionData
                {
                    UserID = user_id.ToString(),
                    LoginTime = formattedTime
                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string fullFilePath = Path.Combine(fullDirectoryPath, formattedTime.Replace(":", ".") + ".json");
                File.WriteAllText(fullFilePath, json);
                memez();
            }
            else
            {
                textBox1.Text = ("Jeszcze nie mo¿na zacz¹æ oliczania czasu :(");
            }
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            //elapsed += DateTime.Now - startTime;
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
            Thread newThread = new Thread(() =>
            {
                ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (" + user_id + ",\"" + formattedTime + "\")", 0);
            });
            newThread.Start();
            ActionData data = new ActionData
            {
                UserID = user_id.ToString(),
                LogoutTime = formattedTime
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string fullFilePath = Path.Combine(fullDirectoryPath, formattedTime.Replace(":", ".") + ".json");
            File.WriteAllText(fullFilePath, json);
        }

        private void hide_secret_button_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            string sql = "SELECT UserID AS \"exists\" FROM users WHERE Username = \"" + username_input.Text + "\" AND `Password` = \"" + password_input.Text + "\"";
            List<Object[]> res = ExecuteCommandSync(conn, sql, 0);
            if (res.Count > 0)
            {
                var temp = res[0][0];
                user_id = (int)temp;
                start_button.Enabled = true;
                conf.UserID = temp.ToString();
                ActionData data = new ActionData
                {
                    UserID = temp.ToString()

                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string fullFilePath = Path.Combine(fullDirectoryPath, "conf.json");
                File.WriteAllText(fullFilePath, json);
                start_button.Enabled = true;
                MessageBox.Show("Logged in :D");
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
        }

        private void reset_login_button_Click(object sender, EventArgs e)
        {
            string fullFilePath = Path.Combine(fullDirectoryPath, "conf.json");

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
        }

        private void show_dir_button_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", fullDirectoryPath);
        }
        private void auto_sync_files(object sender, EventArgs e)
        {
            Thread newThread = new Thread(() =>
            {
                ActionData get_from_file(string path)
                {
                    string json = File.ReadAllText(path);
                    ActionData data = JsonConvert.DeserializeObject<ActionData>(json);
                    return data;
                }
                bool already_uploaded(ActionData ad)
                {
                    string sql = "SELECT * FROM useractivity WHERE UserID = " + ad.UserID + " AND(LoginTime = \"" + ad.LoginTime + "\" OR LogoutTime = \"" + ad.LogoutTime + "\") AND(LoginTime IS NULL OR LogoutTime IS NULL)";
                    List<object[]> sql_response = ExecuteCommandSync(conn, sql, 0);
                    if (sql_response.Count > 0) return true;
                    return false;
                }
                List<FileInfo> files = new();
                DirectoryInfo dir = new DirectoryInfo(fullDirectoryPath);
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Name.Equals("conf.json")) continue;
                    files.Add(file);
                }
                for (int i = 0; i < files.Count; i++)
                {
                    ActionData data = get_from_file(files[i].FullName);
                    if (already_uploaded(data)) continue;
                    if (data.LoginTime != null)
                    {
                        Thread newThread = new Thread(() =>
                        {
                            ExecuteCommandSync(conn, "insert into useractivity(UserID, LoginTime) values (" + data.UserID + ",\"" + data.LoginTime + "\")", 0);
                        });
                        newThread.Start();
                    }
                    else
                    {
                        Thread newThread = new Thread(() =>
                        {
                            ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (" + data.UserID + ",\"" + data.LogoutTime + "\")", 0);
                        });
                        newThread.Start();
                    }
                }
            });
            newThread.Start();
        }

        private void sync_button_Click(object sender, EventArgs e)
        {
            FileSync j = new();
            j.Show();
        }

        private void open_ping_button_Click(object sender, EventArgs e)
        {
            Pinger j = new Pinger();
            j.Show();
        }

        private void memes_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (conf.memesEnabled) conf.memesEnabled = false;
            else conf.memesEnabled = true;
            string json = JsonConvert.SerializeObject(conf, Formatting.Indented);
            File.WriteAllText(fullDirectoryPath + "/conf.json", json);
        }
    }
}
