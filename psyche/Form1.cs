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
        private readonly FPSTimer fpsTimer;
        private readonly SimpleFPSMeasurer fpsMeasurer;
        private int frameCounter = 0;
        private double fps = Double.NaN;

        public Form1()
        {
            InitializeComponent();

            this.fpsTimer = new FPSTimer(frameTimer_Tick, 60, this);
            this.fpsMeasurer = new SimpleFPSMeasurer();

            int width = pictureBox.ClientRectangle.Width;
            int height = pictureBox.ClientRectangle.Height;
            pictureBox.Image = new Bitmap(width, height);
        }

        private void frameTimer_Tick(double time)
        {
            this.pictureBox.Refresh();
            frameCounter++;
            if (frameCounter % 60 == 0) fps = fpsMeasurer.Measure();
            fpsMeasurer.Tick();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            var font = new Font("MS UI Gothic", 16);
            g.DrawString($"{fps:0.00} fps",
                font, Brushes.Cyan, 20, 20);
            font.Dispose();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (playButton.Text == "play") {
                this.fpsTimer.Start();
                playButton.Text = "resume";
            }
            else {
                this.fpsTimer.Stop();
                playButton.Text = "play";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            fpsTimer.Stop();
        }
    }
}
