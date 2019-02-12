using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ProgramPlannerServer
{
    /// <summary>
    /// Класс, описывающий список программ ожидающих запуска
    /// Список программ сохраняется в файле на диске в формате XML:
    /// <?xml version="1.0" encoding="utf-8"?>
    ///<programs>
    ///<program>
    ///<path>путь к программе1</path>
    ///<startDate>дата и время запуска программы1</startDate>
    ///<repeat>повторяемость запуска программы1</repeat>
    ///</program>
    ///<program>
    ///<path>путь к программе2</path>
    ///<startDate>дата и время запуска программы2</startDate>
    ///<repeat>повторяемость запуска программы2</repeat>
    ///</program>
    ///...
    ///<program>
    ///<path>путь к программеN</path>
    ///<startDate>дата и время запуска программыN</startDate>
    ///<repeat>повторяемость запуска программыN</repeat>
    ///</program>
    ///</programs>
    ///
    /// </summary>
    class WaitingProgramList
    {
        public enum RepeatMode
        {
            Once,
            EveryMinute,
            EveryHour = 60,
            EveryDay = 1440
        }
        //имя файла, в котором будет сохранен список папок
        string programListFileName;
        //Непосредственно список папок
        public List<Dictionary<string, object>> ProgramList;

        /// <summary>
        /// Конструктор класса получает имя файла, в котором хранится список программ,
        /// открывает или создает его, при наличии - считывает содержимое программ во внутренний массив 
        /// </summary>
        /// <param name="fileName">имя файла, в котором (будет) хранится список программ</param>
        public WaitingProgramList(string fileName)
        {
            programListFileName = fileName;
            ProgramList = new List<Dictionary<string, object>>();
            if (File.Exists(programListFileName))
                ReadProgramList();
            else
                InitProgramList();

        }

        /// <summary>
        /// Создает пустой список программ в файле XML-формата
        /// </summary>
        void InitProgramList()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement(string.Empty, "programs", string.Empty);
            doc.AppendChild(element1);
            doc.Save(programListFileName);
        }

        /// <summary>
        /// Считывает из файла XML-фомата список программ во внутренний объект-список
        /// </summary>
        void ReadProgramList()
        {
            ProgramList.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(programListFileName);
            XmlElement xRoot = doc.DocumentElement;
            foreach (XmlNode fnode in xRoot)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (XmlNode mNode in fnode.ChildNodes)
                {
                    dict.Add(mNode.Name, mNode.InnerText);
                }
                ProgramList.Add(dict);
            }

        }

        /// <summary>
        /// Свойство - количество папок в списке
        /// </summary>
        public int Count
        {
            get { return ProgramList.Count; }
        }

        /// <summary>
        /// Метод добавления программы в список
        /// </summary>
        /// <param name="program">Параметры программы для запуска</param>
        /// <returns>Статус выполнения операции: программа добавлена (true) или нет (false)</returns>
        public bool AddProgram(Dictionary<string,object> program)
        {
            try
            {
                ProgramList.Add(program);
                XmlDocument doc = new XmlDocument();
                doc.Load(programListFileName);
                XmlElement froot = doc.DocumentElement;
                XmlElement newProgramNode = doc.CreateElement("program");
                XmlElement newProgramPath = doc.CreateElement("path");
                newProgramPath.InnerText = program["path"].ToString();
                XmlElement newProgramStartDate = doc.CreateElement("startDate");
                newProgramStartDate.InnerText = program["startDate"].ToString();
                XmlElement newProgramRepeat = doc.CreateElement("repeat");
                newProgramRepeat.InnerText = program["repeat"].ToString();
                newProgramNode.AppendChild(newProgramPath);
                newProgramNode.AppendChild(newProgramStartDate);
                newProgramNode.AppendChild(newProgramRepeat);
                froot.AppendChild(newProgramNode);
                doc.Save(programListFileName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
       }

        /// <summary>
        /// Метод удаления программы из списка
        /// </summary>
        /// <param name="programId">Номер удаляемой программы в списке</param>
        /// <returns>Статус выполнения операции: программа удалена (true) или нет (false)</returns>
        public bool DelProgram(int programId)
        {
            if (ProgramList.Count > programId)
            {
                ProgramList.RemoveAt(programId);
                XmlDocument doc = new XmlDocument();
                doc.Load(programListFileName);
                XmlElement fRoot = doc.DocumentElement;
                XmlNode delNode = fRoot.ChildNodes[programId];
                fRoot.RemoveChild(delNode);
                doc.Save(programListFileName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод обновления даты запуска программы
        /// </summary>
        /// <param name="programId">Номер обновляемой программы в списке</param>
        /// <param name="startDate">Новая дата запуска программы</param>
        /// <returns>Статус выполнения операции: дата запуска обновлена (true) или нет (false)</returns>
        public bool UpdateStartDateProgram(int programId, string startDate)
        {
            if (ProgramList.Count > programId)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(programListFileName);
                XmlElement fRoot = doc.DocumentElement;
                XmlNode editNode = fRoot.ChildNodes[programId];
                editNode["startDate"].InnerText = startDate;
                doc.Save(programListFileName);
                return true;
            }
            return false;
        }
    }
}
