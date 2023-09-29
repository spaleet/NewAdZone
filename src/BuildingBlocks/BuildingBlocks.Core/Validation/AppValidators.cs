using System.Text.RegularExpressions;
using BuildingBlocks.Core.Utilities.ImageRelated;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Core.Validation;

public static class AppValidators
{
    #region Required Validator

    public static IRuleBuilder<T, TProperty> RequiredValidator<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder,
        string propertyName)
    {
        return ruleBuilder.NotEmpty()
                          .WithMessage(string.Format("لطفا {0} را وارد کنید", propertyName));
    }

    #endregion Required Validator

    #region Range Validator

    public static IRuleBuilder<T, long> RangeValidator<T>(this IRuleBuilder<T, long> ruleBuilder, string propertyName,
        int min, int max)
    {
        return ruleBuilder.GreaterThanOrEqualTo(min)
            .WithMessage(string.Format("لطفا {0} را وارد کنید", propertyName))
            .LessThanOrEqualTo(max)
            .WithMessage(string.Format("لطفا {0} را وارد کنید", propertyName));
    }

    public static IRuleBuilder<T, int> RangeValidator<T>(this IRuleBuilder<T, int> ruleBuilder, string propertyName,
        int min, int max)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(min)
            .WithMessage(string.Format("لطفا {0} را وارد کنید", propertyName))
            .LessThanOrEqualTo(max)
            .WithMessage(string.Format("لطفا {0} را وارد کنید", propertyName));
    }

    #endregion Range Validator

    #region MaxLengthValidator

    public static IRuleBuilder<T, string> MaxLengthValidator<T>(this IRuleBuilder<T, string> ruleBuilder,
        string propertyName, int maxLength)
    {
        return ruleBuilder.MaximumLength(maxLength)
            .WithMessage(string.Format("مقدار {0} نباید بیشتر از {1} کاراکتر باشد", propertyName, maxLength));
    }

    #endregion MaxLengthValidator

    #region MinLengthValidator

    public static IRuleBuilder<T, string> MinLengthValidator<T>(this IRuleBuilder<T, string> ruleBuilder,
        string propertyName, int minLength)
    {
        return ruleBuilder.MinimumLength(minLength)
            .WithMessage(string.Format("مقدار {0} نباید کمتر از {1} کاراکتر باشد", propertyName, minLength));
    }

    #endregion MinLengthValidator

    #region EmailAddressValidator

    public static IRuleBuilder<T, string> CustomEmailAddressValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.EmailAddress()
            .WithMessage("ایمیل وارد شده نامعتبر است");
    }

    #endregion EmailAddressValidator

    #region MobileValidator

    public static IRuleBuilder<T, string> MobileValidator<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var regex = new Regex(@"^(?:0|98|\+98|\+980|0098|098|00980)?(9\d{9})$");

        return ruleBuilder
            .RequiredValidator("شماره موبایل")
            .Matches(regex)
            .WithMessage("شماره موبایل وارد شده نامعتبر است");
    }

    #endregion MobileValidator

    #region MaxFileSizeValidator

    public static IRuleBuilder<T, IFormFile> MaxFileSizeValidator<T>(this IRuleBuilder<T, IFormFile> ruleBuilder,
        int size, bool isRequired = true)
    {
        if (isRequired is false)
            return ruleBuilder
                .Must(x => { return MaxFileSize.IsValid(size, x, isRequired); })
                .WithMessage("حجم فایل بیشتر از مقدار مجاز است. لطفا فایل دیگری آپلود کنید");

        return ruleBuilder.NotNull()
            .WithMessage("لطفا فایل را وارد کنید")
            .Must(x => { return MaxFileSize.IsValid(size, x, isRequired); })
            .WithMessage("حجم فایل بیشتر از مقدار مجاز است. لطفا فایل دیگری آپلود کنید");
    }

    #endregion MaxFileSizeValidator
}