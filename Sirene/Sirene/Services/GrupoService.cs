using Firebase.Database;
using Firebase.Database.Query;
using Sirene.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirene.Services
{
    public class GrupoService : FirebaseService
    {
        private static GrupoService _instance;

        public static GrupoService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GrupoService();
            }
            return _instance;
        }

        public GrupoService()
        {
        }

        public async Task<bool> IsGroupExists(string key, string keyValue)
        {
            try
            {
                if (Constants.NameQuery.Equals(key))
                {
                    var grp = (await client.Child(Constants.TabGrupo)
                    .OnceAsync<Grupo>())
                    .Where(g => g.Object.Nome == keyValue)
                    .FirstOrDefault();
                    return (grp != null);
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

        public async Task<bool> AddGroup(Grupo newGroup)
        {
            try
            {
                if (await IsGroupExists(Constants.NameQuery, newGroup.Nome) == false)
                {
                    await client.Child(Constants.TabGrupo).PostAsync(newGroup);
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

        public async Task<Grupo> GetGroup(string keyValue)
        {
            try
            {
                FirebaseObject<Grupo> grp;
                grp = (await client.Child(Constants.TabGrupo).OnceAsync<Grupo>()).Where(g => g.Object.Nome == keyValue).FirstOrDefault();
                if (grp != null)
                {
                    return await client.Child(Constants.TabGrupo).Child(grp.Key).OnceSingleAsync<Grupo>();
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

        public async Task<List<Grupo>> GetGroupList(string key, string keyValue)
        {
            try
            {
                IEnumerable<FirebaseObject<Grupo>> grpList = null;
                if (Constants.NameQuery.Equals(key))
                {
                    grpList = (await client.Child(Constants.TabGrupo).OnceAsync<Grupo>()).Where(g => g.Object.Nome == keyValue);
                }
                else
                {
                    grpList = (await client.Child(Constants.TabGrupo).OnceAsync<Grupo>());
                }

                return grpList.Select(item => new Grupo
                {
                    Id = item.Object.Id,
                    Nome = item.Object.Nome,
                    Usuarios = item.Object.Usuarios
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGroup(Grupo grp)
        {
            try
            {
                var toUpdateGrp = (await client.Child(Constants.TabGrupo).OnceAsync<Grupo>()).Where(g => g.Object.Id == grp.Id).FirstOrDefault();
                await client.Child(Constants.TabGrupo).Child(toUpdateGrp.Key).PutAsync(grp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteGroup(Grupo grp)
        {
            try
            {
                var toDeleteGrp = (await client.Child(Constants.TabGrupo).OnceAsync<Grupo>()).Where(g => g.Object.Id == grp.Id).FirstOrDefault();
                await client.Child(Constants.TabGrupo).Child(toDeleteGrp.Key).DeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}