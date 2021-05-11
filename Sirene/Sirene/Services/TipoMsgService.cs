using Firebase.Database;
using Firebase.Database.Query;
using Sirene.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirene.Services
{
    public class TipoMsgService : FirebaseService
    {
        private static TipoMsgService _instance;

        public static TipoMsgService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TipoMsgService();
            }
            return _instance;
        }

        public TipoMsgService()
        {
        }

        public async Task<bool> IsMsgTypeExists(string condoId, string key, string keyValue)
        {
            try
            {
                if (Constants.IdQuery.Equals(key))
                {
                    var msgType = (await client.Child(Constants.TabTipoMsg)
                    .OnceAsync<TipoMessagem>())
                    .Where(t => t.Object.MsgIdComunidade == condoId && t.Object.Id == keyValue)
                    .FirstOrDefault();
                    return (msgType != null);
                }
                else if (Constants.NameQuery.Equals(key))
                {
                    var msgType = (await client.Child(Constants.TabTipoMsg)
                    .OnceAsync<TipoMessagem>())
                    .Where(t => t.Object.MsgIdComunidade == condoId && t.Object.MsgTipo == keyValue)
                    .FirstOrDefault();
                    return (msgType != null);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddMsgType(TipoMessagem newMsgType)
        {
            try
            {
                if (await IsMsgTypeExists(newMsgType.MsgIdComunidade, Constants.NameQuery, newMsgType.MsgTipo) == false)
                {
                    await client.Child(Constants.TabTipoMsg).PostAsync(newMsgType);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TipoMessagem> GetMsgType(string keyValue)
        {
            try
            {
                FirebaseObject<TipoMessagem> msgType;
                msgType = (await client.Child(Constants.TabTipoMsg).OnceAsync<TipoMessagem>()).Where(t => t.Object.MsgIdComunidade == keyValue).FirstOrDefault();
                if (msgType != null)
                {
                    return await client.Child(Constants.TabTipoMsg).Child(msgType.Key).OnceSingleAsync<TipoMessagem>();
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

        public async Task<List<TipoMessagem>> GetMsgTypeList(string key, string keyValue)
        {
            try
            {
                IEnumerable<FirebaseObject<TipoMessagem>> msgTypeList = null;
                if (Constants.IdComunidadeQuery.Equals(key))
                {
                    msgTypeList = (await client.Child(Constants.TabTipoMsg).OnceAsync<TipoMessagem>()).Where(t => t.Object.MsgIdComunidade == keyValue);
                }
                else
                {
                    msgTypeList = (await client.Child(Constants.TabTipoMsg).OnceAsync<TipoMessagem>());
                }
                return msgTypeList.Select(item => new TipoMessagem
                {
                    Id = item.Object.Id,
                    MsgIdComunidade = item.Object.MsgIdComunidade,
                    MsgComunidade = item.Object.MsgComunidade,
                    MsgTipo = item.Object.MsgTipo,
                    MsgTexto = item.Object.MsgTexto,
                    MsgDataCriacao = item.Object.MsgDataCriacao,
                    MsgDataAlteracao = item.Object.MsgDataAlteracao,
                    MsgLogAlteracao = item.Object.MsgLogAlteracao
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateMsgType(TipoMessagem msgType)
        {
            try
            {
                var toUpdateMsgType = (await client.Child(Constants.TabTipoMsg).OnceAsync<TipoMessagem>()).Where(t => t.Object.Id == msgType.Id).FirstOrDefault();
                await client.Child(Constants.TabTipoMsg).Child(toUpdateMsgType.Key).PutAsync(msgType);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteMsgType(TipoMessagem msgType)
        {
            try
            {
                var toDeleteMsgType = (await client.Child(Constants.TabTipoMsg).OnceAsync<TipoMessagem>()).Where(t => t.Object.Id == msgType.Id).FirstOrDefault();
                await client.Child(Constants.TabTipoMsg).Child(toDeleteMsgType.Key).DeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}