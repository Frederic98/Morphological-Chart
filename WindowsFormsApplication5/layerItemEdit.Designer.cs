namespace MorphologicalChart
{
    partial class layerItemEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(layerItemEdit));
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.rating1 = new System.Windows.Forms.TrackBar();
            this.labelName = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxPros = new System.Windows.Forms.TextBox();
            this.textBoxCons = new System.Windows.Forms.TextBox();
            this.labelPros = new System.Windows.Forms.Label();
            this.labelCons = new System.Windows.Forms.Label();
            this.rating1Label = new System.Windows.Forms.Label();
            this.labelImage = new System.Windows.Forms.Label();
            this.rating2Label = new System.Windows.Forms.Label();
            this.rating2 = new System.Windows.Forms.TrackBar();
            this.rating3Label = new System.Windows.Forms.Label();
            this.rating3 = new System.Windows.Forms.TrackBar();
            this.ratingBar = new System.Windows.Forms.ProgressBar();
            this.ratingLabel = new System.Windows.Forms.Label();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.rating1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rating2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rating3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(213, 25);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(200, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.AllowDrop = true;
            this.textBoxDescription.Location = new System.Drawing.Point(213, 64);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(200, 180);
            this.textBoxDescription.TabIndex = 2;
            this.textBoxDescription.TextChanged += new System.EventHandler(this.textBoxDescription_TextChanged);
            // 
            // rating1
            // 
            this.rating1.LargeChange = 1;
            this.rating1.Location = new System.Drawing.Point(4, 144);
            this.rating1.Maximum = 5;
            this.rating1.Name = "rating1";
            this.rating1.Size = new System.Drawing.Size(200, 45);
            this.rating1.TabIndex = 5;
            this.rating1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.rating1.Value = 1;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.ForeColor = System.Drawing.Color.Black;
            this.labelName.Location = new System.Drawing.Point(210, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "Name";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.ForeColor = System.Drawing.Color.Black;
            this.labelDescription.Location = new System.Drawing.Point(210, 48);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(60, 13);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // textBoxPros
            // 
            this.textBoxPros.AcceptsReturn = true;
            this.textBoxPros.AllowDrop = true;
            this.textBoxPros.Location = new System.Drawing.Point(419, 25);
            this.textBoxPros.Multiline = true;
            this.textBoxPros.Name = "textBoxPros";
            this.textBoxPros.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPros.Size = new System.Drawing.Size(200, 100);
            this.textBoxPros.TabIndex = 3;
            this.textBoxPros.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBoxCons
            // 
            this.textBoxCons.AcceptsReturn = true;
            this.textBoxCons.AllowDrop = true;
            this.textBoxCons.Location = new System.Drawing.Point(419, 144);
            this.textBoxCons.Multiline = true;
            this.textBoxCons.Name = "textBoxCons";
            this.textBoxCons.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCons.Size = new System.Drawing.Size(200, 100);
            this.textBoxCons.TabIndex = 4;
            this.textBoxCons.TextChanged += new System.EventHandler(this.textBoxCons_TextChanged);
            // 
            // labelPros
            // 
            this.labelPros.AutoSize = true;
            this.labelPros.ForeColor = System.Drawing.Color.Black;
            this.labelPros.Location = new System.Drawing.Point(416, 9);
            this.labelPros.Name = "labelPros";
            this.labelPros.Size = new System.Drawing.Size(28, 13);
            this.labelPros.TabIndex = 8;
            this.labelPros.Text = "Pros";
            // 
            // labelCons
            // 
            this.labelCons.AutoSize = true;
            this.labelCons.ForeColor = System.Drawing.Color.Black;
            this.labelCons.Location = new System.Drawing.Point(420, 128);
            this.labelCons.Name = "labelCons";
            this.labelCons.Size = new System.Drawing.Size(31, 13);
            this.labelCons.TabIndex = 9;
            this.labelCons.Text = "Cons";
            // 
            // rating1Label
            // 
            this.rating1Label.AutoSize = true;
            this.rating1Label.Location = new System.Drawing.Point(1, 128);
            this.rating1Label.Name = "rating1Label";
            this.rating1Label.Size = new System.Drawing.Size(66, 13);
            this.rating1Label.TabIndex = 10;
            this.rating1Label.Text = "Functionality";
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.ForeColor = System.Drawing.Color.Black;
            this.labelImage.Location = new System.Drawing.Point(1, 9);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(36, 13);
            this.labelImage.TabIndex = 11;
            this.labelImage.Text = "Image";
            // 
            // rating2Label
            // 
            this.rating2Label.AutoSize = true;
            this.rating2Label.Location = new System.Drawing.Point(1, 179);
            this.rating2Label.Name = "rating2Label";
            this.rating2Label.Size = new System.Drawing.Size(52, 13);
            this.rating2Label.TabIndex = 13;
            this.rating2Label.Text = "Feasibility";
            // 
            // rating2
            // 
            this.rating2.LargeChange = 1;
            this.rating2.Location = new System.Drawing.Point(4, 195);
            this.rating2.Maximum = 5;
            this.rating2.Name = "rating2";
            this.rating2.Size = new System.Drawing.Size(200, 45);
            this.rating2.TabIndex = 12;
            this.rating2.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.rating2.Value = 1;
            // 
            // rating3Label
            // 
            this.rating3Label.AutoSize = true;
            this.rating3Label.Location = new System.Drawing.Point(1, 230);
            this.rating3Label.Name = "rating3Label";
            this.rating3Label.Size = new System.Drawing.Size(78, 13);
            this.rating3Label.TabIndex = 15;
            this.rating3Label.Text = "Time-efficiency";
            // 
            // rating3
            // 
            this.rating3.LargeChange = 1;
            this.rating3.Location = new System.Drawing.Point(4, 246);
            this.rating3.Maximum = 5;
            this.rating3.Name = "rating3";
            this.rating3.Size = new System.Drawing.Size(200, 45);
            this.rating3.TabIndex = 14;
            this.rating3.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.rating3.Value = 1;
            // 
            // ratingBar
            // 
            this.ratingBar.Location = new System.Drawing.Point(213, 267);
            this.ratingBar.Maximum = 15;
            this.ratingBar.Name = "ratingBar";
            this.ratingBar.Size = new System.Drawing.Size(200, 23);
            this.ratingBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ratingBar.TabIndex = 16;
            this.ratingBar.Value = 15;
            // 
            // ratingLabel
            // 
            this.ratingLabel.AutoSize = true;
            this.ratingLabel.Location = new System.Drawing.Point(213, 251);
            this.ratingLabel.Name = "ratingLabel";
            this.ratingLabel.Size = new System.Drawing.Size(38, 13);
            this.ratingLabel.TabIndex = 17;
            this.ratingLabel.Text = "Rating";
            // 
            // imageBox
            // 
            this.imageBox.BackColor = System.Drawing.Color.White;
            this.imageBox.Location = new System.Drawing.Point(4, 25);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(200, 100);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox.TabIndex = 0;
            this.imageBox.TabStop = false;
            this.imageBox.Click += new System.EventHandler(this.changeImage);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(574, 267);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(50, 23);
            this.buttonDelete.TabIndex = 99;
            this.buttonDelete.TabStop = false;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(419, 267);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(149, 23);
            this.buttonOk.TabIndex = 18;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // layerItemEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(683, 345);
            this.ControlBox = false;
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.ratingLabel);
            this.Controls.Add(this.ratingBar);
            this.Controls.Add(this.rating3Label);
            this.Controls.Add(this.rating3);
            this.Controls.Add(this.rating2Label);
            this.Controls.Add(this.rating2);
            this.Controls.Add(this.labelImage);
            this.Controls.Add(this.rating1Label);
            this.Controls.Add(this.labelCons);
            this.Controls.Add(this.labelPros);
            this.Controls.Add(this.textBoxCons);
            this.Controls.Add(this.textBoxPros);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.rating1);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.imageBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "layerItemEdit";
            this.Text = "layerEdit";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.layerItemEdit_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.rating1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rating2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rating3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TrackBar rating1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxPros;
        private System.Windows.Forms.TextBox textBoxCons;
        private System.Windows.Forms.Label labelPros;
        private System.Windows.Forms.Label labelCons;
        private System.Windows.Forms.Label rating1Label;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Label rating2Label;
        private System.Windows.Forms.TrackBar rating2;
        private System.Windows.Forms.Label rating3Label;
        private System.Windows.Forms.TrackBar rating3;
        private System.Windows.Forms.ProgressBar ratingBar;
        private System.Windows.Forms.Label ratingLabel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonOk;
    }
}