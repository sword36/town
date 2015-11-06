﻿using System;
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
        public LogForm()
        {
            InitializeComponent();

            comboBox.Text = "All";
            comboBox.Items.Add("All");
            comboBox.Items.Add("Buildings");
            comboBox.Items.Add("Citizens");
            comboBox.Items.Add("Things");
            comboBox.Items.Add("Path");
            comboBox.Items.Add("Other");

            textBox.Text = Log.Print("all");
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = comboBox.Text;
            textBox.Text = Log.Print(type);
        }
    }
}
