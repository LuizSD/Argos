using System;
using System.Runtime.InteropServices;
using ArduinoSignalRClient.Models;
using Microsoft.AspNet.SignalR.Client;

namespace ArduinoSignalRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://localhost:7030/");
            var arduinoHubProxy = hubConnection.CreateHubProxy("arduinoHub");

            arduinoHubProxy.On<string>("receiveCommand", commandText =>
            {
                var receivedCommand = new Command(commandText);
                if (receivedCommand.IsValid())
                {
                    Console.WriteLine("Recebido: {0}", commandText);
                }
            });
                
            var start = hubConnection.Start();
            start.Wait();

            var join = arduinoHubProxy.Invoke("JoinAsServer");
            join.Wait();

            var command = string.Empty;

            while (command != "exit")
            {
                command = Console.ReadLine();

                if (command != "exit")
                {
                    var send = arduinoHubProxy.Invoke("SendToClients", command);
                    send.Wait();
                }
            }
        }
    }
}
