using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProgramPlannerClient
{
    /// <summary>
    /// Интефейс сериализации-десериализации команды
    /// </summary>
    public interface ICommandFormatter
    {
        string FormatCommand(Command comand);
       Command ParseCommand(string msg);
    }

    /// <summary>
    /// Реализация интерфейса ICommandFormatter для преобразования в/из XML-формат
    /// </summary>
    public class XMLCommandFormatter: ICommandFormatter
    {
        /// <summary>
        /// Преобразует команду в строку XML-формата
        /// </summary>
        /// <param name="command">команда для преобразования</param>
        /// <returns>Строка с командой в XML-формате</returns>
        public string FormatCommand(Command command)
        {
            var sb = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(
                                            sb,
                                            new XmlWriterSettings()
                                            {
                                                Encoding = Encoding.UTF8
                                            }))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Command");
                if (command != null)
                {
                    xmlWriter.WriteElementString("Code", command.Code);

                    if (command.commandParams != null && command.commandParams.Count > 0)
                    {
                        xmlWriter.WriteStartElement("CommandParams");
                        foreach (string param in command.commandParams.Keys)
                            xmlWriter.WriteElementString(param, command.commandParams[param].ToString());
                        xmlWriter.WriteEndElement();
                    }

                }
                else
                    xmlWriter.WriteElementString("Code", "None");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            return sb.ToString();

    
        }

        /// <summary>
        /// Разбор строки с XML-представлением команды и формаирование из нее команды как объекта Command
        /// </summary>
        /// <param name="msg">Строка с XML-представлением команды</param>
        /// <returns>Представление команды в виде объекта Command</returns>
        public Command ParseCommand(string msg)
        {
            Command command=null;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {

                xmlDoc.LoadXml(msg);
                command = new Command();
                command.SetFormatter(this);
                //считываем код поступившей команды
                XmlNode codeNode = xmlDoc.SelectSingleNode("/Command/Code");
                command.Code = codeNode.InnerText;
                XmlNode parList = xmlDoc.SelectSingleNode("/Command/CommandParams");
                if(parList!=null  && parList.ChildNodes!=null)
                  foreach (XmlNode par in parList.ChildNodes)
                    {
                       command.commandParams.Add(par.Name, par.InnerText);
                    }
            }
            catch(Exception ex)
            {
                command=null;

            }
            return command;
        }
    }

    /// <summary>
    /// Класс, описывающий команды, которыми обмениваются клиент и сервер
    /// </summary>
    /// Пример команды:
    /// <Command>
    /// <Code>addWrongFolder</Code>
    /// <CommandParams>
    /// <folderName>d:\temp</folderName>
    /// </CommandParams>
    /// </Command>
    public class Command
    {  //код команды
        public string Code;
        //параметры команды
        public Dictionary<string, object> commandParams = new Dictionary<string, object>();
        //Интерфейс форматирования команды
        ICommandFormatter cf;
        public string senderAddress;
        public Command(){}
        public Command(string code, ICommandFormatter formatter)
        {
            Code = code;
            cf = formatter;
        }
        //Добавить параметр команды
        public void AddParam(string parName, object parValue)
        {
            if (commandParams == null || !commandParams.ContainsKey(parName))
            {
                commandParams.Add(parName, parValue);
            }
        }

        //сменить объект форматирования команды
        public void SetFormatter(XMLCommandFormatter formatter)
        {
            cf = formatter;
        }

        //получить отформатированную команду
        public string GetFormattedCommand()
        {
            return cf.FormatCommand(this);
        }
    }
}
