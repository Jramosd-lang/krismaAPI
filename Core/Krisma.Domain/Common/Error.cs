using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Domain.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Failure
}

public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
}
