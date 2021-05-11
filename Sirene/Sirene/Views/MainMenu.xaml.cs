using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Sirene.Models;
using Sirene.Services;
using Sirene.Views.Notification;
using Sirene.Views.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sirene.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public static string SireneIdAccess { get; private set; }
        public Usuario CurrentUser { get; set; }

        public MainMenu()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            SireneIdAccess = CrossDeviceInfo.Current.Id;

            if (SireneIdAccess != null)
            {
                CurrentUser = await UsuarioService.GetInstance().GetUser(Constants.IdAccessQuery, SireneIdAccess);
                if (CurrentUser != null)
                {
                    //Logando na API no Azure.
                    await SireneServices.GetInstance().Login(CurrentUser);

                    OpenNotificationScreen(CurrentUser);

                    //List<string> profile = JsonConvert.DeserializeObject<List<string>>(CurrentUser.UserProfile);
                    menuHeader.IsVisible = true;

                    if (CurrentUser.PerfilUsuario == "Adm Geral")
                    {
                        emergencia.IsVisible = true;
                        comunidade.IsVisible = true;
                        mensagem.IsVisible = true;
                        usuario.IsVisible = true;
                    }
                    else if (CurrentUser.PerfilUsuario == "Adm Comunidade")
                    {
                        emergencia.IsVisible = true;
                        mensagem.IsVisible = true;
                        usuario.IsVisible = true;
                    }
                    else if (CurrentUser.PerfilUsuario == "Operador")
                    {
                        emergencia.IsVisible = true;
                    }
                }
                else
                {
                    emergencia.IsVisible = false;
                    comunidade.IsVisible = false;
                    mensagem.IsVisible = false;
                    usuario.IsVisible = false;

                    regAtivHeader.IsVisible = true;
                    regRotuloAtiv.IsVisible = true;
                    codigoUsu.IsVisible = true;
                    regBtAtivacao.IsVisible = true;
                }
            }
            else
            {
                emergencia.IsVisible = false;
                comunidade.IsVisible = false;
                mensagem.IsVisible = false;
                usuario.IsVisible = false;

                regAtivHeader.IsVisible = true;
                regRotuloAtiv.IsVisible = true;
                codigoUsu.IsVisible = true;
                regBtAtivacao.IsVisible = true;
            }
        }

        public async void AddUserConnectionId(Usuario user)
        {
            await SireneServices.GetInstance().AddUserConnectionId(user);
        }

        private async void OpenNotificationScreen(Usuario currentUser)
        {
            StringBuilder groupName = new StringBuilder();
            groupName.Append(currentUser.IdComunidadeUsu);
            //groupName.Append("-");
            //groupName.Append(currentUser.UserPlaceBlock);

            //bool openedScreen = false;

            //Verificando se há mensagens para serem respondidas
            IList<Notificacao> msgList = await NotifMsgService.GetInstance().GetNotifMsgList(Constants.GrupoNotifQuery, groupName.ToString());
            if (msgList.Count > 0)
            {
                foreach (Notificacao msg in msgList)
                {
                    if (msg.Completed != true)
                    {
                        if (currentUser.Id != msg.UserId)
                        {
                            if (string.IsNullOrEmpty(currentUser.RespostaNotifUsu))
                            {
                                await Navigation.PushAsync(new Views.Notification.EmergencyNotificationReceiver(groupName.ToString())
                                {
                                    BindingContext = msg as Notificacao
                                });
                                //openedScreen = true;
                            }
                        }
                    }
                }
            }
        }

        async void NotificacaoEmergencia(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmergencyNotificationTrigger());
        }

        async void CadastroComunidade(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListPageCadastroComunidade());
        }

        private void CadastroMensagens(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MessageTypeRegistrationList());
        }

        private void CadastroUsuario(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListPageCadastroUsuario());
        }

        private async void OnActivated(object sender, EventArgs e)
        {
            Usuario usuario = await UsuarioService.GetInstance().GetUser(Constants.UserCodeQuery, codigoUsu.Text);

            if (usuario != null)
            {
                StringBuilder messageSB = new StringBuilder();

                messageSB.Append("Você é o usuário(a) ");
                messageSB.Append(usuario.NomeUsuario);
                messageSB.Append("?");

                var userOp = await DisplayAlert("ATENÇÃO!", messageSB.ToString(), "Sim", "Não");

                if (userOp == true)
                {
                    DateTime now = DateTime.Now;
                    var idAccess = CrossDeviceInfo.Current.Id;

                    StringBuilder sbLog = new StringBuilder();
                    sbLog.Append("- O usuário ");
                    sbLog.Append(usuario.NomeUsuario);
                    sbLog.Append(" ativou o cadastro em ");
                    sbLog.Append(now.ToString());

                    List<string> changeLog = JsonConvert.DeserializeObject<List<string>>(usuario.LogAlteracaoUsuario);
                    changeLog.Add(sbLog.ToString());

                    usuario.IdAccess = idAccess;
                    usuario.LogAlteracaoUsuario = JsonConvert.SerializeObject(changeLog);
                    usuario.DataAlteracaoUsuario = now;
                    await UsuarioService.GetInstance().UpdateUser(usuario);

                    messageSB.Clear();
                    messageSB.Append("A ativãção do cadastro foi realizada com sucesso.");
                    messageSB.Append(Environment.NewLine);
                    messageSB.Append("Reinicie o App para efetivar seu cadastro.");
                    await DisplayAlert("Ativar Cadastro", messageSB.ToString(), "OK");
                }
            }
            else
            {
                await DisplayAlert("Atenção!", "Existe um erro no cadastro do usuário." + Environment.NewLine + "Contate o administrador do sistema.", "OK");
            }
        }
    }
}