using BuildingBlocks.Core.Exceptions.Base;

namespace BuildingBlocks.Core.Exceptions.Other;
public class InvalidPhoneNumberException : BadRequestException
{
    public string PhoneNumber { get; }

    public InvalidPhoneNumberException(string phoneNumber)
        : base($"PhoneNumber: '{phoneNumber}' is invalid.")
    {
        PhoneNumber = phoneNumber;
    }

    public static void ThrowIfNotValid(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new InvalidPhoneNumberException(phoneNumber);

        if (phoneNumber.Length < 7)
            throw new InvalidPhoneNumberException(phoneNumber);

        if (phoneNumber.Length > 15)
            throw new InvalidPhoneNumberException(phoneNumber);
    }
}
