﻿using Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SharedForms
{
    public partial class sms : UserControl
    {
        ImageAttributes imgAtt;
        private Point[] piccyBounds;

        Brush blackBrush = Brushes.Black;
        Font titleFont = new Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        // Font contentFont = new Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        Image topimg = Resource1.lt;
        Image middleimg = Resource1.lm;
        Image bottomimg = Resource1.lb;
        Image topimgR = Resource1.rt;
        Image middleimgR = Resource1.rm;
        Image bottomimgR = Resource1.rb;
        Image imgTech = Resource1.老师;
        Image imgStu = Resource1.学生;
        string _message;
        string _title;
        bool _isMySelf;
        int _messageHeight;
        int _sizeHeight;
        public bool IsMySelf { get { return _isMySelf; } }
        public readonly int SizeWidth = 420;
        Image headIcon;
        bool messageIsString = false;
        public sms(ChatMessage messageInfo, bool isMySelf)
        {
            _title = messageInfo.Title;
            _message = messageInfo.Message;
            _isMySelf = isMySelf;
            InitializeComponent();

            if (messageInfo.MessageType == Common.MessageType.String)
            {
                messageIsString = true;
                this.txtSMS.Show();
                this.txtLink.Hide();
            }
            else
            {
                messageIsString = false;
                this.txtSMS.Hide();
                this.txtLink.Show();
                this.txtLink.LinkClicked += TxtLink_LinkClicked;
            }
            if (messageInfo.UserType == Common.ClientRole.Student)
            {
                headIcon = imgStu;
            }
            else
            {
                headIcon = imgTech;
            }
        }

        private void TxtLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileHelper.DownloadFile(this.txtLink.Text);
        }

        private void sms_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality; //高质量 
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
            piccyBounds = new Point[3];
            int ptop = 0;// 4;
            int pleft = 0;// 5;
            Rectangle rectArea;
            if (IsMySelf)
            {
                g.DrawString(_title, titleFont, blackBrush, new PointF(pleft, ptop));
            }
            else
            {
                g.DrawString(_title, titleFont, blackBrush, new PointF(pleft + 40, ptop));
            }
            ptop += 20;

            if (IsMySelf)
            {
                rectArea = new Rectangle(pleft + 388, _sizeHeight - 32, 32, 32);
                g.DrawImage(headIcon, rectArea);
            }
            else
            {
                rectArea = new Rectangle(pleft, _sizeHeight - 32, 32, 32);
                g.DrawImage(headIcon, rectArea);
                pleft += 32;
            }
            rectArea = new Rectangle(pleft, ptop, 388, 17);
            if (IsMySelf)
            {
                g.DrawImage(topimgR, rectArea);
            }
            else
            {
                g.DrawImage(topimg, rectArea);
            }
            ptop += 17;
            piccyBounds[0] = new Point(pleft, ptop);
            piccyBounds[1] = new Point(388 + pleft, ptop);
            piccyBounds[2] = new Point(pleft, ptop + _messageHeight);
            if (IsMySelf)
            {
                g.DrawImage(middleimgR, piccyBounds, new Rectangle(0, 0, 388, _messageHeight), GraphicsUnit.Pixel, imgAtt);
            }
            else
            {
                g.DrawImage(middleimg, piccyBounds, new Rectangle(0, 0, 388, _messageHeight), GraphicsUnit.Pixel, imgAtt);
            }
            ptop += _messageHeight;
            rectArea = new Rectangle(pleft, ptop, 388, 17);
            if (IsMySelf)
            {
                g.DrawImage(bottomimgR, rectArea);
            }
            else
            {
                g.DrawImage(bottomimg, rectArea);
            }
        }

        private void sms_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            imgAtt = new ImageAttributes();
            imgAtt.SetWrapMode(WrapMode.Tile);
            var textSize = this.CreateGraphics().MeasureString(_message, messageIsString ? txtSMS.Font : txtLink.Font);
            int count = (int)Math.Floor(textSize.Width / 389) + (textSize.Width % 389 == 0 ? 0 : 1);
            _messageHeight = count * 20 + 10;
            _sizeHeight = 20 + 17 * 2 + _messageHeight;
            Size = new Size(388 + 32, _sizeHeight + 5);
            if (IsMySelf)
            {
                if (messageIsString)
                {
                    txtSMS.Location = new Point(7, 37);
                    txtSMS.BackColor = Color.FromArgb(198, 225, 252);
                }
                else
                {
                    txtLink.Location = new Point(7, 37);
                    txtLink.BackColor = Color.FromArgb(198, 225, 252);
                }

            }
            else
            {
                if (messageIsString)
                {
                    txtSMS.Location = new Point(42, 37);
                    txtSMS.BackColor = Color.FromArgb(244, 244, 244);
                }
                else
                {
                    txtLink.Location = new Point(42, 37);
                    txtLink.BackColor = Color.FromArgb(244, 244, 244);
                }


            }
            if (messageIsString)
            {
                txtSMS.Size = new Size(388 - 10 - 5, _messageHeight);
                txtSMS.Text = _message;
            }
            else
            {
                txtLink.Size = new Size(388 - 10 - 5, _messageHeight);
                txtLink.Text = _message;
            }

        }
    }
}
