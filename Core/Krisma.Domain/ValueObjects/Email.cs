using Krisma.Domain.Common;
using System;
using System.Net.Mail;
using System.Text;

namespace Krisma.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get;} = default!;
    private Email(string value)
    {
        Value = value;
    }

    

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email>(new Error("email.empty", "Email cannot be null or empty.", ErrorType.Validation));
        }

        if(value.Length > 254)
        {
            return Result.Failure<Email>(new Error("email.tooLong", "Email cannot be longer than 254 characters.", ErrorType.Validation));
        }
        // Basic email format validation
        if (!MailAddress.TryCreate(value, out _))
        {
            return Result.Failure<Email>(new Error("email.invalid", "Invalid email format.", ErrorType.Validation));
        }
        return Result.Success(new Email(value.Trim().ToLowerInvariant()));
    }


}
