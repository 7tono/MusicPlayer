using NAudio.Gui;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;



// c# NAudio waveViewer

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        AudioFileReader afr;
        CustomWaveViewer customWaveViewer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "No";
            dataGridView1.Columns[1].HeaderText = "‹È–¼";
        }

        int Column_num = 0;


        string filePath = string.Empty;
        string filePathn = string.Empty;
        string filePatho = string.Empty;
        string filePathf = string.Empty;

        WaveOutEvent outputDevice = new WaveOutEvent();
        private void button1_Click(object sender, EventArgs e)
        {

            outputDevice.Stop();

            Column_num++;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                filePath = openFileDialog1.FileName;

                filePathn = System.IO.Path.GetExtension(filePath);

                readfile(filePath, filePathn);



            }
        }

        private void readfile(string filePath, string f_ext)
        {

            if (f_ext == ".wav")

            {
                filePatho = filePath.Replace(".wav", "");
                WaveFormat format = new WaveFormat(48000, 16, 2);
                WaveFileReader reader = new WaveFileReader(filePath);


                ///
                Wave32To16Stream stream32 = null;
                Wave16ToFloatProvider stream16 = null;

                if (reader.WaveFormat.BitsPerSample == 32)//32bit‚Ì‚Æ‚«
                {

                    stream32 = new Wave32To16Stream(reader);
                    WaveFileWriter.CreateWaveFile("./newfilename.wav", stream32);
                    //return;



                }

                if (reader.WaveFormat.BitsPerSample == 16)//16bit‚Ì‚Æ‚«
                {
                    WaveFileWriter.CreateWaveFile(filePatho + "tmp.wav", reader);
                    //stream16 = new Wave16ToFloatProvider(reader);
                    //WaveFileWriter.CreateWaveFile("./newfilename.wav", stream16);
                    //return;
                }




            }

            if (f_ext == ".mp3")
            {

                filePatho = filePath.Replace(".mp3", "");
                /*
                //filePatho = filePatho.Replace(filePatho, ".wav"); 
                filePatho = filePatho + ".wav";
                AudioFileReader reader = new AudioFileReader(filePath);
                WaveFileWriter.CreateWaveFile(filePatho, reader);
                */

                WaveFormat format2 = new WaveFormat(16000, 16, 1);
                Mp3FileReader reader2 = new Mp3FileReader(filePath);

                using (WaveFormatConversionStream stream2 = new WaveFormatConversionStream(format2, reader2))
                {
                    WaveFileWriter.CreateWaveFile(filePatho + "tmp.wav", stream2);
                }
            }
            filePathf = filePatho + "tmp.wav";
            //afr = new AudioFileReader(filePath);
            //outputDevice.Init(afr);
            //outputDevice.Play();






            //dataGridView1.Rows.Add();
            dataGridView1.Rows.Add(new string[] { Column_num + "", filePath });


            customWaveViewer1.WaveStream = new WaveFileReader(filePathf);
            //customWaveViewer1.AutoScroll = true;
            customWaveViewer1.SamplesPerPixel = 400;
            customWaveViewer1.FitToScreen();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            outputDevice.Pause();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filePath == "") return;
            outputDevice.Stop();
            afr = new AudioFileReader(filePath);
            outputDevice.Init(afr);
            outputDevice.Play();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            afr.Position = 0;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            outputDevice.Volume = (float)(trackBar1.Value / 100f);

            // waveViewer1.Scroll += new System.EventHandler(this.button3_Click);
            //waveViewer1.ScrollControlIntoView =

            musiclng = (int)afr.Length;
            musiclng /= 100;
            customWaveViewer1.StartPosition = trackBar1.Value * musiclng;
            customWaveViewer1.Refresh();



            this.Text = afr.Length + "";
        }
        int musiclng = 0;


        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            musiclng = (int)afr.Length;
            musiclng /= 100;
            customWaveViewer1.StartPosition = trackBar2.Value * musiclng;
            customWaveViewer1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            outputDevice.Stop();
            afr.Position = 0;
        }




        int Msx = System.Windows.Forms.Cursor.Position.X;
        int Msy = System.Windows.Forms.Cursor.Position.Y;
        object DDDD;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            //date1 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
            int i = Convert.ToInt16(dataGridView1.CurrentCell.RowIndex);
            int ii = Convert.ToInt16(dataGridView1.CurrentCell.ColumnIndex);

            string musicstr;
            try
            {
                musicstr = dataGridView1.Rows[i].Cells[1].Value.ToString();
            }
            catch
            {
                return;
            }




            afr = new AudioFileReader(musicstr);
            outputDevice.Stop();
            outputDevice.Init(afr);
            //outputDevice.Play();

        }
        
        public Graphics g = null;
        private void customWaveViewer1_DragDrop(object sender, DragEventArgs e)
        {


            // g = customWaveViewer1.CreateGraphics();
            outputDevice.Stop();

            //g.DrawLine(Pens.Red, 100,100,100,200);

            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            for (int i = 0; i < files.Length; i++)
            {
                var fileName = files[i];

                filePath = fileName;

                filePathn = System.IO.Path.GetExtension(filePath);

                readfile(filePath, filePathn);
                //Console.Write(fileName);
            }

            //ü•`ŽÊ
            Bitmap canvas = new Bitmap(customWaveViewer1.Width, customWaveViewer1.Height);
            // Graphics g = Graphics.FromImage(canvas);
            g.DrawLine(Pens.Red, 10, 20, 100, 200);
            //g.Dispose();
            //customWaveViewer1. = canvas;

            


            
        }

        private void customWaveViewer1DragAndDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        int testp = 0;

        /*
        private void timer1_Tick(object sender, EventArgs e)
        {
            AudioFileReader reader = new AudioFileReader(filePath);

            WaveOut waveOut = new WaveOut();
            waveOut.Init(reader);
            Bitmap canvas = new Bitmap(customWaveViewer1.Width, customWaveViewer1.Height);
            

            Debug.WriteLine("fff");
            testp++;
            //customWaveViewer1.currentpoint = testp;
            double currentSec1 = reader.CurrentTime.TotalSeconds;
            double currentSec2 = (double)waveOut.GetPosition() / reader.WaveFormat.AverageBytesPerSecond;


            customWaveViewer1.setpoint(testp);
            customWaveViewer1.Refresh();
        }
        */

    }
}