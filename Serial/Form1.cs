using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Serial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            cmb_name.DataSource = SerialPort.GetPortNames();// Quét các cổng COM đang hoạt động lên comboBox1
            serialPort.DataReceived +=new System.IO.Ports.SerialDataReceivedEventHandler(serialPort_DataReceived);
            
        }

        private void btn_connect_Click(object sender, EventArgs e)//Connect to COM
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.PortName = cmb_name.Text;
                    //serialPort.BaudRate = Convert.ToInt32(comboBox3.Text);
                    serialPort.Open();
                    lb_status.Text = ("Đã kết nối");
                    lb_status.ForeColor = Color.Green;
                }
             }

            catch
            {
                MessageBox.Show("khong the mo cong com");
            }
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            lb_status.Text = ("Chưa kết nối");
            lb_status.ForeColor = Color.Red;
         }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string line = serialPort.ReadTo("\n");
                this.BeginInvoke(new LineReceivedEvent(LineReceived),line);

                //txt_dem.Text = serialPort.ReadExisting();
              // String dataFromArduino = serialPort.ReadLine().ToString();
              // txt_dem.Text = dataFromArduino;
             }
            catch (Exception)
            {
                MessageBox.Show(" khong nhan duoc du lieu");
            }
        }

        private delegate void LineReceivedEvent(string line);
        private void LineReceived(string line)
        {                                            
            int do_dai = line.Length;
            String data_from_lora_slave = line.Substring(0,7);
            try
            {
                if (data_from_lora_slave == "L01-SET")
                {
                    txt_status.Text = "Recived data ok";
                }
                else if (data_from_lora_slave == "L01-DEM")
                {
                    String so_dem = line.Substring(8);
                    txt_dem.Text = so_dem;
                }
                else if (data_from_lora_slave == "L01-ONN")
                {
                    txt_status.Text = "bat dau";
                }
                else if (data_from_lora_slave == "L01-OFF")
                {
                    txt_status.Text = "ngung";
                }         
            }
           catch (Exception)
           {

           }        
        }
        private void btn_set_Click(object sender, EventArgs e)
        {
           Form frm = new Form2();
           frm.ShowDialog(); 
           String so_set = this.txt_set.Text;
           String bien_so = this.txt_bienso.Text;
           try
           {
               serialPort.Write("L01-SET-" + so_set +"|"+ bien_so +"\n");
           }
           catch (Exception)
           {
                             
           }

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Write("L01-CMD-ONN" + "\n");
            }
            catch (Exception)
            {

            }
           
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Write("L01-CMD-OFF" + "\n");
            }
            catch (Exception)
            {

            }
          
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }


        }
    }

