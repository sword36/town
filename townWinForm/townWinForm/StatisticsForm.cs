using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace townWinForm
{
    public partial class StatisticsForm : Form
    {
        public StatisticsForm()
        {
            InitializeComponent();
        }

        private MainForm mainForm;

        public delegate void statsUpdateHandler();
        public event statsUpdateHandler UpdateEvent;

        public StatisticsForm(MainForm main)
        {
            InitializeComponent();

            mainForm = main;
            
        }

        private void Statistics_Load(object sender, EventArgs e)
        {

        }

        public void UpdateStats()
        {
            if (this.Visible == true)
            {
                
            }
        }
    }
}
