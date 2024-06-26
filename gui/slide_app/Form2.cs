using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slide_app
{
    public partial class Form2 : Form
    {
        Form1 form1;
        public Form2(Form1 f1)
        {
            form1 = f1;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            AudioVoiceCheck.Checked = true;
            FontTextBox.Enabled = false;
            FontTextBox.Text = 24.ToString();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            form1.AudioVoiceLabel.Text = AudioVoiceCheck.Checked.ToString();
            form1.HosokuLabel.Text = HosokuCheck.Checked.ToString();
            form1.CaptionLabel.Text = CaptionCheck.Checked.ToString();
            form1.FontLabel.Text = FontTextBox.Text;
            form1.CharacterLabel.Text = CharaCheck.Checked.ToString();
            this.Close();
        }

        private void CaptionCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (CaptionCheck.Checked == true)
            {
                FontTextBox.Enabled = true;
            }
            else
            {
                FontTextBox.Enabled = false;
            }
        }

        private void AudioVoiceCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (AudioVoiceCheck.Checked)
            {
                HosokuCheck.Enabled = true;
            }
            else
            {
                HosokuCheck.Checked = false;
                HosokuCheck.Enabled = false;
            }
        }
    }
}
