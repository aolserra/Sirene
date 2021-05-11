using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Notification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyNotificationMonitor : ContentPage
    {
        public EmergencyNotificationMonitor(Notificacao msg)
        {
            InitializeComponent();

            //OnFinalizeEmergencyActivated.IsVisible = (msg.NotificationType.Equals(Constants.TypeEmergency)) ? true : false;
            //OnFinalizeAlarmActivated.IsVisible = (msg.NotificationType.Equals(Constants.TypeAlarm)) ? true : false;

            Task.Run(async () => { await SireneServices.GetInstance().MonitoringNotification(msg); });

            OnFinalizeEmergencyActivated.Clicked += async (sender, args) =>
            {
                await SireneServices.GetInstance().FinalizeNotification(Constants.IdComunidadeQuery, UserManager.GetLogUser());
                await Navigation.PopToRootAsync();
            };
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }

    public class MonitoringListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Monitoring> _monitoring;
        public ObservableCollection<Monitoring> Monitoring
        {
            get
            {
                return _monitoring;
            }
            set
            {
                _monitoring = value;
                NotifyPropertyChanged(nameof(Monitoring));
            }
        }

        public MonitoringListViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}