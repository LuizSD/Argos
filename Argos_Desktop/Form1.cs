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
        private SerialPort _porta = new SerialPort("COM3", 9600);
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
            Thread.Sleep(60);

            _porta.Write("l2");
            Thread.Sleep(60);

            _porta.Write("l3");
            Thread.Sleep(60);

            //_porta.Write("p1");
            //Thread.Sleep(60);

            //_porta.Write("p2");
            //Thread.Sleep(60);

            _porta.Write("p3");
            Thread.Sleep(60);

            //_porta.Write("j1");
            //Thread.Sleep(60);

            //_porta.Write("j2");
            //Thread.Sleep(60);

            _porta.Write("j3");
            Thread.Sleep(60);
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
            if (texto == "l10" || texto == "l11")
            {
                labelLamp1.Text = texto;
                //var lampada1 = texto;
                if (texto == "l10")
                    pictureLamp1.Image = Argos_Desktop.Properties.Resources.lamp_off;
                else
                    pictureLamp1.Image = Argos_Desktop.Properties.Resources.lamp_on;
            }

            if (texto == "l20" || texto == "l21")
            {
                labelLamp2.Text = texto;
                //var lampada2 = texto;
                if (texto == "l20")
                    pictureLamp2.Image = Argos_Desktop.Properties.Resources.lamp_off;
                else
                    pictureLamp2.Image = Argos_Desktop.Properties.Resources.lamp_on;
            }
                

            if (texto == "l30" || texto == "l31")
            {
                labelLamp3.Text = texto;
                //var lampada3 = texto;
                if (texto == "l30")
                    pictureLamp3.Image = Argos_Desktop.Properties.Resources.lamp_off;
                else
                    pictureLamp3.Image = Argos_Desktop.Properties.Resources.lamp_on;
            }
                
            if (texto == "p30" || texto == "p31")
            {
                if (texto == "p30")
                    picturePorta3.Image = Argos_Desktop.Properties.Resources.porta_off;
                else
                    picturePorta3.Image = Argos_Desktop.Properties.Resources.porta_on;
            }

            if (texto == "j30" || texto == "j31")
            {
                if (texto == "j30")
                    pictureJanela3.Image = Argos_Desktop.Properties.Resources.janela_off;
                else
                    pictureJanela3.Image = Argos_Desktop.Properties.Resources.janela_on;
            }
        }

        private void buttonApagador1_Click(object sender, EventArgs e)
        {
            alteraStatus("a1", "l1", labelLamp1.Text);
        }

        private void buttonApagador2_Click(object sender, EventArgs e)
        {
            alteraStatus("a2", "l2", labelLamp2.Text);
        }

        private void buttonApagador3_Click(object sender, EventArgs e)
        {
            alteraStatus("a3", "l3", labelLamp3.Text);
        }


        private void alteraStatus(string dispositivo, string dispositivosaida, string statusatual)
        {
            _timer.Enabled = false;
            _timer.Stop();

            Thread.Sleep(100);

            dispositivosaida += "0";

            if (!_porta.IsOpen)
                _porta.Open();

            if (dispositivosaida == statusatual)
                _porta.Write(dispositivo + "1");
            else
                _porta.Write(dispositivo + "0");

            Thread.Sleep(100);

            _timer.Enabled = true;
            _timer.Start();
        }

        private void buttonDesliga_Click(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            _timer.Stop();
            _worker.CancelAsync();

            if (_porta.IsOpen)
                _porta.Close();

            Form1.ActiveForm.Close();
        }

    }
}