using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Alchemi.Examples.Renderer
{
    public partial class MoviePreview : Form
    {
        public MoviePreview()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //open a set of files and show the animation
                List<Image> list = new List<Image>();
                string dir = @"D:\Program Files\POV-Ray for Windows v3.6\scenes\animations\boing";
                string[] files = Directory.GetFiles(
                    dir,
                    "bounce*.bmp", SearchOption.TopDirectoryOnly);
                foreach (String filename in files)
                {
                    Image img = Bitmap.FromFile(Path.Combine(dir, filename));
                    list.Add(img);
                }
                Graphics g = movieBox.CreateGraphics();
                foreach (Image img in list)
                {
                    g.DrawImage(img, new Point(0, 0));
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}