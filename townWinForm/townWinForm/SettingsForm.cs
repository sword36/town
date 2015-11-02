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
    public partial class SettingsForm : Form
    {
        private int checkAndGetFromTextBox(TextBox tb, int left, int right, string errMessage)
        {
            int value = int.Parse(tb.Text);
            Util.ValidateInterval(left, right, value, errMessage);
            return value;
        }

        private void updateConfigFromInputs()
        {
            Config.ScrollSpeed = 250 + trackBar1.Value * 50;
            Config.MaxCitizens = checkAndGetFromTextBox(this.maxCitizensInput, 10, 100, "Max citizens error");
        }

        public void UpdateInputsFromConfig()
        {
            ScrollSpeedLabel.Text = "Scroll Speed: " + (250 + trackBar1.Value * 50).ToString();
            maxCitizensInput.Text = Config.MaxCitizens.ToString();
            
        }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsOK_Click(object sender, EventArgs e)
        {
            try
            {
                updateConfigFromInputs();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
                ErrorLabel.Visible = true;
                this.DialogResult = DialogResult.None;
            }
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ScrollSpeedLabel.Text = "Scroll Speed: " + (250 + trackBar1.Value * 50).ToString();
        }
    }
}
