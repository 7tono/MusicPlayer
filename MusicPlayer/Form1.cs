using NAudio.Gui;
using NAudio.Wave;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



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
            dataGridView1.Columns[1].HeaderText = "曲名";
        }

        int Column_num = 0;


        WaveOutEvent outputDevice = new WaveOutEvent();
        private void button1_Click(object sender, EventArgs e)
        {

            outputDevice.Stop();
            var filePath = string.Empty;
            var filePathn = string.Empty;
            var filePatho = string.Empty;
            var filePathf = string.Empty;
            Column_num++;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                filePatho = filePath = openFileDialog1.FileName;

                filePathn = System.IO.Path.GetExtension(filePath);
                if (filePathn == ".wav")
                {
                    /*
                     
                    WaveFormat format = new WaveFormat(16000, 16, 1);
                    WaveFileReader reader = new WaveFileReader("sample1.wav");

                    using (WaveFormatConversionStream stream = new WaveFormatConversionStream(format, reader))
                    {
                        WaveFileWriter.CreateWaveFile("sample2.wav", stream);
                    }

                     */


                    
                    filePatho = filePath.Replace(".wav", "");

                    WaveFormat format = new WaveFormat(48000, 16, 2);
                    WaveFileReader reader = new WaveFileReader(filePath);


                    ///
                    Wave32To16Stream stream32 = null;
                    if (reader.WaveFormat.BitsPerSample == 32)//32bitのとき
                    {
                        
                        stream32 = new Wave32To16Stream(reader);
                        WaveFileWriter.CreateWaveFile("./newfilename.wav", stream32);
                        return;
                    }


                    ///

                    using (WaveFormatConversionStream stream = new WaveFormatConversionStream(format, reader))
                    {
                        WaveFileWriter.CreateWaveFile(filePatho + "tmp.wav", stream);
                    }
                    
                }

                if (filePathn == ".mp3")
                {

                    filePatho = filePath.Replace(".mp3", "");/*
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
                afr = new AudioFileReader(filePath);
                outputDevice.Init(afr);
                outputDevice.Play();




            }

            //dataGridView1.Rows.Add();
            dataGridView1.Rows.Add(new string[] { Column_num + "", openFileDialog1.FileName });


            customWaveViewer1.WaveStream = new WaveFileReader(filePath);//
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
            outputDevice.Play();

        }

        /*
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo info = ((DataGridView)sender).HitTest(e.X, e.Y);//dataGridView1型なんかなくね？
            //↑DataGrid　じゃなくdataGridViewじゃね
            switch (info.Type)
            {
                case DataGridView.HitTestType.Cell:
                    MessageBox.Show(info.Row + " 行 "+ info.Column + " 列目がクリックされました！");
                    break;
                default:
                    MessageBox.Show("クリックした場所はセルではありません。");
                    break;
            }
        }
        */



    }
}