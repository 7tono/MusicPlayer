using NAudio.Wave;
using System.Drawing;
using System.Windows.Forms;



// c# NAudio waveViewer

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        AudioFileReader afr;


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
        

        WaveOutEvent outputDevice = new WaveOutEvent();
        private void button1_Click(object sender, EventArgs e)
        {

            var filePath = string.Empty;
            var filePathn = string.Empty;
            var filePatho = string.Empty;
            Column_num++;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                
                filePatho =filePath = openFileDialog1.FileName;

                filePathn = System.IO.Path.GetExtension(filePath);


                if (filePathn == ".mp3")
                {

                    filePatho = filePath.Replace(".mp3", "");
                    //filePatho = filePatho.Replace(filePatho, ".wav"); 
                    filePatho = filePatho + ".wav";
                    AudioFileReader reader = new AudioFileReader(filePath);
                    WaveFileWriter.CreateWaveFile(filePatho, reader);
                    
                }
                
                afr = new AudioFileReader(filePatho);
                outputDevice.Init(afr);
                outputDevice.Play();
                

                

            }

            //dataGridView1.Rows.Add();
            dataGridView1.Rows.Add(new string[] { Column_num + "", openFileDialog1.FileName });


            waveViewer1.WaveStream = new WaveFileReader(filePatho);//
            
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
            waveViewer1.StartPosition = trackBar1.Value * musiclng;
            waveViewer1.Refresh();



            this.Text=afr.Length+"";
        }
        int musiclng = 0;
       

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            musiclng = (int)afr.Length;
            musiclng /= 100;
            waveViewer1.StartPosition = trackBar2.Value * musiclng;
            waveViewer1.Refresh();
        }



        /*
        int Msx = System.Windows.Forms.Cursor.Position.X;
        int Msy = System.Windows.Forms.Cursor.Position.Y;
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        */
    }
}