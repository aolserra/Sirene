using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views.Notification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyNotificationTrigger : ContentPage
    {
        public static List<TipoMessagem> TiposMensagens { get; private set; }
        private string NomeGrupo { get; set; }

        public EmergencyNotificationTrigger()
        {
            InitializeComponent();

            OnEmergencyActivated.Clicked += async (sender, args) =>
            {
                try
                {
                    var msg = spreadedMessage.Text.Trim();
                    string msgType = spreadedMessageType.Items[spreadedMessageType.SelectedIndex];
                    var grp = GetGroupName();
                    if (msg.Length > 0)
                    {
                        await SireneServices.GetInstance().SendMessage(UserManager.GetLogUser(), msgType, msg, grp);
                    }
                }
                catch(Exception e)
                {
                    StringBuilder errorSb = new StringBuilder();
                    errorSb.Append("Error: ");
                    errorSb.Append(e.ToString());
                }
            };
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                string condoId = UserManager.GetLogUser().IdComunidadeUsu;
                await SireneServices.GetInstance().CreateOrOpenGroup(UserManager.GetLogUser());

                TiposMensagens = await TipoMsgService.GetInstance().GetMsgTypeList(Constants.IdComunidadeQuery, condoId);
                foreach (TipoMessagem msg in TiposMensagens)
                {
                    spreadedMessageType.Items.Add(msg.MsgTipo);
                }
            }
            catch(Exception e)
            {
                StringBuilder errorSb = new StringBuilder();
                errorSb.Append("Error: ");
                errorSb.Append(e.ToString());
            }
        }

        public void TipoMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = ((Picker)sender).SelectedIndex;
            spreadedMessage.Text = TiposMensagens[i].MsgTexto;
        }

        public void SetGroupName(string groupName)
        {
            NomeGrupo = groupName;
        }

        public string GetGroupName()
        {
            return NomeGrupo;
        }
    }
}