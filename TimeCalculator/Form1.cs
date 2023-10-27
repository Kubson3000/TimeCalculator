using MySqlConnector;

namespace TimeCalculator
{
    public partial class Form1 : Form
    {
        private string input = string.Empty;
        private string secret = "SECRET";
        MySqlConnection conn;
        DateTime startTime;
        private TimeSpan elapsed = TimeSpan.Zero;
        TimeSpan min_time = new TimeSpan(7, 0, 0);
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int user_id = 1;
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            if (user_id == -1)
            {
                start_button.Enabled = false;
                stop_button.Enabled = false;
            }
            progressBar1.Maximum = 8 * 60 * 60;
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer1_Tick);

            conn = new MySqlConnection("Server=172.25.254.125;User ID=root;Password=toor;Database=evidence");
        }
        List<object[]> ExecuteCommandSync(MySqlConnection connection, string sqlCommand)
        {
            var results = new List<object[]>();
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
            connection.Close();
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
            if (totalElapsedSeconds >= progressBar1.Maximum)
            {
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum;
                textBox1.Text = "Work finnished :D";
            }
            else
            {
                progressBar1.Value = (int)totalElapsedSeconds;
                var timeLeft = TimeSpan.FromSeconds(progressBar1.Maximum) - TimeSpan.FromSeconds(totalElapsedSeconds);
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

        private void start_button_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.TimeOfDay > min_time)
            {
                textBox1.Text = ("Zaczêto odliczaæ czas :)");
                DateTime now = DateTime.Now;
                startTime = DateTime.Now;
                string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                timer.Start();
                ExecuteCommandSync(conn, "insert into useractivity(UserID, LoginTime) values (1,\"" + formattedTime + "\")");
                return;
            }
            else
            {
                textBox1.Text = ("Jeszcze nie mo¿na zacz¹æ oliczania czasu :(");
            }
            textBox1.Text = "";
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            timer.Stop();
            elapsed += DateTime.Now - startTime;
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
            ExecuteCommandSync(conn, "insert into useractivity(UserID, LogoutTime) values (1,\"" + formattedTime + "\")");
        }

        private void hide_secret_button_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}