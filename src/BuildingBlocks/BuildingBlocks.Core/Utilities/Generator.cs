using System.Text;

namespace BuildingBlocks.Core.Utilities;

public static class Generator
{
    private readonly static char[] Letters = "ABCDEFGHJKMNPQRSTUVWXYZ".ToCharArray();

    private readonly static char[] Numbers = "0123456789".ToCharArray();

    private readonly static char[] Chars =
        "$%#@!*?;:abcdefghijklmnopqrstuvxxyzABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();

    public static string Code(int subString = 5)
    {
        return Guid.NewGuid().ToString().ToUpper().Substring(0, subString);
    }

    public static string IssueTrackingCode()
    {
        var sb = new StringBuilder();

        Random random = new();

        for (int i = 0; i < 4; i++)
            sb.Append(Numbers[random.Next(0, Numbers.Length)]);

        sb.Append('-');

        for (int i = 0; i < 4; i++)
            sb.Append(Numbers[random.Next(0, Numbers.Length)]);

        return sb.ToString().ToUpper();
    }
}