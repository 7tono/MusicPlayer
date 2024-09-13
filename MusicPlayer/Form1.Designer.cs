namespace MusicPlayer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PlayPauseButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.customWaveViewer1 = new MusicPlayer.CustomWaveViewer();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.StopButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(842, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "開く";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(948, 360);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(264, 118);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // PlayPauseButton
            // 
            this.PlayPauseButton.BackColor = System.Drawing.Color.White;
            this.PlayPauseButton.BackgroundImage = global::MusicPlayer.Properties.Resources.play;
            this.PlayPauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PlayPauseButton.Location = new System.Drawing.Point(26, 415);
            this.PlayPauseButton.Name = "PlayPauseButton";
            this.PlayPauseButton.Size = new System.Drawing.Size(100, 50);
            this.PlayPauseButton.TabIndex = 3;
            this.PlayPauseButton.Text = " ";
            this.PlayPauseButton.UseVisualStyleBackColor = false;
            this.PlayPauseButton.Click += new System.EventHandler(this.PlayPauseButton_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.White;
            this.button4.BackgroundImage = global::MusicPlayer.Properties.Resources.prv;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button4.Location = new System.Drawing.Point(238, 417);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 50);
            this.button4.TabIndex = 4;
            this.button4.Text = " ";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(344, 422);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(201, 45);
            this.trackBar1.TabIndex = 5;
            this.trackBar1.Value = 5;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.customWaveViewer1);
            this.panel1.Location = new System.Drawing.Point(8, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1213, 342);
            this.panel1.TabIndex = 7;
            // 
            // customWaveViewer1
            // 
            this.customWaveViewer1.AllowDrop = true;
            this.customWaveViewer1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.customWaveViewer1.Location = new System.Drawing.Point(2, 7);
            this.customWaveViewer1.Name = "customWaveViewer1";
            this.customWaveViewer1.PenColor = System.Drawing.Color.Black;
            this.customWaveViewer1.PenWidth = 1F;
            this.customWaveViewer1.SamplesPerPixel = 128;
            this.customWaveViewer1.Size = new System.Drawing.Size(1200, 326);
            this.customWaveViewer1.StartPosition = ((long)(0));
            this.customWaveViewer1.TabIndex = 7;
            this.customWaveViewer1.WaveStream = null;
            this.customWaveViewer1.DragDrop += new System.Windows.Forms.DragEventHandler(this.customWaveViewer1_DragDrop);
            this.customWaveViewer1.DragEnter += new System.Windows.Forms.DragEventHandler(this.customWaveViewer1DragAndDrop_DragEnter);
            this.customWaveViewer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customWaveViewer1_MouseDown);
            this.customWaveViewer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.customWaveViewer1_MouseMove);
            this.customWaveViewer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.customWaveViewer1_MouseUp);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(12, 366);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(930, 45);
            this.trackBar2.TabIndex = 8;
            this.trackBar2.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // StopButton
            // 
            this.StopButton.BackColor = System.Drawing.Color.White;
            this.StopButton.ForeColor = System.Drawing.Color.White;
            this.StopButton.Image = global::MusicPlayer.Properties.Resources.stop;
            this.StopButton.Location = new System.Drawing.Point(132, 415);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(100, 50);
            this.StopButton.TabIndex = 9;
            this.StopButton.Text = " ";
            this.StopButton.UseVisualStyleBackColor = false;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 486);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.PlayPauseButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private OpenFileDialog openFileDialog1;
        private DataGridView dataGridView1;
        private Button PlayPauseButton;
        private Button button4;
        private TrackBar trackBar1;
        private Panel panel1;
        private TrackBar trackBar2;
        private CustomWaveViewer customWaveViewer1;
        private Button StopButton;
        private System.Windows.Forms.Timer timer1;
    }
}