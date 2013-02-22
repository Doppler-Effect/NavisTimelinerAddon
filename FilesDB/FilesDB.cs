using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilesDB
{
    public class DataBase
    {
        string folderName = "FilesDB\\";
        string Path;
        string[] colNames = {"StableID", "Value", "MaxValue", "Units"};
        List<string> StableIDs = new List<string>();

        Encoding currentEnc = Encoding.UTF8;
        IOException CorruptedException = new IOException("Data file corrupted.");
        FileStream stream;

        public DataBase(string Project)
        {
            this.Path = folderName + Project;
            System.IO.Directory.CreateDirectory(folderName);
            if (!File.Exists(this.Path))
                AddHeader();
            else
                getStableIDs();
        }

        private void getStableIDs()
        {
            try
            {
                this.stream = File.Open(this.Path, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(this.stream, currentEnc);
                using (reader)
                {
                    while (!reader.EndOfStream)
                    {
                        string row = reader.ReadLine();
                        string id = row.Split(';').First();
                        if (!this.StableIDs.Contains(id))
                            this.StableIDs.Add(id);
                    }
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void AddHeader()
        {
            this.stream = File.Open(this.Path, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(this.stream, currentEnc);
            using (writer)
            {
                string header = compileHeader();
                writer.WriteLine(header);
            }
            stream.Dispose();
        }

        private string compileHeader()
        {
            string row = null;
            foreach (string colName in this.colNames)
            {
                row += colName + ";";
            }
            row = row.TrimEnd(';');
            return row;
        }

        public void Insert(string StableID, string Value, string MaxValue, string Units)
        {
            try
            {
                if (!this.StableIDs.Contains(StableID))
                {
                    this.stream = File.Open(this.Path, FileMode.Append, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(stream, currentEnc);
                    using (writer)
                    {
                        writer.WriteLine(string.Format("{0};{1};{2};{3}", StableID, Value, MaxValue, Units));
                        this.StableIDs.Add(StableID);
                    }
                }
                else
                {

                }
            }
            catch
            {

            }
        }

        public Dictionary<string, string> Select(string StableID)
        {
            try
            {
                Dictionary<string, string> Data = new Dictionary<string, string>();
                this.stream = File.Open(this.Path, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(this.stream, currentEnc);
                using (reader)
                {
                    if (reader.ReadLine() != compileHeader())
                        throw CorruptedException;

                    while (!reader.EndOfStream)
                    {
                        Data.Clear();
                        string row = reader.ReadLine();
                        string[] tokens = row.Split(';');

                        if (tokens.Count() != this.colNames.Count())
                            throw CorruptedException;

                        for (int i = 0; i < this.colNames.Count(); i++)
                        {
                            Data.Add(colNames[i], tokens[i]);
                        }

                        if (Data[colNames[0]] == StableID)
                            return Data;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
