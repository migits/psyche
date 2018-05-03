using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace psyche
{
    public partial class Form1 : Form
    {
        private readonly FPSTimer fpsTimer;
        private readonly SimpleFPSMeasurer fpsMeasurer;
        private readonly World w;
        private readonly WorldScope scope;
        private int frameCounter = 0;
        private double fps = Double.NaN;
        private double idealFPS = 60.0;
        private double focusSpeed = 8.0;
        private double zoomSpeed = 2.0;

        public Form1()
        {
            InitializeComponent();

            int width = pictureBox.ClientRectangle.Width;
            int height = pictureBox.ClientRectangle.Height;
            pictureBox.Image = new Bitmap(width, height);

            this.fpsTimer = new FPSTimer(frameTimer_Tick, idealFPS, this);
            this.fpsMeasurer = new SimpleFPSMeasurer();
            this.w = new World(16, 16);
            this.scope = new WorldScope(w);

            this.fpsTimer.Start();
        }

        private void frameTimer_Tick(double time)
        {
            if (Keyboard.IsKeyDown(Key.Up)) {
                scope.CenterY += focusSpeed / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.Down)) {
                scope.CenterY -= focusSpeed / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.Left)) {
                scope.CenterX -= focusSpeed / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.Right)) {
                scope.CenterX += focusSpeed / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.Z)) {
                scope.ZoomLevel += zoomSpeed / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.X)) {
                scope.ZoomLevel -= zoomSpeed / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.R)) {
                scope.RotDeg += 180 / idealFPS;
            }
            if (Keyboard.IsKeyDown(Key.E)) {
                scope.RotDeg -= 180 / idealFPS;
            }
            w.Update();

            pictureBox.Refresh();
            frameCounter++;
            if (frameCounter % (int)idealFPS == 0) fps = fpsMeasurer.Measure();
            fpsMeasurer.Tick();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            scope.Paint(g);

            var font = new Font("MS UI Gothic", 16);
            g.DrawString($"{fps:0.00} fps",
                font, Brushes.Cyan, 10, 10);
            g.DrawString($"center: ({scope.CenterX:0.00},{scope.CenterY:0.00})",
                font, Brushes.Cyan, 10, 30);
            g.DrawString($"zoom level: {scope.ZoomLevel:0.00}",
                font, Brushes.Cyan, 10, 50);
            g.DrawString($"scale: {scope.Scale:0.00}",
                font, Brushes.Cyan, 10, 70);
            font.Dispose();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            // ここキモい
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
