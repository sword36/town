using System;
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

        private bool isPause = false;

        public bool IsPause
        {
            get { return isPause; }
        }
        //Vk vkapi;

        public MainForm()
        {
            //vkapi = new Vk();
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

        public void Pause()
        {
            animationTimer.Enabled = false;
            isPause = true;
        }

        public void UnPause()
        {
            lastTime = DateTime.Now.Ticks;
            animationTimer.Enabled = true;
            isPause = false;
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
            Pause();
            settingsForm.UpdateInputsFromConfig();

            settingsForm.ShowDialog();

            settingsForm.ErrorLabel.Visible = false;
            UnPause();
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

                case Keys.Home:
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

            if ((h == null) && (clickedHuman != null))
            {
                clickedHuman.IsClicked = false;
                clickedHuman = null;
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
            logForm = new LogForm(this);
            Log.UpdateEvent += logForm.UpdateLog;
            logForm.Show();
            logForm.TopMost = true;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)32:
                    {
                        if (clickedHuman != null)
                        {
                            Config.dx = -clickedHuman.Position.X + Width / 2;
                            Config.dy = -clickedHuman.Position.Y + Height / 2;
                        }
                        else
                        {
                            Config.dx = 0;
                            Config.dy = 24;
                            break;
                        }

                        break;
                    }
            }
        }
    }
}
