using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.IO;
using System.Drawing;

namespace MorphologicalChart
{
    public partial class main
    {
        public const int workerSettings = 0;
        public const int workerFunctions = 1;

        private void bgWork(object sender, DoWorkEventArgs e)
        {
            int work = (int)e.Argument;
            e.Result = work;

            if (work == workerSettings)
            {
                try
                {
                    String functionListUrl = Properties.Settings.Default.urlFunctions;
                    using (var client = new WebClient())
                    {
                        client.DownloadFileTaskAsync(Properties.Settings.Default.urlSettings, filesFolder + "settingsTemp.xml");
                        while (client.IsBusy)
                        {
                            if (bgWorker.CancellationPending)
                            {
                                client.CancelAsync();
                                break;
                            }
                            Thread.Sleep(50);
                        }
                        if(new FileInfo(filesFolder + "settingsTemp.xml").Length == 0)
                        {
                            File.Delete(filesFolder + "settingsTemp.xml");
                            throw new Exception();
                        }
                        else
                        {
                            if (File.Exists(filesFolder + "settings.xml")) File.Delete(filesFolder + "settings.xml");
                            File.Move(filesFolder + "settingsTemp.xml", filesFolder + "settings.xml");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't acces the internet.");
                }
                try {
                    if (File.Exists(filesFolder + "settings.xml") && !bgWorker.CancellationPending)
                    {
                        XmlDocument reader = new XmlDocument();
                        reader.Load(filesFolder + "settings.xml");
                        foreach (XmlNode setting in reader.DocumentElement.ChildNodes)
                        {
                            switch (setting.Name)
                            {
                                case "Projects":
                                    robotProjects.Clear();
                                    foreach (XmlNode project in setting.ChildNodes)
                                    {
                                        if (project.Name == "Project")
                                        {
                                            String n = project.Attributes["name"].Value;
                                            String u = project.Attributes["url"].Value;
                                            if (!(String.IsNullOrWhiteSpace(n) || String.IsNullOrWhiteSpace(u)))
                                            {
                                                robotProjects.Add(new robotProject(n, u));
                                            }
                                        }
                                    }
                                    break;
                                case "Color":
                                    colorBack = ColorTranslator.FromHtml(setting.Attributes["back"].Value);
                                    colorFore = ColorTranslator.FromHtml(setting.Attributes["fore"].Value);
                                    colorBorder = ColorTranslator.FromHtml(setting.Attributes["border"].Value);
                                    colorHover = ColorTranslator.FromHtml(setting.Attributes["hover"].Value);
                                    colorClick = ColorTranslator.FromHtml(setting.Attributes["click"].Value);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't get new settings.\n" + ex.Message);
                }
            }

            else if (work == workerFunctions)
            {
                try
                {
                    //String functionListUrl = Properties.Settings.Default.urlFunctions;
                    using (var client = new WebClient())
                    {
                        client.DownloadFileTaskAsync(robotProjects[activeProject].functionsFileUrl, filesFolder + "functions.xml");
                        while (client.IsBusy)
                        {
                            if (bgWorker.CancellationPending)
                            {
                                client.CancelAsync();
                                break;
                            }
                            Thread.Sleep(50);
                        }
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Couldn't update functions\n\n" + ex.Message);
                }
            }
            /*
            DateTime dtstart = DateTime.Now;
            while((DateTime.Now - dtstart).TotalMilliseconds < 10000 && !bgWorker.CancellationPending)
            {
                Thread.Sleep(100);
            }
            */
        }

        private void startupWorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            int work = (int)e.Result;
            switch (work)
            {
                case workerSettings:
                    loadingScreen.comboBox1.Items.Clear();
                    foreach(robotProject p in robotProjects)
                    {
                        loadingScreen.comboBox1.Items.Add(p.Name);
                    }
                    if(loadingScreen.comboBox1.Items.Count > 0) loadingScreen.comboBox1.SelectedIndex = 0;
                    loadingScreen.setStatus();
                    BackColor = colorBack;
                    addL1Button.BackColor = colorFore;
                    addL1Button.FlatStyle = FlatStyle.Flat;
                    addL1Button.FlatAppearance.BorderColor = colorBorder;
                    addL1Button.FlatAppearance.MouseDownBackColor = colorClick;
                    addL1Button.FlatAppearance.MouseOverBackColor = colorHover;
                    addL1Button.FlatAppearance.BorderSize = 3;
                    comboBox_functions.BackColor = colorFore;
                    comboBox_subFunctions.BackColor = colorFore;
                    break;
                case workerFunctions:
                    loadingScreen.setStatus();
                    break;
            }
        }
        private void bgWorkerCancel(object sender, EventArgs e)
        {
            if (bgWorker.IsBusy) bgWorker.CancelAsync();
            else
            {
                bgWorker.RunWorkerAsync(workerSettings);
                loadingScreen.setStatus("Checking for updates...");
            }
        }
    }
}