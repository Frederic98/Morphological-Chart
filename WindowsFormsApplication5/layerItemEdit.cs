using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorphologicalChart
{
    public partial class layerItemEdit : Form
    {
        public String imagePath;
        public String name;
        public String description;
        public int rating;
        public int ratingFunctionability { get; set; }
        public int ratingFeasibility { get; set; }
        public int ratingTime { get; set; }
        public int l1Index { get; set; }
        public int l2Index { get; set; }
        public bool layer2 { get; set; }
        public List<String> pros { get; set; }
        public List<String> cons { get; set; }
        public bool imgChanged { get; set; } = false;
        public bool delete { get; set; } = false;
        public bool closing { get; set; } = false;

        public layerItemEdit(int l1)
        {
            l1Index = l1;
            l2Index = 0;
            layer2 = false;
            layerItemEditInit();
        }
        public layerItemEdit(int l1, int l2)
        {
            l1Index = l1;
            l2Index = l2;
            layer2 = true;
            layerItemEditInit();
        }
        public void layerItemEditInit()
        {
            InitializeComponent();
            BackColor = main.colorBack;
            labelName.ForeColor = main.colorFore;
            labelDescription.ForeColor = main.colorFore;
            rating1Label.ForeColor = main.colorFore;
            rating2Label.ForeColor = main.colorFore;
            rating3Label.ForeColor = main.colorFore;
            ratingLabel.ForeColor = main.colorFore;
            labelPros.ForeColor = main.colorFore;
            labelCons.ForeColor = main.colorFore;
            labelImage.ForeColor = main.colorFore;
            closing = false;

            if (layer2)
            {
                imagePath = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].imgName;
                name = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].Name;
                ratingFunctionability = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].ratingFunctionability;
                ratingFeasibility = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].ratingFeasibility;
                ratingTime = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].ratingTime;
                description = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].description;
                pros = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].pros;
                cons = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].cons;
            }
            else
            {
                imagePath = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].imgName;
                name = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].Name;
                ratingFunctionability = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].ratingFunctionability;
                ratingFeasibility = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].ratingFeasibility;
                ratingTime = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].ratingTime;
                description = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].description;
                pros = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].pros;
                cons = main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].cons;
            }

            Text = Properties.Settings.Default.programName + (name != null && name != "" ? " - " : "") + name;
            textBoxName.Text = name;
            textBoxName.SelectionStart = name.Length;
            textBoxDescription.Text = description;
            rating1.Value = ratingFunctionability;
            rating2.Value = ratingFeasibility;
            rating3.Value = ratingTime;
            rating1.ValueChanged += new EventHandler(rating_change);
            rating2.ValueChanged += new EventHandler(rating_change);
            rating3.ValueChanged += new EventHandler(rating_change);
            rating_change();
            if (pros != null)
            {
                textBoxPros.Text = String.Join("\r\n", pros);
            }
            if (cons != null)
            {
                textBoxCons.Text = String.Join("\r\n", cons);
            }

            if (imagePath != null && imagePath != "")
            {
                using (FileStream fs = new FileStream(main.imgFolder + imagePath, FileMode.Open, FileAccess.Read))
                {
                    imageBox.Image = Image.FromStream(fs);
                    fs.Close();
                }
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            name = textBoxName.Text;
            Text = Properties.Settings.Default.programName + (name != null && name != "" ? " - " : "") + name;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pros = textBoxPros.Text.Replace("\r", String.Empty).Split('\n').ToList();
        }

        private void textBoxCons_TextChanged(object sender, EventArgs e)
        {
            cons = textBoxCons.Text.Replace("\r", String.Empty).Split('\n').ToList();
        }

        private void rating_change(object sender = null, EventArgs e = null)
        {
            ratingFunctionability = rating1.Value;
            ratingFeasibility = rating2.Value;
            ratingTime = rating3.Value;
            rating = ratingFunctionability + ratingFeasibility + ratingTime;
            ratingBar.Value = rating;
            ratingLabel.Text = (rating / 3.0).ToString("Rating - 0.00");
        }

        private void layerItemEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (layer2)
            {
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].imgName = imagePath;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].Name = name;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].description = description;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].ratingFunctionability = ratingFunctionability;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].ratingFeasibility = ratingFeasibility;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].ratingTime = ratingTime;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].pros = pros;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].level2Solutions[l2Index].cons = cons;
            }
            else
            {
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].imgName = imagePath;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].Name = name;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].description = description;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].ratingFunctionability = ratingFunctionability;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].ratingFeasibility = ratingFeasibility;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].ratingTime = ratingTime;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].pros = pros;
                main.roboFunctions[main.activeFunction].subFunctions[main.activeSubfuntion].level1Solutions[l1Index].cons = cons;
            }
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            description = textBoxDescription.Text;
        }
        private void changeImage(object sender, EventArgs e)
        {
            PictureBox pbox = (PictureBox)sender;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (JPG,PNG,GIF)|*.JPG;*.PNG;*.GIF";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                imgChanged = true;
                if (imagePath == null || imagePath == "")
                {
                    imagePath = Path.GetRandomFileName() + Path.GetExtension(fileDialog.FileName);
                }
                bool overwriteOld = false;

                if (File.Exists(main.imgFolder + imagePath))
                {
                    File.Delete(main.imgFolder + imagePath);
                }
                File.Copy(fileDialog.FileName, main.imgFolder + imagePath, overwriteOld);
                using (FileStream fs = new FileStream(main.imgFolder + imagePath, FileMode.Open, FileAccess.Read))
                {
                    pbox.Image = Image.FromStream(fs);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3);
            switch (deleteResult)
            {
                case DialogResult.Yes:
                    delete = true;
                    buttonOk_Click();
                    break;
                case DialogResult.No:
                    buttonOk_Click();
                    break;
            }
        }

        private void layerItemEdit_KeyPress(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                buttonOk_Click();
            }
        }

        private void buttonOk_Click(object sender = null, EventArgs e = null)
        {
            closing = true;
            Close();
        }
    }
}
