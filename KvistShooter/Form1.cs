//#define My_Debug

using KvistShooter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KvistShooter
{
    public partial class KvistShooter : Form
    {

        const int frameNum = 8;
        const int splatNum = 1;

        bool splat = false;

        int _gameFrame = 0;
        int _splatTime = 0;

        int _cursX = 0;
        int _cursY = 0;

        CKvist _kvist;
        CSign _sign;
        CSplat _splat;
        CScoreFrame _scoreFrame;

        Random rnd = new Random();

        int _hits = 0;
        int _misses = 0;
        int _totalshots = 0;
        double _averageHits = 0;

        public KvistShooter()
        {
            InitializeComponent();

            Bitmap b = new Bitmap(Resources.Pointer);
            this.Cursor = CustomCurser.CreateCursor(b, b.Height / 2, b.Width / 2);

            _scoreFrame = new CScoreFrame() { Left = 11, Top = 10 };
            _sign = new CSign() { Left = 1700, Top = 800};
            _kvist = new CKvist() { Left = 1600, Top = 60 };
            _splat = new CSplat();
        }

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            if (_gameFrame >= frameNum)
            {
                UpdateKvist();
                _gameFrame = 0;
            }
            if (splat)
            {
                if (_splatTime >= splatNum)
                {
                    splat = false;
                    _splatTime = 0;
                    UpdateKvist();
                }
                _splatTime++;
            }
            _gameFrame++;

            this.Refresh();
        }

        private void UpdateKvist()
        {
            _kvist.Update(
                rnd.Next( Resources.Kvist.Width, this.Width - Resources.Kvist.Width),
                rnd.Next( this.Height / 4, this.Height - Resources.Kvist.Height * 2));
        }

        // Denne funktion tegner på formen
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            if (splat == true)
            {
                _splat.DrawImage(dc);
            }
            else
            {
                _kvist.DrawImage(dc);
            }


           

            _sign.DrawImage(dc);
            _scoreFrame.DrawImage(dc);


            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Stencil", 22, FontStyle.Regular);
#if My_Debug 
            //bruges til at vise hvor på skærmen markøren er

            TextRenderer.DrawText(dc, "x=" + _cursX.ToString() + ":" + "Y=" + _cursY.ToString(), _font,
                new Rectangle(310, 0, 500, 50), SystemColors.ControlText, flags);
#endif                                                                                                   //x, y, længde, bredde
            TextRenderer.DrawText(e.Graphics, "Shots:" + _totalshots.ToString(), _font, new Rectangle(90, 80, 200, 50), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Hits:" + _hits.ToString(), _font, new Rectangle(90, 110, 150, 50), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Misses:" + _misses.ToString(), _font, new Rectangle(90, 140, 180, 50), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Avg :" + _averageHits.ToString("F0") + "%", _font, new Rectangle(90, 170, 180, 50), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Skyd Kvist", _font, new Rectangle(75, 220, 200, 50), SystemColors.ControlText, flags);



            base.OnPaint(e);
        }
        // Denne funktion tager positionen fra mussemarkøren og tillægger positionerne variabler
        private void KvistShooter_MouseMove(object sender, MouseEventArgs e)
        {

            _cursX = e.X;
            _cursY = e.Y;

            this.Refresh();
        }

        private void KvistShooter_Load(object sender, EventArgs e)
        {

        }

        private void KvistShooter_MouseClick(object sender, MouseEventArgs e)
        {
            FireGun();

            if (e.X > 1724 && e.X < 1790 && e.Y > 840 && e.Y < 862)
            {
                timerGameLoop.Start();
                StartGame();
            }
            else if (e.X > 1812 && e.X < 1864 && e.Y > 840 && e.Y < 862)
            {
                timerGameLoop.Stop();
                StopGame();
            }
            else if (e.X > 1724 && e.X < 1790 && e.Y > 888 && e.Y < 910)
            {
                timerGameLoop.Stop();
                _hits = 0;
                _misses = 0;
                _totalshots = 0;
                _averageHits = 0;
                _kvist.Left = 1600;
                _kvist.Top = 60;

                ResetGame();
                
                this.Refresh();
            }
            else if (e.X > 1808 && e.X < 1858 && e.Y > 888 && e.Y < 910)
            {
                timerGameLoop.Stop();
                Application.Exit();
            }
            else
            {
                if (_kvist.Hit(e.X, e.Y))
                {
                    splat = true;
                    _splat.Left = _kvist.Left - Resources.Kvist_splat.Width / 10;
                    _splat.Top = _kvist.Top - Resources.Kvist_splat.Width / 10;

                    _hits++;
                }
                else
                {
                    _misses++;
                }
                _totalshots = _hits + _misses;
                _averageHits = (double)_hits / (double)_totalshots * 100.0;

            }
            
            
           
        }

        private void StartGame()
        {
            SoundPlayer startLyd = new SoundPlayer(Resources.Start);
            startLyd.PlaySync();
        }

        private void StopGame()
        {
            SoundPlayer stopLyd = new SoundPlayer(Resources.Stop);
            stopLyd.PlaySync();
        }

        private void ResetGame()
        {
            SoundPlayer resetGame = new SoundPlayer(Resources.Reset);
            resetGame.PlaySync();
        }

        private void FireGun()
        {
            SoundPlayer simpleSound = new SoundPlayer(Resources.smg);
            simpleSound.Play();
        }
    }
}
