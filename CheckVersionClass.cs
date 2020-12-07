using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace NetworkPlaybackSample
{
    class CheckVersionClass
    {
        public static string GetVersion()
        {
            string sVersion = SearchIstalledVersion(32);

            return string.IsNullOrEmpty(sVersion) ? SearchIstalledVersion(64) : sVersion;
        }

        private static string SearchIstalledVersion(int bit)
        {
            string subKey = string.Empty;
            RegistryKey m__ReadKey;
            RegistryKey m_ReadSubKey;

            switch (bit)
            {
                case 64:
                    subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
                    break;
                case 32:
                    subKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
                    break;
            }

            m__ReadKey = Registry.LocalMachine.OpenSubKey(subKey);
            string[] subKeyNames = m__ReadKey.GetSubKeyNames();

            foreach (string subKeyName in subKeyNames)
            {
                m_ReadSubKey = Registry.LocalMachine.OpenSubKey(subKey + subKeyName);

                string[] subValuesNames = m_ReadSubKey.GetValueNames();
                foreach (string subname in subValuesNames)
                {
                    if (subname == "DisplayName")
                    {
                        try
                        {
                            string displayNameValue = (string)m_ReadSubKey.GetValue(subname);
                            if (displayNameValue.Contains("MPlatform"))
                            {
                                return (string)m_ReadSubKey.GetValue("DisplayVersion");
                            }
                        }
                        catch { }
                    }
                }
            }

            return string.Empty;
        }
    }
}
