using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeCalculator
{
    public partial class Pinger : Form
    {
        public Pinger()
        {
            InitializeComponent();
        }
        public void press_ping()
        {
            ping_button.PerformClick();
        }

        private async void ping_button_Click(object sender, EventArgs e)
        {
            Ping pingSender = new Ping();
            string server = "10.144.0.1";
            int numberOfPings = 10;

            for (int i = 0; i < numberOfPings; i++)
            {
                PingReply reply = await pingSender.SendPingAsync(server);

                if (reply.Status == IPStatus.Success)
                {
                    textBox1.AppendText("Ping to " + reply.Address.ToString() + " Successful" + " Response delay = " + reply.RoundtripTime.ToString() + " ms" + Environment.NewLine);
                }
                else
                {
                    textBox1.AppendText("Ping failed." + Environment.NewLine);
                }
            }
        }
    }
}
