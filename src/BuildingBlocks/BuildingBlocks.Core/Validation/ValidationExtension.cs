using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation.Results;

namespace BuildingBlocks.Core.Validation;
public static class ValidationExtension
{
    public static void ApiValidation(this ValidationResult result)
    {
        var failures = result.Errors;

        if (failures.Count != 0)
            throw new BadRequestException(failures[0].ErrorMessage);
    }
}
