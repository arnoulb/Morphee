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

namespace WindowsFormsApp1
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
                dr = MessageBox.Show("Extinction à :" + shutdown.ToString("HH:mm:ss"), "", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    now = System.DateTime.Now;
                    shutdown = now.AddMilliseconds((double)time);
                    timer1.Start();
                    timer2.Start();
                    tmp = shutdown - System.DateTime.Now;
                    label3.Text = "Extinction dans :" + tmp.ToString("hh") + ":" + tmp.ToString("mm") + ":" + tmp.ToString("ss");
                }
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
            this.Show();
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "\n" + "Extinction non programmée";
            this.Text = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
        }
    }
 
}
