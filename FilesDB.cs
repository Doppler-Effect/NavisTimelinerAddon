using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilesDB
{
    public class DataBase
    {
        string folderName = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NavisTimelinerPlugin Database\\";
        string Path;
        string[] colNames = {"StableID", "Value", "MaxValue", "Units"};
        List<string> StableIDs = new List<string>();

        Encoding currentEnc = Encoding.UTF8;
        IOException CorruptedException = new IOException("Data file corrupted.");
        FileStream stream;

        /// <summary>
        /// Конструктор, при вызове проверяет существование файла БД указанного проекта и создаёт его при необходимости.
        /// </summary>
        /// <param name="Project">Имя проекта Нэвис, оно же имя файла</param>
        public DataBase(string Project)
        {            
            this.Path = folderName + Project;
            System.IO.Directory.CreateDirectory(folderName);
            if (!File.Exists(this.Path))
            {
                AddHeader();
            }
            getStableIDs();
        }

        /// <summary>
        /// Наполняет массив ID уже существующих в базе элементов.
        /// </summary>
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

        /// <summary>
        /// Добавляет в свежесозданный файл "шапку" из имён полей.
        /// </summary>
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

        /// <summary>
        /// Собирает "шапку" из имён полей и ";".
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Записывает данные в таблицу. Если уже существует такой StableID - правит существующие значения, если нет - добавляет новую запись в конец файла.
        /// </summary>
        public void Insert(string StableID, string Value, string MaxValue, string Units)
        {
            try
            {
                if (!string.IsNullOrEmpty(StableID))
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
                        string[] lines = File.ReadAllLines(this.Path, currentEnc);
                        if (lines[0] != this.compileHeader())
                            throw CorruptedException;

                        for (int i = 0; i < lines.Count(); i++)
                        {
                            string oldLine = lines[i];
                            string[] tokens = oldLine.Split(';');
                            if (tokens.Count() < this.colNames.Count())
                                throw CorruptedException;

                            if (tokens[0] == StableID)
                            {                                
                                tokens[1] = Value;
                                tokens[2] = MaxValue;
                                tokens[3] = Units;

                                string newLine = null;
                                foreach (string token in tokens)
                                {
                                    newLine += token + ";";
                                }
                                newLine = newLine.TrimEnd(';');

                                if (newLine != oldLine)
                                {
                                    lines.SetValue(newLine, i);
                                    File.WriteAllLines(this.Path, lines, currentEnc);
                                }
                                return;
                            }

                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("У элемента не определяется Unique ID");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Ищет в файле запись с нужным StableID.
        /// </summary>
        /// <returns>Массив данных - вся строка файла.</returns>
        public Dictionary<string, string> Select(string StableID)
        {
            try
            {
                Dictionary<string, string> result; 
                string[] lines = File.ReadAllLines(this.Path, currentEnc);
                if (lines[0] != this.compileHeader())
                    throw CorruptedException;
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(';');

                    if (tokens.Count() < this.colNames.Count())
                        throw CorruptedException;

                    if (tokens[0] == StableID)
                    {
                        result = new Dictionary<string, string>();
                        for (int i = 0; i < this.colNames.Count(); i++)
                        {
                            result.Add(colNames[i], tokens[i]);
                        }
                        return result;
                    }
                }
                return null;

                #region the Old way - readline style.
                //this.stream = File.Open(this.Path, FileMode.Open, FileAccess.Read);
                //StreamReader reader = new StreamReader(this.stream, currentEnc);
                //using (reader)
                //{
                //    if (reader.ReadLine() != compileHeader())
                //        throw CorruptedException;

                //    while (!reader.EndOfStream)
                //    {
                //        result.Clear();
                //        string row = reader.ReadLine();
                //        string[] tokens = row.Split(';');

                //        if (tokens.Count() != this.colNames.Count())
                //            throw CorruptedException;

                //        for (int i = 0; i < this.colNames.Count(); i++)
                //        {
                //            result.Add(colNames[i], tokens[i]);
                //        }

                //        if (result[colNames[0]] == StableID)
                //            return result;
                //    }
                //} 
                #endregion

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        ////Удаляет из базы элемент с заданным StableID
        /// </summary>
        public void Remove(string StableID)
        {
            try
            {
                string[] lines = File.ReadAllLines(this.Path, currentEnc);
                if (lines[0] != this.compileHeader())
                    throw CorruptedException;

                List<string> resultLines = new List<string>();

                foreach (string line in lines)
                {
                    string[] tokens = line.Split(';');

                    if (tokens.Count() == 0)
                        throw CorruptedException;

                    if (tokens[0] != StableID)
                    {
                        resultLines.Add(line);
                    }
                    else
                    {
                        this.StableIDs.Remove(StableID);
                    }
                }
                string[] result = resultLines.ToArray();
                File.WriteAllLines(this.Path, result, currentEnc);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
