using Microsoft.Extensions.Configuration;
using Model.Domain;
using Service.ServiceContracts;
using System.Text.Json;

namespace Service.Implementations
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly IConfiguration _configuration;
        public RecaptchaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> ValidateRecaptchaAsync(string recaptchaResponse)
        {
            var httpClient = new HttpClient();
            var secretKey = _configuration["Recaptcha:SecretKey"];
            var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}", null);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var recaptchaResult = JsonSerializer.Deserialize<RecaptchaResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return recaptchaResult != null && recaptchaResult.Success;
        }

        public string GetSiteKey()
        {
            return _configuration["Recaptcha:SiteKey"] ?? string.Empty;
        }
    }
}
