using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Imover_Servo_Ardu_led
{

    public partial class Form1 : Form
    {
        bool isConnected = false;
        SerialPort port;
        

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyValue == (char)Keys.E) button4_Click(button4, null); };
            KeyDown += (s, e) => { if (e.KeyValue == (char)Keys.R) button3_Click(button3, null); };

        }
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private void Button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            //Getting list of COM ports
            string[] portnames = SerialPort.GetPortNames();
            //Check, are there some available ports?
            if (portnames.Length == 0)
            {
                MessageBox.Show("COM Port not found");
            }
            foreach (string s in portnames)
            {
                comboBox1.Items.Add(s);
            }
        }
        private void connectToArduino()
        {
            try
            {
                isConnected = true;
                string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
                port = new SerialPort(selectedPort, 38400, Parity.None, 8, StopBits.One);
                port.Open();
                button2.Text = "Disconnect";
            }
            catch
            {
                MessageBox.Show("Появилась незапланированная ошибка", "ОШИБКА",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
            }
            
        }

        private void disconnectFromArduino()
        {
            try
            {
                isConnected = false;
                port.Close();
                button2.Text = "Connect";
            }
            catch

            {
                MessageBox.Show("Появилась незапланированная ошибка", "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                connectToArduino();
            }
            else
            {
                disconnectFromArduino();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                port.Write("#x|");
            }
            catch
            {
                MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                port.Write("#c|");
            }
            catch
            {
                MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void HScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {


                String UgolServo = "#A";
                UgolServo = UgolServo + Convert.ToString(hScrollBar1.Value, 16);
                UgolServo = UgolServo + "|";
                port.Write(UgolServo);
                label1.Text = String.Format("Угол сервопривода :{0}", hScrollBar1.Value);
            }
            catch
            {
                MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void HScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                String UgolServo1 = "#B";
                UgolServo1 = UgolServo1 + Convert.ToString(hScrollBar2.Value, 16);
                UgolServo1 = UgolServo1 + "|";
                port.Write(UgolServo1);
                port.Write(UgolServo1);
                port.Write(UgolServo1);
                label1.Text = String.Format("Угол сервопривода :{0}", hScrollBar2.Value);
            }
            catch
            {
                MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.A || e.KeyValue == (char)Keys.Left)
            {
                try
                {
                    HScrollBar1_Scroll(hScrollBar1.Value--.ToString(), null);


                }
                catch
                {
                    MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
                }
            }
            else if (e.KeyValue == (char)Keys.D || e.KeyValue == (char)Keys.Right)

            {
                try
                {
                    HScrollBar1_Scroll(hScrollBar1.Value++.ToString(), null);
                }
                catch
                {
                    MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);

                }

            }
            else if (e.KeyValue == (char)Keys.W || e.KeyValue == (char)Keys.Up)

            {
                try
                {
                    HScrollBar2_Scroll(hScrollBar2.Value++.ToString(), null);
                }
                catch
                {
                    MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);

                }

            }
            else if (e.KeyValue == (char)Keys.S || e.KeyValue == (char)Keys.Down)

            {
                try
                {
                    HScrollBar2_Scroll(hScrollBar2.Value--.ToString(), null);
                }
                catch
                {
                    //Console.WriteLine("Выход за диапозон!");
                    MessageBox.Show("Порт еще закрыт ,либо появилась незапланированная ошибка", "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
           
          
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].
             MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();
                
          
        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pic0.Image = (Bitmap)eventArgs.Frame.Clone();
            
          
            

        }
         void Stop_button_click(object sender, EventArgs e)
        {
            if (videoCaptureDevice.IsRunning == true)
            {
                
                videoCaptureDevice.SignalToStop();
                
            }
            else
            {
                MessageBox.Show("Видео уже остановлено!", "ЭЫЫЭ");
            }
            
        }
        private void VideoCaptureDevice1_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pic0.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            {
                filterInfoCollection = new FilterInfoCollection(
       FilterCategory.VideoInputDevice);
                foreach (FilterInfo filterInfo in filterInfoCollection)
                    cboCamera.Items.Add(filterInfo.Name);
                cboCamera.SelectedIndex = 0;
                videoCaptureDevice = new VideoCaptureDevice();

            }
        }
    }
}
