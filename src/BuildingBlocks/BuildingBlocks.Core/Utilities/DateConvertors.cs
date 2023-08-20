using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    #endregion

    #region ToDetailedShamsi

    public static string ToLongShamsi(this DateTime value)
    {
        var pc = new PersianCalendar();
        var pd = new PersianDateShamsi();
        return
            $"{pc.GetHour(value)} {pd.GetShamsiDayName(value)} {pd.GetShamsiDay(value)} {pd.GetShamsiMonthName(value)} {pd.GetShamsiYear(value)}";
    }

    #endregion

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

    #endregion

    #region ToFileName

    public static string ToFileName(this DateTime value)
    {
        return $"{value.Year:0000}_{value.Month:00}_{value.Day:00}_{value.Hour:00}-{value.Minute:00}_{value.Second:00}_"
               + Guid.NewGuid().ToString("N").Substring(0, 4);
    }

    #endregion
}

public class PersianDateShamsi
{
    #region Constants

    private readonly PersianCalendar persianCalendar;
    private readonly string[] DaysOfWeek;
    private readonly string[] DaysOfWeekShort;
    private readonly string[] Months;
    private readonly string[] Pn;
    private readonly string[] En;

    public PersianDateShamsi()
    {
        persianCalendar = new PersianCalendar();
        DaysOfWeek = new[] { "شنبه", "يكشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };
        DaysOfWeekShort = new[] { "ش", "ي", "د", "س", "چ", "پ", "ج" };
        Months = new[] {
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };
        Pn = new[] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        En = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    }

    #endregion

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

    #endregion

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

    #endregion

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
        return DaysOfWeek[(int)persianCalendar.GetDayOfWeek(dateTime) + 1];
    }

    /// <summary>
    /// Get Shamsi Day ShortName From Miladi Month
    /// </summary>
    /// <param name="dateTime">Enter The Jalali DateTime</param>
    /// <returns></returns>
    public string GetShamsiDayShortName(DateTime dateTime)
    {
        return DaysOfWeekShort[(int)persianCalendar.GetDayOfWeek(dateTime) + 1];
    }

    #endregion

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

    #endregion
}