using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirene.Models;
using Sirene.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageTypeRegistrationList : ContentPage
    {
        public MessageTypeRegistrationList()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!UserManager.GetLogUser().PerfilUsuario.Equals("Adm Geral"))
            {
                msgTypeListView.ItemsSource = await TipoMsgService.GetInstance().GetMsgTypeList(Constants.IdComunidadeQuery, UserManager.GetLogUser().IdComunidadeUsu);
            }
            else
            {
                msgTypeListView.ItemsSource = await TipoMsgService.GetInstance().GetMsgTypeList(null, null);
            }
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Registration.CadastroTipoMensagem(true)
            {
                BindingContext = new TipoMessagem
                {
                    Id = Guid.NewGuid().ToString()
                }
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new CadastroTipoMensagem
            {
                BindingContext = e.SelectedItem as TipoMessagem
            });
        }

        private async void OnBackForwardActivated(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}