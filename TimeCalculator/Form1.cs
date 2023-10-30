using MySqlConnector;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Data;

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
        private string input = string.Empty;
        private string secret = "SECRET";
        string fullDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Timer");
        MySqlConnection conn;
        DateTime startTime;
        private TimeSpan elapsed = TimeSpan.Zero;
        TimeSpan min_time = new TimeSpan(7, 0, 0);
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int user_id = -1;
        public Form1()
        {
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }
            InitializeComponent();
            this.KeyPreview = true;
            string fullFilePath = Path.Combine(fullDirectoryPath, "conf.json");

            if (File.Exists(fullFilePath))
            {
                string json = File.ReadAllText(fullFilePath);
                ActionData data = JsonConvert.DeserializeObject<ActionData>(json);
                user_id = int.Parse(data.UserID);
            }
            if (user_id == -1)
            {
                start_button.Enabled = false;
                stop_button.Enabled = false;
            }
            else
            {
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
                MessageBox.Show("Woopsie. Nie mo�a po��czy� si� z serverem, skontatuj si� z lokalnym informatykiem.");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return results;
        }
        public void PrintResults(List<object[]> results)
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
            var totalElapsedSeconds = elapsed.TotalSeconds + (DateTime.Now - startTime).TotalSeconds;
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum;
                textBox1.Text = "Work finnished :D";
            }
            else
            {
                progressBar1.Value++;
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
            progressBar1.Value = (int)progressInSeconds;
            if (earliestDate != earliestDateCopy && user_id != -1)
            {
                timer.Start();
                start_button.Enabled = false;
                stop_button.Enabled = true;
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.TimeOfDay > min_time)
            {
                textBox1.Text = ("Zacz�to odlicza� czas :)");
                DateTime now = DateTime.Now;
                startTime = DateTime.Now;
                string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                timer.Start();
                start_button.Enabled = false;
                stop_button.Enabled = true;
                ExecuteCommandSync(conn, "insert into useractivity(UserID, LoginTime) values (1,\"" + formattedTime + "\")");
                ActionData data = new ActionData
                {
                    UserID = user_id.ToString(),
                    LoginTime = formattedTime
                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string fullFilePath = Path.Combine(fullDirectoryPath, formattedTime.Replace(":", ".") + ".json");
                File.WriteAllText(fullFilePath, json);
            }
            else
            {
                textBox1.Text = ("Jeszcze nie mo�na zacz�� oliczania czasu :(");
            }
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            timer.Stop();
            elapsed += DateTime.Now - startTime;
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
            ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (1,\"" + formattedTime + "\")");
            ActionData data = new ActionData
            {
                UserID = user_id.ToString(),
                LogoutTime = formattedTime
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string fullFilePath = Path.Combine(fullDirectoryPath, formattedTime.Replace(":", ".") + ".json");
            File.WriteAllText(fullFilePath, json);
            start_button.Enabled = true;
        }

        private void hide_secret_button_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            string sql = "SELECT UserID AS \"exists\" FROM users WHERE Username = \"" + username_input.Text + "\" AND `Password` = \"" + password_input.Text + "\"";
            List<Object[]> res = ExecuteCommandSync(conn, sql);
            if (res.Count > 0)
            {
                var temp = res[0][0];
                user_id = (int)temp;
                start_button.Enabled = true;
                ActionData data = new ActionData
                {
                    UserID = temp.ToString(),
                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string fullFilePath = Path.Combine(fullDirectoryPath, "conf.json");
                File.WriteAllText(fullFilePath, json);
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
    }
}