using RC2K.Logic.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RC2K.Presentation.Blazor;

public class ReCaptchaV3Verifier : ICaptchaVerifier
{
    private readonly string _captchaSecret;
    private readonly ILogger<ReCaptchaV3Verifier> _logger;

    public ReCaptchaV3Verifier(string captchaSecret, ILogger<ReCaptchaV3Verifier> logger)
    {
        _captchaSecret = captchaSecret;
        _logger = logger;
    }

    public async Task<int> IsRobot(string token)
    {
        const int ROBOT = 1;
        const int USER = 0;
        const int UNKNOWN = -1;

        var result = await Verify(token);
        if (result is null)
        {
            return UNKNOWN;
        }

        if (!result.Success)
        {
            _logger.LogError("reCAPTCHA verification returned not a success code.");
            return UNKNOWN;
        }

        _logger.LogInformation($"reCAPTCHA verification returned Score={result.Score}");

        return result.Score switch
        {
            >= 0.6 => USER,
            <= 0.3 => ROBOT,
            _ => UNKNOWN
        };
    }


    private async Task<GooglereCAPTCHAv3Response?> Verify(string token)
    {
        GooglereCAPTCHAv3Response? reCaptchaResponse;
        using (var httpClient = new HttpClient())
        {
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("secret", _captchaSecret),
                new KeyValuePair<string, string>("response", token)
            });
            try
            {
                var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify", content);
                var jsonString = await response.Content.ReadAsStringAsync();
                reCaptchaResponse = JsonSerializer.Deserialize<GooglereCAPTCHAv3Response>(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while trying to verify reCAPTCHA token.");
                return null;
            }

            return reCaptchaResponse;
        }
    }

    private class GooglereCAPTCHAv3Response
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTs { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
    }
}
