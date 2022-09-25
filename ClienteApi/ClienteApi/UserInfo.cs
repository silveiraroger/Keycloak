namespace ClienteApi
{
    public class UserInfo
    {
        public UserInfoAttributes attributes { get; set; }
    }

    public class UserInfoAttributes
    {
        public string[] permissoes { get; set; }
    }
}
