using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgramPlannerServer
{ 
    /// <summary>
    /// Класс, обеспечивающий ведение лог-файла в текстовом формате
    /// </summary>
    class LoggerClass
    {
        string fileName;
        public LoggerClass(string fn="log.txt")
        {
            fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + fn; 
        }

        //добавляем запись в лог-файл
        public void WriteLogEntry(string logType, string msg)
        {
            DateTime data;
            data = DateTime.Now;
            StreamWriter swLog = new StreamWriter(fileName, true, Encoding.Default);
            swLog.WriteLine(data.ToString("{0} (dddd, dd MMMM yyyy HH:mm:ss)") + ": {1}", logType, msg);
            swLog.Close();
        }
    }
}
