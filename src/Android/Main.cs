using Android.App;
using Android.OS;
using Android.Widget;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Android
{
    [Activity(Label = "Argos", MainLauncher = true, Icon = "@drawable/icon")]
    public class Main : Activity
    {
        private HubConnection _hubConnection;
        private IHubProxy _arduinoHubProxy;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            try
            {
                await ConnectToArduinoHub();
                CreateEvents();
            }
            catch (Exception e)
            {
                ShowAlert("Erro", e.Message);
            }
        }

        private void CreateEvents()
        {
            FindViewById<ImageButton>(Resource.Id.imageButton1).Click += (sender, e) => OnClickLampada1();
            FindViewById<ImageButton>(Resource.Id.imageButton2).Click += (sender, e) => OnClickLampada2();
            FindViewById<ImageButton>(Resource.Id.imageButton3).Click += (sender, e) => OnClickLampada3();
        }

        private void OnClickLampada1()
        {
            RunOnUiThread(async () =>
            {
                var button = FindViewById<ImageButton>(Resource.Id.imageButton1);

                bool active;
                bool.TryParse(button.GetTag(Resource.Id.imageButton1).ToString(), out active);
                await Send(active ? "l10" : "l11");
            });
        }

        private void OnClickLampada2()
        {
            RunOnUiThread(async () =>
            {
                var button = FindViewById<ImageButton>(Resource.Id.imageButton2);

                bool active;
                bool.TryParse(button.GetTag(Resource.Id.imageButton2).ToString(), out active);
                await Send(active ? "l20" : "l21");
            });
        }

        private void OnClickLampada3()
        {
            RunOnUiThread(async () =>
            {
                var button = FindViewById<ImageButton>(Resource.Id.imageButton3);

                bool active;
                bool.TryParse(button.GetTag(Resource.Id.imageButton3).ToString(), out active);
                await Send(active ? "l30" : "l31");
            });
        }

        private void ShowAlert(string title, string message)
        {
            RunOnUiThread(() =>
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetCancelable(true);
                builder.SetPositiveButton("OK", delegate { });
                builder.Show();
            });
        }

        private async Task ConnectToArduinoHub()
        {
            _hubConnection = new HubConnection("http://10.0.2.2/ArduinoSignalR/")
            {
                TraceLevel = TraceLevels.All,
                TraceWriter = Console.Out
            };

            _arduinoHubProxy = _hubConnection.CreateHubProxy("arduinoHub");
            _arduinoHubProxy.On<string, string>("receiveCommand", (name, status) => RunOnUiThread(() => Receive(name, status)));
            await _hubConnection.Start();
        }

        private void Receive(string name, string status)
        {
            var resourceLampada = status == "Active" ? Resource.Drawable.lamp_on : Resource.Drawable.lamp_off;
            var resourcePorta = status == "Active" ? Resource.Drawable.porta_on : Resource.Drawable.porta_off;
            var resourceJanela = status == "Active" ? Resource.Drawable.janela_on : Resource.Drawable.janela_off;
            var state = status == "Active";

            switch (name)
            {
                case "l1":
                    {
                        var button1 = FindViewById<ImageButton>(Resource.Id.imageButton1);
                        button1.SetImageResource(resourceLampada);
                        button1.SetTag(Resource.Id.imageButton1, state);
                        break;
                    }
                case "l2":
                    {
                        var button2 = FindViewById<ImageButton>(Resource.Id.imageButton2);
                        button2.SetImageResource(resourceLampada);
                        button2.SetTag(Resource.Id.imageButton2, state);
                        break;
                    }
                case "l3":
                    {
                        var button3 = FindViewById<ImageButton>(Resource.Id.imageButton3);
                        button3.SetImageResource(resourceLampada);
                        button3.SetTag(Resource.Id.imageButton3, state);
                        break;
                    }
                case "p1":
                    FindViewById<ImageView>(Resource.Id.imageView2).SetImageResource(resourcePorta);
                    break;
                case "p2":
                    FindViewById<ImageView>(Resource.Id.imageView4).SetImageResource(resourcePorta);
                    break;
                case "p3":
                    FindViewById<ImageView>(Resource.Id.imageView6).SetImageResource(resourcePorta);
                    break;
                case "j1":
                    FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(resourceJanela);
                    break;
                case "j2":
                    FindViewById<ImageView>(Resource.Id.imageView3).SetImageResource(resourceJanela);
                    break;
                case "j3":
                    FindViewById<ImageView>(Resource.Id.imageView5).SetImageResource(resourceJanela);
                    break;
            }
        }

        private async Task Send(string command)
        {
            await _arduinoHubProxy.Invoke("sendToArduino", command);
        }
    }
}