namespace Service.ServiceContracts
{
    public interface IRecaptchaService
    {
        Task<bool> ValidateRecaptchaAsync(string recaptchaResponse);
        string GetSiteKey();
    }
}
