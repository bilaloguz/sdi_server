using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Web.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using MPLATFORMLib;
namespace NetworkPlaybackSample
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        public Form1()
        {
            InitializeComponent();
        }
        MFileClass m_objFile;
        MRendererClass m_objRenderer;
        private int startVideoFormat;
        private int startAudioFormat;
        private double[] m_arrBufferMinMax = new double[2];
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.UrlHistory != null)
                {
                    object[] strHistory = NetworkPlaybackSample.Properties.Settings.Default.UrlHistory.ToArray();
                }
                m_objFile = new MFileClass();
                m_objFile.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                m_objFile.PreviewEnable("", 1, 1);
                m_objRenderer = new MRendererClass();
                FillVideoFormats();
                FillAudioFormats();
                comboBoxVF.SelectedIndex = startVideoFormat;
                comboBoxAF.SelectedIndex = startAudioFormat;
                FillRenderers();
                UpdateDelay();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
            string JSON = "{ \"Number of Input Ports\": \"1\", \"Device List\": [";
            for (int i = 0; i < comboBoxRenderer.Items.Count; i++)
            {
                if (i != comboBoxRenderer.Items.Count - 1) {
                    JSON += "\"" + comboBoxRenderer.Items[i] + "\",";
                }
                else {
                    JSON += "\"" + comboBoxRenderer.Items[i] + "\"]}";
                }
            }
            var client = new HttpClient();
            string serverIP = textBox1.Text;
            string serverPort = textBox2.Text;
            var stringContent = new StringContent(JSON);
            client.PostAsync("http://" + serverIP + ":" + serverPort + "/device_list", stringContent);
            var data = GetIP(serverIP, serverPort).Result;
            var sdpContent = GetSDP(serverIP, serverPort).Result;
            string sdp = sdpContent.Replace("\\n", Environment.NewLine);
            sdp = sdp.Replace("\"", "");
            string[] lines = Regex.Split(sdp, "\r\n|\r|\n");
            string result = "";
            foreach(string line in lines)
            {
                result += line.Trim() + Environment.NewLine;
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\stream.sdp"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\stream.sdp");
            }
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\stream.sdp", result);
            try
            {
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\stream.sdp";
                m_objFile.FileNameSet(filePath, "");
                m_objFile.FilePlayStart();
                m_arrBufferMinMax[0] = GetDblProps(m_objFile, "file::network.buffer_min");
                m_arrBufferMinMax[1] = GetDblProps(m_objFile, "file::network.buffer_max");
                timerStat.Enabled = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\settings.json")) {
                string numberPorts = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\settings.json");
                textBox3.Text = numberPorts;
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_objFile != null)
                m_objFile.ObjectClose();
            if (m_objRenderer != null)
                m_objRenderer.ObjectClose();
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\settings.json"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\settings.json");
            }
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\settings.json", textBox3.Text);
        }
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\this.sdp";
                m_objFile.FileNameSet(filePath, "");
                m_objFile.FilePlayStart();
                m_arrBufferMinMax[0] = GetDblProps(m_objFile, "file::network.buffer_min");
                m_arrBufferMinMax[1] = GetDblProps(m_objFile, "file::network.buffer_max");
                timerStat.Enabled = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_objFile != null)
                {
                    m_objFile.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void FillVideoFormats()
        {
            int nCount = 0;
            int nIndex;
            string strFormat;
            M_VID_PROPS vidProps;
            comboBoxVF.Items.Clear();
            m_objFile.FormatVideoGetCount(eMFormatType.eMFT_Convert, out nCount);
            comboBoxVF.Enabled = nCount > 0;
            if (nCount > 0)
            {
                for (int i = 0; i < nCount; i++)
                {
                    m_objFile.FormatVideoGetByIndex(eMFormatType.eMFT_Convert, i, out vidProps, out strFormat);
                    if (vidProps.eVideoFormat == eMVideoFormat.eMVF_HD1080_5994i) startVideoFormat = i;
                    comboBoxVF.Items.Add(strFormat);
                }
                m_objFile.FormatVideoGet(eMFormatType.eMFT_Convert, out vidProps, out nIndex, out strFormat);
                if (nIndex > 0)
                    comboBoxVF.SelectedIndex = nIndex;
                else comboBoxVF.SelectedIndex = 0;
            }
        }
        private void FillAudioFormats()
        {
            int nCount = 0;
            int nIndex;
            string strFormat;
            M_AUD_PROPS audProps;
            comboBoxAF.Items.Clear();
            m_objFile.FormatAudioGetCount(eMFormatType.eMFT_Convert, out nCount);
            comboBoxAF.Enabled = nCount > 0;
            if (nCount > 0)
            {
                for (int i = 0; i < nCount; i++)
                {
                    m_objFile.FormatAudioGetByIndex(eMFormatType.eMFT_Convert, i, out audProps, out strFormat);
                    if (audProps.nBitsPerSample == 16 && audProps.nChannels == 16 && audProps.nSamplesPerSec == 48000) startAudioFormat = i;
                    comboBoxAF.Items.Add(strFormat);

                }
                m_objFile.FormatAudioGet(eMFormatType.eMFT_Convert, out audProps, out nIndex, out strFormat);
                if (nIndex > 0)
                    comboBoxAF.SelectedIndex = nIndex;
                else comboBoxAF.SelectedIndex = 0;
            }
        }
        void FillRenderers()
        {
            int nDevices = 0;
            m_objRenderer.DeviceGetCount(0, "renderer", out nDevices);
            if (nDevices > 0)
            {
                checkBoxOutput.Enabled = true;
                comboBoxRenderer.Enabled = true;
                for (int i = 0; i < nDevices; i++)
                {
                    string strName;
                    string strXML;
                    m_objRenderer.DeviceGetByIndex(0, "renderer", i, out strName, out strXML);
                    comboBoxRenderer.Items.Add(strName);
                }
                comboBoxRenderer.SelectedIndex = 0;
            }
            else
            {
                checkBoxOutput.Enabled = false;
                comboBoxRenderer.Enabled = false;
            }
        }
        private void comboBoxVF_SelectedIndexChanged(object sender, EventArgs e)
        {
            M_VID_PROPS vidProps = new M_VID_PROPS();
            string strFormat;
            m_objFile.FormatVideoGetByIndex(eMFormatType.eMFT_Convert, comboBoxVF.SelectedIndex, out vidProps, out strFormat);
            m_objFile.FormatVideoSet(eMFormatType.eMFT_Convert, ref vidProps);
        }
        private void comboBoxAF_SelectedIndexChanged(object sender, EventArgs e)
        {
            M_AUD_PROPS audProps = new M_AUD_PROPS();
            string strFormat;
            m_objFile.FormatAudioGetByIndex(eMFormatType.eMFT_Convert, comboBoxAF.SelectedIndex, out audProps, out strFormat);
            m_objFile.FormatAudioSet(eMFormatType.eMFT_Convert, ref audProps);
        }
        private void checkBoxOutput_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxOutput.Checked)
            {
                try
                {
                    m_objRenderer.PropsSet("rate-control", "true");
                    m_objRenderer.DeviceSet("renderer", comboBoxRenderer.SelectedItem.ToString(), "");
                    m_objRenderer.ObjectStart(m_objFile);
                }
                catch (System.Exception ex)
                {
                    checkBoxOutput.Checked = false;
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                try
                {
                    m_objRenderer.ObjectClose();
                }
                catch { }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_objFile.ObjectClose();
            m_objRenderer.ObjectClose();
        }
        private void timerDelay_Tick(object sender, EventArgs e)
        {
            UpdatePos();
        }
        void UpdateDelay()
        {
            try
            {
                IMProps pProps = (IMProps)m_objFile;
                string sValue;
                pProps.PropsGet("object::mdelay.enabled", out sValue);
                pProps.PropsGet("object::mdelay.buffer_duration", out sValue);  // The value in seconds
                pProps.PropsGet("object::mdelay.quality", out sValue);
                pProps.PropsGet("object::mdelay.available", out sValue);
                pProps.PropsGet("object::mdelay.time", out sValue);
            }
            catch (System.Exception) { }
        }
        void UpdatePos()
        {
            try
            {
                IMProps pProps = (IMProps)m_objFile;
                string sValue;
                pProps.PropsGet("object::mdelay.available", out sValue);
                pProps.PropsGet("object::mdelay.time", out sValue);
            }
            catch (System.Exception) { }
        }
        double GetDblProps(object _pObject, string strName)
        {
            string strValue = "";
            ((IMProps)_pObject).PropsGet(strName, out strValue);
            return Double.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
        }
        private void timerStat_Tick(object sender, EventArgs e)
        {
            try
            {
                double dblABuffer = GetDblProps(m_objFile, "file::buffer.audio");
                double dblVBuffer = GetDblProps(m_objFile, "file::buffer.video");
            }
            catch (System.Exception) { }
        }
        private void comboBoxRenderer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBoxOutput.Checked)
            {
                m_objRenderer.ObjectClose();
                m_objRenderer.DeviceSet("renderer", comboBoxRenderer.SelectedItem.ToString(), "");
                m_objRenderer.ObjectStart(m_objFile);
            }
        }
        public async Task<bool> CheckStatus(string serverIP, string serverPort)
        {
            var client = new HttpClient();
            var result = await client.GetAsync("http://" + serverIP + ":" + serverPort + "/connect");
            if (result.StatusCode.ToString() == "OK")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> GetSDP(string serverIP, string serverPort)
        {
            var client = new HttpClient();
            var ip = GetIP(serverIP, serverPort).Result;
            try
            {
                var result = client.GetStringAsync("http://" + serverIP + ":" + serverPort + "/get_sdp/" + ip).Result;
                return await Task.FromResult(result);
            }
            catch
            {
                return "False";
            }
        }
        private async Task<string> GetIP(string serverIP, string serverPort) {
            var client = new HttpClient();
            try
            {
                var response = client.GetStringAsync("http://" + serverIP + ":" + serverPort + "/connect").Result;
                return await Task.FromResult(response);
            }
            catch
            {
                return null;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            string serverIP = textBox1.Text;
            string serverPort = textBox2.Text;
            var client = new HttpClient();
            try
            {
                var response = client.GetAsync("http://" + serverIP + ":" + serverPort + "/connect").Result;
                if (response.StatusCode.ToString() == "OK")
                {
                    pictureBox1.BackColor = Color.Green;
                    label2.Text = "Connected!";
                }
            } 
            catch
            {
                pictureBox1.BackColor = Color.Red;
                label2.Text = "Disconnected";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }
    }
}