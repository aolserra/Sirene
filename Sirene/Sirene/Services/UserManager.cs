using Newtonsoft.Json;
using Sirene.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sirene.Services
{
    class UserManager
    {
        public static void SetLogUser(Usuario currentUser)
        {
            App.Current.Properties["Logged"] = JsonConvert.SerializeObject(currentUser);
            App.Current.SavePropertiesAsync();
        }

        public static Usuario GetLogUser()
        {
            if (App.Current.Properties.ContainsKey("Logged"))
            {
                var logged = (string)App.Current.Properties["Logged"];
                return JsonConvert.DeserializeObject<Usuario>(logged);
            }

            return null;
        }

        public static void DelLogUser()
        {
            App.Current.Properties.Remove("Logged");
            App.Current.SavePropertiesAsync();
        }

        public static void SetUser(Usuario currentUser)
        {
            App.Current.Properties["User"] = JsonConvert.SerializeObject(currentUser);
            App.Current.SavePropertiesAsync();
        }

        public static Usuario GetUser()
        {
            if (App.Current.Properties.ContainsKey("User"))
            {
                var logged = (string)App.Current.Properties["User"];
                return JsonConvert.DeserializeObject<Usuario>(logged);
            }

            return null;
        }
    }
}