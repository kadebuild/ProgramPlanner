using System;
using System.ServiceProcess;

namespace ProgramPlannerServer
{
    public partial class Server : ServiceBase
    {
        SocketServer server;
        public Server()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (server == null)
                server = new SocketServer("127.0.0.1", 9999, new XMLCommandFormatter(), AppDomain.CurrentDomain.BaseDirectory);
            if (!server.IsServerRunning())
            {
                server.StartService();
            }
        }

        protected override void OnStop()
        {
            if (server != null)
                server.StopService();
        }
    }
}
