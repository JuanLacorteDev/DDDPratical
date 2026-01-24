using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Exceptions;

internal class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public static void When(bool hasError, string message)
    {
        if (hasError)
            throw new DomainException(message);
    }
}
