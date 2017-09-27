using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO.Compression;

namespace MorphologicalChart
{
    public partial class main : Form
    {
        public static List<robotFunction> roboFunctions { get; set; } = new List<robotFunction>();
        List<robotProject> robotProjects = new List<robotProject>();

        bool skipNextTextChangeEvent = false;
        public static int activeFunction { get; set; }
        public static int activeSubfuntion { get; set; }
        public static int activeProject { get; set; }

        public static String filesFolder { get; set; } = "res\\";
        public static String imgFolder { get; set; } = "res\\img\\";
        public static String tempFolder = "res\\temp\\";
        String outputFile = "res\\output.xml";
        String outputFileLastSave = "res\\outputLast.xml";
        static String zipExt = "morphchart";
        String outputZip = Environment.MachineName + "_{0}." + zipExt;
        String outputZipAutosave = Environment.MachineName + "_{0}.autosave." + zipExt;

        public static Color colorBack { get; set; } = Color.FromArgb(255, 45, 45, 48);
        public static Color colorFore { get; set; } = Color.FromArgb(255, 239, 124, 4);
        public static Color colorBorder { get; set; } = Color.FromArgb(255, 167, 87, 3);
        public static Color colorHover { get; set; } = Color.FromArgb(255, 200, 110, 4);
        public static Color colorClick { get; set; } = Color.FromArgb(255, 150, 80, 3);

        String pcName = Environment.MachineName;
        layerItemEdit layerItemEditForm;
        Timer timer;

        Timer timerCureBoredom;
        Timer timerCureBoredomStop;
        private bool fullscreenActive;
        private Size windowSize;
        private int[] windowPos = new int[2];
        private int moveScreenFuction = 0;
        private int moveScreenCount = 0;
        private int moveScreen2x = 1;
        private int moveScreen2y = 1;
        private Random random = new Random();
        private DateTime prevCureBoredom = DateTime.Now;

        private LoadingScreen loadingScreen;
        DateTime startTime;
        BackgroundWorker bgWorker = new BackgroundWorker();
        /*
         * ---Things to do:---
         * ? Resize images
         * X Load data from xml into roboFunctions
         * X Autosave
         * M Join data of multiple files
         * M Create PDF
         * X Rating
         * X Pros/Cons
         * X Replace image
         * X Create zip
         * X Auto close popup windows for Jory-people
         * X Change .zip file to .morphchart
         * X Add option "delete" in layerItemEdit
         * X Esc to close layerItemEdit
         * 
         * 
         * Mail password: frc4481MorphChart
         */

        public main()
        {
            InitializeComponent();

            BackColor = Properties.Settings.Default.colorBack;
            addL1Button.BackColor = Properties.Settings.Default.colorFront;
            addL1Button.FlatStyle = FlatStyle.Flat;
            addL1Button.FlatAppearance.BorderColor = colorBorder;
            addL1Button.FlatAppearance.MouseDownBackColor = colorClick;
            addL1Button.FlatAppearance.MouseOverBackColor = colorHover;
            comboBox_functions.BackColor = colorFore;
            comboBox_subFunctions.BackColor = colorFore;
            StartPosition = FormStartPosition.CenterScreen;
            Text = Properties.Settings.Default.programName;
            windowSize = new Size((int)(Screen.FromControl(this).WorkingArea.Size.Width * 0.8), (int)(Screen.FromControl(this).WorkingArea.Size.Height * 0.8));
            Size = windowSize;

            startTime = DateTime.Now;
            loadingScreen = new LoadingScreen();
            loadingScreen.buttonCancel.Click += new EventHandler(bgWorkerCancel);
            loadingScreen.buttonContinue.Click += new EventHandler(projectContinue);
            loadingScreen.buttonOpen.Click += new EventHandler(projectOpen);
            loadingScreen.buttonNew.Click += new EventHandler(projectNew);
            loadingScreen.comboBox1.SelectedIndexChanged += new EventHandler(projectChanged);
            loadingScreen.buttonClose.Click += new EventHandler(loadingScreenClose);
            loadingScreen.setStatus("Checking for updates...");
            loadingScreen.Show();

            bgWorker.DoWork += new DoWorkEventHandler(bgWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(startupWorkDone);
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerAsync(workerSettings);


            Directory.CreateDirectory(imgFolder);
            DirectoryInfo di = new DirectoryInfo(filesFolder);
            di.Attributes |= FileAttributes.Hidden;

            try
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\." + zipExt, "", "Morphchart.file");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\Morphchart.file", "", "Morphological chart file");
                using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\morphchart.ico", FileMode.Create))
                {
                    Properties.Resources.TR_morph_icon_small.Save(fs);
                }
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\Morphchart.file\\DefaultIcon", "", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\morphchart.ico");
            }
            catch { }

            timer = new Timer();
            timer.Tick += new EventHandler(saveXml);
            timer.Interval = Properties.Settings.Default.autosaveTime * 1000;

            timerCureBoredom = new Timer();
            timerCureBoredom.Tick += new EventHandler(moveScreen);
            timerCureBoredom.Interval = 10;
            timerCureBoredom.Enabled = false;

            timerCureBoredomStop = new Timer();
            timerCureBoredomStop.Tick += new EventHandler(stopCureBoredom);
            timerCureBoredomStop.Interval = Properties.Settings.Default.cureBoredomMaxSeconds * 1000;
            timerCureBoredomStop.Enabled = false;

            Opacity = 0;
            Top = Screen.FromControl(this).Bounds.Height + 100;
            //WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        private void projectChanged(object sender, EventArgs e)
        {
            activeProject = loadingScreen.comboBox1.SelectedIndex;
            bgWorker.RunWorkerAsync(workerFunctions);
            loadingScreen.setStatus("Downloading functions");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(outputFile);
                loadingScreen.enableContinue = document.DocumentElement.Attributes["project"].Value == robotProjects[activeProject].Name;
            }
            catch
            {
                loadingScreen.enableContinue = false;
            }
        }

        private void projectContinue(object sender, EventArgs e)
        {
            closeLoadingScreen();
        }
        private void projectOpen(object sender, EventArgs e)
        {
            try
            {
                if (!FileEquals(outputFile, outputFileLastSave))
                {
                    DialogResult r = MessageBox.Show("Previous progress has not yet been saved.\nSave now?", "", MessageBoxButtons.YesNoCancel);
                    if (r == DialogResult.Yes)
                    {
                        makeZip(showSaveDialog: true);
                        clearFilesDir();
                    }
                    else if (r == DialogResult.No)
                    {
                        clearFilesDir();
                    }
                    else
                    {
                        return;
                    }
                }
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Morphological chart files|*." + zipExt + "|All files|*.*";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.Exists(filesFolder + "\\temp")) Directory.Delete(filesFolder + "\\temp", true);
                    ZipFile.ExtractToDirectory(fileDialog.FileName, tempFolder);

                    if (Directory.Exists(imgFolder)) Directory.Delete(imgFolder, true);
                    if (File.Exists(outputFile)) File.Delete(outputFile);
                    Directory.Move(tempFolder + "\\img", imgFolder);
                    File.Move(tempFolder + "output.xml", outputFile);
                    closeLoadingScreen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't load new file.\nAre the files open in another program?\n" + ex.Message);
            }
        }
        private void projectNew(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("This will delete the previous progress\nContinue?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Directory.Exists(imgFolder)) Directory.Delete(imgFolder, true);
                    Directory.CreateDirectory(imgFolder);
                    if (File.Exists(outputFile)) File.Delete(outputFile);
                    closeLoadingScreen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't start new file.\nAre the files open in another program?\n" + ex.Message);
            }
        }

        private void closeLoadingScreen()
        {
            try
            {
                loadFunctionsFromFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't load functions.\nMake sure the files are not open in another program.\n" + ex.Message);
                return;
            }
            try
            {
                if (File.Exists(outputFile)) loadFromXml(outputFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't load previous work.\nMake sure the files are not open in another program.\n" + ex.Message);
            }
            timer.Start();
            fillFunctionDropdown();
            loadingScreen.Close();
            Opacity = 100;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.Sizable;
            Left = Screen.FromControl(this).WorkingArea.Size.Width / 2 - windowSize.Width / 2;
            Top = Screen.FromControl(this).WorkingArea.Size.Height / 2 - windowSize.Height / 2;
        }

        static bool FileEquals(string path1, string path2)
        {
            if (!File.Exists(path1) || !File.Exists(path2)) return false;

            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private void fillFunctionDropdown()
        {
            comboBox_functions.Items.Clear();
            foreach (robotFunction f in roboFunctions)
            {
                comboBox_functions.Items.Add(f.Name);
            }
        }

        private void loadFunctionsFromFile()
        {
            XmlDocument functionsFile = new XmlDocument();
            functionsFile.Load(filesFolder + "functions.xml");
            int i = 0;
            foreach (XmlNode function in functionsFile.DocumentElement.ChildNodes)
            {
                roboFunctions.Add(new robotFunction(function.Attributes["name"].InnerText));
                Console.WriteLine(function.Attributes["name"].InnerText);
                foreach (XmlNode subFunction in function.ChildNodes)
                {
                    roboFunctions[i].addSubFunction(subFunction.InnerText);
                    Console.WriteLine("\t" + subFunction.InnerText);

                }
                i++;
            }
        }


        /**********************************************************************
         ******************************* GUI **********************************
         *↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓*
         */
        private void addLayerItem(Control lp, String n, String i)
        {
            FlowLayoutPanel layerParent = (FlowLayoutPanel)lp;
            int buttonIndex = layerParent.Controls.Count - 1;

            FlowLayoutPanel newLayerItem = new FlowLayoutPanel();
            PictureBox newLayerPicture = new PictureBox();
            TextBox newLayerText = new TextBox();
            PictureBox newLayerRemove = new PictureBox();

            newLayerItem.FlowDirection = FlowDirection.TopDown;
            newLayerItem.AutoSize = true;
            newLayerPicture.Click += new EventHandler(layerImg_Click);
            newLayerPicture.BorderStyle = BorderStyle.FixedSingle;
            newLayerPicture.Width = 200;
            newLayerPicture.Height = 100;
            newLayerPicture.SizeMode = PictureBoxSizeMode.Zoom;
            newLayerPicture.BackColor = Color.White;
            if (i != "" && i != null)
            {
                try
                {
                    using (FileStream fs = new FileStream(imgFolder + i, FileMode.Open, FileAccess.Read))
                    {
                        newLayerPicture.Image = Image.FromStream(fs);
                    }
                }
                catch
                {
                    MessageBox.Show("Couldn't load image", "Oops");
                }
            }

            newLayerText.Width = 200;
            newLayerText.TextChanged += new EventHandler(newDataL2);
            newLayerText.Text = n;
            newLayerText.MaxLength = 314;

            newLayerItem.Controls.Add(newLayerPicture);
            newLayerItem.Controls.Add(newLayerText);
            layerParent.Controls.Add(newLayerItem);
            layerParent.Controls.SetChildIndex(newLayerItem, buttonIndex);

        }
        private void addLayerItem(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int layer1Index = solutionsBox.Controls.IndexOf(b.Parent.Parent);
            roboFunctions[comboBox_functions.SelectedIndex].subFunctions[comboBox_subFunctions.SelectedIndex].level1Solutions[layer1Index].addSolutions("");
            addLayerItem(b.Parent, "", "");
        }
        private void addLayerItem(int i, String name, String imgName)
        {
            addLayerItem(solutionsBox.Controls[i].Controls[2], name, imgName);
        }

        private void addLayer(String n, String i)
        {
            int buttonIndex = solutionsBox.Controls.Count - 1;

            FlowLayoutPanel newLayer = new FlowLayoutPanel();
            FlowLayoutPanel newLayerMain = new FlowLayoutPanel();
            FlowLayoutPanel newLayerSub = new FlowLayoutPanel();
            PictureBox newLayerPicture = new PictureBox();
            TextBox newLayerText = new TextBox();
            Button newLayerButton = new Button();
            Splitter newLayerSplit = new Splitter();

            newLayer.Padding = new Padding(5);
            newLayer.FlowDirection = FlowDirection.LeftToRight;
            newLayer.AutoSize = true;

            newLayerMain.Padding = new Padding(8);
            newLayerMain.FlowDirection = FlowDirection.TopDown;
            newLayerMain.AutoSize = true;
            newLayerPicture.Click += new EventHandler(layerImg_Click);
            newLayerPicture.BorderStyle = BorderStyle.FixedSingle;
            newLayerPicture.Width = 200;
            newLayerPicture.Height = 100;
            newLayerPicture.SizeMode = PictureBoxSizeMode.Zoom;
            newLayerPicture.BackColor = Color.White;
            if (i != "" && i != null)
            {
                try
                {
                    using (FileStream fs = new FileStream(imgFolder + i, FileMode.Open, FileAccess.Read))
                    {
                        newLayerPicture.Image = Image.FromStream(fs);
                        fs.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Couldn't load image", "Oops");
                }
            }
            newLayerText.Width = 200;
            newLayerText.TextChanged += new EventHandler(newDataL1);
            newLayerText.Text = n;
            newLayerText.MaxLength = 314;
            newLayerMain.Controls.Add(newLayerPicture);
            newLayerMain.Controls.Add(newLayerText);

            newLayerSplit.Width = 2;
            newLayerSplit.BackColor = Color.Black;
            newLayerSplit.Margin = new Padding(0);
            newLayerSplit.Cursor = Cursors.Default;

            newLayerSub.Padding = new Padding(5);
            newLayerSub.FlowDirection = FlowDirection.LeftToRight;
            newLayerSub.AutoSize = true;
            newLayerButton.Size = new Size(75, 126);
            newLayerButton.Text = "+\nAdd layer 2 item";
            newLayerButton.Click += new EventHandler(addLayerItem);
            newLayerButton.BackColor = colorFore;                //Orange buttons
            newLayerButton.FlatStyle = FlatStyle.Flat;
            newLayerButton.FlatAppearance.BorderColor = colorBorder;
            newLayerButton.FlatAppearance.MouseDownBackColor = colorClick;
            newLayerButton.FlatAppearance.MouseOverBackColor = colorHover;
            newLayerButton.FlatAppearance.BorderSize = 3;
            newLayerSub.Controls.Add(newLayerButton);

            newLayer.Controls.Add(newLayerMain);
            newLayer.Controls.Add(newLayerSplit);
            newLayer.Controls.Add(newLayerSub);
            solutionsBox.Controls.Add(newLayer);
            solutionsBox.Controls.SetChildIndex(newLayer, buttonIndex);
        }
        private void addLayer(Object sender, EventArgs e)
        {
            roboFunctions[comboBox_functions.SelectedIndex].subFunctions[comboBox_subFunctions.SelectedIndex].addSolutions("");
            addLayer("", "");
        }

        private void comboBox_functions_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_subFunctions.Items.Clear();
            comboBox_subFunctions.Text = "";

            if (comboBox_functions.SelectedIndex >= 0 && comboBox_functions.SelectedIndex < roboFunctions.Count)
            {
                foreach (robotSubFunction s in roboFunctions[comboBox_functions.SelectedIndex].subFunctions)
                {
                    comboBox_subFunctions.Items.Add(s.Name);
                }
                activeFunction = comboBox_functions.SelectedIndex;
            }
            if (comboBox_functions.SelectedIndex < 0 || comboBox_subFunctions.SelectedIndex < 0)
            {
                solutionsBox.Visible = false;
            }
            else
            {
                clearInputFields();
                fillInputFields();
                solutionsBox.Visible = true;
            }
        }
        private void comboBox_subFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_functions.SelectedIndex < 0 || comboBox_subFunctions.SelectedIndex < 0)
            {
                solutionsBox.Visible = false;
            }
            else
            {
                clearInputFields();
                fillInputFields();
                solutionsBox.Visible = true;
            }

            activeSubfuntion = comboBox_subFunctions.SelectedIndex;
        }

        private void newDataL2(object sender, EventArgs e)
        {
            if (!skipNextTextChangeEvent)
            {
                TextBox source = (TextBox)sender;
                FlowLayoutPanel itemBox = (FlowLayoutPanel)source.Parent;
                FlowLayoutPanel layer2Box = (FlowLayoutPanel)itemBox.Parent;
                FlowLayoutPanel layer1Box = (FlowLayoutPanel)layer2Box.Parent;
                FlowLayoutPanel functionsBox = (FlowLayoutPanel)layer1Box.Parent;

                int subsolutionIndex = layer2Box.Controls.IndexOf(itemBox);
                int solutionIndex = functionsBox.Controls.IndexOf(layer1Box);

                roboFunctions[comboBox_functions.SelectedIndex].subFunctions[comboBox_subFunctions.SelectedIndex].level1Solutions[solutionIndex].level2Solutions[subsolutionIndex].Name = source.Text;
            }
        }
        private void newDataL1(object sender, EventArgs e)
        {
            if (!skipNextTextChangeEvent)
            {
                TextBox source = (TextBox)sender;
                FlowLayoutPanel itemBox = (FlowLayoutPanel)source.Parent;
                FlowLayoutPanel layer1Box = (FlowLayoutPanel)itemBox.Parent;
                FlowLayoutPanel functionsBox = (FlowLayoutPanel)layer1Box.Parent;

                int solutionIndex = functionsBox.Controls.IndexOf(layer1Box);

                roboFunctions[comboBox_functions.SelectedIndex].subFunctions[comboBox_subFunctions.SelectedIndex].level1Solutions[solutionIndex].Name = source.Text;
            }
        }
        private void clearInputFields(object sender = null, EventArgs e = null)
        {
            while (solutionsBox.Controls.Count > 1)
            {
                solutionsBox.Controls.RemoveAt(0);
            }
        }
        private void fillInputFields(object sender = null, EventArgs e = null)
        {
            int fIndex = comboBox_functions.SelectedIndex;
            int sIndex = comboBox_subFunctions.SelectedIndex;
            int l1Index = 0;
            skipNextTextChangeEvent = true;
            foreach (robotLayer1Item s in roboFunctions[fIndex].subFunctions[sIndex].level1Solutions)
            {
                addLayer(s.Name, s.imgName);
                foreach (robotLayer2Item s2 in roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions)
                {
                    addLayerItem(l1Index, s2.Name, s2.imgName);
                }
                l1Index++;
            }
            skipNextTextChangeEvent = false;
        }
        /*↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑*
         ************************** GUI ***************************
         **********************************************************
         */

        private void layerImg_Click(object sender, EventArgs e)
        {
            PictureBox pbox = (PictureBox)sender;
            if (layerItemEditForm != null)
            {
                layerItemEditForm.Close();
                layerItemEditForm = null;
            }
            if (pbox.Parent.Parent.Parent.Equals(solutionsBox))
            {
                //Layer1 item
                int l1Index = solutionsBox.Controls.IndexOf(pbox.Parent.Parent);
                layerItemEditForm = new layerItemEdit(l1Index);
            }
            else
            {
                //Layer2 item
                int l1Index = solutionsBox.Controls.IndexOf(pbox.Parent.Parent.Parent);
                int l2Index = pbox.Parent.Parent.Controls.IndexOf(pbox.Parent);
                layerItemEditForm = new layerItemEdit(l1Index, l2Index);
            }
            layerItemEditForm.StartPosition = FormStartPosition.CenterScreen;
            layerItemEditForm.FormClosing += new FormClosingEventHandler(layerItemEdit_FormClosing);
            layerItemEditForm.Show();
        }
        private void close_layerItemEdit(object sender, EventArgs e)
        {
            if (layerItemEditForm != null)
            {
                if (layerItemEditForm.closing == false)
                {
                    layerItemEditForm.Close();
                    Console.WriteLine("Closing layerItemEdit");
                }
            }
        }
        private void layerItemEdit_FormClosing(object sender = null, FormClosingEventArgs e = null)
        {
            Console.WriteLine("Form close triggered");
            int l1 = layerItemEditForm.l1Index;
            int l2 = layerItemEditForm.l2Index;
            bool l2Item = layerItemEditForm.layer2;
            if (layerItemEditForm.delete)
            {
                //Remove from list
                if (l2Item)
                {
                    deleteItem(l1, l2);
                }
                else
                {
                    deleteItem(l1, -1);
                }

            }
            else
            {
                if (l2Item)
                {
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].imgName = layerItemEditForm.imagePath;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].Name = layerItemEditForm.name;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].description = layerItemEditForm.description;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].ratingFunctionability = layerItemEditForm.ratingFunctionability;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].ratingFeasibility = layerItemEditForm.ratingFeasibility;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].ratingTime = layerItemEditForm.ratingTime;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].pros = layerItemEditForm.pros;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].level2Solutions[l2].cons = layerItemEditForm.cons;
                    if (layerItemEditForm.imgChanged)
                    {
                        PictureBox pbox = (PictureBox)solutionsBox.Controls[l1].Controls[2].Controls[l2].Controls[0];
                        using (FileStream fs = new FileStream(imgFolder + layerItemEditForm.imagePath, FileMode.Open, FileAccess.Read))
                        {
                            pbox.Image = Image.FromStream(fs);
                            fs.Close();
                        }
                    }
                    TextBox tbox = (TextBox)solutionsBox.Controls[l1].Controls[2].Controls[l2].Controls[1];
                    tbox.Text = layerItemEditForm.name;
                }
                else
                {
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].imgName = layerItemEditForm.imagePath;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].Name = layerItemEditForm.name;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].description = layerItemEditForm.description;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].ratingFunctionability = layerItemEditForm.ratingFunctionability;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].ratingFeasibility = layerItemEditForm.ratingFeasibility;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].ratingTime = layerItemEditForm.ratingTime;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].pros = layerItemEditForm.pros;
                    roboFunctions[activeFunction].subFunctions[activeSubfuntion].level1Solutions[l1].cons = layerItemEditForm.cons;
                    if (layerItemEditForm.imgChanged)
                    {
                        PictureBox pbox = (PictureBox)solutionsBox.Controls[l1].Controls[0].Controls[0];
                        using (FileStream fs = new FileStream(imgFolder + layerItemEditForm.imagePath, FileMode.Open, FileAccess.Read))
                        {
                            pbox.Image = Image.FromStream(fs);
                            fs.Close();
                        }
                    }
                    TextBox tbox = (TextBox)solutionsBox.Controls[l1].Controls[0].Controls[1];
                    tbox.Text = layerItemEditForm.name;
                }
            }
            layerItemEditForm.Dispose();
            layerItemEditForm = null;
        }
        private void deleteEmptyItems(object sender = null, EventArgs e = null)
        {
            foreach (robotFunction f in roboFunctions)
            {
                foreach (robotSubFunction s in f.subFunctions)
                {
                    int i = 0;
                    while (i < s.level1Solutions.Count)
                    {
                        robotLayer1Item l1 = s.level1Solutions[i];
                        int j = 0;
                        while (j < l1.level2Solutions.Count)
                        {
                            robotLayer2Item l2 = l1.level2Solutions[j];
                            bool delete2 = true;
                            if (!String.IsNullOrWhiteSpace(l2.Name)) delete2 = false;
                            if (!String.IsNullOrWhiteSpace(l2.description)) delete2 = false;
                            if (!String.IsNullOrWhiteSpace(l2.imgName)) delete2 = false;
                            if (l2.pros.Count > 0 && l2.pros != null) delete2 = false;
                            if (l2.cons.Count > 0 && l2.cons != null) delete2 = false;
                            if (delete2)
                            {
                                deleteItem(roboFunctions.IndexOf(f), f.subFunctions.IndexOf(s), i, j);
                            }
                            else j++;
                        }
                        bool delete1 = true;
                        if (!String.IsNullOrWhiteSpace(l1.Name)) delete1 = false;
                        if (!String.IsNullOrWhiteSpace(l1.description)) delete1 = false;
                        if (!String.IsNullOrWhiteSpace(l1.imgName)) delete1 = false;
                        if (l1.pros.Count > 0 && l1.pros != null) delete1 = false;
                        if (l1.cons.Count > 0 && l1.cons != null) delete1 = false;
                        if (delete1)
                        {
                            deleteItem(roboFunctions.IndexOf(f), f.subFunctions.IndexOf(s), i, -1);
                        }
                        else i++;
                    }
                }
            }
        }
        private void deleteItem(int l1, int l2)
        {
            deleteItem(activeFunction, activeSubfuntion, l1, l2);
        }
        private void deleteItem(int f, int s, int l1, int l2)
        {
            if (l2 < 0)
            {
                if (f == activeFunction && s == activeSubfuntion) solutionsBox.Controls.RemoveAt(l1);
                roboFunctions[f].subFunctions[s].level1Solutions.RemoveAt(l1);
            }
            else
            {
                if (f == activeFunction && s == activeSubfuntion) solutionsBox.Controls[l1].Controls[2].Controls.RemoveAt(l2);
                roboFunctions[f].subFunctions[s].level1Solutions[l1].level2Solutions.RemoveAt(l2);
            }
        }

        private void fullScreen(object sender, EventArgs e)
        {
            if (fullscreenActive)
            {
                fullscreenActive = false;
                fullscreenButton.Text = "Go fullscreen";
                FormBorderStyle = FormBorderStyle.Sizable;
                Size = windowSize;
                Left = windowPos[0];
                Top = windowPos[1];
            }
            else
            {
                fullscreenActive = true;
                windowPos[0] = Left;
                windowPos[1] = Top;
                windowSize = Size;
                fullscreenButton.Text = "Exit fullscreen";
                FormBorderStyle = FormBorderStyle.None;
                Left = 0;
                Top = 0;
                Size = Screen.FromControl(this).WorkingArea.Size;
            }
        }

        private void main_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    startCureBoredom(1);
                    break;
                case Keys.F2:
                    startCureBoredom(2);
                    break;
                case Keys.F3:
                    startCureBoredom(3);
                    break;
                case Keys.Escape:
                    stopCureBoredom();
                    break;
            }
        }

        /**********************************************************************
         *********************** Cure boredom *********************************
         *↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓*
         */
        private void moveScreen(object sender, EventArgs e)
        {
            switch (moveScreenFuction)
            {
                case 1:
                    Left += random.Next(-1, 2);
                    Top += random.Next(-1, 2);
                    break;
                case 2:
                    Left = (int)(Math.Cos(moveScreenCount * Math.PI / 180) * 100) + Screen.FromControl(this).WorkingArea.Size.Width / 2 - Width / 2;
                    Top = (int)(Math.Sin(moveScreenCount * Math.PI / 180) * 100) + Screen.FromControl(this).WorkingArea.Size.Height / 2 - Height / 2;
                    break;
                case 3:
                    Left += moveScreen2x;
                    Top += moveScreen2y;
                    if (Left + Width >= Screen.FromControl(this).WorkingArea.Size.Width || Left <= 0) moveScreen2x *= -1;
                    if (Top + Height >= Screen.FromControl(this).WorkingArea.Size.Height || Top <= 0) moveScreen2y *= -1;
                    break;
            }
            moveScreenCount += 1;
        }
        private void startCureBoredom(int i = -1)
        {
            if (i < 0) i = random.Next(0, 4);
            moveScreenFuction = i;
            if ((DateTime.Now - prevCureBoredom).TotalMinutes > Properties.Settings.Default.cureBoredomMinMinutes)
            {
                prevCureBoredom = DateTime.Now;
                timerCureBoredomStop.Start();
                timerCureBoredom.Start();
            }
            else MessageBox.Show("You're bored too quick....\nGo back to work!");
        }
        private void stopCureBoredom(object sender = null, EventArgs e = null)
        {
            timerCureBoredom.Stop();
        }
        /*↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑*
         *********************** Cure boredom *********************************
         **********************************************************************
         */

        private void saveButton_click(object sender, EventArgs e)
        {
            saveZip();
        }

        private void main_Load(object sender, EventArgs e)
        {
            Hide();
        }

        private void loadingScreenClose(object sender, EventArgs e)
        {
            Close();
        }
    }
}
