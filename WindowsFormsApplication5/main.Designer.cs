namespace MorphologicalChart
{
    partial class main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.comboBox_functions = new System.Windows.Forms.ComboBox();
            this.comboBox_subFunctions = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addL1Button = new System.Windows.Forms.Button();
            this.solutionsBox = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonDeleteEmpty = new System.Windows.Forms.Button();
            this.fullscreenButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.solutionsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_functions
            // 
            this.comboBox_functions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_functions.FormattingEnabled = true;
            this.comboBox_functions.Location = new System.Drawing.Point(39, 29);
            this.comboBox_functions.Name = "comboBox_functions";
            this.comboBox_functions.Size = new System.Drawing.Size(121, 21);
            this.comboBox_functions.TabIndex = 0;
            this.comboBox_functions.SelectedIndexChanged += new System.EventHandler(this.comboBox_functions_SelectedIndexChanged);
            // 
            // comboBox_subFunctions
            // 
            this.comboBox_subFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_subFunctions.FormattingEnabled = true;
            this.comboBox_subFunctions.Location = new System.Drawing.Point(178, 29);
            this.comboBox_subFunctions.MaxDropDownItems = 99;
            this.comboBox_subFunctions.Name = "comboBox_subFunctions";
            this.comboBox_subFunctions.Size = new System.Drawing.Size(121, 21);
            this.comboBox_subFunctions.TabIndex = 6;
            this.comboBox_subFunctions.SelectedIndexChanged += new System.EventHandler(this.comboBox_subFunctions_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.addL1Button);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(235, 164);
            this.flowLayoutPanel1.TabIndex = 33;
            // 
            // addL1Button
            // 
            this.addL1Button.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.addL1Button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(87)))), ((int)(((byte)(3)))));
            this.addL1Button.FlatAppearance.BorderSize = 3;
            this.addL1Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(80)))), ((int)(((byte)(3)))));
            this.addL1Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(110)))), ((int)(((byte)(4)))));
            this.addL1Button.Location = new System.Drawing.Point(19, 19);
            this.addL1Button.Margin = new System.Windows.Forms.Padding(14);
            this.addL1Button.Name = "addL1Button";
            this.addL1Button.Size = new System.Drawing.Size(197, 126);
            this.addL1Button.TabIndex = 34;
            this.addL1Button.Text = "+\r\nAdd layer 1 item";
            this.addL1Button.UseVisualStyleBackColor = false;
            this.addL1Button.Click += new System.EventHandler(this.addLayer);
            // 
            // solutionsBox
            // 
            this.solutionsBox.AutoSize = true;
            this.solutionsBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.solutionsBox.BackColor = System.Drawing.Color.Transparent;
            this.solutionsBox.Controls.Add(this.flowLayoutPanel1);
            this.solutionsBox.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.solutionsBox.Location = new System.Drawing.Point(12, 56);
            this.solutionsBox.Name = "solutionsBox";
            this.solutionsBox.Padding = new System.Windows.Forms.Padding(5);
            this.solutionsBox.Size = new System.Drawing.Size(251, 180);
            this.solutionsBox.TabIndex = 35;
            this.solutionsBox.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 38;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.saveButton_click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(387, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 39;
            this.button2.Text = "Send files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.sendZip);
            // 
            // buttonDeleteEmpty
            // 
            this.buttonDeleteEmpty.Location = new System.Drawing.Point(469, 29);
            this.buttonDeleteEmpty.Name = "buttonDeleteEmpty";
            this.buttonDeleteEmpty.Size = new System.Drawing.Size(95, 23);
            this.buttonDeleteEmpty.TabIndex = 40;
            this.buttonDeleteEmpty.Text = "Delete all empty";
            this.buttonDeleteEmpty.UseVisualStyleBackColor = true;
            this.buttonDeleteEmpty.Click += new System.EventHandler(this.deleteEmptyItems);
            // 
            // fullscreenButton
            // 
            this.fullscreenButton.Location = new System.Drawing.Point(571, 29);
            this.fullscreenButton.Name = "fullscreenButton";
            this.fullscreenButton.Size = new System.Drawing.Size(99, 23);
            this.fullscreenButton.TabIndex = 41;
            this.fullscreenButton.Text = "Go Fullscreen";
            this.fullscreenButton.UseVisualStyleBackColor = true;
            this.fullscreenButton.Click += new System.EventHandler(this.fullScreen);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1272, 539);
            this.Controls.Add(this.fullscreenButton);
            this.Controls.Add(this.buttonDeleteEmpty);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.solutionsBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox_subFunctions);
            this.Controls.Add(this.comboBox_functions);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "main";
            this.Text = "Morphological Chart";
            this.Activated += new System.EventHandler(this.close_layerItemEdit);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.saveXml);
            this.Load += new System.EventHandler(this.main_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.main_KeyUp);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.solutionsBox.ResumeLayout(false);
            this.solutionsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_functions;
        private System.Windows.Forms.ComboBox comboBox_subFunctions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button addL1Button;
        private System.Windows.Forms.FlowLayoutPanel solutionsBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonDeleteEmpty;
        private System.Windows.Forms.Button fullscreenButton;
    }
}

