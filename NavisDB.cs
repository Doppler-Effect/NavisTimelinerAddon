//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using System.Threading.Tasks;
//using System.Data.SQLite;
//using System.Collections.Specialized;

//namespace NaviSQLite
//{
//    public class NavisDB
//    {
//        private string dbFileName = "NavisProjectsDatabase.sq3";
//        private string[] colNames = { "Project", "StableID", "Value", "MaxValue", "Units", "uniqID" }; //PrimaryKey должен быть в конце!!!
//        private string tableName = "NavisDataTable";
//        private SQLiteConnection connection;
//        public bool Connected
//        {
//            get
//            {
//                return this.connection.State == System.Data.ConnectionState.Open;
//            }
//        }

//        public NavisDB()
//        {
//            if (!File.Exists(dbFileName))
//            {
//                SQLiteConnection.CreateFile(dbFileName);
//            }
//            string ConnectionString = "Data Source = " + dbFileName;
//            this.connection = new SQLiteConnection(ConnectionString);
//            connection.Open();
//            checkNavisTableOK();
//        }

//        /// <summary>
//        /// Селектит данные по идентификатору элемента и имени проекта
//        /// </summary>
//        //public Dictionary<string, string> Select(string Project, string ID)
//        //{
//        //    string[] resultFields = { "Value", "MaxValue", "Units" };
//        //    SQLiteCommand command =
//        //        new SQLiteCommand(string.Format("SELECT DISTINCT * FROM {0} WHERE Project=\"{1}\" AND StableID=\"{2}\"", this.tableName, Project, ID), this.connection);
//        //    SQLiteDataReader reader = command.ExecuteReader();
//        //    if (reader.HasRows)
//        //    {
//        //        Dictionary<string, string> result = new Dictionary<string, string>();
//        //        reader.Read();
//        //        NameValueCollection row = reader.GetValues();

//        //        foreach (string field in resultFields)
//        //        {
//        //            result.Add(field, row[field]);
//        //        }

//        //        return result;
//        //    }
//        //    else
//        //        return null;
//        //}

//        /// <summary>
//        /// Записывает данные о процессе выполнения в базу
//        /// </summary>
//        public bool Insert(string Project, string ID, string Value, string MaxValue, string Units)
//        {
//            string columns = null;
//            foreach (string colName in this.colNames)
//            {
//                columns += string.Format(" {0},", colName);
//            }
//            columns = columns.TrimEnd(',');

//            SQLiteCommand command = new SQLiteCommand(string.Format(
//                "INSERT OR REPLACE INTO {0} ({1}) VALUES (\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\")",
//                    tableName, columns, Project, ID, Value, MaxValue, Units, Project + ID), this.connection);
//            int rowsChanged = command.ExecuteNonQuery();

//            if (rowsChanged > 0)
//                return true;
//            else
//                return false;
//        }

//        /// <summary>
//        /// Проверяет, существует ли нужная таблица и нужные колонки в ней. При необходимости создаёт.
//        /// </summary>
//        private void checkNavisTableOK()
//        {
//            if (this.Connected)
//            {
//                //проверяем наличие таблицы
//                SQLiteCommand command =
//                    new SQLiteCommand(string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", this.tableName), this.connection);
//                Object res = command.ExecuteScalar();

//                //создаём новую таблицу
//                if (res == null)
//                {
//                    string columns = null;
//                    foreach (string colName in this.colNames)
//                    {
//                        columns += string.Format(" {0} VARCHAR(50),", colName);
//                    }
//                    columns = columns.TrimEnd(',');
//                    columns += " UNIQUE NOT NULL PRIMARY KEY";
//                    command = new SQLiteCommand(string.Format("CREATE TABLE {0} ({1})", tableName, columns), this.connection);
//                    int foo = command.ExecuteNonQuery();
//                }
//                //если таблица на месте - пытаемся создать колонки
//                else
//                {
//                    foreach (string colName in this.colNames)
//                    {
//                        try
//                        {
//                            command = new SQLiteCommand(string.Format("ALTER TABLE {0} ADD COLUMN {1}", tableName, colName), this.connection);
//                            int foo = command.ExecuteNonQuery();
//                        }
//                        catch (SQLiteException)
//                        {

//                        }
//                    }
//                }
//            }
//        }
//    }
//}
