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
    public partial class Form1 : Form {
        public const double FPS = 60.0;

        private readonly FPSTimer fpsTimer;
        private readonly SimpleFPSMeasurer fpsMeasurer;
        private readonly World w;
        private readonly WorldScope scope;
        private int frameCounter = 0;
        private double fps = Double.NaN;
        private double focusSpeed = 8.0;
        private double zoomSpeed = 2.0;

        public Form1()
        {
            InitializeComponent();

            int width = pictureBox.ClientRectangle.Width;
            int height = pictureBox.ClientRectangle.Height;
            pictureBox.Image = new Bitmap(width, height);
            
            this.fpsMeasurer = new SimpleFPSMeasurer();
            this.w = new World(64);
            this.scope = new WorldScope(w);

            this.fpsTimer.Start();
        }

        public void frameTimer_Tick(double time)
        {
            if (Keyboard.IsKeyDown(Key.Up)) {
                scope.CenterY += focusSpeed / FPS;
            }
            if (Keyboard.IsKeyDown(Key.Down)) {
                scope.CenterY -= focusSpeed / FPS;
            }
            if (Keyboard.IsKeyDown(Key.Left)) {
                scope.CenterX -= focusSpeed / FPS;
            }
            if (Keyboard.IsKeyDown(Key.Right)) {
                scope.CenterX += focusSpeed / FPS;
            }
            if (Keyboard.IsKeyDown(Key.Z)) {
                scope.ZoomLevel += zoomSpeed / FPS;
            }
            if (Keyboard.IsKeyDown(Key.X)) {
                scope.ZoomLevel -= zoomSpeed / FPS;
            }
            if (Keyboard.IsKeyDown(Key.A)) {
                scope.RotDeg += 180 / FPS;
            }
            if (Keyboard.IsKeyDown(Key.S)) {
                scope.RotDeg -= 180 / FPS;
            }
            if (Keyboard.IsKeyDown(Key.R)) {
                w.InitRandom(0.1);
            }
            w.Update();

            pictureBox.Refresh();
            frameCounter++;
            if (frameCounter % (int)FPS == 0) fps = fpsMeasurer.Measure();
            fpsMeasurer.Tick();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            scope.Paint(g, this.Height, this.Width);

            var font = new Font("MS UI Gothic", 11);
            g.DrawString($"{fps:0.00} fps",
                font, Brushes.Cyan, 10, 10);
            g.DrawString($"center: ({scope.CenterX:0.00},{scope.CenterY:0.00})",
                font, Brushes.Cyan, 10, 25);
            g.DrawString($"zoom level: {scope.ZoomLevel:0.00}",
                font, Brushes.Cyan, 10, 40);
            g.DrawString($"scale: {scope.Scale:0.00}",
                font, Brushes.Cyan, 10, 55);
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
