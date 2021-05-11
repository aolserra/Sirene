using Firebase.Database;
using Firebase.Database.Query;
using Sirene.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirene.Services
{
    public class UsuarioService : FirebaseService
    {
        private static UsuarioService _instance;

        public static UsuarioService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UsuarioService();
            }
            return _instance;
        }

        private UsuarioService()
        {
        }

        public async Task<bool> IsUserExists(string condoId, string key, string keyValue)
        {
            try
            {
                if (Constants.IdQuery.Equals(key))
                {
                    var user = (await client.Child(Constants.TabUsuario)
                    .OnceAsync<Usuario>())
                    .Where(u => u.Object.IdComunidadeUsu == condoId && u.Object.Id == keyValue)
                    .FirstOrDefault();
                    return (user != null);
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

        public async Task<bool> AddUser(Usuario newUser)
        {
            try
            {
                await client.Child(Constants.TabUsuario).PostAsync(newUser);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Usuario> GetUser(string key, string keyValue)
        {
            try
            {
                FirebaseObject<Usuario> user;
                if (Constants.IdAccessQuery.Equals(key))
                {
                    user = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.IdAccess == keyValue).FirstOrDefault();
                }
                else if (Constants.UserCodeQuery.Equals(key))
                {
                    user = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.CodigoUsuario == keyValue).FirstOrDefault();
                }
                else if (Constants.IdQuery.Equals(key))
                {
                    user = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.Id == keyValue).FirstOrDefault();
                }
                else if (Constants.ConnectionIdQuery.Equals(key))
                {
                    user = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.ConnectionID == keyValue).FirstOrDefault();
                }
                else
                {
                    return null;
                }

                if (user != null)
                {
                    return await client.Child(Constants.TabUsuario).Child(user.Key).OnceSingleAsync<Usuario>();
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

        public async Task<List<Usuario>> GetUsersList(string key, string keyValue)
        {
            try
            {
                IEnumerable<FirebaseObject<Usuario>> usersList = null;
                if (Constants.IdComunidadeQuery.Equals(key))
                {
                    usersList = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.IdComunidadeUsu == keyValue);
                }
                else
                {
                    usersList = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>());
                }
                return usersList.Select(item => new Usuario
                {
                    Id = item.Object.Id,
                    IdAccess = item.Object.IdAccess,
                    ConnectionID = item.Object.ConnectionID,
                    IsOnline = item.Object.IsOnline,
                    CodigoUsuario = item.Object.CodigoUsuario,
                    NomeUsuario = item.Object.NomeUsuario,
                    ComunidadeUsuario = item.Object.ComunidadeUsuario,
                    IdComunidadeUsu = item.Object.IdComunidadeUsu,
                    CodigoComunidadeUsu = item.Object.CodigoComunidadeUsu,
                    PerfilUsuario = item.Object.PerfilUsuario,
                    TemPessoasComProbMobilidade = item.Object.TemPessoasComProbMobilidade,
                    QtdPessoasComProbMobilidade = item.Object.QtdPessoasComProbMobilidade,
                    RespostaNotifUsu = item.Object.RespostaNotifUsu,
                    DataCriacaoUsuario = item.Object.DataCriacaoUsuario,
                    DataAlteracaoUsuario = item.Object.DataAlteracaoUsuario,
                    LogAlteracaoUsuario = item.Object.LogAlteracaoUsuario
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUser(Usuario user)
        {
            try
            {
                var toUpdateUser = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.Id == user.Id).FirstOrDefault();
                await client.Child(Constants.TabUsuario).Child(toUpdateUser.Key).PutAsync(user);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteUser(Usuario user)
        {
            try
            {
                var toDeleteUser = (await client.Child(Constants.TabUsuario).OnceAsync<Usuario>()).Where(u => u.Object.Id == user.Id).FirstOrDefault();
                await client.Child(Constants.TabUsuario).Child(toDeleteUser.Key).DeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}