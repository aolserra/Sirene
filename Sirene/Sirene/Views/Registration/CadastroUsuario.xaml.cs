using Firebase.Database;
using Newtonsoft.Json;
using Sirene.Models;
using Sirene.Services;
using Sirene.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroUsuario : ContentPage
    {
        public IList<Comunidade> ListComunidade { get; set; }
        readonly bool isNewDoc;
        private int pickerSelItemComunidade;

        public CadastroUsuario(bool isNew = false)
        {
            InitializeComponent();
            isNewDoc = isNew;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var docItem = (Usuario)BindingContext;

            if (docItem.TemPessoasComProbMobilidade)
            {
                temPessoasComProbMobilidadeHabilitado.IsChecked = true;
                temPessoasComProbMobilidadeDesabilitado.IsChecked = false;
            }
            else
            {
                temPessoasComProbMobilidadeHabilitado.IsChecked = false;
                temPessoasComProbMobilidadeDesabilitado.IsChecked = true;
            }

            if (!UserManager.GetLogUser().PerfilUsuario.Equals("Adm Geral"))
            {
                ListComunidade = await ComunidadeService.GetInstance().GetComunidadeList(Constants.IdComunidadeQuery, UserManager.GetLogUser().IdComunidadeUsu);
            }
            else
            {
                ListComunidade = await ComunidadeService.GetInstance().GetComunidadeList(null, null);
            }

            foreach (Comunidade condo in ListComunidade)
            {
                comunidadeUsuario.Items.Add(condo.NomeComunidade);
            }

            comunidadeUsuario.SelectedIndex = comunidadeUsuario.Items.IndexOf(docItem.ComunidadeUsuario);
        }

        private async void OnBackForwardActivated(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void OnSaveActivated(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            var docItem = (Usuario)BindingContext;

            docItem.IdComunidadeUsu = ListComunidade[pickerSelItemComunidade].Id;
            docItem.CodigoComunidadeUsu = ListComunidade[pickerSelItemComunidade].CodigoComunidade;
            docItem.ComunidadeUsuario = comunidadeUsuario.SelectedItem.ToString();
            docItem.DataAlteracaoUsuario = now;
            docItem.PerfilUsuario = perfilUsuario.Items[perfilUsuario.SelectedIndex];
            docItem.TemPessoasComProbMobilidade = temPessoasComProbMobilidadeHabilitado.IsChecked ? true : false;

            //List<string> profile = null;
            List<string> changeLog;
            if (isNewDoc)
            {
                docItem.CodigoUsuario = GenerateCodes.GeraCodigoUsuario(docItem.CodigoComunidadeUsu, docItem);
                docItem.DataCriacaoUsuario = now;
                docItem.RespostaNotifUsu = null;

                StringBuilder sbLog = new StringBuilder();
                sbLog.Append("- O usuário ");
                sbLog.Append(UserManager.GetLogUser().NomeUsuario);
                sbLog.Append(" criou o documento em ");
                sbLog.Append(now.ToString());

                changeLog = new List<string>();
                changeLog.Add(sbLog.ToString());

                docItem.LogAlteracaoUsuario = JsonConvert.SerializeObject(changeLog);

                if (await UsuarioService.GetInstance().AddUser(docItem))
                {
                    await DisplayAlert("Cadastro de Usuário", "Usuário cadastrado com sucesso!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Atenção!", "Este usuário já está cadastrado!", "OK");
                }
            }
            else
            {
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append("- O usuário ");
                sbLog.Append(UserManager.GetLogUser().NomeUsuario);
                sbLog.Append(" alterou o documento em ");
                sbLog.Append(now.ToString());

                changeLog = JsonConvert.DeserializeObject<List<string>>(docItem.LogAlteracaoUsuario);
                changeLog.Add(sbLog.ToString());

                docItem.LogAlteracaoUsuario = JsonConvert.SerializeObject(changeLog);

                if (await UsuarioService.GetInstance().UpdateUser(docItem))
                {
                    await DisplayAlert("Cadastro de Usuário", "Usuário salvo com sucesso!", "OK");
                }
                else
                {
                    await DisplayAlert("Atenção!", "Falha ao tentar salvar o usuário!", "OK");
                }
            }
        }

        async void OnDeleteActivated(object sender, EventArgs e)
        {
            var docItem = (Usuario)BindingContext;
            await UsuarioService.GetInstance().DeleteUser(docItem);
            await Navigation.PopAsync();
        }

        private async void Search(object sender, EventArgs e)
        {
            await DisplayAlert("Atenção!", "Pesquisa!", "OK");
        }

        private void PickerComunidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            pickerSelItemComunidade = ((Picker)sender).SelectedIndex;
        }
    }
}