using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;

namespace ProgramPlannerServer
{
    /// <summary>
    /// Класс, инкапсулирующий работу сервера приложения.
    /// Обеспечивает обработку команд от клиентских приложений,
    /// отслеживание запускаемых приложений и закрытие тех из них, которые
    /// запущены из запрещенных папок
    /// </summary>
    class SocketServer
    {
        //Ip-адрес сервера     
        private IPAddress ipAddress;
        //порт, который слушает сервер
        private int port;
        //ссылка на класс, обеспечивающий запись событий в журнал сервера
        LoggerClass log;

        //запущен ли сервер
        bool isRunning = false;

        //запущен ли таймер
        public bool timerEnabled;

        public Timer timer = new Timer();

        //объект-сокет для обмена данными с клиентами
        TcpListener listener;

        //Объект для хранения (в том числе сохранения в файле) список папок, из которых запрещен запуск приложений
        WaitingProgramList programs;

        //интерфейс форматирования команд (сериализации-десериализации)
        ICommandFormatter commandFormatter;

        delegate Command commandHandler(Command command);

        //список обработчиков команд клиентов
        Dictionary<string, commandHandler> mapCommandHandlers;

        /// <summary>
        /// конструктор класса
        /// </summary>
        /// <param name="ipAddress">адрес сервера</param>
        /// <param name="port">порт сервера</param>
        /// <param name="formatter">акой форматтер будет использоваться для сериализации-десериализации команд при обмене с клиентами<</param>
        /// <param name="appPath">путь к папке с сервером</param>
        public SocketServer(string ipAddress, int port, XMLCommandFormatter formatter, string appPath)
        {
            this.port = port;
            string hostName = Dns.GetHostName();
            this.ipAddress = IPAddress.Parse(ipAddress);
            programs = new WaitingProgramList(appPath + "program.xml");
            log = new LoggerClass();
            commandFormatter = formatter;
            // сопоставляем возможные команды клиентов с ссылками на методы-обработчики этих команд
            mapCommandHandlers = new Dictionary<string, commandHandler>() { {"finish", FinishCommandHandler},
                                                                            {"start", StartCommandHandler},
                                                                            {"getServiceStatus", GetStatusCommandHandler},
                                                                            {"getProgramList", GetProgramListHandler},
                                                                            {"wrongCommand", WrongCommandHandler},
                                                                            {"addProgram", AddProgramHandler},
                                                                            {"delProgram", DelProgramHandler}
                                                                          };
        }

        public bool IsServerRunning()
        {
            return timerEnabled;
        }

        //Основной цикл работы сервера - ожидание и отправка на обработку команд от клиентов с использованием сокетов
        public async void Run()
        {
            listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();
            isRunning = true;
            while (isRunning)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Task t = ProcessRequest(tcpClient);
                    await t;
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// метод чтения команд от клиента и передачи их на обработку
        /// </summary>
        /// <param name="tcpClient">сокет для связи с клиентом</param>
        /// <returns>асинхронно выполнямая задача</returns>
        private async Task ProcessRequest(TcpClient tcpClient)
        {
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            bool bReqDone = false;
            Command response;
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(networkStream);
                StreamWriter writer = new StreamWriter(networkStream);
                writer.AutoFlush = true;
                while (!bReqDone)
                {   //читаем строку с сериализованной командой клиента
                    string request = await reader.ReadLineAsync();
                    if (request != null)
                    {
                        //преобразуем команду в объект Command с использованием форматтера
                        Command command = commandFormatter.ParseCommand(request);
                        command.senderAddress = clientEndPoint;
                        if (command != null && mapCommandHandlers.ContainsKey(command.Code))
                            response = mapCommandHandlers[command.Code](command);
                        else
                            response = mapCommandHandlers["wrongCommand"](command);
                        //асинхронно отправляем ответ клиенту
                        await writer.WriteLineAsync(response.GetFormattedCommand());
                    }
                    else
                        bReqDone = true; // клиент закрыл соединение
                }
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                lock (log)
                {
                    log.WriteLogEntry("Ошибка", ex.Message + " / " + ex.StackTrace);
                }
                if (tcpClient.Connected)
                    tcpClient.Close();
            }

        }

        /// <summary>
        /// Обработчик команды заверщшения работы сервера
        /// Останавливает отслеживание процессов, запущенных из запрещенных папок
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду на завершение работы сервера</param>
        /// <returns>результат обработки команды в виде объекта Command</returns>
        public Command FinishCommandHandler(Command command)
        {
            Command answer;
            answer = new Command("changeServiceStatus", commandFormatter);
            if (IsServerRunning())
                StopTimer();
            answer.AddParam("status", "Служба остановлена");
            answer.AddParam("serviceState", "stopped");
            lock (log)
            {
                log.WriteLogEntry("event", "Клиент " + command.senderAddress + " остановил службу");
            }
            return answer;
        }

        /// <summary>
        /// Обработчик команды запуска сервера
        /// Запускаетт отслеживание процессов, запущенных из запрещенных папок
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду на завершение работы сервера</param>
        /// <returns>результат обработки команды в виде объекта Command</returns>
        public Command StartCommandHandler(Command command)
        {
            Command answer;
            answer = new Command("changeServiceStatus", commandFormatter);
            if (!IsServerRunning())
                StartTimer();
            answer.AddParam("status", "Служба запущена");
            answer.AddParam("serviceState", "running");
            lock (log)
            {
                log.WriteLogEntry("event", "Клиент " + command.senderAddress + " запустил службу");
            }
            return answer;
        }

        /// <summary>
        /// Обработчик команды получения текущего состояния сервера
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду - запрос текущего состояния сервера</param>
        /// <returns>результат обработки команды в виде объекта Command (в параметре - текущее состояние сервера)</returns>
        Command GetStatusCommandHandler(Command command)
        {
            if (IsServerRunning())
            {
                Command answer = new Command("serviceStatus", commandFormatter);
                answer.AddParam("serviceState", isRunning ? "running" : "stopped");
                return answer;
            }
            else
                return СreateServiceDownAnswer();

        }

        /// <summary>
        /// Обработчик команды запроса текущего списка программ ожидающих запуска
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду</param>
        /// <returns>результат обработки команды в виде объекта Command (в параметрах  - список 
        /// зафикированных на сервере программа ожидающих запуска)</returns>
        Command GetProgramListHandler(Command command)
        {
            lock (programs)
            {
                if (IsServerRunning())
                {
                    Command answer = new Command("programs", commandFormatter);
                    for (int i = 0; i < programs.ProgramList.Count; i++)
                    {
                        answer.AddParam("path" + i, programs.ProgramList[i]["path"]);
                        answer.AddParam("startDate" + i, programs.ProgramList[i]["startDate"]);
                        answer.AddParam("repeat" + i, programs.ProgramList[i]["repeat"]);
                    }
                    return answer;
                }
                else
                    return СreateServiceDownAnswer();
            }
        }

        /// <summary>
        /// Обработчик нераспознанной команды
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду</param>
        /// <returns>результат обработки команды в виде объекта Command</returns>
        Command WrongCommandHandler(Command command)
        {
            Command answer = new Command("error", commandFormatter);
            answer.AddParam("status", "Команда не распознана");
            lock (log)
            {
                log.WriteLogEntry("event", "Ошибочная команда от " + command.senderAddress);
            }
            return answer;
        }

        /// <summary>
        /// Обработчик команды добавления новой программы в список ожидающих
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду (имя папки в параметре folderName)</param>
        /// <returns>результат обработки команды в виде объекта Command</returns>
        Command AddProgramHandler(Command command)
        {
            Command answer;
            lock (programs)
            {
                if (IsServerRunning())
                {
                    if (command.commandParams.Count == 0)
                    {
                        answer = new Command("error", commandFormatter);
                        answer.AddParam("status", "программа не задана");
                        lock (log)
                        {
                            log.WriteLogEntry("event", "Неудачная попытка добавления программы клиентом " + command.senderAddress);
                        }
                    }
                    else
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("path", command.commandParams["path"]);
                        dict.Add("startDate", command.commandParams["startDate"]);
                        dict.Add("repeat", command.commandParams["repeat"]);
                        if (programs.AddProgram(dict))
                        {
                            answer = new Command("success", commandFormatter);
                            answer.AddParam("status", "программа добавлена");
                            lock (log)
                            {
                                log.WriteLogEntry("event", "Клиент " + command.senderAddress + " добавил программу " + command.commandParams["path"]);
                            }
                        }
                        else
                        {
                            answer = new Command("error", commandFormatter);
                            answer.AddParam("status", "ошибка при добавлении программы на запуск");
                            lock (log)
                            {
                                log.WriteLogEntry("event", "Неудачная попытка добавления программы клиентом " + command.senderAddress);
                            }

                        }
                    }
                    return answer;
                }
                else
                    return СreateServiceDownAnswer();
            }
        }

        /// <summary>
        /// Формирование ответа в случае неработающего сервера
        /// </summary>
        /// <returns>результат обработки команды в виде объекта Command</returns>
        Command СreateServiceDownAnswer()
        {
            Command answer = new Command("error", commandFormatter);
            answer.AddParam("status", "служба не запущена");
            return answer;

        }

        /// <summary>
        /// Обработчик команды удаления программы из списка ожидающих на запуск
        /// </summary>
        /// <param name="command">Объект типа Command, хранящий команду (номер удаляемой программы в параметре programId)</param>
        /// <returns>результат обработки команды в виде объекта Command</returns>
        Command DelProgramHandler(Command command)
        {
            Command answer;
            lock (programs)
            {
                if (IsServerRunning())
                {
                    if (command.commandParams.Count != 1)
                    {
                        answer = new Command("error", commandFormatter);
                        answer.AddParam("status", "программа не задана");
                        lock (log)
                        {
                            log.WriteLogEntry("event", "Неудачная попытка удаления программы из ожидания клиентом " + command.senderAddress);
                        }
                    }
                    else
                    {
                        string delProgramPath = programs.ProgramList[Int32.Parse(command.commandParams["programId"].ToString())]["path"].ToString();
                        if (programs.DelProgram(Int32.Parse(command.commandParams["programId"].ToString())))
                        {
                            answer = new Command("success", commandFormatter);
                            answer.AddParam("status", "программа удалена из списка запуска");
                            lock (log)
                            {
                                log.WriteLogEntry("event", "Клиент " + command.senderAddress + " удалил программу из ожидания " + delProgramPath);
                            }
                        }
                        else
                        {
                            answer = new Command("error", commandFormatter);
                            answer.AddParam("status", "программа с таким номером не существует");
                            lock (log)
                            {
                                log.WriteLogEntry("event", "Неудачная попытка удаления программы из ожидания клиентом " + command.senderAddress);
                            }
                        }
                    }
                    return answer;
                }
                else
                    return СreateServiceDownAnswer();
            }
        }

        /// <summary>
        /// запуск сервера
        /// </summary>
        public bool StartService()
        {
            if (!isRunning)
            {
                StartTimer();
                Run();
                return true;
            }
            return false;
        }

        /// <summary>
        /// остановка сервера
        /// </summary>
        /// <returns>Удалось (true) или нет (false) остановить сервер </returns>
        public bool StopService()
        {
            if (isRunning)
            {
                isRunning = false;
                StopTimer();
                listener.Stop();
                return true;
            }
            return false;
        }

        /// <summary>
        /// старт таймера, следящего за запуском программ по времени
        /// </summary>
        public void StartTimer()
        {
            timerEnabled = true;
            Task watchigTask = new Task(ProgramTimer);
            watchigTask.Start();
        }

        /// <summary>
        /// остановка таймера, следящего за запуском программ по времени
        /// </summary>
        public void StopTimer()
        {
            timerEnabled = false;
            timer.Stop();
        }

        //метод отслеживания запуска программ
        private void ProgramTimer()
        {
            timer.Interval = (60 - DateTime.Now.Second) * 1000;
            timer.Elapsed += (sender, e) =>
            {
                timer.Interval = 60000;
                for (int i = 0; i < programs.Count; i++)
                {
                    string buf = "";
                    DateTime startDate;
                    if (programs.ProgramList[i]["startDate"].GetType() == buf.GetType())
                    {
                        startDate = DateTime.Parse(programs.ProgramList[i]["startDate"].ToString());
                    }
                    else
                    {
                        startDate = (DateTime)programs.ProgramList[i]["startDate"];
                    }
                    int compareDateResult = startDate.CompareTo(DateTime.Now.AddSeconds(1));
                    if (compareDateResult < 1)
                    {
                        ProcessStartInfo programToRun = new ProcessStartInfo(programs.ProgramList[i]["path"].ToString());
                        programToRun.CreateNoWindow = true;
                        programToRun.UseShellExecute = false;
                        Process.Start(programToRun);
                        int repeatMode = Int32.Parse(programs.ProgramList[i]["repeat"].ToString());
                        if (repeatMode == 0)
                        {
                            programs.DelProgram(i);
                        }
                        else
                        {
                            if (compareDateResult == -1)
                                startDate = DateTime.Now;
                            startDate = startDate.AddMinutes((double)repeatMode);
                            startDate = startDate.AddSeconds(-startDate.Second);
                            programs.ProgramList[i]["startDate"] = startDate;
                            programs.UpdateStartDateProgram(i, startDate.ToString());
                        }
                    }
                }
            };
            timer.Start();
        }
    }
}
