using System;
using System.Windows.Forms;
using System.Collections.Generic;

//Клиентское приложение позволяет изменять список программ, ожидающих запуска 
//служба, следит за временем и выполняет запуск программ в указанное время
//Информация от клиента к службе передается в виде команд xml-формата
/*
  Команда завершения работы сервера
 <?xml version="1.0" encoding="utf-8"?>
<Command>
  <Code>Finish</Code>
</Command>
 
  Команда старта сервера
 <?xml version="1.0" encoding="utf-8"?>
<Command>
  <Code>Start</Code>
</Command>
 
   Команда запроса состояния сервера
 <?xml version="1.0" encoding="utf-8"?>
<Command>
  <Code>getServiceStatus</Code>
</Command>


Команда запроса списка программ, ожидающих запуска
 <?xml version="1.0" encoding="utf-8"?>
<Command>
  <Code>getProgramList</Code>
</Command>

Команда добавления папки в список наблюдаемых
 <?xml version="1.0" encoding="utf-8"?>
<Command>
  <Code>addProgram</Code>
 <CommandParams>
    <path>Путь к программе</path>
    <startDate>Дата и время запуска программы</startDate>
    <repeat>Периодичность запуска</repeat>
 </CommandParams>
</Command>

 
Команда удления программы из списка ожидания
 <?xml version="1.0" encoding="utf-8"?>
<Command>
  <Code>delProgram</Code>
 <CommandParams>
    <programId>Номер программы в списке</programId>
 </CommandParams>
</Command>

 */
namespace ProgramPlannerClient
{       
    public partial class Client : Form
    {  
        //интерфейс объекта форматирования команды в/из XML-формат(-а)
        ICommandFormatter cf;
        public Client()
        {
            InitializeComponent();
            timePicker.Format = DateTimePickerFormat.Custom;
            timePicker.ShowUpDown = true;
            cf = new XMLCommandFormatter();
            CheckServiceStatus();
        }
 
        //функция выключения службы клиентом
        void Disconnect()
        {
            try
            {
                ServiceClient.instance.DisconnectFromServer();
            }
            catch {
                slStatus.Text = "Ошибка при подключении к службе";
            }
        }

      //обработчик кнопки добавления папки, за которой будет следить служба
        async private void addFolderButton_Click(object sender, EventArgs e)
        {
            string programPath = txtProgramPath.Text;
            DateTime dateTime = datePicker.Value.Date;
            dateTime = dateTime.AddHours(timePicker.Value.Hour);
            dateTime = dateTime.AddMinutes(timePicker.Value.Minute);
            int repeatModeValue = 0;
            switch (repeatModeBox.SelectedItem)
            {
                case "Каждую минуту":
                {
                    repeatModeValue = 1;
                    break;
                }
                case "Каждый час":
                {
                    repeatModeValue = 60;
                    break;
                }
                case "Каждый день":
                {
                    repeatModeValue = 1440;
                    break;
                }
                default:
                {
                    break;
                }
            }
            // Показываем диалог.
            if (programPath != "")
            {
                Command command = new Command("addProgram", cf);
                command.AddParam("path", programPath);
                command.AddParam("startDate", dateTime);
                command.AddParam("repeat", repeatModeValue);
                Command resp=await ServiceClient.instance.SendRequest(command.GetFormattedCommand());
                lblStatus.Text = ServiceClient.instance.ProcessAnswer(resp);
                if (ServiceClient.instance.BLastCommandStatus)
                    waitingProgramList.Rows.Add(programPath, dateTime, repeatModeBox.SelectedItem);
                txtProgramPath.Text = "";
            }
        }  
     
        /// <summary>
        /// Изменение состояния доступности элементов управления в зависимости от состояния сервера
        /// </summary>
        /// <param name="bServiceRunnig">Запущен ли сервер</param>
        void SetControlStatus(bool bServiceRunning)
        {
            включитьToolStripMenuItem.Enabled = !bServiceRunning;
            отключитьToolStripMenuItem.Enabled = bServiceRunning;
            addProgramButton.Enabled = bServiceRunning;
            getProgramListButton.Enabled = bServiceRunning;
            delProgramButton.Enabled = bServiceRunning;
            if(!bServiceRunning)
            {
                waitingProgramList.Rows.Clear();
                txtProgramPath.Text = "";
            }
            else
            {
                getProgramListButton_Click(null, null);
            }
        }
     
        //При закрытии формы не забываем отключиться от службы
        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ServiceClient.instance.IsServiceRunning())
            {
                Disconnect();
            }
        }
      
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Обработчик нажатия на ссылку получения списка папок
        async private void getProgramListButton_Click(object sender, EventArgs e)
        {
            Command command = new Command("getProgramList", cf);
            Command resp = await ServiceClient.instance.SendRequest(command.GetFormattedCommand());
            lblStatus.Text = ServiceClient.instance.ProcessAnswer(resp);
            waitingProgramList.Rows.Clear();
            foreach (Dictionary<string, object> program in ServiceClient.instance.programs)
            {
                string repeatMode = "Один раз";
                switch (program["repeat"].ToString())
                {
                    case "1":
                        {
                            repeatMode = "Каждую минуту";
                            break;
                        }
                    case "60":
                        {
                            repeatMode = "Каждый час";
                            break;
                        }
                    case "1440":
                        {
                            repeatMode = "Каждый день";
                            break;
                        }
                    default:
                        break;
                }
                waitingProgramList.Rows.Add(program["path"].ToString(), program["startDate"].ToString(), repeatMode);
            }
        }

        //оработчик нажатия на ссылку удаления папки из списка наблюдаемых
        async private void delProgramButton_Click(object sender, EventArgs e)
        {
            if (waitingProgramList.Rows.Count > 0 && waitingProgramList.CurrentRow.Index != -1)
            {
                try
                {
                    Command command = new Command("delProgram", cf);
                    command.AddParam("programId", waitingProgramList.CurrentRow.Index);
                    Command resp = await ServiceClient.instance.SendRequest(command.GetFormattedCommand());
                    lblStatus.Text = ServiceClient.instance.ProcessAnswer(resp);
                    if (ServiceClient.instance.BLastCommandStatus)
                    {
                        ServiceClient.instance.programs.RemoveAt(waitingProgramList.CurrentRow.Index);
                        waitingProgramList.Rows.RemoveAt(waitingProgramList.CurrentRow.Index);
                    }
                } catch (Exception ex)
                {
                    lblStatus.Text = ex.Message;
                }
            }
        }

        //метод, отправляющий команду на получение статус сервера
        async void CheckServiceStatus()
        {
            Command command = new Command("getServiceStatus", cf);
            Command resp = await ServiceClient.instance.SendRequest(command.GetFormattedCommand());
            ServiceClient.instance.ProcessAnswer(resp);
            SetControlStatus(ServiceClient.instance.IsServiceRunning());
        }

        //По таймеру проверяем статус сервера, чтобы обнаружить момент его остановки/запуска
        private void timerCheckService_Tick(object sender, EventArgs e)
        {
            CheckServiceStatus();
        }

       //обработчик выбора пункта меню запуска сервера
       async private void включитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Command command = new Command("start", cf);
            Command resp = await ServiceClient.instance.SendRequest(command.GetFormattedCommand());
            lblStatus.Text = ServiceClient.instance.ProcessAnswer(resp);
            if (resp.commandParams.ContainsKey("serviceState") && resp.commandParams["serviceState"] == "running")
            {
                SetControlStatus(true);
            }
        }

       //обработчик выбора пункта меню остановки сервера
       async private void отключитьToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Command command = new Command("finish", cf);
           Command resp = await ServiceClient.instance.SendRequest(command.GetFormattedCommand());
           lblStatus.Text = ServiceClient.instance.ProcessAnswer(resp);
           if (resp.commandParams.ContainsKey("serviceState") && resp.commandParams["serviceState"]=="stopped")
           {
                SetControlStatus(false);
           }
       }

       //обработчик выбора пункта меню изменения параметров подключения к серверу
       private void параметрыПодключенияToolStripMenuItem_Click(object sender, EventArgs e)
       {
           FormConnectionParams frmParams = new FormConnectionParams();
           frmParams.txtHost.Text = ServiceClient.Host;
           frmParams.txtPort.Text = ServiceClient.Port.ToString(); ;
           if(frmParams.ShowDialog()==DialogResult.OK)
           {
               ServiceClient.Host=frmParams.txtHost.Text;
               ServiceClient.Port=Convert.ToInt32(frmParams.txtPort.Text);
           }
       }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtProgramPath.Text = openFileDialog.FileName;
            }
        }
    }
}
