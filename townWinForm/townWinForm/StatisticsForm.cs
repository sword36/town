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

        public StatisticsForm(MainForm main)
        {
            Location = new Point(600, 100);
            StartPosition = new FormStartPosition();
            InitializeComponent();
            //Location = new Point(600, 100);
            mainForm = main;
            
        }

        private void Statistics_Load(object sender, EventArgs e)
        {

        }

        public void UpdateStats()
        {
            if (this.Visible == true)
            {
                Statistics happinessStat = mainForm.HappinessStat;

                StatsChart.Series["Happiness"].Points.Clear();
                StatsChart.Series["Happiness"].MarkerStyle = MarkerStyle.None;

                for (int i = 0; i < happinessStat.Values.Count; i++)
                {

                    string minutes = (happinessStat.Time[i] / 10 / 60).ToString();
                    string seconds = (happinessStat.Time[i] / 10 % 60).ToString();

                    StatsChart.Series["Happiness"].Points.AddXY(minutes + ":" + seconds, happinessStat.Values[i]);
                    
                }

                
            }
        }
    }
}
