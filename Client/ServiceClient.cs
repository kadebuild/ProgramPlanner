using System;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ProgramPlannerClient
{
    /// <summary>
    /// Класс-клиент службы
    /// Подкдючается к серверу, передает ему команды и принимает сообщения с результатами обработки команд
    /// </summary>
    sealed class  ServiceClient
    {
        //адрес сервера со значением по умолчанию
        static string serverAddress = "127.0.0.1";
        //порт сервера со значением по умолчанию
        static int port = 9999;

        public static string Host
        {
            get { return serverAddress; }
            set { serverAddress = value; }
        }

        public static int Port
        {
            get { return port; }
            set { port = value; }  
        }
        //объект-сокет для обмена данными с сервером
        TcpClient client;

        //список папок, оттслеживаемых сервером
        public List<Dictionary<string,object>> programs;

        delegate string answerHandler(Command command);
        //список обработчиков ответов сервера
        Dictionary<string, answerHandler> mapAnswerHandlers;

        //string errorStr;
        bool bServiceRunning, bLastCommandOK;
        ICommandFormatter commandFormatter;
        /// <summary>
        /// Закрытое поле-экземпляр клиента. Реализуем синглтон
        /// </summary>
        private static readonly Lazy<ServiceClient> _instance = new Lazy<ServiceClient>(() => new ServiceClient(new XMLCommandFormatter()));
        /// <summary>
        /// Закрытый конструктор
        /// </summary>
        /// <param name="cf">какой форматтер будет использоваться для сериализации-десериализации команд при обмене с сервером</param>
        ServiceClient(ICommandFormatter cf) {
            commandFormatter = cf; 
            ///сопоставляем возможные ответы сервера их обработчикам со стороны клиента
            mapAnswerHandlers = new Dictionary<string, answerHandler>() { {"success", SuccessAndErrorHandler},
                                                                          {"changeServiceStatus", ChangeStateStatusHandler},
                                                                          {"error", SuccessAndErrorHandler},
                                                                          {"programs", ProgramsHandler},
                                                                          {"serviceStatus", ServiceStatusHandler} };
            programs = new List<Dictionary<string,object>>();
        }
        /// <summary>
        /// свойство для доступа к экземпляру синглтона-клиента службы
        /// </summary>
        public static ServiceClient instance {
            get { return _instance.Value; }
        }
     
        /// <summary>
        /// Свойство - статус завершения последней команды: удачно (true) или нет (false)
        /// </summary>
        public bool BLastCommandStatus
        {
            get
            {
                return bLastCommandOK;
            }

        }
        /// <summary>
        /// Соединиться с сервером
        /// </summary>
        /// <returns>Удалось (true) или нет (false) соединиться</returns>
        public async Task<bool> СonnectToServer()
        {
            client = new TcpClient();
            try
            {
                await client.ConnectAsync(serverAddress, port); // соединение
                return true;
            }
            catch( SocketException ex)
            {
                bServiceRunning=false;
                return false;
            }
        }
        /// <summary>
        /// Работает ли сервер
        /// </summary>
        /// <returns>Работает (true) или нет (false) сервер</returns>
        public bool IsServiceRunning()
        {
            return bServiceRunning;
        }

        /// <summary>
        /// обработка ответа сервера. По коду ответа выбирается метод обработчик из словаря делегатов mapAnswerHandlers,
        /// которому передается ответ сервера в виде объекта Command
        /// </summary>
        /// <param name="answer">ответ сервера в виде объекта Command</param>
        /// <returns>строковое представление ответа сервера</returns>
        public string ProcessAnswer(Command answer)
        {
           bLastCommandOK = false;

            if (answer != null && mapAnswerHandlers.ContainsKey(answer.Code))
               return mapAnswerHandlers[answer.Code](answer);
            else
                return "Ошибка: неизвестный ответ " + answer.Code;

        }
        /// <summary>
        /// Обработчик статусного ответа сервера
        /// </summary>
        /// <param name="answer">ответ сервера в виде объекта Command</param>
        /// <returns>строковое представление ответа сервера</returns>
        string SuccessAndErrorHandler(Command answer)
        {
            string strAnswer;
            if (answer.Code == "success")
            {
                strAnswer = "Успех";
                bLastCommandOK = true;
            }
            else
                strAnswer = "Ошибка";
            if (answer.commandParams.ContainsKey("status"))
                strAnswer = strAnswer + ": " + answer.commandParams["status"];
            return strAnswer;
        }
        /// <summary>
        /// обработчик ответа programs от сервера
        /// приходит в ответ на запрос о списке программ, ожидающих запуска
        /// </summary>
        /// <param name="answer">ответ сервера в виде объекта Command</param>
        /// <returns>всегда пустая строка</returns>
        string ProgramsHandler(Command answer)
        {
            programs.Clear();
            if (answer.commandParams != null)
            {
                int currentNumber = 0;
                for (int i = 0; i < answer.commandParams.Count; i+=3)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("path", answer.commandParams["path"+ currentNumber]);
                    dict.Add("startDate", answer.commandParams["startDate"+ currentNumber]);
                    dict.Add("repeat", answer.commandParams["repeat"+ currentNumber]);
                    currentNumber++;
                    programs.Add(dict);
                }
                bLastCommandOK = true;
            }
            return "";
        }

        /// <summary>
        /// обработчик ответа serviceStatus от сервера
        /// приходит в ответ на запрос состояние сервера
        /// </summary>
        /// <param name="answer">ответ сервера в виде объекта Command</param>
        /// <returns></returns>
        string ServiceStatusHandler(Command answer)
        {
            bServiceRunning = false;
            if(answer.commandParams != null && answer.commandParams.ContainsKey("serviceState") && answer.commandParams["serviceState"].ToString()=="running")
                  bServiceRunning=true;
            bLastCommandOK = true;
            return "";
        }
        /// <summary>
        /// обработчик ответа changeStateStatus от службы 
        /// ответ приходит в ответ на запрос об изменении статуса службы
        /// </summary>
        /// <param name="answer">команда, содержащая ответ сервера</param>
        /// <returns>cтроковый ответ, отражающий изменение статуса сервера</returns>
        string ChangeStateStatusHandler(Command answer)
        {
            string strAnswer;
            bLastCommandOK = false;
            if (answer.commandParams != null && answer.commandParams.ContainsKey("serviceState") && answer.commandParams["serviceState"].ToString() == "running")
                bServiceRunning = true;
            if (answer.commandParams != null && answer.commandParams.ContainsKey("serviceState") && answer.commandParams["serviceState"].ToString() == "stopped")
                bServiceRunning = false;

            if (answer.commandParams.ContainsKey("status"))
            {
                strAnswer = answer.commandParams["status"].ToString();
                bLastCommandOK = true;
            }
            else
                strAnswer = "Не удалось получить ответ от службы";

            return strAnswer;
        }

        /// <summary>
        /// Есть ли подключение к серверу
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return client.Connected;
        }

        /// <summary>
        /// Отключиться от сервера
        /// </summary>
        public void DisconnectFromServer()
        {
            if (client.Connected)
                client.Close();
        }


        /// <summary>
        /// Отправляет запрос серверу
        /// </summary>
        /// <param name="msg">запрос в отфтрматированном виде</param>
        /// <returns>ответ сервера в формате объекта Command</returns>
        public async Task<Command> SendRequest(string msg)
        {
            if (await СonnectToServer())
            {
                NetworkStream networkStream = client.GetStream();
                StreamWriter writer = new StreamWriter(networkStream);
                StreamReader reader = new StreamReader(networkStream);
                writer.AutoFlush = true;
                await writer.WriteLineAsync(msg);
                string response = await reader.ReadLineAsync();
                DisconnectFromServer();
                return commandFormatter.ParseCommand(response);
            }
            else
            {
                Command answer = new Command("error", commandFormatter);
                answer.AddParam("status", "нет соединения с сервером");
                return answer;

            }
        }
    }
}
