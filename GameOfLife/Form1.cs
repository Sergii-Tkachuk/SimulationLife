﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;       //розширення клітинки
        private GameEngine gameEngine;//логіка нашої гри

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            //якщо таймер запущений, ми нічо не можемо змінити
            if (timer1.Enabled) 
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled    = false;
            resolution            = (int)nudResolution.Value;

            gameEngine = new GameEngine
                (
                    rows: pictureBox1.Height / resolution, 
                    columns: pictureBox1.Width / resolution, 
                    density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value
                );

            Text = $"Generation {gameEngine.currentGeneration}";

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics          = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void ContinueGame()
        {
            if (timer1.Enabled)
                return;
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;
            timer1.Start();
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(Color.Black);
            var field = gameEngine.GetCurrentGeneration();

            for(int x = 0; x < field.GetLength(0); ++x)
            {
                for(int y = 0; y < field.GetLength(1); ++y)
                {
                    if(field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }

            pictureBox1.Refresh();//тут картинка оновлюється повністю
            Text = $"Generation {gameEngine.currentGeneration}";
            gameEngine.nextGeneration();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!timer1.Enabled)
            //    return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);
                graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                pictureBox1.Refresh();//тут картинка оновлюється повністю
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.DeleteCell(x, y);
                graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution - 1, resolution - 1);
                pictureBox1.Refresh();//тут картинка оновлюється повністю
            }
        }

        private void bContinue_Click_Click(object sender, EventArgs e)
        {
            ContinueGame();
        }
    }
}
