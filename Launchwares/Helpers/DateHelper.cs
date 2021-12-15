using System;
using System.Windows;

namespace Launchwares.Helpers
{
    internal static class DateHelper
    {
        internal static string GetDiffForHumans(DateTime Time)
        {
            if (GetSeconds(Time) > 60)
                if (GetMinutes(Time) > 60)
                    if (GetHours(Time) > 24)
                        if (GetDays(Time) > 7)
                            if (GetWeeks(Time) > 4)
                                if (GetMonths(Time) > 12)
                                    return $"{GetYears(Time)} {Application.Current.Resources["date.years"]}";
                                else
                                    return $"{GetMonths(Time)} {Application.Current.Resources["date.months"]}";
                            else
                                return $"{GetWeeks(Time)} {Application.Current.Resources["date.weeks"]}";
                        else
                            return $"{GetDays(Time)} {Application.Current.Resources["date.days"]}";
                    else
                        return $"{GetHours(Time)} {Application.Current.Resources["date.hours"]}";
                else
                    return $"{GetMinutes(Time)} {Application.Current.Resources["date.minutes"]}";
            else
                return $"{GetSeconds(Time)} {Application.Current.Resources["date.seconds"]}";
        }

        internal static int GetSeconds(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalSeconds);
        internal static int GetMinutes(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalMinutes);
        internal static int GetHours(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalHours);
        internal static int GetDays(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalDays);
        internal static int GetWeeks(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalDays / 4);
        internal static int GetMonths(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalDays / 30);
        internal static int GetYears(DateTime Time) => Convert.ToInt32((DateTime.UtcNow - Time).TotalDays / 365);

        internal static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date) {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
    }
}
