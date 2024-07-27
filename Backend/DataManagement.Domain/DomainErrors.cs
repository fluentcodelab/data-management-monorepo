namespace DataManagement.Domain;

public static class DomainErrors
{
    public static Error InvalidValue(string message = null)
    {
        var errorMessage = string.IsNullOrEmpty(message) ? "Value provided is not valid." : message;
        return new Error("value.not.valid", errorMessage);
    }
}