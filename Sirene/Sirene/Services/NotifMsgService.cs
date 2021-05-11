using Firebase.Database;
using Firebase.Database.Query;
using Sirene.Models;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirene.Services
{
    public class NotifMsgService : FirebaseService
    {
        private static NotifMsgService _instance;

        public static NotifMsgService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NotifMsgService();
            }
            return _instance;
        }

        public NotifMsgService()
        {
        }

        public async Task<bool> IsNotifMsgExists(string keyValue)
        {
            try
            {
                var notifMsg = (await client.Child(Constants.TabNotificacao)
                .OnceAsync<Notificacao>())
                .Where(m => m.Object.GroupName == keyValue)
                .FirstOrDefault();
                return (notifMsg != null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddNotifMsg(Notificacao newNotifMsg)
        {
            try
            {
                await client.Child(Constants.TabNotificacao).PostAsync(newNotifMsg);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Notificacao> GetNotifMsg(string keyValue)
        {
            try
            {
                FirebaseObject<Notificacao> notifMsg;
                notifMsg = (await client.Child(Constants.TabNotificacao).OnceAsync<Notificacao>()).Where(m => m.Object.GroupName == keyValue).FirstOrDefault();
                if (notifMsg != null)
                {
                    return await client.Child(Constants.TabNotificacao).Child(notifMsg.Key).OnceSingleAsync<Notificacao>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Notificacao>> GetNotifMsgList(string key, string keyValue)
        {
            try
            {
                IEnumerable<FirebaseObject<Notificacao>> toolNotifMsg = null;
                if (Constants.GrupoNotifQuery.Equals(key))
                {
                    toolNotifMsg = (await client.Child(Constants.TabNotificacao).OnceAsync<Notificacao>()).Where(m => m.Object.GroupName == keyValue);
                }
                else
                {
                    toolNotifMsg = (await client.Child(Constants.TabNotificacao).OnceAsync<Notificacao>());
                }
                return toolNotifMsg.Select(item => new Notificacao
                {
                    Id = item.Object.Id,
                    GroupName = item.Object.GroupName,
                    UserId = item.Object.UserId,
                    UserJason = item.Object.UserJason,
                    MsgType = item.Object.MsgType,
                    Text = item.Object.Text,
                    Completed = item.Object.Completed,
                    NotificationType = item.Object.NotificationType,
                    CreatedDate = item.Object.CreatedDate
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateNotifMsg(Notificacao notifMsg)
        {
            try
            {
                var toUpdateNotifMsg = (await client.Child(Constants.TabNotificacao).OnceAsync<Notificacao>()).Where(m => m.Object.Id == notifMsg.Id).FirstOrDefault();
                await client.Child(Constants.TabNotificacao).Child(toUpdateNotifMsg.Key).PutAsync(notifMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteNotifMsg(Notificacao notifMsg)
        {
            try
            {
                var toDeleteNotifMsg = (await client.Child(Constants.TabNotificacao).OnceAsync<Notificacao>()).Where(m => m.Object.Id == notifMsg.Id).FirstOrDefault();
                await client.Child(Constants.TabNotificacao).Child(toDeleteNotifMsg.Key).DeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
