using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;

namespace BuildingBlocks.Core.Validation;

public static class ValidationExtension
{
    public static void ValidateWithResponse<T>(this AbstractValidator<T> validator, T request)
    {
        var validationRes = validator.Validate(request);

        var failures = validationRes.Errors;

        if (failures.Count != 0)
            throw new BadRequestException(failures[0].ErrorMessage);
    }
}