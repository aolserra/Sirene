using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sirene.Models;
using Sirene.Util;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Correios;
using Newtonsoft.Json;

namespace Sirene.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroComunidade : ContentPage
    {
        readonly bool isNewDoc;

        public CadastroComunidade(bool isNew = false)
        {
            InitializeComponent();
            isNewDoc = isNew;
        }

        async void BackForward(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void OnSaveActivated(object sender, EventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;
                var docItem = (Comunidade)BindingContext;
                
                docItem.DataAlteracaoComunidade = now;

                List<string> changeLog = null;
                if (isNewDoc)
                {
                    docItem.CodigoComunidade = GenerateCodes.GeraCodigoComunidade(docItem.UFComunidade, docItem.NomeComunidade);
                    docItem.DataCriacaoComunidade = now;

                    StringBuilder sbLog = new StringBuilder();
                    sbLog.Append("- O usuário ");
                    sbLog.Append(UserManager.GetLogUser().NomeUsuario);
                    sbLog.Append(" criou o documento em ");
                    sbLog.Append(now.ToString());

                    changeLog = new List<string>();
                    changeLog.Add(sbLog.ToString());

                    docItem.LogAlteracaoComunidade = JsonConvert.SerializeObject(changeLog);

                    if (await ComunidadeService.GetInstance().AddComunidade(docItem))
                    {
                        await DisplayAlert("Cadastro de Comunidade", "Comunidade cadastrada com sucesso!", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Atenção!", "Esta comunidade já está cadastrada!", "OK");
                    }
                }
                else
                {
                    StringBuilder sbLog = new StringBuilder();
                    sbLog.Append("- O usuário ");
                    sbLog.Append(UserManager.GetLogUser().NomeUsuario);
                    sbLog.Append(" alterou o documento em ");
                    sbLog.Append(now.ToString());

                    changeLog = JsonConvert.DeserializeObject<List<string>>(docItem.LogAlteracaoComunidade);
                    changeLog.Add(sbLog.ToString());

                    docItem.LogAlteracaoComunidade = JsonConvert.SerializeObject(changeLog);

                    if (await ComunidadeService.GetInstance().UpdateComunidade(docItem))
                    {
                        await DisplayAlert("Cadastro de Comunidade", "Comunidade salva com sucesso!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Atenção!", "Falha ao tentar salvar a comunidade !", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append("Erro ao salvar a comunidade no banco de dados!");
                sbLog.Append(Environment.NewLine);
                sbLog.Append("Erro: ");
                sbLog.Append(ex.ToString());
            }
        }

        async void OnDeleteActivated(object sender, EventArgs e)
        {
            var docItem = (Comunidade)BindingContext;
            await ComunidadeService.GetInstance().DeleteComunidade(docItem);
            await Navigation.PopAsync();
        }

        async void Search(object sender, EventArgs e)
        {
            await DisplayAlert("Atenção!", "Pesquisa!", "OK");
        }
    }
}