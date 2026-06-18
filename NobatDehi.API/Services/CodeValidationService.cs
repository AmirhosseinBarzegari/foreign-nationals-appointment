namespace NobatDehi.API.Services;

public class CodeValidationService
{
    public bool CheckCode(string code, string codeType)
    {
        return codeType switch
        {
            "Yekta" => code.Length == 10 && code.StartsWith("9") && code.All(char.IsDigit),
            "Passport" => code.Length >= 6 && code.Length <= 10,
            "Faragir" => code.Length == 14 && code.All(char.IsDigit),
            "Khanevade" => code.Length == 10 && code.All(char.IsDigit),
            "Ekhtesasi" => code.Length == 12 && code.All(char.IsDigit),
            "Shenasaei" => code.Length == 12 && code.All(char.IsDigit),
            _ => false
        };
    }
}