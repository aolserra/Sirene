using Sirene.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = new NavigationPage(new Views.MainMenu());
            MainPage = page;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //Handle when your app sleeps
            //Task.Run(async()=> { await SaferProjectApi.GetInstance().GetOut(UserManager.GetLogUser()); });
        }

        protected override void OnResume()
        {
            //Handle when your app resumes
            Task.Run(async () => { await SireneServices.GetInstance().GetIn(UserManager.GetLogUser()); });
        }
    }
}
