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
            for (int i = 0; i < 10; i++)
            {
                DB.Insert(i.ToString(), "Value" + i.ToString(), "MaxValue" + i.ToString(), "%");
            }

            for (int i = 5; i < 7; i++)
            {
                DB.Insert(i.ToString(), "Value" + i.ToString(), "MaxValue" + i.ToString(), "%");
            }
        }
    }
}
