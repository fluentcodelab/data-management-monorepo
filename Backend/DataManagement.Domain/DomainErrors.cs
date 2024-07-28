namespace DataManagement.Domain;

public static class DomainErrors
{
    public static Error InvalidValue(string message = null)
    {
        var errorMessage = string.IsNullOrEmpty(message) ? "Value provided is not valid." : message;
        return new Error("value.not.valid", errorMessage);
    }

    public static Error NotFound(int id)
    {
        return new Error("resource.not.found", $"The resource with ID '{id}' does not exist.");
    }

    public static Error AlreadyExists()
    {
        return new Error("resource.already.exists", "A resource with same key attributes already exists.");
    }
}