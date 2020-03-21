using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace Hardware
{
    public partial class AnaForm : Form
    {
        public AnaForm()
        {
            InitializeComponent();
        }

        int check2 = 0;
        public int check = 0;

        private void AnaForm_Load(object sender, EventArgs e)
        {
            //////////////////////RAM/////////////////////////////
            int type = 0;
            var searcher = new ManagementObjectSearcher("Select * from Win32_PhysicalMemory");
            try
            {
                string Manufacturer = "";
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_PhysicalMemory");
                foreach (ManagementObject MO in MOS.Get())
                {
                    Manufacturer = MO["Manufacturer"].ToString();
                }
                label_RamGB.Text = label_RamGB.Text + Manufacturer + " ";
            }
            catch (Exception){}
            try
            {
                ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
                foreach (ManagementObject Mobject in Search.Get())
                {
                    double Ram_Bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));
                    double ramgb = Ram_Bytes / 1073741824;
                    double islem = Math.Ceiling(ramgb);
                    label_RamGB.Text = " " + label_RamGB.Text + islem.ToString() + " GB ";
                }
            }
            catch (Exception) { }
            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    type = Int32.Parse(obj.GetPropertyValue("MemoryType").ToString());
                }
                string Stype = "";
                if (type >= 24 || type == 0)
                {
                    Stype = "DDR3";
                }
                else if (type == 21)
                {
                    Stype = "DDR2";
                }
                else if (type == 20)
                {
                    Stype = "DDR";
                }
                else if (type == 17)
                {
                    Stype = "SDRAM";
                }
                else
                {
                    Stype = "(Unkown or DDR4)";
                }
                label_RamGB.Text = label_RamGB.Text; //+ Stype + " ";       Working Wrong!
            }
            catch (Exception) { }
            try
            {
                string Speed = "";
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_PhysicalMemory");
                foreach (ManagementObject MO in MOS.Get())
                {
                    Speed = MO["Speed"].ToString();
                }
                label_RamGB.Text = label_RamGB.Text + Speed + " Mhz";
            }
            catch (Exception) { }
            //////////////////////RAM/////////////////////////////

            //////////////////////CPU/////////////////////////////
            try
            {
                string CPU = "";
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_Processor");
                foreach (ManagementObject MO in MOS.Get())
                {
                    CPU = MO["Name"].ToString();
                }
                label_Cpu.Text = CPU;
            }
            catch (Exception) { }
            //////////////////////CPU/////////////////////////////

            //////////////////////GPU/////////////////////////////
            try
            {
                string VideoController = "";
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_VideoController");
                foreach (ManagementObject MO in MOS.Get())
                {
                    VideoController = MO["Caption"].ToString();
                }
                label_VideoCard.Text = VideoController;
            }
            catch (Exception) { }
            //////////////////////Ekran-Kartı/////////////////////////////

            //////////////////////Mainboard/////////////////////////////
            string Mainboard = "";
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_BaseBoard");
                foreach (ManagementObject MO in MOS.Get())
                {
                    Mainboard = MO["Manufacturer"].ToString();
                }
            }
            catch (Exception) { }
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_BaseBoard");
                foreach (ManagementObject MO in MOS.Get())
                {
                    Mainboard = Mainboard + " Model:" + MO["Product"].ToString();
                }
            }
            catch (Exception) { }
            label_Anakart.Text = Mainboard;
            //////////////////////Disc-Space/////////////////////////////
            DiscSpace(0);
            domainUpDown1.SelectedIndex = 0;
            domainUpDown1.Items.Reverse();
            /////////////////////////Disc-Space/////////////////////////////

         ////////////////////////////Internet-Check/////////////////////////////////
            check2 = 0;
            try
            {
                System.Net.Sockets.TcpClient check = new System.Net.Sockets.TcpClient("www.google.com.tr", 80);
                check.Close();
                Internet.Text = "Internet Connection : YES";
                Internet.ForeColor = Color.LightGreen;
                check2++;
            }
            catch (Exception)
            {
                Internet.Text = "Internet Connection : NO";
                Internet.ForeColor = Color.Red;
            }
        }
        ////////////////////////////Internet-Check/////////////////////////////////

        void DiscSpace(int x)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            domainUpDown1.Items.Clear();
            if (x == 1)
            {
                for (int i = 0; i < allDrives.Length; i++)
                {
                    try
                    {
                        domainUpDown1.Items.Add("Disc " + (i + 1).ToString() + ": " + ((allDrives[i].TotalSize) / 1073741824).ToString() + " GB " + allDrives[0].DriveFormat.ToString());
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                for (int i = 0; i < allDrives.Length; i++)
                {
                    try
                    {
                        domainUpDown1.Items.Add("Disc " + (i + 1).ToString() + ": " + ((allDrives[i].AvailableFreeSpace) / 1073741824).ToString() + " GB " + allDrives[0].DriveFormat.ToString());
                    }
                    catch (Exception) { }                    
                }
            }
        }

        ////////////////////////////Disc-Space-Checkbox/////////////////////////////////
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                DiscSpace(1);
            }
            else if (checkBox1.Checked == false)
            {
                DiscSpace(0);
            }
            try
            {
                domainUpDown1.SelectedIndex = domainUpDown1.SelectedIndex + 1;
                domainUpDown1.SelectedIndex = domainUpDown1.SelectedIndex - 1;
            }
            catch (Exception)
            {
                domainUpDown1.SelectedIndex = domainUpDown1.SelectedIndex - 1;
                domainUpDown1.SelectedIndex = domainUpDown1.SelectedIndex + 1;
            }
            domainUpDown1.Items.Reverse();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
