using OnlineCoursesPlatform.DTO.RequestDTO;
using OnlineCoursesPlatform.DTO.ResponseDTO;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace OnlineCoursesPlatform.CLIENT.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<HttpResponseMessage> Login(LoginRequestDTO loginRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/v1/Auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    // Usar UserResponseDTO en lugar de TokenResponseDTO
                    var userResponse = await response.Content.ReadFromJsonAsync<UserResponseDTO>();

                    if (userResponse != null)
                    {
                        // Guardar tanto el token como el refresh token
                        await _localStorageService.SaveTokenAsync("authToken", userResponse.Token);
                        await _localStorageService.SaveTokenAsync("refreshToken", userResponse.RefreshToken);
                    }
                }

                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> Register(RegisterRequestDTO registerRequest)
        {
            try
            {
                return await _httpClient.PostAsJsonAsync("/api/v1/Auth/register", registerRequest);
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> RefreshToken(RefreshTokenRequestDTO refreshTokenRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/v1/Auth/refresh-token", refreshTokenRequest);

                if (response.IsSuccessStatusCode)
                {
                    // Usar UserResponseDTO en lugar de TokenResponseDTO
                    var userResponse = await response.Content.ReadFromJsonAsync<UserResponseDTO>();

                    if (userResponse != null)
                    {
                        // Guardar tanto el nuevo token como el refresh token
                        await _localStorageService.SaveTokenAsync("authToken", userResponse.Token);
                        await _localStorageService.SaveTokenAsync("refreshToken", userResponse.RefreshToken);
                    }
                }

                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> RequestOtp(RequestOtpDTO otpRequest)
        {
            try
            {
                return await _httpClient.PostAsJsonAsync("/api/v1/Auth/request-otp", otpRequest);
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        public async Task<HttpResponseMessage> VerifyOtp(VerifyOtpRequestDTO otpVerifyRequest)
        {
            try
            {
                return await _httpClient.PostAsJsonAsync("/api/v1/Auth/verify-otp", otpVerifyRequest);
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request error: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
    }
}