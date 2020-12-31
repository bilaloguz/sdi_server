using MPLATFORMLib;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
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

            /* Config read */
            textBox1.Text = ConfigurationManager.AppSettings.Get("applicationServer");
            var firm = ConfigurationManager.AppSettings.Get("firm");
            var customerID = ConfigurationManager.AppSettings.Get("customerID");
            /* Config read */

            /* Connect method */
            var client = new HttpClient();
            string serverIP = textBox1.Text;
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Start();
            try
            {
                var response = client.GetAsync("https://" + serverIP + "/connect").Result;
                if (response.StatusCode.ToString() == "OK")
                {
                    pictureBox1.BackColor = Color.Green;
                    label2.Font = new Font("Arial", 7);
                    label2.Text = "Connected!";
                    label3.Text = "Last checked: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                }
                else
                {
                    pictureBox1.BackColor = Color.Red;
                    label2.Font = new Font("Arial", 7);
                    label2.Text = "Disconnected!";
                    label3.Text = "Last checked: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                }
            }
            catch
            {
                pictureBox1.BackColor = Color.Red;
                label2.Font = new Font("Arial", 7);
                label2.Text = "Disconnected!";
                label3.Text = "Last checked: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            }
            /* Connect method */

            /* device_list method */
            string JSON = "{ \"customerID\":\"" + customerID + "\", \"firm\":\"" + firm + "\", \"Device List\": [";
            for (int i = 0; i < comboBoxRenderer.Items.Count; i++)
            {
                if (i != comboBoxRenderer.Items.Count - 1)
                {
                    JSON += "\"" + comboBoxRenderer.Items[i] + "\",";
                }
                else
                {
                    JSON += "\"" + comboBoxRenderer.Items[i] + "\"]}";
                }
            }
            var stringContent = new StringContent(JSON);
            try
            {
                var response = client.PostAsync("https://" + serverIP + "/device_list", stringContent).Result;
                if (response.StatusCode.ToString() == "OK")
                {}
                else
                {
                    MessageBox.Show("Device list can not be sent to application server.");
                }
            }
            catch
            {
                MessageBox.Show("Device list can not be sent to application server.");
            }
            /* device_list method */

            string sdp = "";
            string portName = "";
            var jsonContent = GetJSON(serverIP).Result;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json");
            }
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json", jsonContent);
            Newtonsoft.Json.Linq.JArray json = Newtonsoft.Json.Linq.JArray.Parse(jsonContent);
            foreach (Newtonsoft.Json.Linq.JObject o in json.Children<Newtonsoft.Json.Linq.JObject>())
            {
                foreach (Newtonsoft.Json.Linq.JProperty p in o.Properties())
                {
                    if (p.Name == "name")
                    {
                        portName = p.Value.ToString();
                    }
                    if (p.Name == "sdp")
                    {
                        sdp = p.Value.ToString();
                    }
                }
                sdp = sdp.Replace("\\n", Environment.NewLine);
                sdp = sdp.Replace("\"", "");
                string[] lines = Regex.Split(sdp, "\r\n|\r|\n");
                string result = "";
                foreach (string line in lines)
                {
                    result += line.Trim() + Environment.NewLine;
                }
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp"))
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp");
                }
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp", result);
            }
            timer2.Tick += new EventHandler(Timer2_Tick);
            timer2.Start();
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp"))
            {
                //sdpFile = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp");
                var sdpFile = "";
                var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete);
                using (var sr = new StreamReader(fs))
                {
                    sdpFile = sr.ReadToEnd();
                    sr.Close();
                }
                fs.Dispose();
                sdpFile = sdpFile.Trim();
                if (!String.Equals(sdpFile, "Vacant"))
                {
                    if (!String.Equals(sdpFile, "Not assigned"))
                    {
                        m_objFile.FileNameSet(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp", "");
                        m_objFile.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                        m_objFile.PreviewEnable("", 1, 1);
                        try
                        {
                            m_objFile.FilePlayStart();
                        }
                        catch
                        {
                            panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                        }
                        m_objRenderer.ObjectStart(m_objFile);
                    }
                    else
                    {
                        m_objFile.ObjectClose();
                        m_objRenderer.ObjectClose();
                        panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                    }
                }
                else
                {
                    m_objFile.ObjectClose();
                    m_objRenderer.ObjectClose();
                    panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_objFile != null)
            {
                m_objFile.ObjectClose();
            }
            if (m_objRenderer != null)
            {
                m_objRenderer.ObjectClose();
            }
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
            confCollection["applicationServer"].Value = textBox1.Text;
            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
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
        private void FillRenderers()
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
                    if (strName != "WebRTC")
                    {
                        comboBoxRenderer.Items.Add(strName);
                    }
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
        private void UpdateDelay()
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
        private void UpdatePos()
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
        private double GetDblProps(object _pObject, string strName)
        {
            string strValue = "";
            ((IMProps)_pObject).PropsGet(strName, out strValue);
            return Double.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
        }
        private void TimerStat_Tick(object sender, EventArgs e)
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
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp"))
            {
                m_objFile.ObjectClose();
                m_objRenderer.ObjectClose();
                var sdpFile = "";
                var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete);
                using (var sr = new StreamReader(fs))
                {
                    sdpFile = sr.ReadToEnd();
                    sr.Close();
                }
                fs.Dispose();
                sdpFile = sdpFile.Trim();
                if (!String.Equals(sdpFile, "Vacant"))
                {
                    if (!String.Equals(sdpFile, "Not assigned"))
                    {
                        m_objFile.FileNameSet(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp", "");
                        m_objFile.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                        m_objFile.PreviewEnable("", 1, 1);
                        try
                        {
                            m_objFile.FilePlayStart();
                        }
                        catch
                        {
                            panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                        }
                        m_objRenderer.ObjectStart(m_objFile);
                    }
                }
            }
        }
        public async Task<string> GetJSON(string serverIP)
        {
            var client = new HttpClient();
            try
            {
                var result = client.GetStringAsync("https://" + serverIP + "/getJson").Result;
                return await Task.FromResult(result);
            }
            catch
            {
                return "False";
            }
        }
        public void SaveJSON()
        {
            string sdp = "";
            string portName = "";
            string serverIP = textBox1.Text;
            var jsonContent = GetJSON(serverIP).Result;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json"))
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json", String.Empty);
                //File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json");
            }
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json", jsonContent);
            Newtonsoft.Json.Linq.JArray json = Newtonsoft.Json.Linq.JArray.Parse(jsonContent);
            foreach (Newtonsoft.Json.Linq.JObject o in json.Children<Newtonsoft.Json.Linq.JObject>())
            {
                foreach (Newtonsoft.Json.Linq.JProperty p in o.Properties())
                {
                    if (p.Name == "name")
                    {
                        portName = p.Value.ToString();
                    }
                    if (p.Name == "sdp")
                    {
                        sdp = p.Value.ToString();
                    }
                }
                sdp = sdp.Replace("\\n", Environment.NewLine);
                sdp = sdp.Replace("\"", "");
                string[] lines = Regex.Split(sdp, "\r\n|\r|\n");
                string result = "";
                foreach (string line in lines)
                {
                    result += line.Trim() + Environment.NewLine;
                }
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp"))
                {
                    System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp", string.Empty);
                    //File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp");
                }
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + portName + ".sdp", result);
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            string serverIP = textBox1.Text;
            var client = new HttpClient();
            try
            {
                var response = client.GetAsync("https://" + serverIP + "/connect").Result;
                if (response.StatusCode.ToString() == "OK")
                {
                    pictureBox1.BackColor = Color.Green;
                    label2.Font = new Font("Arial", 7);
                    label2.Text = "Connected!";
                    label3.Font = new Font("Arial", 7);
                    label3.Text = "Last checked: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                }
                else
                {
                    pictureBox1.BackColor = Color.Red;
                    label2.Font = new Font("Arial", 7);
                    label2.Text = "Disconnected!";
                    label3.Font = new Font("Arial", 7);
                    label3.Text = "Last checked: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                }
            }
            catch
            {
                pictureBox1.BackColor = Color.Red;
                label2.Font = new Font("Arial", 7);
                label2.Text = "Disconnected!";
                label3.Font = new Font("Arial", 7);
                label3.Text = "Last checked: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            }
        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
            //SaveJSON();
            string serverIP = textBox1.Text;
            var jsonContentNew = GetJSON(serverIP).Result;
            var jsonContentOld = "";
            var sdpFile = "";
            //var jsonContentOld = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json");
            var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\streams.json", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite|FileShare.Delete);
            using (var sr = new StreamReader(fs))
            {
                jsonContentOld = sr.ReadToEnd();
                sr.Close();
            }
            fs.Dispose();
            if (!String.Equals(jsonContentOld, jsonContentNew))
            {
                SaveJSON();
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp"))
                {
                    //sdpFile = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp");
                    fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete);
                    using (var sr = new StreamReader(fs))
                    {
                        sdpFile = sr.ReadToEnd();
                        sr.Close();
                    }
                    fs.Dispose();
                    sdpFile = sdpFile.Trim();
                    if (!String.Equals(sdpFile, "Vacant"))
                    {
                        if (!String.Equals(sdpFile, "Not assigned"))
                        {
                            m_objFile.FileNameSet(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + comboBoxRenderer.SelectedItem.ToString() + ".sdp", "");
                            m_objFile.PreviewWindowSet("", panelPreview.Handle.ToInt32());
                            m_objFile.PreviewEnable("", 1, 1);
                            try
                            {
                                m_objFile.FilePlayStart();
                            }
                            catch
                            {
                                panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                            }
                            m_objRenderer.ObjectStart(m_objFile);
                        }
                        else
                        {
                            m_objFile.ObjectClose();
                            m_objRenderer.ObjectClose();
                            panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                        }
                    }
                    else
                    {
                        m_objFile.ObjectClose();
                        m_objRenderer.ObjectClose();
                        panelPreview.BackgroundImage = Image.FromFile(Application.StartupPath + "\\nostream.png");
                    }
                }
            }
        }
    }
}