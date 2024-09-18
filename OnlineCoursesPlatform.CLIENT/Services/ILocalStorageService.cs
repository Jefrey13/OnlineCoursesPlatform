namespace OnlineCoursesPlatform.CLIENT.Services
{
    public interface ILocalStorageService
    {
        Task SaveTokenAsync(string key, string token);
        Task<string> GetTokenAsync(string key);
        Task RemoveTokenAsync(string key);
    }
}
