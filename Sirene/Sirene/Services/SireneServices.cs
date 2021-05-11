//using Android.Content;
//using Matcha.BackgroundService;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotifications;
using Sirene.Models;
using Sirene.Views;
using Sirene.Views.Notification;
using Sirene.Views.Registration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sirene.Services
{
    public class SireneServices
    {
        private static HubConnection _connection;
        private static SireneServices _instance;

        public static SireneServices GetInstance()
        {
            if (_connection == null)
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl(Constants.SignalRHubUri)
                    //.AddMessagePackProtocol()
                    .Build();
            }
            if (_connection.State == HubConnectionState.Disconnected)
            {
                _connection.StartAsync();
            }
            _connection.Closed += async (error) =>
            {
                await Task.Delay(5000);
                await _connection.StartAsync();
            };

            if (_instance == null)
            {
                _instance = new SireneServices();
            }
            return _instance;
        }

        private SireneServices()
        {
            _connection.On<bool, Usuario>("ReceiveLogin", (success, currentUser) =>
            {
                if (success)
                {
                    Task.Run(async () => { await GetIn(currentUser); });
                    if (App.Current.MainPage.GetType() == typeof(NavigationPage) && ((NavigationPage)App.Current.MainPage).CurrentPage.GetType() == typeof(MainMenu))
                    {
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        var mainMenu = (MainMenu)navigationPage.CurrentPage;
                        mainMenu.AddUserConnectionId(currentUser);
                    }
                }
                else
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        App.Current.MainPage = new NavigationPage(new Views.AccessDenied());
                    });
                }
            });

            _connection.On<bool, string>("ReceiveAddUserConnectionId", (success, msg) =>
            {
                if (!success)
                {
                    Console.WriteLine(msg);
                }
            });

            _connection.On<string>("OpenGroup", (groupName) =>
            {
                if (App.Current.MainPage.GetType() == typeof(NavigationPage) && ((NavigationPage)App.Current.MainPage).CurrentPage.GetType() == typeof(EmergencyNotificationTrigger))
                {
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    var notificationTrigger = (EmergencyNotificationTrigger)navigationPage.CurrentPage;
                    notificationTrigger.SetGroupName(groupName);
                }
            });

            _connection.On<Notificacao, Grupo, string>("ReceiveMessage", (msg, group, userId) =>
            {
                string currentUserId = UserManager.GetLogUser().Id;
                if (group.Usuarios.Contains(currentUserId))
                {
                    //Se for o próprio Operador, abre o monitor de notificação
                    if (currentUserId == userId)
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.Current.MainPage.Navigation.PushAsync(new Views.Notification.EmergencyNotificationMonitor(msg));
                        });
                    }
                    else //Senão, abre a notificação
                    {
                        //Tela de Notificação de Emergência
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                       {
                           //###############################################################################################################
                           //###############################################################################################################
                           //###############################################################################################################
                           string title = $"NOTIFICAÇÃO DE EMERGÊNCIA";
                           string message = msg.Text;
                           CrossLocalNotifications.Current.Show(title, message);

                           try
                           {
                               IPlaySoundService playSoundService;
                               playSoundService = DependencyService.Get<IPlaySoundService>();
                               playSoundService.PlaySystemSound();
                               DeviceDisplay.KeepScreenOn = true;
                               while (((NavigationPage)App.Current.MainPage).CurrentPage.GetType() != typeof(EmergencyNotificationReceiver))
                               {
                                   var tst = ((NavigationPage)App.Current.MainPage).CurrentPage.GetType();
                                   DeviceDisplay.KeepScreenOn = false;
                                   Vibration.Vibrate();
                                   await Flashlight.TurnOnAsync();
                                   await Flashlight.TurnOffAsync();
                               }

                               playSoundService.StopSystemSound();
                               Vibration.Cancel();
                           }
                           catch (FeatureNotSupportedException fnsEx)
                           {
                               // Handle not supported on device exception
                               string sLine = fnsEx.StackTrace.Substring(fnsEx.StackTrace.IndexOf("line"));
                               sLine = sLine.Substring(0, sLine.IndexOf("at"));
                               if (!sLine.Equals("153"))
                               {
                                   IPlaySoundService playSoundService;
                                   playSoundService = DependencyService.Get<IPlaySoundService>();
                                   playSoundService.StopSystemSound();
                                   Vibration.Cancel();
                               }
                           }
                           catch (PermissionException)
                           {
                               // Handle permission exception
                               IPlaySoundService playSoundService;
                               playSoundService = DependencyService.Get<IPlaySoundService>();
                               playSoundService.StopSystemSound();
                               Vibration.Cancel();
                           }
                           catch (Exception)
                           {
                               // Unable to turn on/off flashlight
                               IPlaySoundService playSoundService;
                               playSoundService = DependencyService.Get<IPlaySoundService>();
                               playSoundService.StopSystemSound();
                               Vibration.Cancel();
                           }
                       });
                    }
                }
            });

            _connection.On<ObservableCollection<Monitoring>, string>("ReceiveMonitoringNotification", (monitoringList, error) =>
            {
                if (App.Current.MainPage.GetType() == typeof(NavigationPage) && ((NavigationPage)App.Current.MainPage).CurrentPage.GetType() == typeof(EmergencyNotificationMonitor))
                {
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    var monitList = (EmergencyNotificationMonitor)navigationPage.CurrentPage;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        ((MonitoringListViewModel)(monitList.BindingContext)).Monitoring = monitoringList;
                    });
                }
            });

            _connection.On<Monitoring, string>("ReceiveReplayNotification", (monitoring, error) =>
            {
                if (App.Current.MainPage.GetType() == typeof(NavigationPage) && ((NavigationPage)App.Current.MainPage).CurrentPage.GetType() == typeof(EmergencyNotificationMonitor))
                {
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    var monitList = (EmergencyNotificationMonitor)navigationPage.CurrentPage;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        ObservableCollection<Monitoring> monitoringListRefresh = ((MonitoringListViewModel)(monitList.BindingContext)).Monitoring;
                        foreach (Monitoring oldMonitoring in monitoringListRefresh)
                        {
                            if (oldMonitoring.Id == monitoring.Id)
                            {
                                monitoringListRefresh.Remove(oldMonitoring);
                                monitoringListRefresh.Add(monitoring);
                                break;
                            }
                        }

                       ((MonitoringListViewModel)(monitList.BindingContext)).Monitoring = monitoringListRefresh;
                    });
                }
            });

            _connection.On<string>("ReceiveFinalization", (receive) =>
            {
                Console.WriteLine(receive);
            });

            _connection.On<string>("BackgroundStarted", (receive) =>
            {
                Console.WriteLine(receive);
                //Console.WriteLine("Api - TESTE: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            });
        }

        public void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };
                //stackLayout.Children.Add(msg);
            });
        }

        //public async Task StartBackground()
        //{
        //    await _connection.InvokeAsync("StartBackground");
        //}

        public async Task Login(Usuario currentUser)
        {
            await _connection.InvokeAsync("Login", currentUser);
        }

        //public async Task GetOut(Usuario currentUser)
        //{
        //    UserManager.DelLogUser();
        //    await _connection.InvokeAsync("Logout", currentUser);
        //}

        public async Task GetIn(Usuario currentUser)
        {
            UserManager.SetLogUser(currentUser);
            await _connection.InvokeAsync("AddUserConnectionId", currentUser);
        }

        public async Task AddUserConnectionId(Usuario currentUser)
        {
            UserManager.SetLogUser(currentUser);
            await _connection.InvokeAsync("AddUserConnectionId", currentUser);
        }

        public async Task CreateOrOpenGroup(Usuario currentUser)
        {
            await _connection.InvokeAsync("CreateOrOpenGroup", currentUser);
        }

        public async Task SendMessage(Usuario currentUser, string msgType, string msg, string groupName)
        {
            await _connection.InvokeAsync("SendMessage", currentUser, msgType, msg, groupName);
        }

        public async Task MonitoringNotification(Notificacao msg)
        {
            await _connection.InvokeAsync("MonitoringNotification", msg);
        }

        public async Task ReplyNotification(Usuario user, string groupName, string response)
        {
            await _connection.InvokeAsync("ReplyNotification", user, groupName, response);
        }

        public async Task FinalizeNotification(string queryKey, Usuario currentUser)
        {
            await _connection.InvokeAsync("FinalizeNotification", queryKey, currentUser);
        }
    }
}