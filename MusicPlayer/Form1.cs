using NAudio.Gui;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


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
            customWaveViewer = new CustomWaveViewer();
            customWaveViewer.setfm(this);
            customWaveViewer.f1 = this;


            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].HeaderText = "No";
            dataGridView1.Columns[1].HeaderText = "曲名";
            dataGridView1.Columns[2].HeaderText = "";
            dataGridView1.Columns[2].Width = 0;
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
                mx = 0;

            }
            
        }

        string filePath_replay;
        private void readfile(string filePath, string f_ext)
        {

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[2].Value == null) continue;

                // object Data = dataGridView1.Rows[0].Cells[i].Value;

                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == filePath) return;
            }


            if (f_ext == ".wav")

            {
                filePatho = filePath.Replace(".wav", "");
                WaveFormat format = new WaveFormat(48000, 16, 2);
                WaveFileReader reader = new WaveFileReader(filePath);


                ///
                Wave32To16Stream stream32 = null;
                Wave16ToFloatProvider stream16 = null;

                if (reader.WaveFormat.BitsPerSample == 32)//32bitのとき
                {

                    stream32 = new Wave32To16Stream(reader);
                    WaveFileWriter.CreateWaveFile("./newfilename.wav", stream32);
                    //return;

                }

                if (reader.WaveFormat.BitsPerSample == 16)//16bitのとき
                {
                    WaveFileWriter.CreateWaveFile(filePatho + "tmp.wav", reader);

                }

            }

            if (f_ext == ".mp3")
            {

                filePatho = filePath.Replace(".mp3", "");


                WaveFormat format2 = new WaveFormat(16000, 16, 1);
                Mp3FileReader reader2 = new Mp3FileReader(filePath);

                using (WaveFormatConversionStream stream2 = new WaveFormatConversionStream(format2, reader2))
                {
                    WaveFileWriter.CreateWaveFile(filePatho + "tmp.wav", stream2);
                }
            }
            filePathf = filePatho + "tmp.wav";


            filePath_replay = filePathf;



            dataGridView1.Rows.Add(new string[] { Column_num + "", filePath, filePath_replay });


            customWaveViewer1.WaveStream = new WaveFileReader(filePathf);


            customWaveViewer1.SamplesPerPixel = 400;
            customWaveViewer1.FitToScreen();
            mx = 0;
        }

        private void Restart(string musicstr)
        {
            afr = new AudioFileReader(musicstr);////tmpをみる　２れつめは曲名

            outputDevice.Stop();
            outputDevice.Init(afr);

            customWaveViewer1.WaveStream = new WaveFileReader(musicstr);

            customWaveViewer1.Refresh();
        }


        bool Playflg = false;
        bool onetime = true;

        double startpos = 0;

        public void PlayPauseButton_Click(object sender, EventArgs e)
        {


            if (Playflg) // playflg が　true で Pause させる時
            {
                outputDevice.Pause();
                timer1.Enabled = false;
                //onetime = true;
                PlayPauseButton.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\user\source\repos\MusicPlayer\play.png");


            }
            else　　　　 // playflg が　false で play させる時
            {

                if (filePath == "") return;




                DataGridViewRow r = dataGridView1.SelectedRows[0];

                Debug.WriteLine(r.Index);

                if (onetime)
                {
                    string dbselect = dataGridView1.Rows[r.Index].Cells[2].Value.ToString();
                    afr = new AudioFileReader(dbselect);

                    outputDevice.Init(afr);
                    //mx = 0;
                    onetime = false;
                }


                PlayPauseButton.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\user\source\repos\MusicPlayer\pose.png");

                outputDevice.Play();
                timer1.Enabled = true;
                if (mx != 0)
                {

                    afr.Position = afr.Length * mx / 1200;
                    mx = 0;

                }






            }
            Playflg = !Playflg;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (afr == null) return;


            afr.Position = afr.Length / 2;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (afr == null) return;
            outputDevice.Volume = (float)(trackBar1.Value / 100f);



            // musiclng = (int)afr.Length;
            // musiclng /= 100;

        }
        long musiclng = 0;


        int slideoffset = 0;
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (customWaveViewer1.WaveStream != null)
            {

                musiclng = afr.Length;//長さ

                musiclng /= 100;//長さを１００で割り１％の値をとる
                musiclng /= 2;
                customWaveViewer1.StartPosition = trackBar2.Value * musiclng;//トラックバーはMAｘ１００なのでパーセントめから書く


                slideoffset = trackBar2.Value * 12;//　1%が１２ドット

                customWaveViewer1.setpoint((int)currentSec.X - slideoffset);
                customWaveViewer1.Refresh();
            }

        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (afr == null) return;
            outputDevice.Stop();
            afr.Position = 0;
            mx = 0;
            timer1.Enabled = false;

            currentSec.X = 0;
            // slideoffset = 0;

            customWaveViewer1.setpoint(-slideoffset);
            customWaveViewer1.Refresh();
            customWaveViewer1.FitToScreen();


            if (Playflg == true) PlayPauseButton_Click(sender, e);

        }




        int Msx = System.Windows.Forms.Cursor.Position.X;
        int Msy = System.Windows.Forms.Cursor.Position.Y;
        object DDDD;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {


            int i = Convert.ToInt16(dataGridView1.CurrentCell.RowIndex);
            int ii = Convert.ToInt16(dataGridView1.CurrentCell.ColumnIndex);

            string musicstr;
            try
            {
                musicstr = dataGridView1.Rows[i].Cells[3].Value.ToString(); //ここのエラーは対策済み
            }
            catch
            {
                return;
            }


            Restart(musicstr);

        }

        public Graphics g = null;
        private void customWaveViewer1_DragDrop(object sender, DragEventArgs e)
        {


            outputDevice.Stop();



            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            for (int i = 0; i < files.Length; i++)
            {
                var fileName = files[i];

                filePath = fileName;

                filePathn = System.IO.Path.GetExtension(filePath);

                readfile(filePath, filePathn);
                //Console.Write(fileName);
            }



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
        Point currentSec;//画面のドット

        private class dPoint
        {

            public double X;
            public double Y;
            int daiji;
            public dPoint()
            {
                X = 0.0;
                Y = 0.0;
                daiji = (int)(X + Y);
            }
            void daijikeisan()
            {
                daiji = (int)(X + Y);
            }
        }



        double Weaveview_pos;
        dPoint dp = new dPoint();
        private void timer1_Tick(object sender, EventArgs e)
        {


            int TotalHours0 = afr.TotalTime.Hours * 3600;
            int TotalMinutes0 = afr.TotalTime.Minutes * 60;
            int TotalSeconds0 = afr.TotalTime.Seconds;
            int TotalTime_sec = TotalHours0 + TotalMinutes0 + TotalSeconds0;


            double currentSec1 = afr.CurrentTime.TotalSeconds;//再生したトータル秒
            //double currentSec1  = (double)outputDevice.GetPosition() / afr.WaveFormat.AverageBytesPerSecond;//再生したトータル秒

            currentSec.X = (int)Weaveview_pos;

            dp.X = (int)currentSec1;


            //ドット　　　　　　　　　　　今の位置(バイト)　　　　　　　　　　　１秒あたりのバイト数
            Weaveview_pos = customWaveViewer1.Width / (TotalTime_sec / currentSec1);


            //Weaveviwの横幅は？　上を使ってどの位置に縦線書くとよい？
            customWaveViewer1.setpoint((int)Weaveview_pos - slideoffset);
            customWaveViewer1.Refresh();

            Debug.WriteLine("→" + (int)Weaveview_pos);
        }



        int Moveint = 0;
        int mx = 0;
        //int Rberpos =0;
        double mouse_persent;
        double viewp;
        private void customWaveViewer1_MouseUp(object sender, MouseEventArgs e)
        {

            if (mouseDrag == true)
            {
                stopf = true;
                mx = e.X;
            }

            mouseDrag = false;

           
            if (e.X < 0) mx = 0;

            if (customWaveViewer1.WaveStream != null && afr != null)
            {



                double onep = 1200 / 100;
                mouse_persent = mx / onep;
                viewp = afr.Length * mouse_persent;
                viewp = viewp / 100;
                int Rberpos = (int)viewp;



                currentSec.X = mx;

                Debug.WriteLine("→→→→" + Rberpos);
                afr.Position = Rberpos;



                customWaveViewer1.setpoint((int)mx - slideoffset);
                customWaveViewer1.Refresh();


            }


        }

        bool stopf = false;
        bool Scrollflg = false;
        bool mouseDrag = false;
        int eX_Lborder = 0;
        int eX_Rborder = 0;
        private void customWaveViewer1_MouseMove(object sender, MouseEventArgs e)
        {


            if (customWaveViewer1.WaveStream != null)
            {
                eX_Lborder = e.X - 5;
                eX_Rborder = e.X + 5;

                if (eX_Lborder < currentSec.X - slideoffset && currentSec.X - slideoffset < eX_Rborder)
                {

                    this.Cursor = Cursors.Hand;
                    Scrollflg = true;
                }
                else
                {

                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                    Scrollflg = false;
                }
            }
            else return;



            if (mouseDrag == true)
            {
                this.Cursor = System.Windows.Forms.Cursors.VSplit;

                customWaveViewer1.setpoint(e.X - slideoffset);
                currentSec.X = e.X;
                customWaveViewer1.Refresh();

            }
        }

        private void customWaveViewer1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

                Point mpos = e.Location;

                if (currentSec.X - slideoffset == mpos.X)
                {

                    Debug.WriteLine("!!!!");


                }
                if (Scrollflg == true)
                {
                    mouseDrag = true;
                }

            }


        }


    }
}