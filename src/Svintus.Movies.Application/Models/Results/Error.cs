namespace Svintus.Movies.Application.Models.Results;

public class Error(string code, string? message = null)
{
    private const string DefaultErrorMessage = "Something went wrong";

    public string Code { get; set; } = code;
    public string Message { get; set; } = message ?? DefaultErrorMessage;
}