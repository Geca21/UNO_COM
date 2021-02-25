using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace UNO_COM
{
    public partial class Form1 : Form
    {
        bool comFound = false;
        public Form1()
        {
            InitializeComponent();
            label1.Text = null;
            lbInfo.Text = null;
            timer1.Enabled = false;
            timer2.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                /* Получаем все доступные COM порты*/
                string[] ports = SerialPort.GetPortNames();
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(ports);

                /* Получаем Бод из настроек и находим в списке */
                string BaudRate = serialPort1.BaudRate.ToString();
                int index = comboBox1.FindStringExact(BaudRate);
                comboBox1.SelectedIndex = index;

                /* Если нашли COM порты*/
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 0;
                    serialPort1.PortName = comboBox2.SelectedItem.ToString();
                    serialPort1.BaudRate = Convert.ToInt32(comboBox1.SelectedItem);
                    comFound = true;
                }
                /* Если не нашли */
                else
                {

                    label1.Text = "НЕТ ДОСТУПНЫХ COM ПОРТОВ\r\n";
                    btnOpenClose.Enabled = false;
                    comboBox2.Enabled = false;
                    comFound = false;
                }
                if (!serialPort1.IsOpen)
                {
                    ProgressBar1.Value = 0;
                    label1.Text += "СОЕДИНЕНИЕ ЗАКРЫТО";
                    label1.ForeColor = Color.DarkRed;
                }
            }
            catch (InvalidOperationException) { lbInfo.Text = "Указанный порт открыт"; }
            catch (ArgumentNullException) { lbInfo.Text = "Нет доступных COM портов";  }
            catch (ArgumentException) { lbInfo.Text = "Недопустимое имя COM порта"; }
            catch (Exception ex){label1.Text = ex.ToString();}
        }


        /* По кнопке открыть ПОРТ */
        private void btnOpenClose_Click(object sender, EventArgs e)
        {
            try {
                if (!serialPort1.IsOpen) {
                    serialPort1.Open();
                    if (serialPort1.IsOpen) {
                        textBox1.Clear();
                        ProgressBar1.Value = 100;
                        label1.ForeColor = Color.DarkGreen;
                        label1.Text = ""+ serialPort1.PortName+" ОТКРЫТ НА СКОРОСТИ " + serialPort1.BaudRate + " БОД";
                        btnClose.Enabled = true;
                        btnOpenClose.Enabled = false;
                        comboBox2.Enabled = false;
                        serialPort1.WriteLine("r1F");
                        r1 = false;
                        r2 = false;
                        r3 = false;
                        r4 = false;
                    }
                } 
            }
            catch (Exception ex) { label1.Text += ex.ToString(); }
        }

        /* По кнопке закрыть ПОРТ */
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen) {
                textBox1.Clear();
                pictureBox1.BackColor = Color.Red;
                serialPort1.WriteLine("r1F");
                 r1 = false;
                 r2 = false;
                 r3 = false;
                 r4 = false;

                serialPort1.Close();
                if (!serialPort1.IsOpen)
                {
                    ProgressBar1.Value = 0;
                    label1.Text = "СОЕДИНЕНИЕ ЗАКРЫТО";
                    label1.ForeColor = Color.DarkRed;
                    btnClose.Enabled = false;
                    btnOpenClose.Enabled = true;
                    comboBox2.Enabled = true;
                }
            }
        }


        /* При изменении БОД*/
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Items.Count > 0 && serialPort1.IsOpen)
                {
                    serialPort1.BaudRate = Convert.ToInt32(comboBox1.SelectedItem);
                    label1.Text = "" + serialPort1.PortName + " ОТКРЫТ НА СКОРОСТИ " + serialPort1.BaudRate + " БОД";
                }
                else if(comboBox1.Items.Count > 0)
                {
                    serialPort1.BaudRate = Convert.ToInt32(comboBox1.SelectedItem);
                }
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            }
        }

        
        /* При изменении COM порта*/
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.Items.Count > 0)
                {
                    serialPort1.PortName = comboBox2.SelectedItem.ToString();
                }
                else { throw new Exception("Нет доступных COM портов"); }
            }

            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            }
        }


        /* По кнопке закрыть программу */
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen) { serialPort1.Close(); }
                this.Close();
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
            }
        }

        bool r1 = false;
        bool r2 = false;
        bool r3 = false;
        bool r4 = false;
        string otvet="";

        private void btnRele1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                timer1.Enabled = true;
                r1 = !r1;
                serialPort1.WriteLine("r1" + r1);
                if (r1 )
                {
                    pictureBox1.BackColor = Color.LawnGreen;
                }
                else if (!r1 )
                {
                    pictureBox1.BackColor = Color.Red;
                }
            }
        }

        private void btnRele2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                r2 = !r2;
                serialPort1.WriteLine("r2"+r2);
                
                if (r2)
                {
                    pictureBox2.BackColor = Color.LawnGreen;
                }
                else if (!r2)
                {
                    pictureBox2.BackColor = Color.Red;
                }
            }
        }

        private void btnRele3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                r3 = !r3;
                serialPort1.WriteLine("r3"+r3);
                
                if (r3)
                {
                    pictureBox3.BackColor = Color.LawnGreen;
                }
                else if (!r3)
                {
                    pictureBox3.BackColor = Color.Red;
                }
            }
        }

        private void btnRele4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                r4 = !r4;
                serialPort1.WriteLine("r4"+r4);
                
                if (r4)
                {
                    pictureBox4.BackColor = Color.LawnGreen;
                }
                else if (!r4)
                {
                    pictureBox4.BackColor = Color.Red;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                if (serialPort1.ReceivedBytesThreshold > 0)
                {
                    otvet = serialPort1.ReadExisting();
                    otvet.Trim();
                    textBox1.Text = otvet;
                    timer1.Enabled = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            /* Получаем Бод из настроек и находим в списке */
            string BaudRate = serialPort1.BaudRate.ToString();
            int index = comboBox1.FindStringExact(BaudRate);
            comboBox1.SelectedIndex = index;
            /* Если нашли COM порты*/
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
                serialPort1.PortName = comboBox2.SelectedItem.ToString();
                serialPort1.BaudRate = Convert.ToInt32(comboBox1.SelectedItem);
                btnOpenClose.Enabled = true;
                comboBox2.Enabled = true;
                comFound = true;
            }
        }
    }
}
