using System;
using System.Globalization;

namespace BCRM_App.Extentions
{
    public static class DateTimeHelpers
    {
        public static long GetTimestamp(this DateTime value)
        {
            var Timestamp = new DateTimeOffset(value).ToUnixTimeSeconds();
            return Timestamp;
        }

        public static readonly CultureInfo ENGLISH_CULTURE_INFO = new CultureInfo("en-US");
        public static readonly CultureInfo THAI_CULTURE_INFO = new CultureInfo("th-TH");

        public static string ToSafeString(this object obj)
        {
            return obj + "";
        }

        public static string ToDateString(this DateTime obj)
        {
            return obj.ToString("dd/MM/yyyy");
        }

        public static string ToDateString(this object date, string format)
        {
            string result = string.Empty;
            DateTime? dateValue = date as DateTime?;
            if (dateValue.HasValue)
            {
                result = dateValue.Value.ToString(format, ENGLISH_CULTURE_INFO);
            }
            return result;
        }

        public static DateTime ToDateTimeTH(this string obj)
        {
            DateTime returnValue;
            DateTime.TryParseExact(obj, "dd/MM/yyyy", THAI_CULTURE_INFO, DateTimeStyles.None, out returnValue);
            return returnValue;
        }
        public static DateTime ToDateTimeTH(this object obj)
        {
            return obj.ToSafeString().ToDateTimeTH();
        }

        public static DateTime ToDateTimeEN(this string obj)
        {
            DateTime returnValue;
            DateTime.TryParseExact(obj, "dd/MM/yyyy", ENGLISH_CULTURE_INFO, DateTimeStyles.None, out returnValue);
            return returnValue;
        }

        public static DateTime ToDateTimeEN(this object obj)
        {
            return obj.ToSafeString().ToDateTimeEN();
        }


        public static string ToDisplayDateTime_Th(this DateTime date)
        {
            var cultureInfo = new CultureInfo("th-TH");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            return $"{date.Day.GetDay_Th()} {date.Month.GetMonth_Th()} {date.ToString("yy")} {date.ToString("HH:mm")}";
        }

        public static string GetMonth_Th(this int month)
        {
            switch (month)
            {
                case 1: return "ม.ค.";
                case 2: return "ก.พ.";
                case 3: return "มี.ค.";
                case 4: return "เม.ย.";
                case 5: return "พ.ค.";
                case 6: return "มิ.ย.";
                case 7: return "ก.ค.";
                case 8: return "ส.ค.";
                case 9: return "ก.ย.";
                case 10: return "ต.ค.";
                case 11: return "พ.ย.";
                case 12: return "ธ.ค.";
                default:
                    return "";
            }
        }

        public static string GetDay_Th(this int day)
        {
            if (day < 10) return $"0{day}";
            return day.ToString();
        }
    }
}
