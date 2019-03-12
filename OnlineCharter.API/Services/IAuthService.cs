namespace Services.Interfaces
{
    public interface IAuthService
    {
        User Authenticate(string name, string email);
    }
}
