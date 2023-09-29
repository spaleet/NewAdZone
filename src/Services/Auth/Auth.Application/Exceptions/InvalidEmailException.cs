using System.Text.RegularExpressions;
using BuildingBlocks.Core.Exceptions.Base;

namespace Auth.Application.Exceptions;

public class InvalidEmailException : BadRequestException
{
    public string Email { get; }

    public InvalidEmailException(string email)
        : base($"Email: '{email}' is invalid.")
    {
        Email = email;
    }

    public static void ThrowIfNotValid(string email)
    {
        Regex regex = new(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
                           + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.Compiled
        );

        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidEmailException(email);

        if (email.Length > 100)
            throw new InvalidEmailException(email);

        var lowerEmail = email.ToLowerInvariant();

        if (!regex.IsMatch(lowerEmail))
            throw new InvalidEmailException(email);
    }
}