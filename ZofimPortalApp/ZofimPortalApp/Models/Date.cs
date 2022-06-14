using System;
using System.Collections.Generic;
using System.Text;

namespace ZofimPortalApp.Models
{
    public class Date
    {
        public Date()
        {
            Day = "0";
            Month = "0";
            Year = "0";
        }

        public Date(string day, string month, string year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public Date(DateTime dateTime)
        {
            int day = dateTime.Day;
            switch (day)
            {
                case 1:
                    Day = "01";
                    break;
                case 2:
                    Day = "02";
                    break;
                case 3:
                    Day = "03";
                    break;
                case 4:
                    Day = "04";
                    break;
                case 5:
                    Day = "05";
                    break;
                case 6:
                    Day = "06";
                    break;
                case 7:
                    Day = "07";
                    break;
                case 8:
                    Day = "08";
                    break;
                case 9:
                    Day = "09";
                    break;
            }
            if (day > 9)
                Day = day.ToString();
            int month = dateTime.Month;
            switch (month)
            {
                case 1:
                    Month = "01";
                    break;
                case 2:
                    Month = "02";
                    break;
                case 3:
                    Month = "03";
                    break;
                case 4:
                    Month = "04";
                    break;
                case 5:
                    Month = "05";
                    break;
                case 6:
                    Month = "06";
                    break;
                case 7:
                    Month = "07";
                    break;
                case 8:
                    Month = "08";
                    break;
                case 9:
                    Month = "09";
                    break;
            }
            if (month > 9)
                Month = month.ToString();
            Year = dateTime.Year.ToString();
        }

        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public DateTime ConvertToDateTime()
        {
            int year = int.Parse(Year);
            int month = int.Parse(Month);
            int day = int.Parse(Day);
            return new DateTime(year, month, day);
        }

        public override string ToString()
        {
            return $"{Day}/{Month}/{Year}";
        }
    }
}
