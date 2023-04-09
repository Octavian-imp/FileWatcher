namespace FileWatcher
{
    internal static class User
    {
        private static int idUser;
        private static string username;
        private static bool isAdmin;
        public static int IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }
        public static string Username
        {
            get { return username; }
            set { username = value; }
        }
        public static bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }
        public const string rootPath = @".\watchingFolder";
    }
}
