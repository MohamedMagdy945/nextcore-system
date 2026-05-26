namespace Common.Shared.Constant
{
    public class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public static List<string> GetAll()
        {
            return new List<string>
            {
                Admin, User
            };
        }
    }
}
