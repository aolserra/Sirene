using Firebase.Database;
using Firebase.Database.Query;
using Sirene.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sirene.Services
{
    public class ComunidadeService : FirebaseService
    {
        private static ComunidadeService _instance;

        public static ComunidadeService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ComunidadeService();
            }
            return _instance;
        }

        public ComunidadeService()
        {
        }

        public async Task<bool> IsComunidadeExists(string key, string keyValue)
        {
            try
            {
                if (Constants.IdComunidadeQuery.Equals(key))
                {
                    var comunidade = (await client.Child(Constants.TabComunidade)
                    .OnceAsync<Comunidade>())
                    .Where(c => c.Object.Id == keyValue)
                    .FirstOrDefault();
                    return (comunidade != null);
                }
                else if (Constants.CodComunidade.Equals(key))
                {
                    var comunidade = (await client.Child(Constants.TabComunidade)
                    .OnceAsync<Comunidade>())
                    .Where(c => c.Object.CodigoComunidade == keyValue)
                    .FirstOrDefault();
                    return (comunidade != null);
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

        public async Task<bool> AddComunidade(Comunidade newComunidade)
        {
            try
            {
                if (await IsComunidadeExists(Constants.NomeComunidade, newComunidade.NomeComunidade) == false)
                {
                    await client.Child(Constants.TabComunidade).PostAsync(newComunidade);
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

        public async Task<Comunidade> GetComunidade(string key, string keyValue)
        {
            try
            {
                FirebaseObject<Comunidade> comunidade;
                if (Constants.IdComunidadeQuery.Equals(key))
                {
                    comunidade = (await client.Child(Constants.TabComunidade).OnceAsync<Comunidade>()).Where(c => c.Object.Id == keyValue).FirstOrDefault();
                }
                else if (Constants.CodComunidade.Equals(key))
                {
                    comunidade = (await client.Child(Constants.TabComunidade).OnceAsync<Comunidade>()).Where(c => c.Object.CodigoComunidade == keyValue).FirstOrDefault();
                }
                else
                {
                    return null;
                }

                if (comunidade != null)
                {
                    return await client.Child(Constants.TabComunidade).Child(comunidade.Key).OnceSingleAsync<Comunidade>();
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

        public async Task<List<Comunidade>> GetComunidadeList(string key, string keyValue)
        {
            try
            {
                IEnumerable<FirebaseObject<Comunidade>> listComunidade = null;
                if (Constants.IdComunidadeQuery.Equals(key))
                {
                    listComunidade = (await client.Child(Constants.TabComunidade).OnceAsync<Comunidade>()).Where(c => c.Object.Id == keyValue);
                }
                else
                {
                    listComunidade = (await client.Child(Constants.TabComunidade).OnceAsync<Comunidade>());
                }

                return listComunidade.Select(item => new Comunidade
                {
                    Id = item.Object.Id,
                    CodigoComunidade = item.Object.CodigoComunidade,
                    NomeComunidade = item.Object.NomeComunidade,
                    ResponsavelComunidade = item.Object.ResponsavelComunidade,
                    BairroComunidade = item.Object.BairroComunidade,
                    CidadeComunidade = item.Object.CidadeComunidade,
                    UFComunidade = item.Object.UFComunidade,
                    DataCriacaoComunidade = item.Object.DataCriacaoComunidade,
                    DataAlteracaoComunidade = item.Object.DataAlteracaoComunidade,
                    LogAlteracaoComunidade = item.Object.LogAlteracaoComunidade
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateComunidade(Comunidade condo)
        {
            try
            {
                var toUpdateComunidade = (await client.Child(Constants.TabComunidade).OnceAsync<Comunidade>()).Where(c => c.Object.Id == condo.Id).FirstOrDefault();
                await client.Child(Constants.TabComunidade).Child(toUpdateComunidade.Key).PutAsync(condo);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteComunidade(Comunidade condo)
        {
            try
            {
                var toDeleteComunidade = (await client.Child(Constants.TabComunidade).OnceAsync<Comunidade>()).Where(c => c.Object.Id == condo.Id).FirstOrDefault();
                await client.Child(Constants.TabComunidade).Child(toDeleteComunidade.Key).DeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}