using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace psyche
{
    public partial class Form1 : Form
    {
        private readonly FPSTimer timer;
        private double time = 0.0;

        public Form1()
        {
            InitializeComponent();

            this.timer = new FPSTimer(frameTimer_Tick, 60);

            int width = pictureBox.ClientRectangle.Width;
            int height = pictureBox.ClientRectangle.Height;
            pictureBox.Image = new Bitmap(width, height);
        }

        private void frameTimer_Tick(double time)
        {
            this.time = time;
            this.Invoke(new Action(() => this.pictureBox.Refresh()));
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            var font = new Font("MS UI Gothic", 20);
            g.DrawString("counter: " + (int)(this.time*60), font, Brushes.Cyan, 20, 20);
            font.Dispose();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (playButton.Text == "play") {
                this.timer.Start();
                playButton.Text = "resume";
            }
            else {
                this.timer.Stop();
                playButton.Text = "play";
            }
        }
    }
}
