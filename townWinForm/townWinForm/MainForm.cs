using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace townWinForm
{
    public partial class MainForm : Form
    {
        private Town town;

        private Timer animationTimer;
        private long lastTime;

        private Graphics g;
        private Bitmap bitmap;

        private SettingsForm settingsForm;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;

            town = new Town();

            //Forms
            settingsForm = new SettingsForm();

            //Timer
            animationTimer = new Timer();
            animationTimer.Interval = 1000 / Config.FPS;
            animationTimer.Enabled = true;
            animationTimer.Tick += AnimationTimer_Tick;
            lastTime = DateTime.Now.Ticks;

            //Drawing
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = pictureBox.CreateGraphics();
        }

        private void update(int dt)
        {
            Util.Move(MousePosition, Width, Height, dt);
            town.Update(dt);
        }

        private void draw()
        {
            Graphics tempGraphics;

            using (tempGraphics = Graphics.FromImage(bitmap))
            {
                tempGraphics.Clear(Color.White);
                town.Draw(tempGraphics);
            }

            g.DrawImage(bitmap, new Point(0, 0));
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
            e.Graphics.Clear(Color.White);
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
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Util.UpdateCamera += Building.UpdateD;
            Util.UpdateCamera += Human.UpdateD;
            Util.UpdateCamera += Town.UpdateD;
        }
    }
}
