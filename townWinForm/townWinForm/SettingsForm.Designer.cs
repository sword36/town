namespace townWinForm
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SettingsOK = new System.Windows.Forms.Button();
            this.maxCitizensLabel = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.maxCitizensInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SettingsOK
            // 
            this.SettingsOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SettingsOK.Location = new System.Drawing.Point(255, 287);
            this.SettingsOK.Name = "SettingsOK";
            this.SettingsOK.Size = new System.Drawing.Size(75, 23);
            this.SettingsOK.TabIndex = 0;
            this.SettingsOK.Text = "OK";
            this.SettingsOK.UseVisualStyleBackColor = true;
            this.SettingsOK.Click += new System.EventHandler(this.SettingsOK_Click);
            // 
            // maxCitizensLabel
            // 
            this.maxCitizensLabel.AutoSize = true;
            this.maxCitizensLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maxCitizensLabel.Location = new System.Drawing.Point(12, 9);
            this.maxCitizensLabel.Name = "maxCitizensLabel";
            this.maxCitizensLabel.Size = new System.Drawing.Size(83, 16);
            this.maxCitizensLabel.TabIndex = 1;
            this.maxCitizensLabel.Text = "Max citizens:";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(12, 268);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(37, 16);
            this.ErrorLabel.TabIndex = 15;
            this.ErrorLabel.Text = "Error";
            this.ErrorLabel.Visible = false;
            // 
            // maxCitizensInput
            // 
            this.maxCitizensInput.Location = new System.Drawing.Point(193, 12);
            this.maxCitizensInput.Name = "maxCitizensInput";
            this.maxCitizensInput.Size = new System.Drawing.Size(137, 20);
            this.maxCitizensInput.TabIndex = 16;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 322);
            this.Controls.Add(this.maxCitizensInput);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.maxCitizensLabel);
            this.Controls.Add(this.SettingsOK);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SettingsOK;
        private System.Windows.Forms.Label maxCitizensLabel;
        public System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.TextBox maxCitizensInput;
    }
}