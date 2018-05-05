using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace psyche
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            // メインループにフレーム毎の描画処理を付け加える
            var mainForm = new Form1();
            mainForm.Show();

            var fpsTimer = new FPSTimer(Form1.FPS);
            fpsTimer.Start();

            while (mainForm.Created) {
                Application.DoEvents();

                mainForm.

                fpsTimer.WaitSurplus();
            }
        }
    }
}
