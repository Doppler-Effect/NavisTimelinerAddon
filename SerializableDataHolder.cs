using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NavisTimelinerPlugin
{
    [Serializable()]
    class SerializableDataHolder
    {
        public Dictionary<string, string> Data
        {
            get
            {
                return data;
            }
        }
        public Dictionary<string, string> data = new Dictionary<string, string>();

        public void Add(string taskName, string ssetName)
        {
            data.Add(taskName, ssetName);
        }

        public void Clear()
        {
            data.Clear();
        }
    }

    class Serializer
    {
        static string FileName = "D:\\SavedLoan.bin";

        public static void serialize(SerializableDataHolder Data)
        {
            try
            {
                FileStream stream = File.Create(FileName);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, Data);
                stream.Close();
            }
            catch (Exception Ex)
            {

            }
        }

        public static SerializableDataHolder deserialize()
        {
            try
            {
                SerializableDataHolder result;
                FileStream stream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                result = (SerializableDataHolder)deserializer.Deserialize(stream);
                stream.Close();
                return result;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
    }
}
