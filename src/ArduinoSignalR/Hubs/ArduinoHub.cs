using System.Collections.Generic;
using System.Threading.Tasks;
using ArduinoSignalR.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ArduinoSignalR.Hubs
{
    [HubName("arduinoHub")]
    public class ArduinoHub : Hub
    {
        private readonly List<string> _servers = new List<string>();
        public void JoinAsServer()
        {
            Groups.Add(Context.ConnectionId, "Server");
            _servers.Add(Context.ConnectionId);
        }

        public void SendToClients(string commandText)
        {
            if (commandText == null)
                return;

            var command = new Command(commandText);

            if (!command.IsValid())
                return;

            Clients.AllExcept(_servers.ToArray()).receiveCommand(command.Name, command.Status);
        }

        public void SendToArduino(string commandText)
        {
            if (commandText == null)
                return;

            var command = new Command(commandText);

            if (!command.IsValid())
                return;

            Clients.OthersInGroup("Server").receiveCommand(commandText);
        }
    }
}