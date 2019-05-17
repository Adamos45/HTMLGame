using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.IO;
using Accord.Video.FFMPEG;
using AForge;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using DotImaging;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        FilterInfoCollection videoDevicesList;
        VideoCaptureDevice cameraOne;
        VideoFileWriter writer;
        private bool record;
        private bool button1WasClicked = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void CameraOne_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap1 = (Bitmap) (eventArgs.Frame.DeepClone());
            Grayscale filter = new Grayscale(0.2,0.7,0.07);
            if(checkBox1.Checked)
                bitmap1 = filter.Apply(bitmap1);
            int x=0;
            Invoke(new MethodInvoker(delegate
            {
                x = trackBar1.Value;
            }));
            RotateNearestNeighbor filterrot = new RotateNearestNeighbor(x,true);
            bitmap1 = filterrot.Apply(bitmap1);
            ResizeBicubic filterResizeBicubic = new ResizeBicubic(320,240);
            bitmap1 = filterResizeBicubic.Apply(bitmap1);
            if (button1WasClicked)
            {
                Convolution conv = new Convolution(new[,]
                {
                    {int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text)},
                    {int.Parse(textBox4.Text), int.Parse(textBox5.Text), int.Parse(textBox6.Text)},
                    {int.Parse(textBox7.Text), int.Parse(textBox8.Text), int.Parse(textBox9.Text)}
                });
                bitmap1 = conv.Apply(bitmap1);
            }
            pictureBox1.Image = bitmap1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cameraOne = new VideoCaptureDevice(videoDevicesList[0].MonikerString);
            cameraOne.NewFrame += CameraOne_NewFrame;
            cameraOne.Start();
            
        }
      
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cameraOne.Stop();
            Bitmap picture = (Bitmap)pictureBox1.Image;
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
            picture.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            fs.Close();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1WasClicked = true;
        }
    }
}
