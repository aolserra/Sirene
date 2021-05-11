using System;
using System.Collections.Generic;
using System.Text;

namespace Sirene
{
    public static class Constants
    {
        public static readonly string SignalRHubUri = "https://sireneapi.azurewebsites.net/SireneHub";
        public static readonly string DbUrl = "https://sirenetst-default-rtdb.firebaseio.com/";

        public static readonly string SafeResponse = "Safe";
        public static readonly string SafeIcon = "safe.png";
        public static readonly string NeedHelpResponse = "NeedHelp";
        public static readonly string NeedHelpIcon = "needhelp.png";
        public static readonly string OnlineIcon = "online.png";
        public static readonly string OfflineIcon = "offline.png";
        public static readonly string HasPeopleWithMobProblemIcon = "wheelchair.png";

        public static readonly string IdQuery = "ID";
        public static readonly string NameQuery = "NAME";
        public static readonly string ConnectionIdQuery = "CONNECTIONID";
        public static readonly string IdAccessQuery = "IDACCESS";
        public static readonly string CodComunidade = "CODCOMUNIDADE";
        public static readonly string NomeComunidade = "NOMECOMUNIDADE";
        public static readonly string GrupoNotifQuery = "GRUPO";
        public static readonly string UserCodeQuery = "USERCODE";
        public static readonly string IdComunidadeQuery = "IDCOMUNIDADE";
        
        public static readonly string TabComunidade = "Comunidades";
        public static readonly string TabGrupo = "Grupos";
        public static readonly string TabTipoMsg = "TipoMensagens";
        public static readonly string TabNotificacao = "Notificacoes";
        public static readonly string TabUsuario = "Usuarios";
    }
}