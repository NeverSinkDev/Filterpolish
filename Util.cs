using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterPolish
{
    public class Util
    {
        public static ListViewGroup DeepCopy(ListViewGroup other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (ListViewGroup)formatter.Deserialize(ms);
            }
        }

        public static string ReadFileToString(string filterstring, string title, string Folder)
        {
            string text = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filterstring;
            dlg.Title = title;
            dlg.InitialDirectory = Folder;

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filename = dlg.FileName;
                using (StreamReader sr = new StreamReader(filename))
                {
                    text = sr.ReadToEnd();

                    if (text.Length <= 1)
                    {
                        MessageBox.Show("NO TEXT IN FILE!!!");
                    }
                }

            }
            return text;
        }

        public static string ReadFileToString(string FullPath)
        {
            return System.IO.File.ReadAllText(FullPath);
        }

        public static string getConfigValue(string key)
        {
            return ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath).AppSettings.Settings[key].Value;
        }

        public static string GetOutputPath()
        {
            return getConfigValue("Output Folder");
        }
    }
}
