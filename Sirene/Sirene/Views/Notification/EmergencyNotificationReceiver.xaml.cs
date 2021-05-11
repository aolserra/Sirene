using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Notification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyNotificationReceiver : ContentPage
    {
        public EmergencyNotificationReceiver(string groupName)
        {
            InitializeComponent();

            OnIamSafe.Clicked += async (sender, args) =>
            {
                Usuario user = UserManager.GetLogUser();
                await SireneServices.GetInstance().ReplyNotification(user, groupName, Constants.SafeResponse);
                await Navigation.PopToRootAsync();
            };

            OnNeedHelp.Clicked += async (sender, args) =>
            {
                Usuario user = UserManager.GetLogUser();
                await SireneServices.GetInstance().ReplyNotification(user, groupName, Constants.NeedHelpResponse);
                await Navigation.PopToRootAsync();
            };
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}