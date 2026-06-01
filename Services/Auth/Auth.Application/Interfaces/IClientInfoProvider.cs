namespace Auth.Application.Interfaces
{
    public interface IClientInfoProvider
    {
        string GetIpAddress();
        string GetUserAgent();
    }
}
