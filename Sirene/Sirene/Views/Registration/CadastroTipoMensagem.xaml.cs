using Newtonsoft.Json;
using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroTipoMensagem : ContentPage
    {
        readonly bool isNewDoc;

        public CadastroTipoMensagem(bool isNew = false)
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
                Usuario curUser = UserManager.GetLogUser();
                DateTime now = DateTime.Now;
                var docItem = (TipoMessagem)BindingContext;

                docItem.MsgDataAlteracao = now;
                docItem.MsgIdComunidade = curUser.IdComunidadeUsu;
                docItem.MsgComunidade = curUser.ComunidadeUsuario;

                List<string> changeLog = null;
                if (isNewDoc)
                {
                    docItem.MsgDataCriacao = now;

                    StringBuilder sbLog = new StringBuilder();
                    sbLog.Append("- O usuário ");
                    sbLog.Append(curUser.NomeUsuario);
                    sbLog.Append(" criou o documento em ");
                    sbLog.Append(now.ToString());

                    changeLog = new List<string>();
                    changeLog.Add(sbLog.ToString());

                    docItem.MsgLogAlteracao = JsonConvert.SerializeObject(changeLog);

                    if (await TipoMsgService.GetInstance().AddMsgType(docItem))
                    {
                        await DisplayAlert("Cadastro de Tipo de Mensagem", "Tipo de mensagem cadastrado com sucesso!", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Atenção!", "Esse tipo de mensagem já está cadastrado!", "OK");
                    }
                }
                else
                {
                    StringBuilder sbLog = new StringBuilder();
                    sbLog.Append("- O usuário ");
                    sbLog.Append(curUser.NomeUsuario);
                    sbLog.Append(" alterou o documento em ");
                    sbLog.Append(now.ToString());

                    changeLog = JsonConvert.DeserializeObject<List<string>>(docItem.MsgLogAlteracao);
                    changeLog.Add(sbLog.ToString());

                    docItem.MsgLogAlteracao = JsonConvert.SerializeObject(changeLog);

                    if (await TipoMsgService.GetInstance().UpdateMsgType(docItem))
                    {
                        await DisplayAlert("Cadastro de Tipo de Mensagem", "Tipo de Mensagem salvo com sucesso!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Atenção!", "Falha ao tentar salvar o tipo de mensagem!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder sbLog = new StringBuilder();
                sbLog.Append("Erro ao salvar o tipo de mensagem no banco de dados!");
                sbLog.Append(Environment.NewLine);
                sbLog.Append("Erro: ");
                sbLog.Append(ex.ToString());
            }
        }

        async void OnDeleteActivated(object sender, EventArgs e)
        {
            var docItem = (TipoMessagem)BindingContext;
            await TipoMsgService.GetInstance().DeleteMsgType(docItem);
            await Navigation.PopAsync();
        }

        async void Search(object sender, EventArgs e)
        {
            await DisplayAlert("Atenção!", "Pesquisa!", "OK");
        }
    }
}