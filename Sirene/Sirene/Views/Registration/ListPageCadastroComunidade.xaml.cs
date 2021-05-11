using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPageCadastroComunidade : ContentPage
    {
        public ListPageCadastroComunidade()
        {
            InitializeComponent();
        }

        //Utilizado para carregar a lista de cadastros de condomínios, antes da tela abrir para o usuário.
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            condoListView.ItemsSource = await ComunidadeService.GetInstance().GetComunidadeList(null, null);
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroComunidade(true)
            {
                BindingContext = new Comunidade
                {
                    Id = Guid.NewGuid().ToString()
                }
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new CadastroComunidade
            {
                BindingContext = e.SelectedItem as Comunidade
            });
        }

        private async void OnBackForwardActivated(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}