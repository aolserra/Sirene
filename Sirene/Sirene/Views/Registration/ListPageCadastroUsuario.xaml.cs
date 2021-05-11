using Sirene.Models;
using Sirene.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPageCadastroUsuario : ContentPage
    {
        public ListPageCadastroUsuario()
        {
            InitializeComponent();
        }

        //Utilizado para carregar a lista de cadastros de usuários, antes da tela abrir para o usuário.
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!UserManager.GetLogUser().PerfilUsuario.Equals("Adm Geral"))
            {
                userListView.ItemsSource = await UsuarioService.GetInstance().GetUsersList(Constants.IdComunidadeQuery, UserManager.GetLogUser().IdComunidadeUsu);
            }
            else
            {
                userListView.ItemsSource = await UsuarioService.GetInstance().GetUsersList(null, null);
            }
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Registration.CadastroUsuario(true)
            {
                BindingContext = new Usuario
                {
                    Id = Guid.NewGuid().ToString()
                }
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new CadastroUsuario
            {
                BindingContext = e.SelectedItem as Usuario
            });
        }

        private async void OnBackForwardActivated(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}