using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;
using System.Diagnostics;

namespace MusicPlayer
{
    /// <summary>
    /// Control for viewing waveforms
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        public Form1 f1;

        public void setfm(Form1 f)
        {
            f1 = f;
        }



        #region �v���p�e�B
        public Color PenColor { get; set; }
        public float PenWidth { get; set; }

        #endregion

        public void FitToScreen()
        {
            if (waveStream == null) return;

            int samples = (int)(waveStream.Length / bytesPerSample);
            this.startPosition = 0;
            this.SamplesPerPixel = samples / this.Width;
        }

        public bool zoomflg = false;
        public int sabun;
        public int rightSample_old;








        public void Zoom(int leftSample, int rightSample)
        {
            Form1 ff = (Form1)(this.TopLevelControl);
            double SX = startPos.X;
            double Musicracio = 1200 / SX;
            double trackracio = 100 / Musicracio;


            if (zoomflg) //Zoom�̉���
            {
                this.FitToScreen();
                zoomflg = false;
                ff.trackBar2.Value = 0;
                //ff.tts.X = 1;
                //ff.tte.X = 1;

                startPos.X = 0;
                endPos.X = 0;
            }
            else //Zoom
            {
                this.startPosition = leftSample * bytesPerSample; // [byte]
                this.SamplesPerPixel = (rightSample - leftSample) / this.Width; // [sample/pixel]
                zoomflg = true;
                sabun = rightSample - leftSample;
                rightSample_old = rightSample;
                ff.trackBar2.Value = (int)trackracio;

            }
        }

        #region Mouse
        public Point mousePos, startPos;
        private bool mouseDrag = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                startPos = e.Location;
                mousePos = new Point(-1, 1);
                mouseDrag = true;
                DrawVerticalLine(e.X);


            }

            base.OnMouseDown(e);
        }


        bool Scrollflg = false;



        protected override void OnMouseMove(MouseEventArgs e)
        {

            if (mouseDrag)
            {
                DrawVerticalLine(e.X); // �}�E�X�ʒu�Ƀ��C���`��
                if (mousePos.X != -1) DrawVerticalLine(mousePos.X); //�O�̃��C��������
                mousePos = e.Location;


            }






            base.OnMouseMove(e);

            ///////////�}�E�X�J�[�\���ƐԐ��̈ʒu���擾���A��������悤�ɂ���B
        }

        public bool stopf = false;
        //public bool zoomflg = false;
        public Point endPos;
        public double calcsec2pos;
        double MoveDistance;
        double Mgn;
        double antMgn = 1;

        public int leftSample;
        public int rightSample;

        protected override void OnMouseUp(MouseEventArgs e)
        {


            if (mouseDrag && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                endPos = e.Location;////oooooo


                mouseDrag = false;
                DrawVerticalLine(startPos.X);

                if (mousePos.X == -1) return;
                DrawVerticalLine(mousePos.X);

                 leftSample = (int)(StartPosition / bytesPerSample + SamplesPerPixel * Math.Min(startPos.X, mousePos.X));
                 rightSample = (int)(StartPosition / bytesPerSample + SamplesPerPixel * Math.Max(startPos.X, mousePos.X));
                /////ooo
                Zoom(leftSample, rightSample);
                //

                MoveDistance = (double)(endPos.X - startPos.X);  // �}�E�X�̈ړ������@= �}�E�X�A�b�v�����ʒu - �}�E�X�_�E�������ʒu
                Mgn = 1200 / MoveDistance;�@�@           �@�@�@�@ //�g��{���@= 1200(WaveViewer�̉���) / �}�E�X�̈ړ�����
                antMgn = MoveDistance / 1200;

            }
            else if (e.Button == MouseButtons.Middle)
            {
                this.FitToScreen();
            }




            base.OnMouseUp(e);
        }
        #endregion

        private void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(
                PointToScreen(new Point(x, 0)),
                PointToScreen(new Point(x, Height)),
                Color.Black);
        }


        protected override void OnResize(EventArgs e)
        {
            this.FitToScreen();

            base.OnResize(e);
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private WaveStream waveStream;
        public int samplesPerPixel = 128;
        private long startPosition;
        public int bytesPerSample;
        /// <summary>
        /// Creates a new WaveViewer control
        /// </summary>
        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;

            this.PenColor = Color.DodgerBlue;
            this.PenWidth = 1;

        }

        /// <summary>
        /// sets the associated wavestream
        /// </summary>
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        /// 




        public bool roopflg = false;
        public double loopAber = 550;
        public double loopBber = 650;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                using (Pen linePen = new Pen(PenColor, PenWidth))
                {

                    for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short low = 0;
                        short high = 0;
                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                        if (bytesRead == 0)
                            break;
                        for (int n = 0; n < bytesRead; n += 2)
                        {
                            short sample = BitConverter.ToInt16(waveData, n);
                            if (sample < low) low = sample;
                            if (sample > high) high = sample;
                        }
                        float lowPercent = ((((float)low) - short.MinValue) / ushort.MaxValue);
                        float highPercent = ((((float)high) - short.MinValue) / ushort.MaxValue);

                        e.Graphics.DrawLine(linePen, x, this.Height * lowPercent, x, this.Height * highPercent);
                    }
                }
            }


            e.Graphics.DrawLine(Pens.Red, currentpoint, 0, currentpoint, 326);



            if (roopflg)  //�t���O��WaveViview�ɂ����グ���낳�낳�������Form1�ōs���B�@Zoom��Loop��؂�ւ����������߂�@�X/30
            {
                e.Graphics.DrawLine(Pens.Yellow, (int)loopAber, 0, (int)loopAber, 326);
                e.Graphics.DrawLine(Pens.Gold, (int)loopBber, 0, (int)loopBber, 326);
            }

            base.OnPaint(e);
        }


        //�J�[�\���̈ʒu��^����

        public int currentpoint = 0;

        public void setpoint(int berpos)
        {


            if (berpos < 0) berpos = -1;
            currentpoint = berpos;

        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}