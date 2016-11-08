using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Meter {
    public partial class Lock : UserControl {
        public Lock() {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint , true);
            UpdateStyles();
        }

        //字段
        private int _value=0;
        private int _bgvalue=0;
        private Image _backGroudImage=Properties.Resources.heading_yaw__1_;
        private Image _forGroundImage = Properties.Resources.heading_mechanics;

        public Image BackImage {
            set {
                if (value.Height==value.Width) {
                    _backGroudImage = value;
                }
            }
        }
        public int BGValue {
            get { return _bgvalue; }
            set {
                value = value%360;
                _bgvalue = value;
                pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox2.BackgroundImage = rotateImage1(Properties.Resources.heading_yaw__1_ , _bgvalue);
            }
        }
        public int Value {
            get { return _value; }
            set {
                value = (value)%360;
                _value = value;
//                pictureBox2.SizeMode = PictureBoxSizeMode.;
//                pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;

//                pictureBox2.Image = RotaImage(Properties.Resources.未标题_11 , _value);
                pictureBox2.Image = rotateImage1(Properties.Resources.fi_needle , _value);
                pictureBox2.Invalidate();
            }
        }

        public static Image RotaImage(Image image, int angle) {
            angle = angle%360;
            double radian = angle*Math.PI/180;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            int w = image.Width;
            int h = image.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin) , Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos) , Math.Abs(w * sin + h * cos)));  
            // 目标位图
            Bitmap dsImage = new Bitmap(W,H);
            System.Drawing.Graphics g = Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;  
            //偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造显示区域 与元图像中心一致
            Rectangle rect = new Rectangle(Offset.X , Offset.Y , w , h);
            Point center = new Point(rect.X + rect.Width / 2 , rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X , center.Y);
            g.RotateTransform(360 - angle);
            //回复图形在水平垂直位置
            g.TranslateTransform(-center.X , -center.Y);
            g.DrawImage(image , rect);
            //重绘所有变换
            g.ResetTransform();
            //释放资源
            g.Save();
            g.Dispose();
            image.Dispose();
            return dsImage;
        }
        // 算法对 没有改大小
        public static Bitmap rotateImage1(Bitmap b , float angle) {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width , b.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
//            g.Clear(Color.Black);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
//            g.InterpolationMode = InterpolationMode.Default;
            //move rotation point to center of image
//            g.TranslateTransform((float)b.Width / 2 , (float)b.Height / 2);
            g.TranslateTransform((float)b.Width/2,(float)b.Height/2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width/2  , -(float)b.Height/2 );
            //draw passed in image onto graphics object
            
            
            g.DrawImageUnscaledAndClipped(b,new Rectangle(0,0,b.Width,b.Height));
//            g.DrawImage(b , new Point(0 , 0));
            return returnBitmap;
        }
    }
}
