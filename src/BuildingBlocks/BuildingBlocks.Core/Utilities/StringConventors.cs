using System.Globalization;
using System.Text.RegularExpressions;

namespace BuildingBlocks.Core.Utilities;

public static class StringConventors
{
    public static string ToSlug(this string value)
    {
        //First to lower case 
        value = value.ToLowerInvariant();

        //Replace spaces 
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

        //Trim dashes from end 
        value = value.Trim('-', '_');

        //Replace double occurences of - or \_ 
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

        return value;
    }

    public static string ToMoney(this decimal text)
    {
        string result = text.ToString("N0", CultureInfo.CreateSpecificCulture("fa-ir"));
        return result;
    }
}