using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace Argos_Desktop
{
    public partial class Form1 : Form
    {
        private SerialPort _porta = new SerialPort("COM5", 9600);
        private BackgroundWorker _worker = new BackgroundWorker();
        private System.Timers.Timer _timer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _timer.Interval = 50;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            _porta.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _worker.DoWork += new DoWorkEventHandler(DoWork);

            _timer.Enabled = true;
            _timer.Start();
        }

        void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            if (!_porta.IsOpen)
                _porta.Open();

            _porta.Write("l1");
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;
            var texto = serialPort.ReadExisting();

            var codes = new List<string> 
                            {
                                "l10", "l11",
                                "l20", "l21",
                                "l30", "l31",
                                "p10", "p11",
                                "p20", "p21",
                                "p30", "p31",
                                "j10", "j11",
                                "j20", "j21",
                                "j30", "j31"
                            };

            if (!codes.Contains(texto))
                return;

            if (!_timer.Enabled)
                return;

            if (InvokeRequired)
                Invoke(new Atualiza(AtualizaConteudoLabel), texto);
            else
                AtualizaConteudoLabel(texto);
        }

        delegate void Atualiza(string texto);
        private void AtualizaConteudoLabel(string texto)
        {
            labelResponse.Text = texto;
        }

        private void buttonApagador1_Click(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            _timer.Stop();

            Thread.Sleep(50);
        
            int response;
            int.TryParse(labelResponse.Text, out response);

            labelResponse.Text = string.Empty;

            if (!_porta.IsOpen)
                _porta.Open();

            if (response == 0)
                _porta.Write("a11");
            else
                _porta.Write("a10");

            Thread.Sleep(50);

            _timer.Enabled = true;
            _timer.Start();
        }
    }
}