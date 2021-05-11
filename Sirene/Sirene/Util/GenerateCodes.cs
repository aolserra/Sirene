using Sirene.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sirene.Util
{
    class GenerateCodes
    {
        public static string GeraCodigoComunidade(string uf, string comunidade)
        {
            StringBuilder condoCode = new StringBuilder();
            condoCode.Append(uf);
            condoCode.Append("_");
            condoCode.Append(GenerateAcronym(comunidade));
            condoCode.Append("_");
            condoCode.Append(GenerateRandomString(4));

            return condoCode.ToString();
        }

        public static string GeraCodigoUsuario(string codigoComunidade, Usuario usuario)
        {
            StringBuilder userCode = new StringBuilder();
            userCode.Append(codigoComunidade);
            userCode.Append("-");
            userCode.Append(GenerateRandomString(2));

            return userCode.ToString();
        }

        private static string GenerateAcronym(string phrase)
        {
            //var words = phrase.Split(new char[] { ' ' });
            var words = phrase.Trim().Split(new char[] { ' ' });

            StringBuilder acronym = new StringBuilder();
            foreach (string word in words)
            {
                acronym.Append(word[0].ToString());
            }
            return acronym.ToString();
        }

        private static string GenerateRandomString(int numChar)
        {
            string randomString = string.Empty;
            for (int i = 0; i < numChar; i++)
            {
                Random random = new Random();
                int code = Convert.ToInt32(random.Next(48, 122).ToString());

                if ((code >= 48 && code <= 57) || (code >= 97 && code <= 122))
                {
                    string _char = ((char)code).ToString();
                    if (!randomString.Contains(_char))
                    {
                        randomString += _char.ToUpper();
                    }
                    else
                    {
                        i--;
                    }
                }
                else
                {
                    i--;
                }
            }
            return randomString;
        }
    }
}