using KvistShooter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KvistShooter
{
    class CKvist : CImageBase
    {
        private Rectangle _kvistHotSpot = new Rectangle();

        public CKvist()
            : base(Resources.Kvist)
        {
            _kvistHotSpot.X = Left;
            _kvistHotSpot.Y = Top;
            _kvistHotSpot.Width = 200;
            _kvistHotSpot.Height = 350;
        }

        public void Update(int X, int Y)
        {
            Left = X;
            Top = Y;
            _kvistHotSpot.X = Left + 20;
            _kvistHotSpot.Y = Top + 20;
        }
        public bool Hit(int X, int Y)
        {
            Rectangle c = new Rectangle(X, Y, 1, 1);

            if (_kvistHotSpot.Contains(c))
            {
                return true;
            }

            return false;
        }
    }
}
