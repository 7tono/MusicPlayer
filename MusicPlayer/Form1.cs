using NAudio.Gui;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
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

        long mujicnum = 0;

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
            //ここでセレクト呼んでやればいいのでは？　oooooooo

            int mujicnum = dataGridView1.RowCount - 2;

            dataGridView1.CurrentCell = dataGridView1[0, mujicnum];


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
            customWaveViewer1.FitToScreen();
            trackBar2.Value = 0;
            customWaveViewer1.Refresh();
        }


        bool Playflg = false;
        bool onetime = true;
        bool stertflg = true;
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

        bool sclolflg = false;
        int slideoffset = 0;
        public void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (customWaveViewer1.WaveStream != null)
            {

                /*
                if (sclolflg)
                {

                }*/

                if (afr == null)
                {
                    //trackBar2.Value = 0;
                    if (afr == null) return;
                }




                musiclng = afr.Length;//長さ

                musiclng /= 100;//長さを１００で割り１％の値をとる
                musiclng /= 2;
                customWaveViewer1.StartPosition = trackBar2.Value * musiclng;//トラックバーはMAｘ１００なのでパーセントめから書く


                slideoffset = trackBar2.Value * 12;//　1%が１２ドット

                customWaveViewer1.setpoint((int)currentSec.X - slideoffset);
                customWaveViewer1.Refresh();
            }
            else
            {
                trackBar2.Value = 0;
                if (afr == null) return;
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
            trackBar2.Value = 0;
            Weaveview_pos = 0;
            customWaveViewer1.startPos.X = 0;
            customWaveViewer1.endPos.X = 0;
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
                musicstr = dataGridView1.Rows[i].Cells[2].Value.ToString(); //ここのエラーは対策済み
                //PlayPauseButton_Click(sender, e);


            }
            catch
            {
                return;
            }



            Restart(musicstr);
            if (Playflg) outputDevice.Play();
            // dataGridView1.CurrentCell = dataGridView1[1, 0];
            
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

        double ttlong = 1200;
        double ttn = 0;


        public Point tts;
        public Point tte;

        private void calcsec2pos(int TotalTime_sec, double currentSec1)
        {


            if (customWaveViewer1.startPos.X < customWaveViewer1.endPos.X)
            {
                tts = customWaveViewer1.startPos;
                tte = customWaveViewer1.endPos;
            }
            else
            {
                tte = customWaveViewer1.startPos;
                tts = customWaveViewer1.endPos;
            }

            ///9/25 Zoom解除後の赤線を制御するためにttsとtteを変える必要がある？


            double currentratio = TotalTime_sec / currentSec1; //今の位置への倍率 = 　曲全体の時間/　今何秒目
            double currentratiozoom;
            if (tte.X != 0)  /////sssss
            {/*
                ttm = tte.X - tts.X;     //Zoom後の全体範囲（ttm）＝　マウス＿エンド位置（tts.X）-マウス＿スタート位置（tte.X）
                ttn = (currentPos - tts.X) / ttm;   //今いる位置（％）＝（今の位置　-　マウス＿スタート位置）/　Zoom後の全体範囲
            */
                //ttm = tte.X - tts.X;     //Zoom後の全体範囲（ttm）＝　マウス＿エンド位置（tts.X）-マウス＿スタート位置（tte.X）
                // Weaveview_pos = ttm / (currentratio - tts.X);   //今いる位置（％）＝Zoom後の全体範囲 /（今の位置　-　マウス＿スタート位置）


                ttn = 1200 / currentratio; // 現在の位置ドット
                ttn = ttn - tts.X; //ズーム後の画面の位置
                ttlong = tte.X - tts.X;　//ズームの範囲

                currentratiozoom = ttlong / ttn; //ズーム後の画面のバーがいる位置の割合
                Weaveview_pos = 1200 / currentratiozoom;　// 全体のバーがいるべき場所
                Debug.WriteLine("now→" + (int)Weaveview_pos);
            }
            else
            {
                Weaveview_pos = customWaveViewer1.Width / (TotalTime_sec / currentSec1); // もともとのコード
                Debug.WriteLine("now→" + (int)Weaveview_pos);
            }


        }







        public double Weaveview_pos;
        dPoint dp = new dPoint();

        private void timer1_Tick(object sender, EventArgs e)
        {
            int TotalHours0 = afr.TotalTime.Hours * 3600;
            int TotalMinutes0 = afr.TotalTime.Minutes * 60;
            int TotalSeconds0 = afr.TotalTime.Seconds;
            int TotalTime_sec = TotalHours0 + TotalMinutes0 + TotalSeconds0;


            double currentSec1 = afr.CurrentTime.TotalSeconds;//再生したトータル秒


            currentSec.X = (int)Weaveview_pos;

            dp.X = (int)currentSec1;



            calcsec2pos(TotalTime_sec, currentSec1);


            Debug.WriteLine("⇒" + (int)Weaveview_pos);


            customWaveViewer1.setpoint((int)Weaveview_pos - slideoffset);
            customWaveViewer1.Refresh();

            if (customWaveViewer1.zoomflg) // 
            {
                if (Weaveview_pos >= 1200)//zoom中の見てるグラフの移動
                {

                    // 書き始めにttlong（zoom後の画面の範囲）を足す。



                    customWaveViewer1.startPos.X = (int)(customWaveViewer1.startPos.X + ttlong);
                    customWaveViewer1.endPos.X = (int)(customWaveViewer1.endPos.X + ttlong);
                    customWaveViewer1.mousePos.X = (int)(customWaveViewer1.mousePos.X + ttlong);



                    Weaveview_pos = 0;
                    int leftSample = customWaveViewer1.rightSample_old;
                    int rightSample = customWaveViewer1.rightSample_old + customWaveViewer1.sabun;

                    customWaveViewer1.Zoom(leftSample, rightSample);
                    customWaveViewer1.setpoint(0);
                    Weaveview_pos = 0;
                    customWaveViewer1.Refresh();
                }
            }




            if (customWaveViewer1.loopBber == currentSec.X)
            {
                if (customWaveViewer1.zoomflg)
                {
                    double QQQ = customWaveViewer1.loopAber / customWaveViewer1.SamplesPerPixel;
                    afr.Position = (long)QQQ;
                }
                else
                {
                    double PPP = customWaveViewer1.loopAber / 12;
                    double PPPP = afr.Length * PPP;
                    double PPPPP = PPPP / 100;
                    afr.Position = (long)PPPPP;
                }
                
            }
        }



        int Moveint = 0;
        int mx = 0;
        //int Rberpos =0;
        double mouse_persent;
        long viewp;
        private void customWaveViewer1_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right) return;


            if (mouseDrag == true)
            {
                if (currentSec.X < 0) currentSec.X = 0;
                if (currentSec.X > 1200) currentSec.X = 1199;
                stopf = true;
                customWaveViewer1.Refresh();
                mx = e.X;
            }

            if (loopADrag == true)
            {
                if (customWaveViewer1.loopAber < 0)
                    customWaveViewer1.loopAber = 0;
                if (customWaveViewer1.loopAber > 1200) customWaveViewer1.loopAber = 1199;
                loopADrag = false;

                if (customWaveViewer1.zoomflg)
                {
                    double QQQ = afr.Length / ((customWaveViewer1.loopAber / (customWaveViewer1.samplesPerPixel + customWaveViewer1.leftSample)) /1200);
                    //mx = e.X;
                    afr.Position = (long)QQQ;　　//10/07 赤バーの処理
                    customWaveViewer1.Refresh();

                }
                else
                {
                    mx = e.X;
                }
                
                customWaveViewer1.Refresh();
            }

            if (loopBDrag == true)
            {
                if (customWaveViewer1.loopBber < 0) customWaveViewer1.loopAber = 0;
                if (customWaveViewer1.loopBber > 1200) customWaveViewer1.loopAber = 1199;
                loopBDrag = false;
                customWaveViewer1.Refresh();
            }

            mouseDrag = false;

            if (customWaveViewer1.loopAber > customWaveViewer1.loopBber)
            {
                double loopbertnp = customWaveViewer1.loopAber;
                customWaveViewer1.loopAber = customWaveViewer1.loopBber;
                customWaveViewer1.loopBber = loopbertnp;
                //customWaveViewer1.Refresh();
            }

            if (customWaveViewer1.loopAber == customWaveViewer1.loopBber)
            {
                customWaveViewer1.loopBber += 5;
            }



            if (e.X < 0) mx = 0;

            if (customWaveViewer1.WaveStream != null && afr != null)
            {



                double onep = 1200 / 100; //Wiew画面の１％の長さ
                mouse_persent = mx / onep;//マウスは何パーセントの位置にいる？
                viewp = (long)afr.Length * (long)mouse_persent;//パーセントを掛ける
                viewp = viewp / 100;//パーセントでの位置に直す


                currentSec.X = mx;

                Debug.WriteLine("→→→→" + viewp);
                afr.Position = viewp;



                customWaveViewer1.setpoint((int)mx - slideoffset);
                //trackBar2.Value = customWaveViewer1.rightSample_old;
                customWaveViewer1.Refresh();


            }


        }

        bool stopf = false;
        bool Scrollflg = false;
        bool mouseDrag = false;

        bool loopberAflg = false;
        bool loopADrag = false;

        bool loopberBflg = false;
        bool loopBDrag = false;
        int eX_Lborder = 0;
        int eX_Rborder = 0;
        private void customWaveViewer1_MouseMove(object sender, MouseEventArgs e)
        {

            this.Cursor = System.Windows.Forms.Cursors.Arrow;

            if (customWaveViewer1.WaveStream != null)
            {
                eX_Lborder = e.X - 5;
                eX_Rborder = e.X + 5;

                if (eX_Lborder < currentSec.X - slideoffset && currentSec.X - slideoffset < eX_Rborder)
                {


                    Scrollflg = true;
                }
                else
                {
                    Scrollflg = false;
                }


                if (eX_Lborder < customWaveViewer1.loopAber && customWaveViewer1.loopAber < eX_Rborder)//loopber start
                {
                    if (eX_Lborder < currentSec.X - slideoffset && currentSec.X - slideoffset < eX_Rborder)
                    {
                        return;
                    }

                    loopberAflg = true;
                }
                else
                {
                    loopberAflg = false;
                }

                if (eX_Lborder < customWaveViewer1.loopBber && customWaveViewer1.loopBber < eX_Rborder)//loopber end
                {
                    if (eX_Lborder < currentSec.X - slideoffset && currentSec.X - slideoffset < eX_Rborder)
                    {
                        return;
                    }

                    loopberBflg = true;
                }
                else
                {
                    loopberBflg = false;
                }


                if (Scrollflg == true || loopberAflg == true || loopberBflg == true)
                {
                    this.Cursor = Cursors.Hand;
                }

            }
            else return;



            if (mouseDrag == true)
            {
                this.Cursor = System.Windows.Forms.Cursors.VSplit;

                customWaveViewer1.setpoint(e.X - slideoffset);
                currentSec.X = e.X;
                if (e.X > 1200) currentSec.X = 1199;
                else if (e.X < 0) currentSec.X = 1;
                else customWaveViewer1.Refresh();

            }


            if (loopADrag == true) //loopber start
            {
                this.Cursor = System.Windows.Forms.Cursors.VSplit;

                if (e.X > 1200) customWaveViewer1.loopAber = 1199;
                else if (e.X < 0) customWaveViewer1.loopAber = 1;
                else customWaveViewer1.loopAber = e.X;

            }


            if (loopBDrag == true) //loopber end
            {
                this.Cursor = System.Windows.Forms.Cursors.VSplit;
                if (e.X > 1200) customWaveViewer1.loopBber = 1199;
                else if (e.X < 0) customWaveViewer1.loopBber = 1;
                else customWaveViewer1.loopBber = e.X;

            }


        }

        private void customWaveViewer1_MouseDown(object sender, MouseEventArgs e)
        {
            Point mpos = e.Location;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {



                if (currentSec.X - slideoffset == mpos.X)
                {

                    Debug.WriteLine("!!!!");


                }
                if (Scrollflg == true)
                {
                    mouseDrag = true;
                }

                if (loopberAflg == true)//loopber start
                {
                    loopADrag = true;
                }

                if (loopberBflg == true)//loopber end
                {
                    loopBDrag = true;
                }
            }


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyData == Keys.Space)
            {

                PlayPauseButton_Click(sender, e);
                ActiveControl = null;
                return;


            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {


            if (!customWaveViewer1.roopflg)
            {
                pictureBox1.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\user\\source\repos\MusicPlayer\swich on.png");
                
                customWaveViewer1.roopflg = true;
                customWaveViewer1.Refresh();
            }
            else
            {
                pictureBox1.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\user\\source\repos\MusicPlayer\swich off.png");
                
                customWaveViewer1.roopflg = false;
                customWaveViewer1.Refresh();
            }
            this.Text = customWaveViewer1.roopflg + "";
        }
    }
}