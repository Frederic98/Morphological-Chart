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
    public partial class LoadingScreen : Form
    {
        private bool ec;
        public bool enableContinue
        {
            get
            {
                return ec;
            }
            set
            {
                ec = value;
                buttonContinue.Enabled = value;
            }
        }
        public LoadingScreen(String t = "")
        {
            InitializeComponent();
            BackColor = main.colorBack;
            StartPosition = FormStartPosition.CenterScreen;
            labelStatus.Text = t;
            label1.ForeColor = main.colorFore;

            /*
            buttonCancel.BackColor = main.colorFore;                //Orange buttons
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.FlatAppearance.BorderColor = main.colorBorder;
            buttonCancel.FlatAppearance.MouseDownBackColor = main.colorClick;
            buttonCancel.FlatAppearance.MouseOverBackColor = main.colorHover;
            buttonCancel.FlatAppearance.BorderSize = 3;

            buttonContinue.BackColor = main.colorFore;                //Orange buttons
            buttonContinue.FlatStyle = FlatStyle.Flat;
            buttonContinue.FlatAppearance.BorderColor = main.colorBorder;
            buttonContinue.FlatAppearance.MouseDownBackColor = main.colorClick;
            buttonContinue.FlatAppearance.MouseOverBackColor = main.colorHover;
            buttonContinue.FlatAppearance.BorderSize = 3;

            buttonNew.BackColor = main.colorFore;                //Orange buttons
            buttonNew.FlatStyle = FlatStyle.Flat;
            buttonNew.FlatAppearance.BorderColor = main.colorBorder;
            buttonNew.FlatAppearance.MouseDownBackColor = main.colorClick;
            buttonNew.FlatAppearance.MouseOverBackColor = main.colorHover;
            buttonNew.FlatAppearance.BorderSize = 3;

            buttonOpen.BackColor = main.colorFore;                //Orange buttons
            buttonOpen.FlatStyle = FlatStyle.Flat;
            buttonOpen.FlatAppearance.BorderColor = main.colorBorder;
            buttonOpen.FlatAppearance.MouseDownBackColor = main.colorClick;
            buttonOpen.FlatAppearance.MouseOverBackColor = main.colorHover;
            buttonOpen.FlatAppearance.BorderSize = 3;
            */
            comboBox1.BackColor = main.colorFore;
        }

        public void setStatus(String t = null)
        {
            labelStatus.Text = t;
            if (String.IsNullOrWhiteSpace(t))
            {
                buttonCancel.Text = "Update";
                bool projectIndexOk = comboBox1.SelectedIndex >= 0 && comboBox1.SelectedIndex < comboBox1.Items.Count;
                buttonContinue.Enabled = enableContinue && projectIndexOk;
                buttonOpen.Enabled = projectIndexOk;
                buttonNew.Enabled = projectIndexOk;
                comboBox1.Enabled = true;
            }
            else
            {
                buttonCancel.Text = "Cancel update";
                buttonContinue.Enabled = false;
                buttonOpen.Enabled = false;
                buttonNew.Enabled = false;
                comboBox1.Enabled = false;
            }
        }
    }
}
