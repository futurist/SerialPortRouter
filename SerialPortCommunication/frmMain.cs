using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PCComm;
namespace PCComm
{
    public partial class frmMain : Form
    {
        CommunicationManager comm = new CommunicationManager();
        CommunicationManager comm2 = new CommunicationManager();
        string transType = string.Empty;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
           LoadValues();
           SetDefaults();
           SetControlState();

           comm.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Hex;
           comm2.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Hex;

           comm.onData = forward1;
           comm2.onData = forward2;
        }


        int forward1(String abc)
        {
            comm2.WriteData(abc);
            return 1;
        }

        int forward2(String abc)
        {
            comm.WriteData(abc);
            return 1;
        }

        private void cmdOpen_Click(object sender, EventArgs e)
        {
            comm.name = "[Left]";
            comm.PortName = cboPort.Text;
            comm.Parity = cboParity.Text;
            comm.StopBits = cboStop.Text;
            comm.DataBits = cboData.Text;
            comm.BaudRate = cboBaud.Text;
            comm.DisplayWindow = rtbDisplay;
            comm.OpenPort();

            comm2.name = "[RIGHT]";
            comm2.PortName = cboPort2.Text;
            comm2.Parity = cboParity2.Text;
            comm2.StopBits = cboStop2.Text;
            comm2.DataBits = cboData2.Text;
            comm2.BaudRate = cboBaud2.Text;
            comm2.DisplayWindow = rtbDisplay2;
            comm2.OpenPort();

            CheckForIllegalCrossThreadCalls = false;


            if (true == comm.isPortOpen)
            {
                cmdOpen.Enabled = false;
                cmdClose.Enabled = true;
                cmdSend.Enabled = true;
                txtSend.Enabled = true;
            }

            if (true == comm2.isPortOpen)
            {
                cmdSend2.Enabled = true;
                txtSend2.Enabled = true;
            }


        }

        /// <summary>
        /// Method to initialize serial port
        /// values to standard defaults
        /// </summary>
        private void SetDefaults()
        {
            cboPort.SelectedIndex = 0;
            cboBaud.SelectedText = "9600";
            cboParity.SelectedIndex = 0;
            cboStop.SelectedIndex = 1;
            cboData.SelectedIndex = 1;

            cboPort2.SelectedIndex = 1;
            cboBaud2.SelectedText = "9600";
            cboParity2.SelectedIndex = 0;
            cboStop2.SelectedIndex = 1;
            cboData2.SelectedIndex = 1;


        }

        /// <summary>
        /// methos to load our serial
        /// port option values
        /// </summary>
        private void LoadValues()
        {
            comm.SetPortNameValues(cboPort);
            comm.SetParityValues(cboParity);
            comm.SetStopBitValues(cboStop);

            comm2.SetPortNameValues(cboPort2);
            comm2.SetParityValues(cboParity2);
            comm2.SetStopBitValues(cboStop2);

        }

        /// <summary>
        /// method to set the state of controls
        /// when the form first loads
        /// </summary>
        private void SetControlState()
        {
            rdoText.Checked = true;
            cmdSend.Enabled = false;
            cmdClose.Enabled = false;


            cmdSend2.Enabled = false;

        }


        private void cmdSend_Click(object sender, EventArgs e)
        {
            sendData();
        }

        private void sendData()
        {
            comm.WriteData(txtSend.Text);
            txtSend.SelectAll();
        }
        private void sendData2()
        {
            comm2.WriteData(txtSend2.Text);
            txtSend2.SelectAll();
        }

        private void rdoHex_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoHex.Checked == true)
            {
                comm.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Hex;
                comm2.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Hex;
            }
            else
            {
                comm.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Text;
                comm2.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Text;
            }
        }

        private void chkBoxEOL_CheckedChanged(object sender, EventArgs e)
        {
            comm.AutoEOL = chkBoxEOL.Checked;
        }

        private void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D)
            {
                sendData();
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            comm.ClosePort();
            comm2.ClosePort();

            if (false == comm.isPortOpen)
            {
                cmdOpen.Enabled = true;
                cmdClose.Enabled = false;
                cmdSend.Enabled = false;
                txtSend.Enabled = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox frmAbout = new AboutBox();
            frmAbout.Show();
        }

        private void cmdSend2_Click(object sender, EventArgs e)
        {
            sendData2();
        }
    }
}