using Firebase.Database;

namespace Sirene.Services
{
    public class FirebaseService
    {
        public FirebaseClient client;

        public FirebaseService()
        {
            client = new FirebaseClient(Constants.DbUrl);
        }
    }
}