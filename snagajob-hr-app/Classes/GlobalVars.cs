using System.Configuration;

namespace snagajob_hr_app.Classes
{
    public class GlobalVars
    {
        #region MongoConnection

        private static string _MongoConnection;
        public static string MongoConnection()
        {
            if (_MongoConnection == null)
            {
                _MongoConnection = ConfigurationManager.AppSettings["mongoConnection"];
            }
            return _MongoConnection;
        }

        #endregion

        #region MongoHrDb

        private static string _MongoHrDb;
        public static string MongoHrDb()
        {
            if (_MongoHrDb == null)
            {
                _MongoHrDb = ConfigurationManager.AppSettings["mongoHrDb"];
            }
            return _MongoHrDb;
        }

        #endregion
    }
}