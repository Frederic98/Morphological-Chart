using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Xml;

namespace MorphologicalChart
{
    public partial class main
    {
        static MailAddress fromAddress = new MailAddress(Properties.Settings.Default.mailFromAddress, Properties.Settings.Default.mailFromName);
        MailAddress toAddress = new MailAddress(Properties.Settings.Default.mailToAddress, Properties.Settings.Default.mailToName);
        static String fromPass = Properties.Settings.Default.mailFromPass;
        MailMessage mailMessage;
        SmtpClient smtp = new SmtpClient
        {
            Host = Properties.Settings.Default.mailFromHost,
            Port = Properties.Settings.Default.mailFromPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPass)
        };
        mailSend mailSendForm;
        private int maxMailSize = 25;
        bool sendingMail = false;

        /*
         * Writing to zip
         */
        private String makeZip(bool autosave = false, bool showSaveDialog = false, String fileName = null)
        {
            saveXml();
            if (fileName == null) fileName = String.Format((autosave ? outputZipAutosave : outputZip), DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"));
            Console.WriteLine("Writing zip to: \"" + fileName + "\"");
            ZipFile.CreateFromDirectory(filesFolder, fileName, CompressionLevel.Optimal, false);
            if (showSaveDialog) saveZip(fileName);
            return fileName;
        }
        private void makeZip(object sender, EventArgs e)
        {
            makeZip(false, true);
        }

        private void saveZip(String fileName = null)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = zipExt;
            saveDialog.AddExtension = true;
            saveDialog.FileName = String.IsNullOrWhiteSpace(fileName) ? String.Format(outputZip, DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")) : fileName;
            saveDialog.Filter = "Morphological chart files|*." + zipExt + "|All files|*.*";
            DialogResult fd = saveDialog.ShowDialog();
            try
            {
                if (File.Exists(saveDialog.FileName)) File.Delete(saveDialog.FileName);
                if (fd == DialogResult.OK)
                {
                    if (String.IsNullOrWhiteSpace(fileName))
                    {
                        makeZip(fileName: saveDialog.FileName);
                    }
                    else
                    {
                        File.Copy(fileName, saveDialog.FileName);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Couldn't save file.\nPlease try again.");
            }
        }

        private void sendZip(String fileName)
        {
            if (sendingMail) MessageBox.Show("Already sending files.\nPlease wait for sending to finish before sending new files");
            else
            {
                if (new FileInfo(fileName).Length > maxMailSize * 1000000)
                {
                    MessageBox.Show("File is too big.\nPlease send it manually.");
                    saveZip(fileName);
                }
                else
                {
                    mailSendForm = new mailSend();
                    mailSendForm.button1.Click += new EventHandler(mailSendCancel);
                    mailSendForm.Show();
                    Application.DoEvents();
                    Activate();
                    try
                    {
                        Attachment a = new Attachment(fileName);
                        mailMessage = new MailMessage(fromAddress, toAddress);
                        mailMessage.Subject = String.Format(Properties.Settings.Default.mailSubject, Environment.MachineName, DateTime.Now);
                        mailMessage.Body = String.Format(Properties.Settings.Default.mailBody, Environment.MachineName, Environment.UserName, DateTime.Now, fileName);
                        mailMessage.Attachments.Add(a);
                        sendingMail = true;
                        smtp.SendMailAsync(mailMessage);
                        smtp.SendCompleted += new SendCompletedEventHandler(sendZipDone);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Couldn't send files.\nPlease send it manually\n" + ex.Message);
                        sendZipDone();
                        saveZip(fileName);
                    }
                }
            }
        }
        private void sendZip(object sender = null, EventArgs e = null)
        {
            sendZip(makeZip());
        }

        private void sendZipDone(object sender = null, AsyncCompletedEventArgs e = null)
        {
            if (e != null)
            {
                if (e.Error != null) MessageBox.Show("Couldn't send files.\nPlease send it manually.\n" + e.Error.Message);
                Console.WriteLine("Mail done\nCanceled: " + e.Cancelled);
                Console.WriteLine("Error: " + e.Error);
                Console.WriteLine("UserState: " + e.UserState);

            }
            if (mailMessage != null)
            {
                mailMessage.Dispose();
            }
            if (mailSendForm != null)
            {
                mailSendForm.Close();
            }
            sendingMail = false;
        }

        public void sendZipCancel(bool confirmation = false)
        {
            if (confirmation)
            {
                if (MessageBox.Show("Are you sure you want to cancel sending the files?", "Confirm cancel", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    smtp.SendAsyncCancel();
                    sendZipDone();
                }
            }
            else
            {
                smtp.SendAsyncCancel();
                sendZipDone();
            }
        }
        private void mailSendCancel(object sender, EventArgs e)
        {
            sendZipCancel(true);
        }
        

        /*
         * Writing/Reading xml
         */

        private void saveXml(string fileName = null)
        {
            if (String.IsNullOrWhiteSpace(fileName)) fileName = outputFile;
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            XmlWriter writer = XmlWriter.Create(fileName, xmlSettings);
            writer.WriteStartDocument();
            writer.WriteStartElement("functions");
            writer.WriteAttributeString("project", robotProjects[activeProject].Name);
            foreach (robotFunction f in roboFunctions)
            {
                writer.WriteStartElement("function");
                writer.WriteAttributeString("name", f.Name);
                foreach (robotSubFunction s in f.subFunctions)
                {
                    writer.WriteStartElement("subFunction");
                    writer.WriteAttributeString("name", s.Name);
                    foreach (robotLayer1Item l1 in s.level1Solutions)
                    {
                        writer.WriteStartElement("layer1Item");
                        writer.WriteAttributeString("name", l1.Name);
                        writer.WriteAttributeString("img", l1.imgName);
                        writer.WriteAttributeString("ratingFunctionability", l1.ratingFunctionability.ToString());
                        writer.WriteAttributeString("ratingFeasibility", l1.ratingFeasibility.ToString());
                        writer.WriteAttributeString("ratingTime", l1.ratingTime.ToString());
                        writer.WriteAttributeString("description", l1.description);
                        if (l1.pros != null)
                        {
                            writer.WriteStartElement("pros");
                            foreach (String p in l1.pros)
                            {
                                writer.WriteElementString("pro", p);
                            }
                            writer.WriteEndElement();
                        }
                        if (l1.cons != null)
                        {
                            writer.WriteStartElement("cons");
                            foreach (String c in l1.cons)
                            {
                                writer.WriteElementString("con", c);
                            }
                            writer.WriteEndElement();
                        }
                        foreach (robotLayer2Item l2 in l1.level2Solutions)
                        {
                            writer.WriteStartElement("layer2Item");
                            writer.WriteAttributeString("name", l2.Name);
                            writer.WriteAttributeString("img", l2.imgName);
                            writer.WriteAttributeString("ratingFunctionability", l2.ratingFunctionability.ToString());
                            writer.WriteAttributeString("ratingFeasibility", l2.ratingFeasibility.ToString());
                            writer.WriteAttributeString("ratingTime", l2.ratingTime.ToString());
                            writer.WriteAttributeString("description", l2.description);
                            if (l2.pros != null)
                            {
                                writer.WriteStartElement("pros");
                                foreach (String p in l2.pros)
                                {
                                    writer.WriteElementString("pro", p);
                                }
                                writer.WriteEndElement();
                            }
                            if (l2.cons != null)
                            {
                                writer.WriteStartElement("cons");
                                foreach (String c in l2.cons)
                                {
                                    writer.WriteElementString("con", c);
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        private void saveXml(object sender, FormClosingEventArgs e)
        {
            saveXml();
        }
        private void saveXml(object sender, EventArgs e)
        {
            saveXml();
        }

        private void loadFromXml(String fileName)
        {
            XmlDocument reader = new XmlDocument();
            try
            {
                reader.Load(fileName);
                foreach (XmlNode function in reader.DocumentElement.ChildNodes)
                {
                    int fIndex = 0;
                    foreach (robotFunction f in roboFunctions)
                    {
                        if (function.Attributes["name"].InnerText == f.Name) break;
                        fIndex++;
                        if (fIndex >= roboFunctions.Count)
                        {
                            roboFunctions.Add(new robotFunction(function.Attributes["name"].InnerText));
                            break;
                        }
                    }
                    foreach (XmlNode subFunction in function.ChildNodes)
                    {
                        int sIndex = 0;
                        if (roboFunctions[fIndex].subFunctions.Count <= 0)
                        {
                            roboFunctions[fIndex].subFunctions.Add(new robotSubFunction(subFunction.Attributes["name"].InnerText));
                        }
                        foreach (robotSubFunction s in roboFunctions[fIndex].subFunctions)
                        {
                            if (subFunction.Attributes["name"].InnerText == s.Name) break;
                            sIndex++;
                            if (sIndex >= roboFunctions[fIndex].subFunctions.Count)
                            {
                                roboFunctions[fIndex].subFunctions.Add(new robotSubFunction(subFunction.Attributes["name"].InnerText));
                                break;
                            }
                        }
                        int l1Index = 0;
                        foreach (XmlNode l1 in subFunction.ChildNodes)
                        {
                            roboFunctions[fIndex].subFunctions[sIndex].addSolutions(l1.Attributes["name"].InnerText, l1.Attributes["img"].InnerText);
                            roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].description = l1.Attributes["description"].InnerText;
                            roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].ratingFunctionability = int.Parse(l1.Attributes["ratingFunctionability"]?.InnerText ?? "0");
                            roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].ratingFeasibility = int.Parse(l1.Attributes["ratingFeasibility"]?.InnerText ?? "0");
                            roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].ratingTime = int.Parse(l1.Attributes["ratingTime"]?.InnerText ?? "0");
                            foreach (XmlNode pro in l1.SelectNodes("pros/pro"))
                            {
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].pros.Add(pro.InnerText);
                            }
                            foreach (XmlNode con in l1.SelectNodes("cons/con"))
                            {
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].cons.Add(con.InnerText);
                            }

                            int l2Index = 0;
                            foreach (XmlNode l2 in l1.SelectNodes("layer2Item"))
                            {
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].addSolutions(l2.Attributes["name"].InnerText, l2.Attributes["img"].InnerText);
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions[l2Index].description = l2.Attributes["description"].InnerText;
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions[l2Index].ratingFunctionability = int.Parse(l2.Attributes["ratingFunctionability"]?.InnerText ?? "0");
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions[l2Index].ratingFeasibility = int.Parse(l2.Attributes["ratingFeasibility"]?.InnerText ?? "0");
                                roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions[l2Index].ratingTime = int.Parse(l2.Attributes["ratingTime"]?.InnerText ?? "0");
                                foreach (XmlNode pro in l2.SelectNodes("pros/pro"))
                                {
                                    roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions[l2Index].pros.Add(pro.InnerText);
                                }
                                foreach (XmlNode con in l2.SelectNodes("cons/con"))
                                {
                                    roboFunctions[fIndex].subFunctions[sIndex].level1Solutions[l1Index].level2Solutions[l2Index].cons.Add(con.InnerText);
                                }
                                l2Index++;
                            }
                            l1Index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't open previous document\n" + ex.Message);
            }
        }

        private void clearFilesDir()
        {
            if(Directory.Exists(imgFolder)) Directory.Delete(imgFolder, true);
            Directory.CreateDirectory(imgFolder);
            if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
            if (File.Exists(outputFile)) File.Delete(outputFile);
            if (File.Exists(outputFileLastSave)) File.Delete(outputFileLastSave);
        }
    }
}
