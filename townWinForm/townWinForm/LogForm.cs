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
    public partial class LogForm : Form
    {
        private string type = "all";
        private townWinForm.MainForm mainForm;
        public LogForm(MainForm main)
        {
            InitializeComponent();

            mainForm = main;
            if (mainForm.IsPause)
            {
                pauseBtn.Text = "Unpause";
            } else
            {
                pauseBtn.Text = "Pause";
            }

            comboBox.Text = "All";
            comboBox.Items.Add("All");
            comboBox.Items.Add("Buildings");
            comboBox.Items.Add("Citizens");
            comboBox.Items.Add("Things");
            comboBox.Items.Add("Path");
            comboBox.Items.Add("Levels");
            comboBox.Items.Add("Other");

            textBox.AppendText(Log.Print(type));
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = comboBox.Text;
            textBox.Text = "";
            textBox.AppendText(Log.Print(type));
        }

        public void UpdateLog()
        {
            if (this.Visible == true)
            {
                textBox.Text = "";
                textBox.AppendText(Log.Print(type));
            }
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (mainForm.IsPause)
            {
                mainForm.UnPause();
                pauseBtn.Text = "Pause";
            } else
            {
                mainForm.Pause();
                pauseBtn.Text = "Unpause";
            }
        }
    }
}
