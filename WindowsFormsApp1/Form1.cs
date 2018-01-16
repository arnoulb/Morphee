using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace Timer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal time;
            DialogResult dr;
            System.TimeSpan tmp;
            System.DateTime now;


            time = numericUpDown2.Value * 60;
            time += numericUpDown1.Value * 3600;
            time *= 1000;
            if (!timer1.Enabled)
            {
                timer1.Interval = (time <= 0) ? 1 : (int)time;
                now = System.DateTime.Now;
                shutdown = now.AddMilliseconds((double)time);
                now = System.DateTime.Now;
                shutdown = now.AddMilliseconds((double)time);
                timer1.Start();
                timer2.Start();
                tmp = shutdown - System.DateTime.Now;
                flag_avt = checkBox1.Checked;
                label3.Text = "Extinction dans :" + tmp.ToString("hh") + ":" + tmp.ToString("mm") + ":" + tmp.ToString("ss");
            }
            else
                MessageBox.Show("Extinction déjà programmée");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                timer2.Stop();
                label3.Text = "Extinction non programmée";
                notifyIcon1.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "\n" + "Extinction non programmée";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                string arguments = "-s -f -t 00";
                ProcessStartInfo startinfo = new ProcessStartInfo("shutdown.exe", arguments);
                Process.Start(startinfo);
                timer1.Stop();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            tmp = shutdown - System.DateTime.Now;
            label3.Text = "Extinction dans :" + tmp.ToString("hh") + ":" + tmp.ToString("mm") + ":" + tmp.ToString("ss");
            notifyIcon1.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "\n" + 
                "Extinction dans :" + tmp.ToString("hh") + ":" + tmp.ToString("mm") + ":" + tmp.ToString("ss");
            if (flag_avt && tmp.TotalMinutes <= 15.0)
            {
                notifyIcon1.BalloonTipText = "L'ordinateur va s'éteindre dans " + Math.Round(tmp.TotalMinutes) + " minutes";
                notifyIcon1.ShowBalloonTip(2000);
                flag_avt = false;
            }
        }

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                this.Hide();
            }

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = true;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "\n" + "Extinction non programmée";
            this.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
            this.Hide();
            e.Cancel = true;
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            notifyIcon1.Visible = true;
            Application.Exit();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                timer2.Stop();
                label3.Text = "Extinction non programmée";
                notifyIcon1.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "\n" + "Extinction non programmée";
            }
        }
    }
 
}
