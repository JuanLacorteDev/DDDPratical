using DDDPratical.Domain.Commom.Exceptions;

namespace DDDPratical.Domain.Commom.Validations;

internal static class Guard
{
    public static void AgainstEmptyGuid(Guid value, string paramName)
    {
        if (value == Guid.Empty)
            throw new DomainException($"{paramName} can not be Guid.Empty");
    }

    public static void AgainstNull<T>(T value, string paramName)
    {
        if(value == null)
            throw new DomainException($"{paramName} can not be null");
    }

    public static void AgainstNullOrWhiteSpace(string value, string paramName)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{paramName} can not be null or white space");
    }

    public static void Against<TException>(bool condition, string message) where TException : Exception
    {
        if(condition) throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }
}
