namespace OnlineCoursesPlatform.CLIENT.Services.Impl
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;

        public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveTokenAsync(string key, string token)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            await _localStorage.SetItemAsync(key, token);
        }

        public async Task<string> GetTokenAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            return await _localStorage.GetItemAsync<string>(key);
        }

        public async Task RemoveTokenAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            await _localStorage.RemoveItemAsync(key);
        }
    }
}