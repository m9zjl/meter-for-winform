﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Meter {
    public partial class UserControl1 : UserControl {
        public UserControl1() {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint , true);
        }
        private int _value;
        private int _bgvalue;

        public int BGValue {
            get { return _bgvalue; }
            set {
                value = value%360;
                _bgvalue = value;
            }
        }
        public int Value {
            get { return _value; }
            set {
                if (value>=0&value<=100){
                    _value = value;
                }
                Bitmap a = new Bitmap(pictureBox1.Image);//得到图片框中的图片  
                
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Invalidate();
            }
        }

        private Image Rotate(Bitmap b , int angle) {
            angle = angle % 360;

            //弧度转换  
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高  
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin) , Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos) , Math.Abs(w * sin + h * cos)));

            //目标位图  
            Bitmap dsImage = new Bitmap(W , H);
            Graphics g = Graphics.FromImage(dsImage);

            g.InterpolationMode = InterpolationMode.Bilinear;

            g.SmoothingMode = SmoothingMode.HighQuality;

            //计算偏移量  
            Point Offset = new Point((W - w) / 2 , (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致  
            Rectangle rect = new Rectangle(Offset.X , Offset.Y , w , h);
            Point center = new Point(rect.X + rect.Width / 2 , rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X , center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移  
            g.TranslateTransform(-center.X , -center.Y);
            g.DrawImage(b , rect);

            //重至绘图的所有变换  
            g.ResetTransform();

            g.Save();
            g.Dispose();
            return dsImage;  
        }

    }
}
