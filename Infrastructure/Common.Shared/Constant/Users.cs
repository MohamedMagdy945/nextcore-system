namespace Common.Shared.Constant
{
    public static class Users
    {
        public const string View = "Users.View";
        public const string Create = "Users.Create";
        public const string Edit = "Users.Edit";
        public const string Delete = "Users.Delete";

        public static List<string> GetAll()
        {
            return new List<string>
            {
                View,Create, Edit,Delete
            };
        }
    }
}
