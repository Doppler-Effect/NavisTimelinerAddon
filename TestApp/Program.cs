using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FilesDB.DataBase DB = new FilesDB.DataBase("foo");
            for (int i = 0; i < 3; i++)
            {
                DB.Insert(i.ToString(), "Value" + i.ToString(), "MaxValue!!!!!!!!!!!!" + i.ToString(), "%%%%%");
            }


            for (int i = 1; i < 4; i++)
            {
                DB.Insert(i.ToString(), "VALUE" + i.ToString(), "MAXVALUE" + i.ToString(), "m2");
            }

            object foo = DB.Select("4");
        }
    }
}
