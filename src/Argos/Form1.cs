using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using ArgosDesktop.Models;
using ArgosDesktop.Properties;
using Microsoft.AspNet.SignalR.Client;

namespace ArgosDesktop
{
    public partial class Form1 : Form
    {
        private readonly SerialPort _porta = new SerialPort("COM3", 9600) { ReceivedBytesThreshold = 3 };
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private IHubProxy _arduinoHubProxy;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectToHub();

            _timer.Interval = 1000;
            _timer.Elapsed += TimerElapsed;
            _porta.DataReceived += DataReceivedHandler;
            _worker.DoWork += DoWork;

            _timer.Enabled = true;
            _timer.Start();
        }

        void ConnectToHub()
        {
            var hubConnection = new HubConnection("http://localhost:7030/");
            _arduinoHubProxy = hubConnection.CreateHubProxy("arduinoHub");

            _arduinoHubProxy.On<string>("receiveCommand", commandText =>
            {
                var receivedCommand = new Command(commandText);
                if (!receivedCommand.IsValid())
                    return;

                SendCommandText(commandText);
            });

            var start = hubConnection.Start();
            start.Wait();

            var join = _arduinoHubProxy.Invoke("JoinAsServer");
            join.Wait();
        }

        void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            if (!_porta.IsOpen)
                _porta.Open();

            _porta.Write("l1"); Thread.Sleep(100);
            _porta.Write("l2"); Thread.Sleep(100);
            _porta.Write("l3"); Thread.Sleep(100);
            //_porta.Write("p1"); Thread.Sleep(100);
            //_porta.Write("p2"); Thread.Sleep(100);
            _porta.Write("p3"); Thread.Sleep(100);
            //_porta.Write("j1"); Thread.Sleep(100);
            //_porta.Write("j2"); Thread.Sleep(100);
            _porta.Write("j3"); Thread.Sleep(100);
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;
            var data = serialPort.ReadExisting();

            if (!_timer.Enabled)
                return;

            var hops = data.Length / 3;
            for (var i = 0; i < hops; i++)
            {
                var commandText = data.Substring(0, 3);

                var command = new Command(commandText);

                if (!command.IsValid())
                    return;

                if (_arduinoHubProxy != null)
                    _arduinoHubProxy.Invoke("SendToClients", commandText);

                if (InvokeRequired)
                    Invoke(new Atualiza(AtualizaConteudoLabel), commandText);
                else
                    AtualizaConteudoLabel(commandText);

                i = i + 2;
            }
        }

        delegate void Atualiza(string texto);
        private void AtualizaConteudoLabel(string texto)
        {
            switch (texto)
            {
                case "l11":
                case "l10":
                    labelLamp1.Text = texto;
                    pictureLamp1.Image = texto == "l10" ? Resources.lamp_off : Resources.lamp_on;
                    break;
                case "l21":
                case "l20":
                    labelLamp2.Text = texto;
                    pictureLamp2.Image = texto == "l20" ? Resources.lamp_off : Resources.lamp_on;
                    break;
                case "l31":
                case "l30":
                    labelLamp3.Text = texto;
                    pictureLamp3.Image = texto == "l30" ? Resources.lamp_off : Resources.lamp_on;
                    break;
                case "p31":
                case "p30":
                    picturePorta3.Image = texto == "p30" ? Resources.porta_off : Resources.porta_on;
                    break;
                case "j31":
                case "j30":
                    pictureJanela3.Image = texto == "j30" ? Resources.janela_off : Resources.janela_on;
                    break;
            }
        }

        private void buttonApagador1_Click(object sender, EventArgs e)
        {
            AlteraStatusDispositivo("a1", "l1", labelLamp1.Text);
        }

        private void buttonApagador2_Click(object sender, EventArgs e)
        {
            AlteraStatusDispositivo("a2", "l2", labelLamp2.Text);
        }

        private void buttonApagador3_Click(object sender, EventArgs e)
        {
            AlteraStatusDispositivo("a3", "l3", labelLamp3.Text);
        }

        private void AlteraStatusDispositivo(string dispositivo, string dispositivosaida, string statusatual)
        {
            DesligaTimer();

            dispositivosaida += "0";

            if (!_porta.IsOpen)
                _porta.Open();

            if (dispositivosaida == statusatual)
                _porta.Write(dispositivo + "1");
            else
                _porta.Write(dispositivo + "0");

            LigaTimer();
        }

        void DesligaTimer()
        {
            _timer.Enabled = false;
            _timer.Stop();
            Thread.Sleep(50);
        }

        private void LigaTimer()
        {
            _timer.Enabled = true;
            _timer.Start();
            Thread.Sleep(50);
        }

        void SendCommandText(string commandText)
        {
            DesligaTimer();

            if (!_porta.IsOpen)
                _porta.Open();

            _porta.Write(commandText.Replace("l", "a"));

            LigaTimer();
        }

        private void buttonDesliga_Click(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            _timer.Stop();
            _worker.CancelAsync();

            if (_porta.IsOpen)
                _porta.Close();

            Close();
        }
    }
}