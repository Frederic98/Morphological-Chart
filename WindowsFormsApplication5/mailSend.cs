using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorphologicalChart
{
    public partial class mailSend : Form
    {
        int dots = 0;
        public mailSend()
        {
            InitializeComponent();
            BackColor = Properties.Settings.Default.colorBack;
            label1.ForeColor = Properties.Settings.Default.colorFront;
            ControlBox = false;
            Text = Properties.Settings.Default.programName;
            Timer t = new Timer();
            t.Interval = 500;
            t.Tick += new EventHandler(animateText);
            t.Start();
        }
        
        private void animateText(object sender, EventArgs e)
        {
            if (dots < 4)
            {
                dots++;
                label1.Text += ".";
            }
            else
            {
                dots = 0;
                label1.Text = "Sending files";
            }
        }
    }
}
