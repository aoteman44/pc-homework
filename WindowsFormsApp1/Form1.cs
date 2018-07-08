using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private bool blIsDrawRectangle = true;
        private Point ptBegin = new Point();
        Thread thDraw;
        delegate void myDrawRectangel();
        myDrawRectangel myDraw;

        private Rectangle _SelectedRectangle;

        PictureBox pb = new PictureBox();

        public Form1()
        {
            InitializeComponent();
        }


        private void PictureBox1_Load(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
        }

        private void Run()
        {
            while (isEnd)
            {
                if (pictureBox1.Image != null)
                {
                    this.BeginInvoke(myDraw);
                }
                Thread.Sleep(50);
            }
        }

        private void ShowDrawRectangle()
        {
            Rectangle rec = new Rectangle(ptBegin.X * pictureBox1.Image.Size.Width / 375,
                ptBegin.Y * pictureBox1.Image.Size.Height / 332, 50 * pictureBox1.Image.Size.Width / 375,
                50 * pictureBox1.Image.Size.Height / 332);
            Graphics g = pictureBox2.CreateGraphics();
            g.DrawImage(pictureBox1.Image, pictureBox2.ClientRectangle, rec, GraphicsUnit.Pixel);
            g.Flush();
        }


        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (blIsDrawRectangle)
            {
                e.Graphics.DrawArc(new Pen(Brushes.Black, 1), ptBegin.X, ptBegin.Y, 50, 50, 0, 360);
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.Location = new Point(e.X + 50, e.Y - 100);

            if (e.X - 25 <= 0)
            {
                ptBegin.X = 0;
            }
            else if (pictureBox1.Size.Width - e.X <= 25)
            {
                ptBegin.X = pictureBox1.Size.Width - 50;
            }
            else
            {
                ptBegin.X = e.X - 25;
            }

            if (e.Y - 25 <= 0)
            {
                ptBegin.Y = 0;
            }
            else if (pictureBox1.Size.Height - e.Y <= 25)
            {
                ptBegin.Y = pictureBox1.Size.Height - 50;
            }
            else
            {
                ptBegin.Y = e.Y - 25;
            }

            pictureBox1.Refresh();
        }

        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            blIsDrawRectangle = false;
            pictureBox1.Refresh();
        }

        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            blIsDrawRectangle = true;
        }


        bool isEnd = false;
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isEnd = true;
            if (isEnd)
            {
                pictureBox2.Visible = true;
                myDraw = new myDrawRectangel(ShowDrawRectangle);
                thDraw = new Thread(Run);
                thDraw.Start();
            }

        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isEnd = false;
            //pictureBox2.Refresh();
            pictureBox2.Visible = false;
        }

        private void PictureBox2_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath gp = new GraphicsPath();

            gp.AddEllipse(pictureBox2.ClientRectangle);

            Region region = new Region(gp);

            pictureBox2.Region = region;

            var g = e.Graphics;
            Pen pp = new Pen(Color.Blue, 5);

            g.DrawEllipse(pp, pictureBox2.ClientRectangle);

            gp.Dispose();

            region.Dispose();
        }
    }
}

