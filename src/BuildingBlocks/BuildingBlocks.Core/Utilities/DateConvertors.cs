using System.Globalization;

namespace BuildingBlocks.Core.Utilities;

public static class DateConvertors
{
    #region ToShamsi

    public static string ToShamsi(this DateTime value)
    {
        var pd = new PersianDateShamsi();
        return
            $"{pd.GetShamsiDayName(value)} {pd.GetShamsiDay(value)} {pd.GetShamsiMonthName(value)} {pd.GetShamsiYear(value)}";
    }

    #endregion ToShamsi

    #region ToDetailedShamsi

    public static string ToLongShamsi(this DateTime value)
    {
        var pc = new PersianCalendar();
        var pd = new PersianDateShamsi();

        return $"{pc.GetHour(value)}:{pc.GetMinute(value)} {pd.GetShamsiDayName(value)} {pd.GetShamsiDay(value)} {pd.GetShamsiMonthName(value)} {pd.GetShamsiYear(value)}";
    }

    #endregion ToDetailedShamsi

    #region ToMiladi

    public static DateTime ToMiladi(this string persianDate)
    {
        ReadOnlySpan<char> dateAsText = new PersianDateShamsi().ToEnglishNumber(persianDate);
        int year = int.Parse(dateAsText.Slice(0, 4));
        int month = int.Parse(dateAsText.Slice(5, 2));
        int day = int.Parse(dateAsText.Slice(8, 2));
        int hour = int.Parse(dateAsText.Slice(11, 2));
        int minute = int.Parse(dateAsText.Slice(14, 2));
        int seconds = int.Parse(dateAsText.Slice(17, 2));

        var miladyDateTime = new DateTime(year, month, day, hour, minute, seconds, new PersianCalendar());

        return miladyDateTime;
    }

    #endregion ToMiladi

    #region ToFileName

    public static string ToFileName(this DateTime value)
    {
        return $"{value.Year:0000}_{value.Month:00}_{value.Day:00}_{value.Hour:00}-{value.Minute:00}_{value.Second:00}_"
               + Guid.NewGuid().ToString("N").Substring(0, 4);
    }

    #endregion ToFileName
}

public class PersianDateShamsi
{
    #region Constants

    private PersianCalendar persianCalendar = new PersianCalendar();
    private string[] DaysOfWeek = new[] { "شنبه", "يكشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };
    private string[] DaysOfWeekShort = new[] { "ش", "ي", "د", "س", "چ", "پ", "ج" };

    private string[] Months = new[] {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };

    private string[] Pn = new[] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
    private string[] En = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    #endregion Constants

    #region Year

    /// <summary>
    /// Get Shamsi Year From Miladi Year
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public int GetShamsiYear(DateTime dateTime)
    {
        return persianCalendar.GetYear(dateTime);
    }

    /// <summary>
    /// Get Short Shamsi Year From Miladi Year In String
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShortShamsiYear(DateTime dateTime)
    {
        var pc = new PersianCalendar();
        return pc.GetYear(dateTime).ToString().Substring(2, 2);
    }

    /// <summary>
    /// Get Shamsi Year From Miladi Year In String
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiYearToString(DateTime dateTime)
    {
        return persianCalendar.GetYear(dateTime).ToString();
    }

    #endregion Year

    #region Month

    /// <summary>
    /// Get Shamsi Month From Miladi Month
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public int GetShamsiMonth(DateTime dateTime)
    {
        return persianCalendar.GetMonth(dateTime);
    }

    /// <summary>
    /// Get Shamsi Month Number From Miladi Month In String
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiMonthString(DateTime dateTime)
    {
        return persianCalendar.GetMonth(dateTime).ToString("00");
    }

    /// <summary>
    /// Get Shamsi Month From Miladi Month Number
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public int GetShamsiMonthBunber(DateTime dateTime)
    {
        return persianCalendar.GetMonth(dateTime);
    }

    /// <summary>
    /// Get Shamsi Month Name From Miladi Month
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiMonthName(DateTime dateTime)
    {
        return Months[persianCalendar.GetMonth(dateTime) - 1];
    }

    #endregion Month

    #region Day

    /// <summary>
    /// Get Shamsi Day From Miladi Month
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public int GetShamsiDay(DateTime dateTime)
    {
        return persianCalendar.GetDayOfMonth(dateTime);
    }

    /// <summary>
    /// Get Shamsi Day From Miladi Month In String
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiDayString(DateTime dateTime)
    {
        return persianCalendar.GetDayOfMonth(dateTime).ToString("00");
    }

    /// <summary>
    /// Get Shamsi Day Name From Miladi Month
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiDayName(DateTime dateTime)
    {
        int dw = (int)persianCalendar.GetDayOfWeek(dateTime);

        if (dw == 6)
            return DaysOfWeek[0]; //saturday

        return DaysOfWeek[dw + 1];
    }

    /// <summary>
    /// Get Shamsi Day ShortName From Miladi Month
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiDayShortName(DateTime dateTime)
    {
        int dw = (int)persianCalendar.GetDayOfWeek(dateTime);

        if (dw == 6)
            return DaysOfWeekShort[0]; //saturday

        return DaysOfWeekShort[dw + 1];
    }

    #endregion Day

    #region Numbers

    public string ToEnglishNumber(string stringNum)
    {
        string cash = stringNum;
        for (int i = 0; i < 10; i++)
            cash = cash.Replace(Pn[i], En[i]);

        return cash;
    }

    public string ToFarsiNumber(int intNum)
    {
        string cash = intNum.ToString();
        for (int i = 0; i < 10; i++)
            cash = cash.Replace(En[i], Pn[i]);

        return cash;
    }

    #endregion Numbers
}