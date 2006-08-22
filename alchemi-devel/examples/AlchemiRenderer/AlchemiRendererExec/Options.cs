using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlchemiRenderer
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txPath.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter a valid base path!", "Alchemi Renderer", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            }
            else
            {
                this.Close();
            }
        }
    }
}