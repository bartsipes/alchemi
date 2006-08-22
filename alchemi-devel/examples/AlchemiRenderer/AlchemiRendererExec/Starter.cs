using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Alchemi.Examples.Renderer
{
    class Starter
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.Run(new MoviePreview());
            Application.Run(new RendererForm());
        }

    }
}
