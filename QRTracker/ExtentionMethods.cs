using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRTracker
{
    public static class ExtentionMethods
    {
        public static DateTime GetEndOfDay(this DateTime date)
        {

            return date.Date.AddSeconds(86399);

        }
        public static DateTime GetStartOfDay(this DateTime date)
        {

            return date.Date;

        }

    }
}