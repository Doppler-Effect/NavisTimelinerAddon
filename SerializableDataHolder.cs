using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NavisTimelinerPlugin
{
    [Serializable()]
    class SerializableDataHolder
    {
        public Dictionary<Collection<int>, string> Data
        {
            get
            {
                return data;
            }
        }
        public Dictionary<Collection<int>, string> data = new Dictionary<Collection<int>, string>();

        public void Add(Collection<int> taskIndex, string ssetName)
        {
            data.Add(taskIndex, ssetName);
        }

        public void Clear()
        {
            data.Clear();
        }
    }

    class Serializer
    {
        static string FileName;

        public static void serialize(SerializableDataHolder Data)
        {
            try
            {
                if (SaveFileOK(ref FileName))
                {
                    FileStream stream = File.Create(FileName);
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(stream, Data);
                    stream.Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public static SerializableDataHolder deserialize()
        {
            SerializableDataHolder result;
            try
            {
                if (OpenFileOK(ref FileName))
                {
                    FileStream stream = File.OpenRead(FileName);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    result = (SerializableDataHolder)deserializer.Deserialize(stream);
                    stream.Close();
                    return result;
                }
                else
                    return null;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return null;
            }
        }

        static bool SaveFileOK(ref string filename)
        {
            SaveFileDialog svfile = new SaveFileDialog();
            svfile.AddExtension = true;
            svfile.CreatePrompt = true;
            svfile.DefaultExt = "bin";
            svfile.Filter = "binary files (*.bin)|*.bin";
            svfile.OverwritePrompt = false;
            svfile.RestoreDirectory = true;
            svfile.Title = "Выберите путь для сохранения файла";

            DialogResult res = svfile.ShowDialog(Core.Self.activeUIForm);
            if (res == DialogResult.OK)
            {
                filename = svfile.FileName;
                return true;
            }
            else
                return false;
        }

        static bool OpenFileOK(ref string filename)
        {
            OpenFileDialog opfile = new OpenFileDialog();
            opfile.AddExtension = true;
            opfile.CheckFileExists = true;
            opfile.Multiselect = false;
            opfile.RestoreDirectory = true;
            opfile.DefaultExt = "bin";
            opfile.Filter = "binary files (*.bin)|*.bin";
            opfile.RestoreDirectory = true;
            opfile.Title = "Выберите файл...";

            DialogResult res = opfile.ShowDialog(Core.Self.activeUIForm);
            if (res == DialogResult.OK)
            {
                filename = opfile.FileName;
                return true;
            }
            else
                return false;
        }
    }
}
