﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace townWinForm
{

    public partial class MainForm : Form
    {
        Human clickedHuman = null;
        private Town town;

        private System.Windows.Forms.Timer animationTimer;
        private long lastTime;

        private SettingsForm settingsForm;
        private LogForm logForm;

        Vk vkapi;

        public MainForm()
        {
            vkapi = new Vk();
            InitializeComponent();
            DoubleBuffered = true;

            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;

            town = new Town();

            //Forms
            settingsForm = new SettingsForm();

            //Timer
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 1000 / Config.FPS;
            animationTimer.Enabled = true;
            animationTimer.Tick += AnimationTimer_Tick;
            lastTime = DateTime.Now.Ticks;
        }

        private void update(int dt)
        {
            Util.Move(MousePosition, Width, Height, dt);
            town.Update(dt);
        }

        private void pause()
        {
            animationTimer.Enabled = false;
        }

        private void unPause()
        {
            lastTime = DateTime.Now.Ticks;
            animationTimer.Enabled = true;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int dt = (int)((DateTime.Now.Ticks - lastTime) / 10000);
            lastTime = DateTime.Now.Ticks;

            update(dt);
            Refresh();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pause();
            settingsForm.UpdateInputsFromConfig();

            settingsForm.ShowDialog();

            settingsForm.ErrorLabel.Visible = false;
            unPause();
        }

        private void draw(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.White);
            town.Draw(e.Graphics);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    {
                        Close();
                        break;
                    }

                case Keys.Space:
                    {
                        Config.dx = 0;
                        Config.dy = 24;
                        break;
                    }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Util.UpdateCamera += Building.UpdateD;
            Util.UpdateCamera += Human.UpdateD;
            Util.UpdateCamera += Town.UpdateD;
            //Util.UpdateCamera += Tile.UpdateD;
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            Human h = town.IsMouseOnHuman(e.Location - new Size((int)Config.dx, (int)Config.dy));

            if ((h == clickedHuman) && (h != null))
            {
                clickedHuman.IsClicked = false;
                clickedHuman = null;
                return;
            }

            if (h != null)
            {
                if (clickedHuman != null)
                clickedHuman.IsClicked = false;
                h.IsClicked = true;
                clickedHuman = h;
                town.Citizens.Add(h);
                town.Citizens.Remove(h);
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logForm = new LogForm();
            Log.UpdateEvent += logForm.Update;
            logForm.Show();
            logForm.TopMost = true;
        }
    }
}
